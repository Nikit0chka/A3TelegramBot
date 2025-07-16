using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace A3TelegramBot.Infrastructure.Services;

internal sealed class TelegramCommandConfigurator:ITelegramCommandConfigurator
{
    public Task SetCommandsAsync(ITelegramBotClient telegramBotClient, CancellationToken cancellationToken) => telegramBotClient.SetMyCommands(GetBotCommands(), cancellationToken: cancellationToken);

    private static IEnumerable<BotCommand> GetBotCommands() => TelegramBotCommands.AllCommands
        .Where(static botCommand => botCommand.Type == TelegramCommandType.TextCommand)
        .Select(static botCommand => new BotCommand
                                     {
                                         Command = botCommand.Command.TrimStart('/'),
                                         Description = botCommand.Description
                                     });
}