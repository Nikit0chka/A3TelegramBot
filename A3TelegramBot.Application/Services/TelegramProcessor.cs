using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;

namespace A3TelegramBot.Application.Services;

internal sealed class TelegramProcessor(
    ITelegramResponseService responseService,
    IUserSessionStateMachine userSessionStateMachine)
    :ITelegramProcessor
{
    public Task ProcessCommandAsync(long chatId, string command, CancellationToken cancellationToken) => !TelegramBotCommands.TryGetCommand(command, out var botCommand)? Task.CompletedTask : HandleBotCommandAsync(chatId, botCommand, cancellationToken);

    public Task ProcessContactAsync(long chatId, string phoneNumber, string userName, CancellationToken cancellationToken) => userSessionStateMachine.HandleContactAsync(chatId, phoneNumber, userName, cancellationToken);

    public Task ProcessLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken) => userSessionStateMachine.HandleLocationAsync(chatId, latitude, longitude, cancellationToken);

    public async Task ProcessTextMessageAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        if (TelegramBotCommands.TryGetCommand(message, out var command))
        {
            await userSessionStateMachine.HandleTextCommandAsync(chatId, command, cancellationToken);
            return;
        }

        await userSessionStateMachine.HandleTextMessageAsync(chatId, message, cancellationToken);
    }

    private Task HandleBotCommandAsync(long chatId, TelegramBotCommand? botCommand, CancellationToken cancellationToken)
    {
        return botCommand switch
        {
            _ when botCommand == TelegramBotCommands.Start => responseService.SendStartAsync(chatId, cancellationToken),
            _ when botCommand == TelegramBotCommands.Help => responseService.SendHelpAsync(chatId, cancellationToken),
            _ when botCommand == TelegramBotCommands.MainMenu => responseService.SendMainMenuAsync(chatId, cancellationToken),
            _ => userSessionStateMachine.HandleTextCommandAsync(chatId, botCommand, cancellationToken)
        };
    }
}