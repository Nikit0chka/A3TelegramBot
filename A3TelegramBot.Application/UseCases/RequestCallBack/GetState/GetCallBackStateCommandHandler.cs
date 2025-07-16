using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.RequestCallBack.GetState;

/// <inheritdoc />
/// <summary>
///     Обработчик команды получения состояния
///     заявки на обратный звонок
/// </summary>
internal sealed class GetCallBackStateCommandHandler(IReadRepository<UserSession> userSessionRepository, ILogger<GetCallBackStateCommandHandler> logger)
    :ICommandHandler<GetCallBackStateCommand, ErrorOr<CallBackRequestStatus?>>
{
    public async Task<ErrorOr<CallBackRequestStatus?>> Handle(GetCallBackStateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(GetCallBackStateCommand), request.ChatId);

        var userSession = await userSessionRepository.SingleOrDefaultAsync(new UserSessionByChatIdSpecificationWithCallBackRequest(request.ChatId), cancellationToken);

        if (userSession is null)
        {
            logger.LogWarning("Сессия не найдена");
            return Error.NotFound(description: "Сессия не найдена");
        }

        logger.LogInformation("Завершена обработка команды {Command}, UserSessionId: {UserSessionId}",
                              nameof(GetCallBackStateCommand),
                              userSession.Id);

        return userSession.CallBackRequest?.CurrentState;
    }
}