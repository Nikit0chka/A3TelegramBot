using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Application.UseCases.RequestCallBack.Cancel;

namespace A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;

/// <inheritdoc />
/// <summary>
///     Стратегия обработки команды отмены
///     заявки на обратный звонок
/// </summary>
internal sealed class CancelCallBackRequestCommandStrategy:ICallbackCommandStrategy
{
    public bool CanHandle(TelegramBotCommand command) => command == TelegramBotCommands.Cancel;

    public async Task ExecuteAsync(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken)
    {
        var result = await context.Mediator.SendMediatorRequest(
                                                                new CancelCallbackStateCommand(chatId),
                                                                chatId,
                                                                context.TelegramResponseService,
                                                                cancellationToken);

        if (!result.IsError)
            await context.TelegramResponseService.SendCallBackRequestCancelledAsync(chatId, cancellationToken);
    }
}