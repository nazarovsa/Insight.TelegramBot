using System;

namespace Insight.TelegramBot.State
{
    public interface IUserContext<out TState>
        where TState : Enum
    {
        long TelegramId { get; }

        TState CurrentState { get; }
    }
}