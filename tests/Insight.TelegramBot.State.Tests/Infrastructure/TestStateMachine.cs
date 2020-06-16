using System;
using System.Threading.Tasks;
using Stateless;

namespace Insight.TelegramBot.State.Tests.Infrastructure
{
    public class TestStateMachine : BotStateMachine<TestState>, IStateMachine
    {
        public TestStateMachine(IUserContext<TestState> userContext, IUserContextRepository<TestState> stateRepository,
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