using Ardalis.Specification;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;

/// <inheritdoc cref="Specification{T}" />
/// <summary>
///     Спецификация поиска сессии пользователя по Id телеграм чата с заявками на обратный звонок
/// </summary>
public sealed class UserSessionByChatIdSpecificationWithCallBackRequest:SingleResultSpecification<UserSession>
{
    public UserSessionByChatIdSpecificationWithCallBackRequest(long chatId) { Query.Where(userSession => userSession.ChatId == chatId).Include(static userSession => userSession.CallBackRequest); }
}