using System;
using System.Collections.Generic;
using System.Linq;
using Insight.TelegramBot.Handling.Handlers;

namespace Insight.TelegramBot.Handling.Infrastructure;

internal static class TypeExtensions
{
    internal static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        Type baseType = givenType.BaseType;
        if (baseType == null)
            return false;

        return IsAssignableToGenericType(baseType, genericType);
    }

    internal static IEnumerable<Type> GetMatchingUpdateHandlersImplementations(this IEnumerable<Type> types)
    {
        return types
            .Where(x => x.IsAssignableToGenericType(typeof(IMatchingUpdateHandler<>)) &&
                        !x.IsAbstract &&
                        !x.IsInterface);
    }
}