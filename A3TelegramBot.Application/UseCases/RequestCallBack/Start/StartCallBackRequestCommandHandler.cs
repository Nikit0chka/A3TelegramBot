using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.Start;

/// <inheritdoc />
/// <summary>
///     Обработчик команды старта заявки на обратный звонок
/// </summary>
internal sealed class StartCallBackRequestCommandHandler(
    IRepository<UserSession> userSessionRepository,
    ILogger<StartCallBackRequestCommandHandler> logger)
    :ICommandHandler<StartCallBackRequestCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        StartCallBackRequestCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(StartCallBackRequestCommand), request.ChatId);

        var userSession = await userSessionRepository.SingleOrDefaultAsync(new UserSessionByChatIdSpecificationWithCallBackRequest(request.ChatId), cancellationToken);

        if (userSession is null)
        {
            logger.LogWarning("Сессия не найдена");
            return Error.NotFound(description: "Сессия не найдена");
        }

        var callBackRequest = new CallBackRequest(userSession.Id);

        userSession.StartCallbackProcess(callBackRequest);

        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, заявка на обратный звонок начата, UserSessionId: {UserSessionId}, CallBackRequestId: {CallBackRequestId}",
                              nameof(StartCallBackRequestCommand),
                              userSession.Id,
                              userSession.CallBackRequest!.Id);

        return Result.Success;
    }
}