using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using Ardalis.SharedKernel;

namespace A3TelegramBot.Application.UseCases.UserSessions.ChangeUserSessionState;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда изменения состояния
///     пользовательской сессии
/// </summary>
internal readonly record struct GetOrCreateUserSessionCommand(long ChatId) : ICommand<UserSession>;