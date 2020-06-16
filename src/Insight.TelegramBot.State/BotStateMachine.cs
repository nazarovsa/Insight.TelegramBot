using System;
using System.Threading.Tasks;
using Stateless;

namespace Insight.TelegramBot.State
{
    public abstract class BotStateMachine<TState>
        where TState : Enum
    {
        private readonly IUserContext<TState> _userContext;

        private readonly IUserContextRepository<TState> _stateRepository;

        protected StateMachine<TState, string> StateMachine;

        public TState State { get; private set; }

        protected BotStateMachine(IUserContext<TState> userContext,
            IUserContextRepository<TState> stateRepository)
        {
            if (userContext == null)
                throw new ArgumentNullException(nameof(userContext));

            if (stateRepository == null)
                throw new ArgumentNullException(nameof(stateRepository));

            _userContext = userContext;
            _stateRepository = stateRepository;

            State = _userContext.CurrentState;

            StateMachine = new StateMachine<TState, string>(
                () => State,
                s => State = s, FiringMode.Immediate);
        }

        protected BotStateMachine(IUserContext<TState> userContext,
            IUserContextRepository<TState> stateRepository,
            Action<StateMachine<TState, string>> configureDelegate)
            : this(userContext, stateRepository)
        {
            Configure(configureDelegate);
        }

        protected virtual async Task CommitState()
        {
            await _stateRepository.SetUserState(_userContext.TelegramId, State);
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