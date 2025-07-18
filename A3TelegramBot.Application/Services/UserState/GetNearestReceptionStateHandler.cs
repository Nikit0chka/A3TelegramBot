using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Application.UseCases.GetNearestReceptions.GetNearestReceptions;
using A3TelegramBot.Application.UseCases.GetNearestReceptions.Start;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using MediatR;

namespace A3TelegramBot.Application.Services.UserState;

/// <inheritdoc />
/// <summary>
///     Обработчик состояния поиска ближайших приемных пунктов
/// </summary>
internal sealed class GetNearestReceptionStateHandler(
    ITelegramResponseService telegramResponseService,
    IUserSessionStateMachine userSessionStateMachine,
    IMediator mediator)
    : BaseStateHandler(telegramResponseService)
{

    public override async Task EnterStateAsync(long chatId, CancellationToken cancellationToken)
    {
        var startResult = await mediator.SendMediatorRequest(
                                                             new StartGetNearestReceptionsCommand(chatId),
                                                             chatId,
                                                             telegramResponseService,
                                                             cancellationToken);

        if (!startResult.IsError)
            await telegramResponseService.SendRequestLocationAsync(chatId, cancellationToken);
    }

    public override async Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            await base.HandleTextCommandAsync(chatId, command, cancellationToken);
            return;
        }

        switch (command)
        {
            case var _ when command == TelegramBotCommands.RequestCallback:
                await userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InCallbackRequest, command, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.CheckPrice:
            case var _ when command == TelegramBotCommands.Services:
                await userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.Idle, command, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.FindNearestReceptions:
                await EnterStateAsync(chatId, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.Cancel:
                await HandleCancelCommandAsync(chatId, cancellationToken);
                break;
            default:
                await telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
                break;
        }
    }

    public override async Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken)
    {
        var result = await mediator.SendMediatorRequest(
                                                        new GetNearestReceptionsCommand(chatId, latitude, longitude),
                                                        chatId,
                                                        telegramResponseService,
                                                        cancellationToken);

        if (!result.IsError)
            await telegramResponseService.SendReceptionsList(chatId, result.Value, cancellationToken);
    }

    private async Task HandleCancelCommandAsync(long chatId, CancellationToken cancellationToken)
    {
        await userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.Idle, null, cancellationToken);
        await telegramResponseService.SendFindNearestReceptionsCancelledAsync(chatId, cancellationToken);
    }
}