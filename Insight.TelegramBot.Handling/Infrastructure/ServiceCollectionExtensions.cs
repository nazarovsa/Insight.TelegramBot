using System;
using System.Linq;
using System.Reflection;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.Handling.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register implementations of <see cref="IMatchingUpdateHandler{TMatcher}"/> as transient. And <see cref="HandlingUpdateProcessor"/> as scoped with required services.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="assemblies">Assemblies to register handlers from.</param>
    public static IServiceCollection AddTelegramBotHandling(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (!assemblies.Any())
        {
            throw new ArgumentException("At least one assembly for auto-registrations should be specified.");
        }

        foreach (var assembly in assemblies)
        {
            services.RegisterHandlers(assembly);
        }

        var updateHandlersProvider = new UpdateHandlersProvider(assemblies);
        services.AddSingleton<IUpdateHandlersProvider>(updateHandlersProvider);
        services.AddScoped<IUpdateProcessor, HandlingUpdateProcessor>();

        return services;
    }

    private static void RegisterHandlers(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.ExportedTypes
            .GetMatchingUpdateHandlersImplementations()
            .ToArray();

        foreach (var type in types)
        {
            var implementedGenericMatcherInterfaceType = type.GetInterfaces()
                .Single(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition()
                        .IsAssignableFrom(typeof(IMatchingUpdateHandler<>))
                );

            var serviceDescriptor = new ServiceDescriptor(implementedGenericMatcherInterfaceType, type, ServiceLifetime.Transient);
            services.Add(serviceDescriptor);
        }
    }
}