using System.Reflection;
using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Services;
using A3TelegramBot.Application.Services.UserStateHandlers;
using A3TelegramBot.Application.Services.UserStateHandlers.CallBackRequestState;
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

        serviceCollection.AddScoped<IUserSessionStateMachine, UserSessionStateMachine>();
        serviceCollection.AddScoped<ITelegramProcessor, TelegramProcessor>();
        serviceCollection.AddScoped<IStateHandlerFactory, StateHandlerFactory>();
        serviceCollection.AddScoped<ITelegramProcessor, TelegramProcessor>();

        serviceCollection.AddScoped<CallBackRequestStateHandler>();
        serviceCollection.AddScoped<GetNearestReceptionStateHandler>();
        serviceCollection.AddScoped<IdleStateHandler>();

        logger.LogInformation("Application сервисы добавлены");
    }
}