﻿using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.AcceptPersonalDataProcessingPolicy;

/// <inheritdoc />
/// <summary>
///     Обработчик команды принятия политики обработки данных
/// </summary>
internal sealed class AcceptPersonalDataProcessingPolicyCommandHandler(
    IRepository<UserSession> userSessionRepository,
    ISdaiLomRfApiService sdaiLomRfApiService,
    ILogger<AcceptPersonalDataProcessingPolicyCommandHandler> logger)
    : ICommandHandler<AcceptPersonalDataProcessingPolicyCommand, ErrorOr<AcceptPersonalDataProcessingPolicyCommandResult>>
{
    public async Task<ErrorOr<AcceptPersonalDataProcessingPolicyCommandResult>> Handle(AcceptPersonalDataProcessingPolicyCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(AcceptPersonalDataProcessingPolicyCommand), request.ChatId);

        var userSession = await userSessionRepository.SingleOrDefaultAsync(new UserSessionByChatIdSpecificationWithCallBackRequest(request.ChatId), cancellationToken);

        var callBackRequestUserName = userSession.CallBackRequest.UserName;
        var callBackRequestPhone = userSession.CallBackRequest.Phone.Value.Value;

        if (userSession is null)
        {
            logger.LogWarning("Сессия не найдена");
            return Error.NotFound(description: "Сессия не найдена");
        }

        if (userSession.CallBackRequest is null)
        {
            logger.LogWarning("Заявка на обратный звонок не найдена, SessionId: {SessionId}", userSession.Id);
            return Error.NotFound(description: "Заявка на обратный звонок не найдена");
        }

        if (userSession.CurrentState != UserSessionState.InCallbackRequest)
        {
            //TODO:Возможно нужно выкидывать ошибку и обернуть все в try, чтобы пользователь не получал это сообщение
            logger.LogWarning("Состояние сессии не валидно, SessionId: {SessionId}", userSession.Id);
            return Error.Conflict(description: "Состояние сессии не валидно");
        }

        userSession.CallBackRequest.AcceptPersonalDataProcessingPolicy();

        var requestResult = await sdaiLomRfApiService.SendCallBackRequestCreated(userSession.CallBackRequest.Id, userSession.CallBackRequest.UserName!, userSession.CallBackRequest.Phone!.Value.Value, cancellationToken);

        if (requestResult.IsError)
        {
            logger.LogWarning("Ошибка Api запроса отправки создания заявки на обратный звонок, Error: {Error}", requestResult.FirstError);
            return requestResult.FirstError;
        }

        userSession.CompleteCallbackProcess();

        //TODO: после отправки заявки на обратный звонок она сохраняется не сразу. 
        //возможно стоит отправлять заявки в фоне
        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, политика обработки персональных данных принята, UserSessionId: {UserSessionId}",
                              nameof(AcceptPersonalDataProcessingPolicyCommand),
                              userSession.Id);

        return new AcceptPersonalDataProcessingPolicyCommandResult(callBackRequestUserName, callBackRequestPhone);
    }
}