using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Contracts;
using MediatR;

namespace A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;

/// <summary>
///     Контекст для передачи зависимостей
///     в обработчики заявки на обратный звонок
/// </summary>
internal sealed class CallBackRequestStateContext(
    ITelegramResponseService telegramResponseService,
    IMediator mediator,
    IUserSessionStateMachine userSessionStateMachine)
{
    public ITelegramResponseService TelegramResponseService { get; } = telegramResponseService;
    public IMediator Mediator { get; } = mediator;
    public IUserSessionStateMachine UserSessionStateMachine { get; } = userSessionStateMachine;
}