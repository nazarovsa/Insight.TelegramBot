using System;
using System.Linq;
using System.Reflection;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Handling.Matchers;
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
            services.RegisterMatchingHandlers(assembly);
            services.RegisterContextMatchingHandlers(assembly);
        }

        var updateHandlersProvider = new UpdateHandlersProvider(assemblies);
        services.AddSingleton<IUpdateHandlersProvider>(updateHandlersProvider);
        services.AddScoped<IUpdateProcessor, HandlingUpdateProcessor>();

        services.AddOptions();
        
        return services;
    }

    private static void RegisterMatchingHandlers(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.ExportedTypes
            .GetMatchingUpdateHandlersImplementations()
            .ToArray();

        foreach (var type in types)
        {
            var implementedGenericMatcherInterfaceTypes = type.GetInterfaces()
                .Where(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition()
                        .IsAssignableFrom(typeof(IMatchingUpdateHandler<>))
                ).ToArray();

            if (implementedGenericMatcherInterfaceTypes.Length > 1)
            {
                throw new InvalidOperationException(
                    $"{type.Name} implements more than one {typeof(IMatchingUpdateHandler<>).Name} interfaces.");
            }

            var implementedGenericMatcherInterfaceType = implementedGenericMatcherInterfaceTypes.Single();
            var serviceDescriptor = new ServiceDescriptor(implementedGenericMatcherInterfaceType,
                type,
                ServiceLifetime.Transient);
            services.Add(serviceDescriptor);
        }
    } 
    
    private static void RegisterContextMatchingHandlers(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.ExportedTypes
            .GetContextMatchingUpdateHandlersImplementations()
            .ToArray();

        foreach (var type in types)
        {
            var implementedGenericMatcherInterfaceTypes = type.GetInterfaces()
                .Where(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition()
                        .IsAssignableFrom(typeof(IContextMatchingUpdateHandler<>))
                ).ToArray();

            if (implementedGenericMatcherInterfaceTypes.Length > 1)
            {
                throw new InvalidOperationException(
                    $"{type.Name} implements more than one {typeof(IContextMatchingUpdateHandler<>).Name} interfaces.");
            }

            var implementedGenericMatcherInterfaceType = implementedGenericMatcherInterfaceTypes.Single();
            var handlerServiceDescriptor = new ServiceDescriptor(implementedGenericMatcherInterfaceType,
                type,
                ServiceLifetime.Transient);
            
            services.Add(handlerServiceDescriptor);

            var matcherType = implementedGenericMatcherInterfaceType.GenericTypeArguments[0];
            var matcherServiceDescriptor = new ServiceDescriptor(typeof(IContextUpdateMatcher), matcherType, ServiceLifetime.Scoped);
            services.Add(matcherServiceDescriptor);
        }
    }
}