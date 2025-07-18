using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Application.UseCases.RequestCallBack.GetState;
using A3TelegramBot.Application.UseCases.RequestCallBack.Start;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

namespace A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;

/// <inheritdoc />
/// <summary>
///     Стратегия обработки команды начала
///     оформления заявки на обратный звонок
/// </summary>
internal sealed class StartCallBackRequestCommandStrategy:ICallbackCommandStrategy
{
    public bool CanHandle(TelegramBotCommand command) => command == TelegramBotCommands.RequestCallback;

    public async Task ExecuteAsync(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken)
    {
        var stateResult = await context.Mediator.SendMediatorRequest(
                                                                     new GetCallBackStateCommand(chatId),
                                                                     chatId,
                                                                     context.TelegramResponseService,
                                                                     cancellationToken);

        if (stateResult.IsError) return;

        await HandleStateBasedOnCallbackStatus(chatId, stateResult.Value, context, cancellationToken);
    }

    private async Task HandleStateBasedOnCallbackStatus(
        long chatId,
        CallBackRequestStatus? status,
        CallBackRequestStateContext context,
        CancellationToken cancellationToken)
    {
        if (status == null)
        {
            await StartNewRequest(chatId, context, cancellationToken);
            return;
        }

        switch (status)
        {
            case CallBackRequestStatus.AwaitingPhone:
                await context.TelegramResponseService.SendRequestPhoneAsync(chatId, cancellationToken);
                break;
            case CallBackRequestStatus.AwaitingName:
                await context.TelegramResponseService.SendRequestNameAsync(chatId, cancellationToken);
                break;
            case CallBackRequestStatus.AwaitingPersonalDataProcessingPolicyAgreement:
                await context.TelegramResponseService.SendRequestPersonalDataProcessingPolicyAsync(chatId, cancellationToken);
                break;
            case CallBackRequestStatus.Completed:
                await context.TelegramResponseService.SendCallbackRequestAlreadyCreatedAsync(chatId, cancellationToken);
                break;
        }
    }

    private static async Task StartNewRequest(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken)
    {
        var result = await context.Mediator.SendMediatorRequest(
                                                                new StartCallBackRequestCommand(chatId),
                                                                chatId,
                                                                context.TelegramResponseService,
                                                                cancellationToken);

        if (!result.IsError)
            await context.TelegramResponseService.SendRequestPhoneAsync(chatId, cancellationToken);
    }
}