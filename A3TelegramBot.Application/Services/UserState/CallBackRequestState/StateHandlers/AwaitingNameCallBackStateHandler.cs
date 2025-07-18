using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;
using A3TelegramBot.Application.UseCases.RequestCallBack.HandleName;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

namespace A3TelegramBot.Application.Services.UserState.CallBackRequestState.StateHandlers;

/// <inheritdoc />
/// <summary>
///     Обработчик состояния "Ожидания ввода имени"
///     при оформлении заявки на обратный звонок
/// </summary>
internal sealed class AwaitingNameCallBackStateHandler:ICallbackStateHandler
{
    public CallBackRequestStatus Status => CallBackRequestStatus.AwaitingName;

    public Task EnterAsync(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken) => context.TelegramResponseService.SendRequestNameAsync(chatId, cancellationToken);

    public async Task HandleMessageAsync(long chatId, string message, CallBackRequestStateContext context, CancellationToken cancellationToken)
    {
        var result = await context.Mediator.SendMediatorRequest(
                                                                new HandleNameCommand(chatId, message),
                                                                chatId,
                                                                context.TelegramResponseService,
                                                                cancellationToken);

        if (!result.IsError)
            await context.TelegramResponseService.SendRequestPersonalDataProcessingPolicyAsync(chatId, cancellationToken);
    }
}