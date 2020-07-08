using System;
using System.Threading.Tasks;
using Insight.TelegramBot.State;
using Stateless;

namespace Insight.TelegramBot.Testing
{
    public class TestStateMachine : BotStateMachine<TestState>, IStateMachine
    {
        public TestStateMachine(IStateContext<TestState> userContext, IStateContextRepository<TestState> stateRepository,
            Action<StateMachine<TestState, string>> configureDelegate) : base(
            userContext, stateRepository, configureDelegate)
        {
        }

        public async Task Process()
        {
            await StateMachine.FireAsync(nameof(Process));

            await CommitState();
        }

        public async Task Finish()
        {
            await StateMachine.FireAsync(nameof(Finish));

            await CommitState();
        }
    }
}