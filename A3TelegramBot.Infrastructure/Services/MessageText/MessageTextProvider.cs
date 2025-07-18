using System.Text;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.Dto;
using Microsoft.Extensions.Options;

namespace A3TelegramBot.Infrastructure.Services.MessageText;

internal sealed class MessageTextProvider(IOptions<MessageTextOptions> messageTextOptions) : IMessageTextProvider
{
    private const string CommandNotFoundBase = "Команда не опознана";
    private const string CommandNotFoundWithParam = "Команда {0} не опознана";

    public string GetMainMenuText() => "Главное меню:";
    public string GetStartText() => "Добро пожаловать!\nХотите сдать лом?\nВам сюда!";

    public string GetHelpText()
    {
        var commands = TelegramBotCommands.AllCommands
            .Where(static telegramBotCommand => telegramBotCommand.Type == TelegramCommandType.TextCommand)
            .Select(static telegramBotCommand => $"{telegramBotCommand.Command} - {telegramBotCommand.Description}");

        return "Основные команды:\n" + string.Join("\n", commands);
    }

    public string GetRequestPhoneText() => "Введите номер телефона, или поделитесь своим контактом.";
    public string GetRequestNameText() => "Как к вам обращаться?";

    public string GetCallBackRequestCompletedText(string name, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Имя и телефон обязательны");

        return $"{name}, мы перезвоним вам по номеру: {phoneNumber} в ближайшее время.";
    }

    public string GetCallBackRequestAlreadyCreatedText() => "Заявка на обратный звонок уже оформлена.";
    public string GetErrorText(string errorMessage) => $"Ошибка: {errorMessage}.";

    public string GetCommandNotFoundText(string? providedCommand) => providedCommand is not null
        ? string.Format(CommandNotFoundWithParam, providedCommand)
        : CommandNotFoundBase;

    public string GetPriceText() => $"Для ознакомления с ценами пройдите по ссылке: {messageTextOptions.Value.PriceLink}.";
    public string GetServicesText() => $"Для ознакомления с услугами пройдите по ссылке: {messageTextOptions.Value.ServicesLink}.";
    public string GetCallBackRequestCancelledText() => "Заявка на обратный звонок отменена.";
    public string GetRequestLocationText() => "Для поиска ближайших приемных пунктов поделитесь гео-локацией.";

    public string GetReceptionsListText(IEnumerable<ReceptionInfo> receptions)
    {
        var receptionInfos = receptions as ReceptionInfo[] ?? receptions.ToArray();

        if (receptionInfos.Length == 0)
            return "Приемные пункты не найдены";

        var sb = new StringBuilder("Ближайшие приемные пункты:\n");

        foreach (var receptionInfo in receptionInfos)
        {
            sb.AppendLine($"📍 {receptionInfo.Address}")
                .AppendLine($"📞 {string.Join("\n      ", receptionInfo.Phone)}")
                .AppendLine($"⏰ График работы: {receptionInfo.Schedule}")
                .AppendLine($"      {receptionInfo.WorkTime}\n");
        }

        return sb.ToString();
    }

    public string GetFindNearestReceptionsCancelledText() => $"Для поиска ближайших приемных пунктов перейдите по ссылке: {messageTextOptions.Value.ReceptionsLink}.";
    public string GetRequestPersonalDataProcessingPolicyText() => $"Вы согласны с политикой обработки персональных данных? {messageTextOptions.Value.PersonalDataPolicyLink}";
    public string GetUnhandledExceptionOccuredText() => "Произошла непредвиденная ошибка, при обработке запроса. Пожалуйста, попробуйте позже";
}