using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.TelegramBot.WebHook.Controllers;

public static class WebHookControllersExtensions
{
    /// <summary>
    /// Adds route to resolve <see cref="DefaultUpdateController"/>
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
            new { Controller = "DefaultUpdate", Action = "Post" });

        return builder;
    }
}