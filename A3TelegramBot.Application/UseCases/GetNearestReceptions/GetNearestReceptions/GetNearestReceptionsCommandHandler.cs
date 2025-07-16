using A3TelegramBot.Application.Dto;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using Ardalis.SharedKernel;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.GetNearestReceptions.GetNearestReceptions;

/// <inheritdoc />
/// <summary>
///     Обработчик команды поиска
///     ближайших приемных пунктов
/// </summary>
internal sealed class GetNearestReceptionsCommandHandler(IRepository<UserSession> userSessionRepository, ILogger<GetNearestReceptionsCommandHandler> logger)
    :ICommandHandler<GetNearestReceptionsCommand, ErrorOr<IReadOnlyCollection<ReceptionInfo>>>
{
    public async Task<ErrorOr<IReadOnlyCollection<ReceptionInfo>>> Handle(GetNearestReceptionsCommand request, CancellationToken cancellationToken)
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

        userSession.CompleteSearchNearestReceptionsProcess();

        await userSessionRepository.UpdateAsync(userSession, cancellationToken);

        var reception1 = new ReceptionInfo("ПП1", "Адрес1", "+79873691806", "24\\7");
        var reception2 = new ReceptionInfo("ПП2", "Адрес2", "+79873691806", "24\\7");
        var reception3 = new ReceptionInfo("ПП3", "Адрес3", "+79873691806", "24\\7");
        var reception4 = new ReceptionInfo("ПП4", "Адрес4", "+79873691806", "24\\7");
        var reception5 = new ReceptionInfo("ПП5", "Адрес5", "+79873691806", "24\\7");
        var reception6 = new ReceptionInfo("ПП6", "Адрес6", "+79873691806", "24\\7");
        var reception7 = new ReceptionInfo("ПП7", "Адрес7", "+79873691806", "24\\1");
        var reception8 = new ReceptionInfo("ПП8", "Адрес8", "+79873691806", "24\\7");
        var reception9 = new ReceptionInfo("ПП9", "Адрес9", "+79873691806", "24\\7");
        var reception10 = new ReceptionInfo("ПП10", "Адрес10", "+79873691806", "24\\7");

        var receptionList = new List<ReceptionInfo> { reception1, reception2, reception3, reception4, reception5, reception6, reception7, reception8, reception9, reception10 };

        logger.LogInformation("Завершена обработка команды {Command} UserSessionId: {UserSessionId}", nameof(GetNearestReceptionsCommand), userSession.Id);

        return receptionList;
    }
}