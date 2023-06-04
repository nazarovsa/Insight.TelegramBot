using System;
using System.Threading.Tasks;
using Stateless;

namespace Insight.TelegramBot.State
{
    public abstract class BotStateMachine<TStateContext, TState>
        where TState : Enum
        where TStateContext : IStateContext<TState>
    {
        protected IStateContextRepository<TStateContext, TState> StateRepository { get; }

        protected StateMachine<TState, string> StateMachine { get; }

        public TState State { get; private set; }

        public long TelegramId { get; private set; }

        protected BotStateMachine(TStateContext stateContext,
            IStateContextRepository<TStateContext, TState> stateRepository)
        {
            if (stateContext == null)
                throw new ArgumentNullException(nameof(stateContext));

            StateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));

            State = stateContext.CurrentState;
            TelegramId = stateContext.TelegramId;

            StateMachine = new StateMachine<TState, string>(
                () => State,
                s => State = s, FiringMode.Immediate);
        }

        protected BotStateMachine(TStateContext stateContext,
            IStateContextRepository<TStateContext, TState> stateRepository,
            Action<StateMachine<TState, string>> configureDelegate)
            : this(stateContext, stateRepository)
        {
            Configure(configureDelegate);
        }

        protected virtual Task CommitState()
        {
            return StateRepository.SetState(TelegramId, State);
        }

        public void Configure(Action<StateMachine<TState, string>> @delegate)
        {
            if (@delegate == null)
                throw new ArgumentNullException(nameof(@delegate));

            @delegate(StateMachine);
        }
    }
}