using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Application.UseCases.GetNearestReceptions.GetNearestReceptions;
using A3TelegramBot.Application.UseCases.GetNearestReceptions.Start;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession;
using MediatR;

namespace A3TelegramBot.Application.Services.UserState;

/// <inheritdoc />
/// <summary>
///     Обработчик состояния поиска ближайших приемных пунктов
/// </summary>
internal sealed class GetNearestReceptionStateHandler(
    ITelegramResponseService telegramResponseService,
    IMediator mediator,
    IUserSessionStateMachine userSessionStateMachine)
    :BaseStateHandler(telegramResponseService, mediator, userSessionStateMachine)
{

    public override async Task EnterStateAsync(long chatId, CancellationToken cancellationToken)
    {
        var startResult = await Mediator.SendMediatorRequest(
                                                             new StartGetNearestReceptionsCommand(chatId),
                                                             chatId,
                                                             TelegramResponseService,
                                                             cancellationToken);

        if (!startResult.IsError)
        {
            await TelegramResponseService.SendRequestLocationAsync(chatId, cancellationToken);
        }
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
                await UserSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InCallbackRequest, null, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.CheckPrice:
            case var _ when command == TelegramBotCommands.Services:
                await UserSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.Idle, command, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.FindNearestReceptions:
                await EnterStateAsync(chatId, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.Cancel:
                await HandleCancelCommandAsync(chatId, cancellationToken);
                break;
            default:
                await TelegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
                break;
        }
    }

    public override async Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendMediatorRequest(
                                                        new GetNearestReceptionsCommand(chatId, latitude, longitude),
                                                        chatId,
                                                        TelegramResponseService,
                                                        cancellationToken);

        if (!result.IsError)
            await TelegramResponseService.SendReceptionsList(chatId, result.Value, cancellationToken);
    }

    private async Task HandleCancelCommandAsync(long chatId, CancellationToken cancellationToken)
    {
        await UserSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.Idle, null, cancellationToken);
        await TelegramResponseService.SendFindNearestReceptionsCancelledAsync(chatId, cancellationToken);
    }
}