using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.UseCases.UserSessions.ChangeUserSessionState;
using A3TelegramBot.Application.UseCases.UserSessionStatistic.PriceCheckRequest;
using A3TelegramBot.Application.UseCases.UserSessionStatistic.ServicesCheckRequest;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using MediatR;

namespace A3TelegramBot.Application.Services.UserState;

/// <inheritdoc />
/// <summary>
///     Обработчик состояния "простоя"
/// </summary>
internal sealed class IdleStateHandler(
    IUserSessionStateMachine userSessionStateMachine,
    IMediator mediator,
    ITelegramResponseService telegramResponseService)
        : BaseStateHandler(telegramResponseService)
{
    public override async Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            await telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
            return;
        }

        switch (command)
        {
            case var _ when command == TelegramBotCommands.FindNearestReceptions:
                await userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InFindingNearestReceptions, null, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.RequestCallback:
                await userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.InCallbackRequest, null, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.CheckPrice:
                await HandlePriceCheckRequest(chatId, cancellationToken);
                break;
            case var _ when command == TelegramBotCommands.Services:
                await HandleServicesCheckRequest(chatId, cancellationToken);
                break;
            default:
                await telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
                break;
        }
    }

    private async Task HandlePriceCheckRequest(long chatId, CancellationToken cancellationToken)
    {
        await mediator.Send(new PriceCheckRequestCommand(chatId), cancellationToken);
        await telegramResponseService.SendPriceAsync(chatId, cancellationToken);
    }

    private async Task HandleServicesCheckRequest(long chatId, CancellationToken cancellationToken)
    {
        await mediator.Send(new ServicesCheckRequestCommand(chatId), cancellationToken);
        await telegramResponseService.SendServicesAsync(chatId, cancellationToken);
    }
}