using System;

namespace Insight.TelegramBot.State
{
    public class StateContextBase<TState> : IStateContext<TState>
        where TState : Enum
    {
        public StateContextBase(long telegramId, TState currentState)
        {
            TelegramId = telegramId;
            CurrentState = currentState;
        }

        public long TelegramId { get; private set; }

        public TState CurrentState { get; private set; }
    }
}