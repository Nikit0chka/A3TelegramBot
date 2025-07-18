using A3TelegramBot.Application.UseCases.RequestCallBack.Cancel;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessions.ChangeUserSessionState;

/// <inheritdoc />
/// <summary>
///     Обработчик команды отмены
///     заявки на обратный звонок
/// </summary>
internal sealed class ChangeUserSessionCommandHandler(
    IRepository<UserSession> userSessionRepository,
    ILogger<ChangeUserSessionCommandHandler> logger)
    : ICommandHandler<ChangeUserSessionStateCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(ChangeUserSessionStateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}, State: {State}", nameof(ChangeUserSessionStateCommand), request.ChatId, request.UserSessionState);

        var userSession = await userSessionRepository.SingleOrDefaultAsync(new UserSessionByChatIdSpecificationWithCallBackRequest(request.ChatId), cancellationToken);

        if (userSession is null)
        {
            logger.LogWarning("Сессия не найдена");
            return Error.NotFound(description: "Сессия не найдена");
        }

        userSession.ChangeState(request.UserSessionState);

        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, состояние сесии изменено, UserSessionId: {UserSessionId}",
                              nameof(ChangeUserSessionStateCommand),
                              userSession.Id);

        return Result.Success;
    }
}