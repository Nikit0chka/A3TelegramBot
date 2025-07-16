using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Services.UserStateHandlers;
using A3TelegramBot.Application.Services.UserStateHandlers.CallBackRequestState;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;
using Microsoft.Extensions.DependencyInjection;

namespace A3TelegramBot.Application.Services;

internal sealed class StateHandlerFactory(IServiceProvider serviceProvider):IStateHandlerFactory
{
    public IStateHandler GetHandler(UserSessionState state)
    {
        return state switch
        {
            UserSessionState.Idle => serviceProvider.GetRequiredService<IdleStateHandler>(),
            UserSessionState.InFindingNearestReceptions => serviceProvider.GetRequiredService<GetNearestReceptionStateHandler>(),
            UserSessionState.InCallbackRequest => serviceProvider.GetRequiredService<CallBackRequestStateHandler>(),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}