using Ardalis.SharedKernel;
using MediatR;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.ServicesCheckRequest;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда сохранение статистики запроса услуг
/// </summary>
internal readonly record struct ServicesCheckRequestCommand(long ChatId):ICommand<Unit>;