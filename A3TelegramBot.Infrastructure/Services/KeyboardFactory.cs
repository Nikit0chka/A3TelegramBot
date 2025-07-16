using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using Telegram.Bot.Types.ReplyMarkups;

namespace A3TelegramBot.Infrastructure.Services;

internal sealed class KeyboardFactory:IKeyboardFactory
{
    public ReplyKeyboardMarkup CreateMainMenuKeyboard() => new(
                                                           [
                                                               [CreateButton(TelegramBotCommands.FindNearestReceptions), CreateButton(TelegramBotCommands.RequestCallback)],
                                                               [CreateButton(TelegramBotCommands.Services), CreateButton(TelegramBotCommands.CheckPrice)]
                                                           ])
                                                           {
                                                               ResizeKeyboard = true
                                                           };

    public ReplyKeyboardMarkup CreateLocationKeyboard() => new(
                                                           [
                                                               [CreateButton(TelegramBotCommands.ShareLocation), CreateButton(TelegramBotCommands.Cancel)]
                                                           ])
                                                           {
                                                               ResizeKeyboard = true,
                                                               OneTimeKeyboard = true
                                                           };

    public ReplyKeyboardMarkup CreatePhoneRequestKeyboard() => new(
                                                               [
                                                                   [CreateButton(TelegramBotCommands.SharePhone), CreateButton(TelegramBotCommands.Cancel)]
                                                               ])
                                                               {
                                                                   ResizeKeyboard = true,
                                                                   OneTimeKeyboard = true
                                                               };

    public ReplyKeyboardMarkup CreateNameRequestKeyboard() => new(
                                                              [
                                                                  [CreateButton(TelegramBotCommands.Cancel)]
                                                              ])
                                                              {
                                                                  ResizeKeyboard = true,
                                                                  OneTimeKeyboard = true
                                                              };

    public ReplyKeyboardMarkup CreatePersonalDataProcessingPolicyKeyboard() => new(
                                                                               [
                                                                                   [CreateButton(TelegramBotCommands.Yes), CreateButton(TelegramBotCommands.Cancel)]
                                                                               ])
                                                                               {
                                                                                   ResizeKeyboard = true,
                                                                                   OneTimeKeyboard = true
                                                                               };

    private static KeyboardButton CreateButton(TelegramBotCommand command)
    {
        return command.Type switch
        {
            TelegramCommandType.ContactButton => KeyboardButton.WithRequestContact(command.DisplayText),
            TelegramCommandType.LocationButton => KeyboardButton.WithRequestLocation(command.DisplayText),
            _ => new(command.DisplayText)
        };
    }
}