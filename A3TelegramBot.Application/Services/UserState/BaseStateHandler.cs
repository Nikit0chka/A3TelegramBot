using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using MediatR;

namespace A3TelegramBot.Application.Services.UserState;

/// <inheritdoc />
/// <summary>
///     Базовый класс обработчик состояния пользовательской сессии
/// </summary>
internal abstract class BaseStateHandler(ITelegramResponseService telegramResponseService)
        : IStateHandler
{
    public virtual Task EnterStateAsync(long chatId, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken) => telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
    public virtual Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken) => telegramResponseService.SendErrorAsync(chatId, "Команда не найдена", cancellationToken);
    public virtual Task HandleContactAsync(long chatId, string phone, string name, CancellationToken cancellationToken) => telegramResponseService.SendErrorAsync(chatId, "Команда не найдена", cancellationToken);
    public virtual Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken) => telegramResponseService.SendErrorAsync(chatId, "Команда не найдена", cancellationToken);
}