using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.UseCases.UserSessionStatistic.PriceCheckRequest;
using A3TelegramBot.Application.UseCases.UserSessionStatistic.ServicesCheckRequest;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession;
using MediatR;

namespace A3TelegramBot.Application.Services.UserState;

/// <inheritdoc />
/// <summary>
///     Обработчик состояния "простоя"
/// </summary>
internal sealed class IdleStateHandler(
    ITelegramResponseService telegramResponseService,
    IMediator mediator,
    IUserSessionStateMachine userSessionStateMachine)
    :BaseStateHandler(telegramResponseService, mediator, userSessionStateMachine)
{
    public override async Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            await TelegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
            return;
        }

        switch (command)
        {
            case var _ when command == TelegramBotCommands.FindNearestReceptions:
                await UserSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InFindingNearestReceptions, null, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.RequestCallback:
                await UserSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InCallbackRequest, null, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.CheckPrice:
                await HandlePriceCheckRequest(chatId, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.Services:
                await HandleServicesCheckRequest(chatId, cancellationToken);
                break;
            default:
                await TelegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
                break;
        }
    }

    private async Task HandlePriceCheckRequest(long chatId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new PriceCheckRequestCommand(chatId), cancellationToken);
        await TelegramResponseService.SendPriceAsync(chatId, cancellationToken);
    }

    private async Task HandleServicesCheckRequest(long chatId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ServicesCheckRequestCommand(chatId), cancellationToken);
        await TelegramResponseService.SendServicesAsync(chatId, cancellationToken);
    }
}