using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession.Specifications;
using Ardalis.SharedKernel;

namespace A3TelegramBot.Application.Services.UserState;

internal sealed class UserSessionStateMachine(
    IStateHandlerFactory stateHandlerFactory,
    IRepository<UserSession> userSessionRepository)
    :IUserSessionStateMachine
{
    public async Task HandleContactAsync(long chatId, string phoneNumber, string userName, CancellationToken ct)
    {
        var session = await GetOrCreateSessionAsync(chatId, ct);
        await GetHandler(session).HandleContactAsync(chatId, phoneNumber, userName, ct);
    }

    public async Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken ct)
    {
        var session = await GetOrCreateSessionAsync(chatId, ct);
        await GetHandler(session).HandleLocationAsync(chatId, latitude, longitude, ct);
    }

    public async Task HandleTextMessageAsync(long chatId, string message, CancellationToken ct)
    {
        var session = await GetOrCreateSessionAsync(chatId, ct);
        await GetHandler(session).HandleTextMessageAsync(chatId, message, ct);
    }

    public async Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken ct)
    {
        var session = await GetOrCreateSessionAsync(chatId, ct);
        await GetHandler(session).HandleTextCommandAsync(chatId, command, ct);
    }

    public async Task TransitionToStateAsync(
        long chatId,
        UserSessionState newState,
        TelegramBotCommand? command,
        CancellationToken ct)
    {
        var session = await GetOrCreateSessionAsync(chatId, ct);

        if (session.CurrentState != newState)
        {
            session.ChangeState(newState);
            await userSessionRepository.UpdateAsync(session, ct);
            await GetHandler(session).EnterStateAsync(chatId, ct);
        }

        if (command != null)
            await GetHandler(session).HandleTextCommandAsync(chatId, command, ct);
    }

    private IStateHandler GetHandler(UserSession session) => stateHandlerFactory.GetHandler(session.CurrentState);

    public async Task<UserSession> GetOrCreateSessionAsync(long chatId, CancellationToken ct) => await userSessionRepository.FirstOrDefaultAsync(
                                                                                                                                                 new UserSessionByChatIdSpecification(chatId),
                                                                                                                                                 ct) ??
                                                                                                 await CreateNewSessionAsync(chatId, ct);

    private async Task<UserSession> CreateNewSessionAsync(long chatId, CancellationToken ct)
    {
        var session = new UserSession(chatId);
        await userSessionRepository.AddAsync(session, ct);
        return session;
    }
}