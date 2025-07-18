using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;
using A3TelegramBot.Application.UseCases.RequestCallBack.HandlePhone;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

namespace A3TelegramBot.Application.Services.UserState.CallBackRequestState.StateHandlers;

/// <inheritdoc />
/// <summary>
///     Обработчик состояния "Ожидания ввода телефона"
///     при оформлении заявки на обратный звонок
/// </summary>
internal sealed class AwaitingPhoneCallBackStateHandler:ICallbackStateHandler
{
    public CallBackRequestStatus Status => CallBackRequestStatus.AwaitingPhone;

    public Task EnterAsync(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken) => context.TelegramResponseService.SendRequestPhoneAsync(chatId, cancellationToken);

    public async Task HandleMessageAsync(long chatId, string message, CallBackRequestStateContext context, CancellationToken cancellationToken)
    {
        var result = await context.Mediator.SendMediatorRequest(
                                                                new HandlePhoneCommand(chatId, message),
                                                                chatId,
                                                                context.TelegramResponseService,
                                                                cancellationToken);

        if (!result.IsError)
            await context.TelegramResponseService.SendRequestNameAsync(chatId, cancellationToken);
    }
}