using System;
using System.Threading.Tasks;
using Stateless;

namespace Insight.TelegramBot.State
{
    public abstract class BotStateMachine<TState>
        where TState : Enum
    {
        protected IStateContextRepository<TState> StateRepository { get; }

        protected StateMachine<TState, string> StateMachine { get; }

        public TState State { get; private set; }

        public long TelegramId { get; private set; }

        protected BotStateMachine(IStateContext<TState> stateContext,
            IStateContextRepository<TState> stateRepository)
        {
            if (stateContext == null)
                throw new ArgumentNullException(nameof(stateContext));

            if (stateRepository == null)
                throw new ArgumentNullException(nameof(stateRepository));

            StateRepository = stateRepository;

            State = stateContext.CurrentState;
            TelegramId = stateContext.TelegramId;

            StateMachine = new StateMachine<TState, string>(
                () => State,
                s => State = s, FiringMode.Immediate);
        }

        protected BotStateMachine(IStateContext<TState> stateContext,
            IStateContextRepository<TState> stateRepository,
            Action<StateMachine<TState, string>> configureDelegate)
            : this(stateContext, stateRepository)
        {
            Configure(configureDelegate);
        }

        protected virtual async Task CommitState()
        {
            await StateRepository.Set(new StateContextBase<TState>(TelegramId, State));
        }

        public void Configure(Action<StateMachine<TState, string>> @delegate)
        {
            if (StateMachine == null)
                throw new ArgumentNullException(nameof(StateMachine));

            if (@delegate == null)
                throw new ArgumentNullException(nameof(@delegate));

            @delegate(StateMachine);
        }
    }
}