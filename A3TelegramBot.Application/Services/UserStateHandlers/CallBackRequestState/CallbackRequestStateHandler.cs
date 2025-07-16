using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.UseCases.RequestCallBack.AcceptPersonalDataProcessingPolicy;
using A3TelegramBot.Application.UseCases.RequestCallBack.Cancel;
using A3TelegramBot.Application.UseCases.RequestCallBack.Continue;
using A3TelegramBot.Application.UseCases.RequestCallBack.GetState;
using A3TelegramBot.Application.UseCases.RequestCallBack.HandleName;
using A3TelegramBot.Application.UseCases.RequestCallBack.HandlePhone;
using A3TelegramBot.Application.UseCases.RequestCallBack.Start;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace A3TelegramBot.Application.Services.UserStateHandlers.CallBackRequestState;

internal sealed class CallBackRequestStateHandler(
    ITelegramResponseService telegramResponseService,
    IUserSessionStateMachine userSessionStateMachine,
    IMediator mediator)
    :IStateHandler
{
    private readonly static HashSet<TelegramBotCommand> IdleStateCommands =
    [
        TelegramBotCommands.CheckPrice,
        TelegramBotCommands.Services,
        TelegramBotCommands.FindNearestReceptions
    ];

    public Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        return command switch
        {
            not null when command == TelegramBotCommands.Cancel => CancelAsync(chatId, cancellationToken),
            not null when command == TelegramBotCommands.RequestCallback => EnterStateAsync(chatId, cancellationToken),
            not null when command == TelegramBotCommands.Yes => ProcessYesCommandAsync(chatId, cancellationToken),
            not null when IdleStateCommands.Contains(command) =>
                userSessionStateMachine.TransitionToStateAsync(chatId, UserSessionState.Idle, command, cancellationToken),
            _ => telegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken)
        };
    }

    public async Task EnterStateAsync(long chatId, CancellationToken cancellationToken)
    {
        var stateResult = await SendMediatorRequest(new GetCallBackStateCommand(chatId), chatId, cancellationToken);
        if (stateResult.IsError) return;

        await HandleStateBasedOnCallbackStatus(chatId, stateResult.Value, cancellationToken);
    }

    public async Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        var stateResult = await SendMediatorRequest(new GetCallBackStateCommand(chatId), chatId, cancellationToken);
        if (stateResult.IsError) return;

        var handler = stateResult.Value switch
        {
            CallBackRequestStatus.AwaitingPhone => (Func<long, string, CancellationToken, Task>) ProcessPhoneInput,
            CallBackRequestStatus.AwaitingName => ProcessNameInput,
            _ => (id, _, ct) => telegramResponseService.SendErrorAsync(
                                                                       id,
                                                                       "Невозможно обработать ввод в текущем состоянии",
                                                                       ct)
        };

        await handler(chatId, message, cancellationToken);
    }

    public async Task HandleContactAsync(long chatId, string phone, string name, CancellationToken cancellationToken)
    {
        var phoneResult = await SendMediatorRequest(new HandlePhoneCommand(chatId, phone), chatId, cancellationToken);
        if (phoneResult.IsError) return;

        var nameResult = await SendMediatorRequest(new HandleNameCommand(chatId, name), chatId, cancellationToken);
        if (nameResult.IsError) return;

        await telegramResponseService.SendRequestPersonalDataProcessingPolicyAsync(chatId, cancellationToken);
    }

    public Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken) => telegramResponseService.SendErrorAsync(chatId, "Команда не найдена", cancellationToken);

    private Task HandleStateBasedOnCallbackStatus(long chatId, CallBackRequestStatus? status, CancellationToken cancellationToken)
    {
        return status switch
        {
            null => StartNewRequest(chatId, cancellationToken),
            CallBackRequestStatus.AwaitingPhone => ContinueAndRequestPhone(chatId, cancellationToken),
            CallBackRequestStatus.AwaitingName => ContinueAndRequestName(chatId, cancellationToken),
            CallBackRequestStatus.AwaitingPersonalDataProcessingPolicyAgreement => ContinueAndRequestPolicyAgreement(chatId, cancellationToken),
            CallBackRequestStatus.Completed => telegramResponseService.SendCallbackRequestAlreadyCreatedAsync(chatId, cancellationToken),
            _ => Task.CompletedTask
        };
    }

    private async Task<TResult> SendMediatorRequest<TResult>(IRequest<TResult> request, long chatId, CancellationToken cancellationToken)
        where TResult : IErrorOr
    {
        var result = await mediator.Send(request, cancellationToken);

        if (!result.IsError)
            return result;

        if (result.Errors != null)
            await SendError(chatId, result.Errors, cancellationToken);

        return result;
    }

    private Task SendError(long chatId, IEnumerable<Error> errors, CancellationToken ct)
    {
        var errorMessage = string.Join("\n", errors.Select(static error => error.Description));
        return telegramResponseService.SendErrorAsync(chatId, errorMessage, ct);
    }

    private async Task ContinueAndRequestPhone(long chatId, CancellationToken cancellationToken)
    {
        await ContinueCallBackRequest(chatId, cancellationToken);
        await telegramResponseService.SendRequestPhoneAsync(chatId, cancellationToken);
    }

    private async Task ContinueAndRequestName(long chatId, CancellationToken cancellationToken)
    {
        await ContinueCallBackRequest(chatId, cancellationToken);
        await telegramResponseService.SendRequestNameAsync(chatId, cancellationToken);
    }

    private async Task ContinueAndRequestPolicyAgreement(long chatId, CancellationToken cancellationToken)
    {
        await ContinueCallBackRequest(chatId, cancellationToken);
        await telegramResponseService.SendRequestPersonalDataProcessingPolicyAsync(chatId, cancellationToken);
    }

    private async Task ContinueCallBackRequest(long chatId, CancellationToken cancellationToken) { await SendMediatorRequest(new ContinueCallBackRequestCommand(chatId), chatId, cancellationToken); }

    private async Task CancelAsync(long chatId, CancellationToken cancellationToken)
    {
        await SendMediatorRequest(new CancelCallbackStateCommand(chatId), chatId, cancellationToken);
        await telegramResponseService.SendCallBackRequestCancelledAsync(chatId, cancellationToken);
    }

    private async Task StartNewRequest(long chatId, CancellationToken cancellationToken)
    {
        await SendMediatorRequest(new StartCallBackRequestCommand(chatId), chatId, cancellationToken);
        await telegramResponseService.SendRequestPhoneAsync(chatId, cancellationToken);
    }

    private async Task ProcessPhoneInput(long chatId, string phone, CancellationToken cancellationToken)
    {
        await SendMediatorRequest(new HandlePhoneCommand(chatId, phone), chatId, cancellationToken);
        await telegramResponseService.SendRequestNameAsync(chatId, cancellationToken);
    }

    private async Task ProcessNameInput(long chatId, string name, CancellationToken cancellationToken)
    {
        await SendMediatorRequest(new HandleNameCommand(chatId, name), chatId, cancellationToken);
        await telegramResponseService.SendRequestPersonalDataProcessingPolicyAsync(chatId, cancellationToken);
    }

    private async Task ProcessYesCommandAsync(long chatId, CancellationToken cancellationToken)
    {
        var result = await SendMediatorRequest(new AcceptPersonalDataProcessingPolicyCommand(chatId), chatId, cancellationToken);
        if (result.IsError) return;

        await telegramResponseService.SendCallbackRequestCompletedAsync(
                                                                        chatId,
                                                                        result.Value.CallBackRequestUserName,
                                                                        result.Value.CallBackRequestUserPhone,
                                                                        cancellationToken);
    }
}