using Ardalis.SharedKernel;
using MediatR;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.PriceCheckRequest;

/// <inheritdoc cref="ICommand{TResponse}" />
/// <summary>
///     Команда сохранение статистики запроса цен
/// </summary>
internal readonly record struct PriceCheckRequestCommand(long ChatId):ICommand<Unit>;