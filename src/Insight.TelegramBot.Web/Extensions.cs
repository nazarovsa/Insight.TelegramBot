using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace Insight.TelegramBot.Web
{
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
				new {Controller = "Update", Action = "Post"});

			return builder;
		}

		/// <summary>
		/// Registers <see cref="Bot"/> as <see cref="IBot"/> and T as <see cref="IBotProcessor"/> in service collection scoped
		/// </summary>
		/// <typeparam name="T">Type of bot processor</typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddDefaultBotAndCustomProcessorScoped<T>(this IServiceCollection services)
			where T : IBotProcessor
		{
			services.AddScoped<IBot, Bot>();
			services.AddScoped(typeof(IBotProcessor), typeof(T));

			return services;
		}
	}
}