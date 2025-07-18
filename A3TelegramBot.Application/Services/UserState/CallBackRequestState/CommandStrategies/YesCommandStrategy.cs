using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Application.UseCases.RequestCallBack.AcceptPersonalDataProcessingPolicy;

namespace A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;

/// <inheritdoc />
/// <summary>
///     Стратегия обработки команды
///     принятия политики обработки персональных данных
/// </summary>
internal sealed class YesCommandStrategy:ICallbackCommandStrategy
{
    public bool CanHandle(TelegramBotCommand command) => command == TelegramBotCommands.Yes;

    public async Task ExecuteAsync(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken)
    {
        var result = await context.Mediator.SendMediatorRequest(
                                                                new AcceptPersonalDataProcessingPolicyCommand(chatId),
                                                                chatId,
                                                                context.TelegramResponseService,
                                                                cancellationToken);

        if (!result.IsError)
        {
            await context.TelegramResponseService.SendCallbackRequestCompletedAsync(
                                                                                    chatId,
                                                                                    result.Value.CallBackRequestUserName,
                                                                                    result.Value.CallBackRequestUserPhone,
                                                                                    cancellationToken);
        }
    }
}