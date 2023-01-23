using System;
using System.Collections.Generic;
using Insight.TelegramBot.Handling.Matchers;

namespace Insight.TelegramBot.Handling;

internal interface IUpdateHandlersProvider
{
    Dictionary<Type, IUpdateMatcher> TypeMap { get; }
}