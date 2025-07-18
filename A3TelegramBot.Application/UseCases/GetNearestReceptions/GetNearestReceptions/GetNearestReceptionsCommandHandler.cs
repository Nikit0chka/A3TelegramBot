using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.Dto;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;

namespace A3TelegramBot.Application.UseCases.GetNearestReceptions.GetNearestReceptions;

/// <inheritdoc />
/// <summary>
///     Обработчик команды поиска
///     ближайших приемных пунктов
/// </summary>
internal sealed class GetNearestReceptionsCommandHandler(
    IRepository<UserSession> userSessionRepository,
    IA3ApiService apiService,
    ILogger<GetNearestReceptionsCommandHandler> logger)
    :ICommandHandler<GetNearestReceptionsCommand, ErrorOr<IReadOnlyCollection<ReceptionInfo>>>
{
    public async Task<ErrorOr<IReadOnlyCollection<ReceptionInfo>>> Handle(GetNearestReceptionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Обработка команды {Command} ChatId: {ChatId}", nameof(GetNearestReceptionsCommand), request.ChatId);

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

            var getReceptionsResult = await apiService.GetNearestReceptionsAsync(request.Latitude, request.Longitude, cancellationToken);

            if (getReceptionsResult.IsError)
            {
                logger.LogWarning("Ошибка Api запроса получения приемных пунктов: {error}", getReceptionsResult.FirstError);
                return getReceptionsResult;
            }

            userSession.CompleteSearchNearestReceptionsProcess();

            await userSessionRepository.UpdateAsync(userSession, cancellationToken);

            logger.LogInformation("Завершена обработка команды {Command} UserSessionId: {UserSessionId}", nameof(GetNearestReceptionsCommand), userSession.Id);

            return getReceptionsResult.Value.ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Ошибка при обработке команды {Command} ChatId: {ChatId}", nameof(GetNearestReceptionsCommand), request.ChatId);
            return Error.Failure(description: "Произошла непредвиденная ошибка, попробуйте позже");
        }
    }
}