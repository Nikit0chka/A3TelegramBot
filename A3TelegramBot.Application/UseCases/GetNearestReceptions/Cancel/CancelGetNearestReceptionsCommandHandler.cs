using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.GetNearestReceptions.Cancel;

/// <inheritdoc />
/// <summary>
///     Обработчик команды отмены поиска
///     ближайших приемных пунктов
/// </summary>
internal sealed class CancelGetNearestReceptionsCommandHandler(IRepository<UserSession> userSessionRepository, ILogger<CancelGetNearestReceptionsCommandHandler> logger)
    :ICommandHandler<CancelGetNearestReceptionsCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        CancelGetNearestReceptionsCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(CancelGetNearestReceptionsCommand), request.ChatId);

        var userSession = await userSessionRepository.SingleOrDefaultAsync(
                                                                           new UserSessionByChatIdSpecification(request.ChatId),
                                                                           cancellationToken);

        if (userSession is null)
        {
            logger.LogWarning("Сессия не найдена");
            return Error.NotFound(description: "Сессия не найдена");
        }

        if (userSession.CurrentState != UserSessionState.InFindingNearestReceptions)
        {
            //TODO:Возможно нужно выкидывать ошибку и обернуть все в try, чтобы пользователь не получал это сообщение
            logger.LogWarning("Состояние сессии не валидно, SessionId: {SessionId}", userSession.Id);
            return Error.Conflict(description: "Состояние сессии не валидно");
        }

        userSession.CancelSearchNearestReceptionsProcess();
        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, поиск ближайших приемных пунктов отменен, UserSessionId: {UserSessionId}", nameof(CancelGetNearestReceptionsCommand), userSession.Id);

        return Result.Success;
    }
}