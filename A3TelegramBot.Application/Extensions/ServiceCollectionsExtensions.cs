using System.Reflection;
using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Services;
using A3TelegramBot.Application.Services.UserState;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState.StateHandlers;
using Ardalis.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.Extensions;

/// <summary>
///     Методы расширения для коллекции сервисов
/// </summary>
public static class ServiceCollectionsExtensions
{
    /// <summary>
    ///     Добавляет application сервисы в ioc контейнер
    /// </summary>
    /// <param name="serviceCollection"> ioc контейнер </param>
    /// <param name="logger"> Логер </param>
    public static void AddApplicationServices(this IServiceCollection serviceCollection, ILogger logger)
    {
        logger.LogInformation("Добавления application сервисов...");

        serviceCollection.AddMediatR(static cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        serviceCollection.AddScoped<ITelegramProcessor, TelegramProcessor>();


        // Регистрация обработчиков состояний
        serviceCollection.AddScoped<ICallbackStateHandler, AwaitingPhoneCallBackStateHandler>();
        serviceCollection.AddScoped<ICallbackStateHandler, AwaitingNameCallBackStateHandler>();
        serviceCollection.AddScoped<ICallbackStateHandler, AwaitingPolicyCallBackStateHandler>();

        serviceCollection.AddScoped<IUserSessionStateMachine, UserSessionStateMachine>();
        serviceCollection.AddScoped<IStateHandlerFactory, StateHandlerFactory>();
        serviceCollection.AddScoped<CallBackRequestStateMachine>();

        // Регистрация стратегий команд
        serviceCollection.AddScoped<ICallbackCommandStrategy, CancelCallBackRequestCommandStrategy>();
        serviceCollection.AddScoped<ICallbackCommandStrategy, YesCommandStrategy>();
        serviceCollection.AddScoped<ICallbackCommandStrategy, StartCallBackRequestCommandStrategy>();

        serviceCollection.AddScoped<GetNearestReceptionStateHandler>();
        serviceCollection.AddScoped<IdleStateHandler>();

        logger.LogInformation("Application сервисы добавлены");
    }
}