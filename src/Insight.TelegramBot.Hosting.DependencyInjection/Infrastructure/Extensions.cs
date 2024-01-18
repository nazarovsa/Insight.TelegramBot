using System;
using Insight.TelegramBot.Hosting.DependencyInjection.Builders;
using Insight.TelegramBot.Hosting.WebHook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.Hosting.DependencyInjection.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
        Action<TelegramBotBuilder> builderConfiguration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var builder = new TelegramBotBuilder(services);
        builderConfiguration(builder);
        return services;
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
}