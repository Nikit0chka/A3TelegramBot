using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.UseCases.GetNearestReceptions.GetNearestReceptions;
using A3TelegramBot.Application.UseCases.GetNearestReceptions.Start;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using ErrorOr;
using MediatR;

namespace A3TelegramBot.Application.Services.UserStateHandlers;

/// <inheritdoc />
/// <summary>
/// Обработчик состояния поиска ближайших приемных пунктов
/// </summary>
internal sealed class GetNearestReceptionStateHandler(
    ITelegramResponseService telegramResponseService,
    IUserSessionStateMachine userSessionStateMachine,
    IMediator mediator):IStateHandler
{
    public Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        return command switch
        {
            _ when command == TelegramBotCommands.RequestCallback => userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InCallbackRequest, null, cancellationToken),
            _ when command == TelegramBotCommands.CheckPrice => userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.Idle, command, cancellationToken),
            _ when command == TelegramBotCommands.Services => userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.Idle, command, cancellationToken),
            _ when command == TelegramBotCommands.FindNearestReceptions => EnterStateAsync(chatId, cancellationToken),
            _ when command == TelegramBotCommands.Cancel => HandleCancelCommandAsync(chatId, cancellationToken),
            _ => telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken)
        };
    }

    public async Task EnterStateAsync(long chatId, CancellationToken cancellationToken)
    {
        var startResult = await mediator.Send(new StartGetNearestReceptionsCommand(chatId), cancellationToken);

        if (startResult.IsError)
        {
            await SendError(chatId, startResult.Errors, cancellationToken);
            return;
        }

        await telegramResponseService.SendRequestLocationAsync(chatId, cancellationToken);
    }

    public Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken) => telegramResponseService.SendErrorAsync(chatId, "Команда не найдена", cancellationToken);

    public Task HandleContactAsync(long chatId, string phoneNumber, string userName, CancellationToken cancellationToken) => telegramResponseService.SendErrorAsync(chatId, "Команда не найдена", cancellationToken);

    public async Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken)
    {
        var getNearestReceptionsResult = await mediator.Send(
                                                             new GetNearestReceptionsCommand(chatId, latitude, longitude),
                                                             cancellationToken);

        if (getNearestReceptionsResult.IsError)
        {
            await SendError(chatId, getNearestReceptionsResult.Errors, cancellationToken);
            return;
        }

        await telegramResponseService.SendReceptionsList(
                                                         chatId,
                                                         getNearestReceptionsResult.Value,
                                                         cancellationToken);
    }

    private async Task HandleCancelCommandAsync(long chatId, CancellationToken cancellationToken)
    {
        await userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.Idle, null, cancellationToken);
        await telegramResponseService.SendFindNearestReceptionsCancelledAsync(chatId, cancellationToken);
    }

    private Task SendError(long chatId, IEnumerable<Error> errors, CancellationToken ct)
    {
        var errorMessage = string.Join("\n", errors.Select(static error => error.Description));
        return telegramResponseService.SendErrorAsync(chatId, errorMessage, ct);
    }
}