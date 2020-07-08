using System;

namespace Insight.TelegramBot.State
{
    public interface IStateContext<out TState>
        where TState : Enum
    {
        long TelegramId { get; }

        TState CurrentState { get; }
    }
}