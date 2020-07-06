using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace Insight.TelegramBot.Web
{
    public static class Extensions
    {
        public static IMvcBuilder AddUpdateController(this IMvcBuilder builder)
        {
            var assembly = Assembly.Load(typeof(UpdateController).Assembly.GetName());
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            builder.AddApplicationPart(assembly).AddControllersAsServices();

            return builder;
        }

        public static IEndpointRouteBuilder AddUpdateControllerRoute(this IEndpointRouteBuilder builder, string route)
        {
            if (string.IsNullOrWhiteSpace(route))
                throw new ArgumentNullException(nameof(route));

            builder.MapControllerRoute("update", route,
                new {Controller = "Update", Action = "Post"});

            return builder;
        }
    }
}