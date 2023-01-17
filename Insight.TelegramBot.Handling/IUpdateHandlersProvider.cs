using System;
using System.Collections.Generic;
using Insight.TelegramBot.Handling.Matchers;

namespace Insight.TelegramBot.Handling;

internal interface IUpdateHandlersProvider
{
    IEnumerable<KeyValuePair<Type, IUpdateMatcher>> GetTypeMap();
}