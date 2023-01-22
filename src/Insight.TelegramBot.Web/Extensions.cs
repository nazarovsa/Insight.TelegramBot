using System;
using System.Reflection;
using Insight.TelegramBot.Configurations;
using Insight.TelegramBot.Web.Hosts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Insight.TelegramBot.Web;

public static class Extensions
{
    /// <summary>
    /// Registers <see cref="UpdateController"/> as a service
    /// </summary>
    /// <param name="builder"><see cref="IMvcBuilder"/></param>
    /// <returns></returns>
    public static IMvcBuilder AddUpdateController(this IMvcBuilder builder)
    {
        var assembly = Assembly.Load(typeof(UpdateController).Assembly.GetName());
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        builder.AddApplicationPart(assembly).AddControllersAsServices();

        return builder;
    }

    /// <summary>
    /// Adds route to resolve <see cref="UpdateController"/>
    /// </summary>
    /// <param name="builder"><see cref="IMvcBuilder"/></param>
    /// <param name="route">Custom route</param>
    /// <remarks>
    /// Telegram recommends to use a token as a part of route for security reasons
    /// </remarks>
    /// <returns></returns>
    public static IEndpointRouteBuilder AddUpdateControllerRoute(this IEndpointRouteBuilder builder, string route)
    {
        if (string.IsNullOrWhiteSpace(route))
            throw new ArgumentNullException(nameof(route));

        builder.MapControllerRoute("update", route,
            new { Controller = "Update", Action = "Post" });

        return builder;
    }

    /// <summary>
    /// Add polling bot.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="receiverOptions"><see cref="ReceiverOptions"/>.</param>
    public static IServiceCollection AddPollingBotHost(this IServiceCollection services, ReceiverOptions? receiverOptions = null)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddHostedService(ctx =>
            new TelegramBotPollingWebHost(ctx.GetRequiredService<IServiceProvider>(),
                ctx.GetRequiredService<IOptions<BotConfiguration>>(),
                ctx.GetRequiredService<ITelegramBotClient>(),
                receiverOptions));

        return services;
    }

    /// <summary>
    /// Add webhook bot.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static IServiceCollection AddWebHookBotHost(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddHostedService(ctx =>
            new TelegramBotWebHookHost(ctx.GetRequiredService<ITelegramBotClient>(),
                ctx.GetRequiredService<IOptions<BotConfiguration>>().Value));

        return services;
    }
}