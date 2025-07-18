using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

namespace A3TelegramBot.Application.Services.UserState.CallBackRequestState.StateHandlers;

/// <inheritdoc />
/// <summary>
///     Обработчик состояния "Ожидания принятия политики обработки персональных данных"
///     при оформлении заявки на обратный звонок
/// </summary>
internal sealed class AwaitingPolicyCallBackStateHandler:ICallbackStateHandler
{
    public CallBackRequestStatus Status => CallBackRequestStatus.AwaitingPersonalDataProcessingPolicyAgreement;

    public Task EnterAsync(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken) => context.TelegramResponseService.SendRequestPersonalDataProcessingPolicyAsync(chatId, cancellationToken);

    public Task HandleMessageAsync(long chatId, string message, CallBackRequestStateContext context, CancellationToken cancellationToken) => Task.CompletedTask;
}