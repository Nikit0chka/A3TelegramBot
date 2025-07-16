using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.GetNearestReceptions.Start;

/// <inheritdoc />
/// <summary>
///     Обработчик команды запуска
///     поиска приемных пунктов
/// </summary>
internal sealed class StartGetNearestReceptionsCommandHandler(IRepository<UserSession> userSessionRepository, ILogger<StartGetNearestReceptionsCommandHandler> logger)
    :ICommandHandler<StartGetNearestReceptionsCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        StartGetNearestReceptionsCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(StartGetNearestReceptionsCommand), request.ChatId);


        var userSession = await userSessionRepository.SingleOrDefaultAsync(
                                                                           new UserSessionByChatIdSpecification(request.ChatId),
                                                                           cancellationToken);

        if (userSession is null)
        {
            logger.LogWarning("Сессия не найдена");
            return Error.NotFound(description: "Сессия не найдена");
        }

        userSession.StartSearchNearestReceptionsProcess();
        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, поиск ближайших приемных пунктов начат, UserSessionId: {UserSessionId}", nameof(StartGetNearestReceptionsCommand), userSession.Id);

        return Result.Success;
    }
}