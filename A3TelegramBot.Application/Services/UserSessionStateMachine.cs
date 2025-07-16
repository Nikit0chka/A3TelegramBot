using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using Ardalis.SharedKernel;

namespace A3TelegramBot.Application.Services;

internal sealed class UserSessionStateMachine(
    IStateHandlerFactory stateHandlerFactory,
    IRepository<UserSession> userSessionRepository):IUserSessionStateMachine
{
    public async Task HandleContactAsync(long chatId, string phoneNumber, string userName, CancellationToken cancellationToken)
    {
        var session = await GetSessionOrCreateAsync(chatId, cancellationToken);
        var handler = stateHandlerFactory.GetHandler(session.CurrentState);
        await handler.HandleContactAsync(chatId, phoneNumber, userName, cancellationToken);
    }

    public async Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken)
    {
        var session = await GetSessionOrCreateAsync(chatId, cancellationToken);
        var handler = stateHandlerFactory.GetHandler(session.CurrentState);
        await handler.HandleLocationAsync(chatId, latitude, longitude, cancellationToken);
    }

    public async Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        var session = await GetSessionOrCreateAsync(chatId, cancellationToken);
        var handler = stateHandlerFactory.GetHandler(session.CurrentState);
        await handler.HandleTextMessageAsync(chatId, message, cancellationToken);
    }

    public async Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        var session = await GetSessionOrCreateAsync(chatId, cancellationToken);
        var handler = stateHandlerFactory.GetHandler(session.CurrentState);
        await handler.HandleTextCommandAsync(chatId, command, cancellationToken);
    }

    public async Task TransitionToStateAsync(long chatId, UserSessionState newState, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        var session = await GetSessionOrCreateAsync(chatId, cancellationToken);

        if (session.CurrentState != newState)
        {
            session.ChangeState(newState);
            await userSessionRepository.UpdateAsync(session, cancellationToken);

            var newHandler = stateHandlerFactory.GetHandler(newState);
            await newHandler.EnterStateAsync(chatId, cancellationToken);

            // Если команда не null, обрабатываем её в новом состоянии
            if (command != null)
            {
                await newHandler.HandleTextCommandAsync(chatId, command, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Получает или создает сессию при каждом запросе
    /// </summary>
    /// <param name="chatId">Идентификатор телеграм чата</param>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции</param>
    private async Task<UserSession> GetSessionOrCreateAsync(long chatId, CancellationToken cancellationToken)
    {
        var userSession = await userSessionRepository.SingleOrDefaultAsync(
                                                                           new UserSessionByChatIdSpecification(chatId),
                                                                           cancellationToken);

        if (userSession is not null)
            return userSession;

        userSession = new(chatId);
        await userSessionRepository.AddAsync(userSession, cancellationToken);

        return userSession;
    }
}