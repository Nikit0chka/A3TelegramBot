using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.HandlePhone;

/// <inheritdoc />
/// <summary>
///     Обработчик команды ввода номера телефона в
///     заявку на обратный звонок
/// </summary>
internal sealed class HandlePhoneCommandHandler(
    IRepository<UserSession> userSessionRepository,
    ILogger<HandlePhoneCommandHandler> logger)
    :ICommandHandler<HandlePhoneCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(HandlePhoneCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(HandlePhoneCommand), request.ChatId);

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

        var setPhoneResult = userSession.CallBackRequest.SetPhone(request.PhoneNumber);

        if (setPhoneResult.IsError)
        {
            logger.LogWarning("Ошибка установки номер телефона для заявки на обратный звонок. UserSessionId: {UserSessionId}, CallBackRequestId: {CallBackRequestId}, Error: {Error}",
                              userSession.Id,
                              userSession.CallBackRequest.Id,
                              string.Join(",", setPhoneResult.Errors));

            return setPhoneResult;
        }

        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, телефон установлен, UserSessionId: {UserSessionId}, CallBackRequestId: {CallBackRequestId}",
                              nameof(HandlePhoneCommand),
                              userSession.Id,
                              userSession.CallBackRequest.Id);

        return Result.Success;
    }
}