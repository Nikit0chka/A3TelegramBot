using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using Ardalis.SharedKernel;
using ErrorOr;

namespace A3TelegramBot.Application.UseCases.UserSessions.ChangeUserSessionState;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда изменения состояния
///     пользовательской сессии
/// </summary>
internal readonly record struct ChangeUserSessionStateCommand(long ChatId, UserSessionState UserSessionState) : ICommand<ErrorOr<Success>>;