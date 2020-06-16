using System;

namespace Insight.TelegramBot.State
{
    public class UserContextBase<TState> : IUserContext<TState>
        where TState : Enum
    {
        public UserContextBase(long telegramId, TState currentState)
        {
            TelegramId = telegramId;
            CurrentState = currentState;
        }

        public long TelegramId { get; private set; }

        public TState CurrentState { get; private set; }
    }
}