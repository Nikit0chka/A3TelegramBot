using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.HandleName;

/// <inheritdoc />
/// <summary>
///     Обработчик команды ввода имени в
///     заявку на обратный звонок
/// </summary>
internal sealed class HandleNameCommandHandler(
    IRepository<UserSession> userSessionRepository,
    ILogger<HandleNameCommandHandler> logger)
    :ICommandHandler<HandleNameCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(HandleNameCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(HandleNameCommand), request.ChatId);

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

        var setNameResult = userSession.CallBackRequest.SetName(request.Name);

        if (setNameResult.IsError)
        {
            logger.LogWarning("Ошибка установки имени пользователя для заявки на обратный звонок. UserSessionId: {UserSessionId}, CallBackRequestId: {CallBackRequestId}, Error: {Error}",
                              userSession.Id,
                              userSession.CallBackRequest.Id,
                              string.Join(",", setNameResult.Errors));

            return setNameResult;
        }


        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, имя пользователя установлено, UserSessionId: {UserSessionId}, CallBackRequestId: {CallBackRequestId}",
                              nameof(HandleNameCommand),
                              userSession.Id,
                              userSession.CallBackRequest.Id);

        return Result.Success;
    }
}