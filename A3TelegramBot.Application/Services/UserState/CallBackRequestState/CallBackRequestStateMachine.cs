using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;
using A3TelegramBot.Application.UseCases.RequestCallBack.GetState;
using A3TelegramBot.Application.UseCases.RequestCallBack.HandleName;
using A3TelegramBot.Application.UseCases.RequestCallBack.HandlePhone;
using A3TelegramBot.Application.UseCases.RequestCallBack.Start;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession;
using MediatR;

namespace A3TelegramBot.Application.Services.UserState.CallBackRequestState;

/// <inheritdoc />
/// <summary>
///     Обработчик состояния оформления
///     заявки на обратный звонок
/// </summary>
internal sealed class CallBackRequestStateMachine(
    ITelegramResponseService telegramResponseService,
    IUserSessionStateMachine userSessionStateMachine,
    IMediator mediator,
    IEnumerable<ICallbackStateHandler> stateHandlers,
    IEnumerable<ICallbackCommandStrategy> commandStrategies):BaseStateHandler(telegramResponseService, mediator, userSessionStateMachine)
{
    private readonly static HashSet<TelegramBotCommand> IdleStateCommands =
    [
        TelegramBotCommands.CheckPrice,
        TelegramBotCommands.Services,
        TelegramBotCommands.FindNearestReceptions
    ];

    private readonly List<ICallbackCommandStrategy> _commandStrategies = commandStrategies.ToList();

    private readonly CallBackRequestStateContext _context = new(
                                                                telegramResponseService,
                                                                mediator,
                                                                userSessionStateMachine);

    private readonly Dictionary<CallBackRequestStatus, ICallbackStateHandler> _stateHandlers = stateHandlers.ToDictionary(static callbackStateHandler => callbackStateHandler.Status);


    public override async Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            await _context.TelegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
            return;
        }

        // Обработка специальной команды RequestCallback
        if (command == TelegramBotCommands.RequestCallback)
        {
            await EnterStateAsync(chatId, cancellationToken);
            return;
        }

        // Обработка стратегий команд
        var strategy = _commandStrategies.FirstOrDefault(s => s.CanHandle(command));

        if (strategy != null)
        {
            await strategy.ExecuteAsync(chatId, _context, cancellationToken);
            return;
        }

        // Обработка команд для перехода в Idle состояние
        if (IdleStateCommands.Contains(command))
        {
            await _context.UserSessionStateMachine.TransitionToStateAsync(
                                                                          chatId,
                                                                          UserSessionState.Idle,
                                                                          command,
                                                                          cancellationToken);

            return;
        }

        await _context.TelegramResponseService.SendCommandNotFoundAsync(chatId, null, cancellationToken);
    }

    public override async Task EnterStateAsync(long chatId, CancellationToken cancellationToken)
    {
        var stateResult = await _context.Mediator.SendMediatorRequest(
                                                                      new GetCallBackStateCommand(chatId),
                                                                      chatId,
                                                                      _context.TelegramResponseService,
                                                                      cancellationToken);

        if (stateResult.IsError) return;

        await HandleStateEntry(chatId, stateResult.Value, cancellationToken);
    }

    public override async Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        var stateResult = await _context.Mediator.SendMediatorRequest(
                                                                      new GetCallBackStateCommand(chatId),
                                                                      chatId,
                                                                      _context.TelegramResponseService,
                                                                      cancellationToken);

        if (stateResult.IsError) return;

        if (stateResult.Value != null &&
            _stateHandlers.TryGetValue(stateResult.Value.Value, out var handler))
            await handler.HandleMessageAsync(chatId, message, _context, cancellationToken);
        else
        {
            await _context.TelegramResponseService.SendErrorAsync(
                                                                  chatId,
                                                                  "Невозможно обработать ввод в текущем состоянии",
                                                                  cancellationToken);
        }
    }

    public override async Task HandleContactAsync(long chatId, string phone, string name, CancellationToken cancellationToken)
    {
        var phoneResult = await _context.Mediator.SendMediatorRequest(
                                                                      new HandlePhoneCommand(chatId, phone),
                                                                      chatId,
                                                                      _context.TelegramResponseService,
                                                                      cancellationToken);

        if (phoneResult.IsError) return;

        var nameResult = await _context.Mediator.SendMediatorRequest(
                                                                     new HandleNameCommand(chatId, name),
                                                                     chatId,
                                                                     _context.TelegramResponseService,
                                                                     cancellationToken);

        if (!nameResult.IsError)
            await _context.TelegramResponseService.SendRequestPersonalDataProcessingPolicyAsync(chatId, cancellationToken);
    }

    public override Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken) => _context.TelegramResponseService.SendErrorAsync(chatId, "Команда не найдена", cancellationToken);

    private async Task HandleStateEntry(long chatId, CallBackRequestStatus? status, CancellationToken cancellationToken)
    {
        if (status == null)
        {
            await StartNewCallbackRequest(chatId, cancellationToken);
            return;
        }

        switch (status)
        {
            case CallBackRequestStatus.Completed:
                await _context.TelegramResponseService.SendCallbackRequestAlreadyCreatedAsync(chatId, cancellationToken);
                break;

            default:
                if (_stateHandlers.TryGetValue(status.Value, out var handler))
                    await handler.EnterAsync(chatId, _context, cancellationToken);

                break;
        }
    }

    private async Task StartNewCallbackRequest(long chatId, CancellationToken cancellationToken)
    {
        var result = await _context.Mediator.SendMediatorRequest(
                                                                 new StartCallBackRequestCommand(chatId),
                                                                 chatId,
                                                                 _context.TelegramResponseService,
                                                                 cancellationToken);

        if (!result.IsError)
            await _context.TelegramResponseService.SendRequestPhoneAsync(chatId, cancellationToken);
    }
}