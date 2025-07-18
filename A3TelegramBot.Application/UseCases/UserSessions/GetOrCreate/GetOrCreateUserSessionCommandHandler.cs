using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessions.ChangeUserSessionState;

/// <inheritdoc />
/// <summary>
///     Обработчик команды отмены
///     заявки на обратный звонок
/// </summary>
internal sealed class GetOrCreateUserSessionCommandHandler(
    IRepository<UserSession> userSessionRepository,
    ILogger<GetOrCreateUserSessionCommandHandler> logger)
    : ICommandHandler<GetOrCreateUserSessionCommand, UserSession>
{
    public async Task<UserSession> Handle(GetOrCreateUserSessionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(GetOrCreateUserSessionCommand), request.ChatId);

        var userSession = await userSessionRepository.FirstOrDefaultAsync(new UserSessionByChatIdSpecification(request.ChatId), cancellationToken);

        if (userSession is null)
        {
            userSession = new UserSession(request.ChatId);
            await userSessionRepository.AddAsync(userSession, cancellationToken);
        }


        logger.LogInformation("Завершена обработка команды {Command}, UserSessionId: {UserSessionId}",
                              nameof(GetOrCreateUserSessionCommand),
                              userSession.Id);

        return userSession;
    }
}