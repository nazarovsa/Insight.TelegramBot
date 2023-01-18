using System;
using System.Collections.Concurrent;
using Insight.TelegramBot.Handling.Matchers;

namespace Insight.TelegramBot.Handling;

internal interface IUpdateHandlersProvider
{
    ConcurrentDictionary<Type, IUpdateMatcher> TypeMap { get; }
}