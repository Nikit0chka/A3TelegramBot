using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession;
using Microsoft.Extensions.DependencyInjection;

namespace A3TelegramBot.Application.Services.UserState;

internal sealed class StateHandlerFactory(IServiceProvider serviceProvider):IStateHandlerFactory
{
    public IStateHandler GetHandler(UserSessionState state)
    {
        return state switch
        {
            UserSessionState.Idle => serviceProvider.GetRequiredService<IdleStateHandler>(),
            UserSessionState.InFindingNearestReceptions => serviceProvider.GetRequiredService<GetNearestReceptionStateHandler>(),
            UserSessionState.InCallbackRequest => serviceProvider.GetRequiredService<CallBackRequestStateMachine>(),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}