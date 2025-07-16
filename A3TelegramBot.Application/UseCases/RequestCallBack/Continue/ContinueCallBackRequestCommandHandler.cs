using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.Continue;

/// <inheritdoc />
/// <summary>
///     Обработчик команды возобновления
///     заявки на обратный звонок
/// </summary>
internal sealed class ContinueCallBackRequestCommandHandler(IRepository<UserSession> userSessionRepository, ILogger<ContinueCallBackRequestCommandHandler> logger)
    :ICommandHandler<ContinueCallBackRequestCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(ContinueCallBackRequestCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(ContinueCallBackRequestCommand), request.ChatId);

        var userSession = await userSessionRepository.SingleOrDefaultAsync(new UserSessionByChatIdSpecificationWithCallBackRequest(request.ChatId), cancellationToken);

        if (userSession is null)
        {
            logger.LogWarning("Сессия не найдена");
            return Error.NotFound(description: "Сессия не найдена");
        }

        if (userSession.CurrentState != UserSessionState.InCallbackRequest)
        {
            //TODO:Возможно нужно выкидывать ошибку и обернуть все в try, чтобы пользователь не получал это сообщение
            logger.LogWarning("Состояние сессии не валидно, SessionId: {SessionId}", userSession.Id);
            return Error.Conflict(description: "Состояние сессии не валидно");
        }

        if (userSession.CallBackRequest is null)
        {
            logger.LogWarning("Заявка на обратный звонок не найдена, SessionId: {SessionId}", userSession.Id);
            return Error.NotFound(description: "Заявка на обратный звонок не найдена");
        }

        userSession.ContinueRequestCallBackProcess();

        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, заявка на обратный звонок возобновлена, UserSessionId: {UserSessionId}, CallBackRequestId: {CallBackRequestId}",
                              nameof(ContinueCallBackRequestCommand),
                              userSession.Id,
                              userSession.CallBackRequest.Id);

        return Result.Success;
    }
}