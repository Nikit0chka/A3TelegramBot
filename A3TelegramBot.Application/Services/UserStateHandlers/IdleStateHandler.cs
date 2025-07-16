using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.UseCases.UserSessionStatistic.PriceCheckRequest;
using A3TelegramBot.Application.UseCases.UserSessionStatistic.ServicesCheckRequest;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using MediatR;

namespace A3TelegramBot.Application.Services.UserStateHandlers;

/// <inheritdoc />
/// <summary>
/// Обработчик состояния "простоя"
/// </summary>
internal sealed class IdleStateHandler(
    ITelegramResponseService telegramResponseService,
    IUserSessionStateMachine userSessionStateMachine,
    IMediator mediator):IStateHandler
{
    public Task EnterStateAsync(long chatId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        return command switch
        {
            _ when command == TelegramBotCommands.FindNearestReceptions => userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InFindingNearestReceptions, null, cancellationToken),
            _ when command == TelegramBotCommands.RequestCallback => userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InCallbackRequest, null, cancellationToken),
            _ when command == TelegramBotCommands.CheckPrice => HandlePriceCheckRequest(chatId, cancellationToken),
            _ when command == TelegramBotCommands.Services => HandleServicesCheckRequest(chatId, cancellationToken),
            _ => telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken)
        };
    }

    public Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken) => telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);

    public Task HandleContactAsync(long chatId, string phoneNumber, string userName, CancellationToken cancellationToken) => telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);

    public Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken) => telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);

    private async Task HandlePriceCheckRequest(long chatId, CancellationToken cancellationToken)
    {
        await mediator.Send(new PriceCheckRequestCommand(), cancellationToken);
        await telegramResponseService.SendPriceAsync(chatId, cancellationToken);
    }

    private async Task HandleServicesCheckRequest(long chatId, CancellationToken cancellationToken)
    {
        await mediator.Send(new ServicesCheckRequestCommand(), cancellationToken);
        await telegramResponseService.SendServicesAsync(chatId, cancellationToken);
    }
}