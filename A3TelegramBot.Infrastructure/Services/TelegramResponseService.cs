using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.Dto;
using Telegram.Bot;

namespace A3TelegramBot.Infrastructure.Services;

internal sealed class TelegramResponseService(
    ITelegramBotClient telegramBotClient,
    IMessageTextProvider messageTextProvider,
    IKeyboardFactory keyboardFactory):ITelegramResponseService
{
    public Task SendCallbackRequestAlreadyCreatedAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                          chatId,
                                                                                                                                          messageTextProvider.GetCallBackRequestAlreadyCreatedText(),
                                                                                                                                          cancellationToken: cancellationToken);

    public Task SendCallbackRequestCompletedAsync(long chatId, string name, string phoneNumber, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                                                      chatId,
                                                                                                                                                                      messageTextProvider.GetCallBackRequestCompletedText(name, phoneNumber),
                                                                                                                                                                      replyMarkup: keyboardFactory.CreateMainMenuKeyboard(),
                                                                                                                                                                      cancellationToken: cancellationToken);

    public Task SendCommandNotFoundAsync(long chatId, string? providedCommand, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                                     chatId,
                                                                                                                                                     messageTextProvider.GetCommandNotFoundText(providedCommand),
                                                                                                                                                     cancellationToken: cancellationToken);

    public Task SendErrorAsync(long chatId, string errorMessage, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                       chatId,
                                                                                                                                       messageTextProvider.GetErrorText(errorMessage),
                                                                                                                                       cancellationToken: cancellationToken);

    public Task SendHelpAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                 chatId,
                                                                                                                 messageTextProvider.GetHelpText(),
                                                                                                                 cancellationToken: cancellationToken);

    public Task SendMainMenuAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                     chatId,
                                                                                                                     messageTextProvider.GetMainMenuText(),
                                                                                                                     replyMarkup: keyboardFactory.CreateMainMenuKeyboard(),
                                                                                                                     cancellationToken: cancellationToken);

    public Task SendRequestNameAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                        chatId,
                                                                                                                        messageTextProvider.GetRequestNameText(),
                                                                                                                        replyMarkup: keyboardFactory.CreateNameRequestKeyboard(),
                                                                                                                        cancellationToken: cancellationToken);

    public Task SendRequestPhoneAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                         chatId,
                                                                                                                         messageTextProvider.GetRequestPhoneText(),
                                                                                                                         replyMarkup: keyboardFactory.CreatePhoneRequestKeyboard(),
                                                                                                                         cancellationToken: cancellationToken);

    public Task SendRequestPersonalDataProcessingPolicyAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                                chatId,
                                                                                                                                                messageTextProvider.GetRequestPersonalDataProcessingPolicyText(),
                                                                                                                                                replyMarkup: keyboardFactory.CreatePersonalDataProcessingPolicyKeyboard(),
                                                                                                                                                cancellationToken: cancellationToken);

    public Task SendStartAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                  chatId,
                                                                                                                  messageTextProvider.GetStartText(),
                                                                                                                  replyMarkup: keyboardFactory.CreateMainMenuKeyboard(),
                                                                                                                  cancellationToken: cancellationToken);

    public Task SendPriceAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                  chatId,
                                                                                                                  messageTextProvider.GetPriceText(),
                                                                                                                  cancellationToken: cancellationToken);

    public Task SendServicesAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                     chatId,
                                                                                                                     messageTextProvider.GetServicesText(),
                                                                                                                     cancellationToken: cancellationToken);

    public Task SendCallBackRequestCancelledAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                     chatId,
                                                                                                                                     messageTextProvider.GetCallBackRequestCancelledText(),
                                                                                                                                     replyMarkup: keyboardFactory.CreateMainMenuKeyboard(),
                                                                                                                                     cancellationToken: cancellationToken);

    public Task SendRequestLocationAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                            chatId,
                                                                                                                            messageTextProvider.GetRequestLocationText(),
                                                                                                                            replyMarkup: keyboardFactory.CreateLocationKeyboard(),
                                                                                                                            cancellationToken: cancellationToken);

    public Task SendReceptionsList(long chatId, IEnumerable<ReceptionInfo> receptions, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                                             chatId,
                                                                                                                                                             messageTextProvider.GetReceptionsListText(receptions),
                                                                                                                                                             replyMarkup: keyboardFactory.CreateMainMenuKeyboard(),
                                                                                                                                                             cancellationToken: cancellationToken);

    public Task SendFindNearestReceptionsCancelledAsync(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                           chatId,
                                                                                                                                           messageTextProvider.GetFindNearestReceptionsCancelledText(),
                                                                                                                                           replyMarkup: keyboardFactory.CreateMainMenuKeyboard(),
                                                                                                                                           cancellationToken: cancellationToken);

    public Task SendUnhandledExceptionOccured(long chatId, CancellationToken cancellationToken) => telegramBotClient.SendMessage(
                                                                                                                                 chatId,
                                                                                                                                 messageTextProvider.GetFindNearestReceptionsCancelledText(),
                                                                                                                                 replyMarkup: keyboardFactory.CreateMainMenuKeyboard(),
                                                                                                                                 cancellationToken: cancellationToken);
}