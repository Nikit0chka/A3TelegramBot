using Ardalis.Specification;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession.Specifications;

/// <inheritdoc cref="Ardalis.Specification.Specification{T}" />
/// <summary>
///     Спецификация поиска сессии пользователя по Id телеграм чата
/// </summary>
public sealed class UserSessionByChatIdSpecification:SingleResultSpecification<UserSession>
{
    public UserSessionByChatIdSpecification(long chatId) { Query.Where(userSession => userSession.ChatId == chatId); }
}