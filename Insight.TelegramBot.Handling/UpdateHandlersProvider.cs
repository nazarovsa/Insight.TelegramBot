using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Handling.Infrastructure;
using Insight.TelegramBot.Handling.Matchers;

namespace Insight.TelegramBot.Handling;

internal sealed class UpdateHandlersProvider : IUpdateHandlersProvider
{
    private Dictionary<Type, IUpdateMatcher> _typeMap = new();

    public UpdateHandlersProvider(params Assembly[] assemblies)
    {
        Initialize(assemblies);
    }

    private void Initialize(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            Initialize(assembly);
    }

    /// <summary>
    /// Adds one handler for one <see cref="IMatchingUpdateHandler{TMatcher}"/> for single matcher type.
    /// </summary>
    private void Initialize(Assembly assembly)
    {
        var types = assembly.ExportedTypes
            .GetMatchingUpdateHandlersImplementations()
            .ToArray();

        foreach (var type in types)
        {
            if (!_typeMap.ContainsKey(type))
            {
                var propertyInfo = type.BaseType
                    .GetProperty("Matcher", BindingFlags.Public | BindingFlags.Static);
                if (propertyInfo == null)
                {
                    throw new InvalidOperationException(
                        $"Failed to extract matcher property info for handler: {type.Name}. Check that your handler inherits from {typeof(MatchingUpdateHandler<>).Name} not from {typeof(IMatchingUpdateHandler<>)}.");
                }
                
                var matcher = propertyInfo.GetValue(null, null) as IUpdateMatcher;
                if (matcher == null)
                {
                    throw new InvalidOperationException($"Failed to extract matcher for handler: {type.Name}");
                }

                var implementedGenericMatcherInterfaceType = type.GetInterfaces()
                    .Single(x =>
                        x.IsGenericType &&
                        x.GetGenericTypeDefinition()
                            .IsAssignableFrom(typeof(IMatchingUpdateHandler<>))
                    );

                _typeMap.Add(implementedGenericMatcherInterfaceType, matcher);
            }
        }
    }

    public IEnumerable<KeyValuePair<Type, IUpdateMatcher>> GetTypeMap()
    {
        foreach (var kv in _typeMap)
        {
            yield return kv;
        }
    }
}