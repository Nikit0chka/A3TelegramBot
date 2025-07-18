using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.UseCases.UserSessions.ChangeUserSessionState;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using MediatR;

namespace A3TelegramBot.Application.Services.UserState;

internal sealed class UserSessionStateMachine(
    IStateHandlerFactory stateHandlerFactory,
    IMediator mediator)
    : IUserSessionStateMachine
{
    public async Task HandleContactAsync(long chatId, string phoneNumber, string userName, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new GetOrCreateUserSessionCommand(chatId), cancellationToken);
        await GetHandler(session).HandleContactAsync(chatId, phoneNumber, userName, cancellationToken);
    }

    public async Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new GetOrCreateUserSessionCommand(chatId), cancellationToken);
        await GetHandler(session).HandleLocationAsync(chatId, latitude, longitude, cancellationToken);
    }

    public async Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new GetOrCreateUserSessionCommand(chatId), cancellationToken);
        await GetHandler(session).HandleTextMessageAsync(chatId, message, cancellationToken);
    }

    public async Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new GetOrCreateUserSessionCommand(chatId), cancellationToken);
        await GetHandler(session).HandleTextCommandAsync(chatId, command, cancellationToken);
    }

    public async Task TransitionToStateAsync(
       long chatId,
       UserSessionState newState,
       TelegramBotCommand? command,
       CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new GetOrCreateUserSessionCommand(chatId), cancellationToken);

        if (session.CurrentState != newState)
        {
            await mediator.Send(new ChangeUserSessionStateCommand(chatId, newState), cancellationToken);
            await GetHandler(session).EnterStateAsync(chatId, cancellationToken);
        }

        if (command != null)
            await GetHandler(session).HandleTextCommandAsync(chatId, command, cancellationToken);
    }

    private IStateHandler GetHandler(UserSession session) => stateHandlerFactory.GetHandler(session.CurrentState);
}