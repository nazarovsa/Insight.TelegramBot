using System.Threading.Tasks;
using Insight.TelegramBot.State.Tests.Infrastructure;
using Stateless;
using Xunit;

namespace Insight.TelegramBot.State.Tests
{
    public class StateMachineTest
    {
        [Fact]
        public async Task Should_change_states()
        {
            IUserContext<TestState> userContext = new UserContextBase<TestState>(default, TestState.None);
            var repository = new InMemoryStateRepository();
            await repository.AddUserState(userContext);
            Assert.NotEqual(repository.Count, 0);

            var stateMachine = new TestStateMachine(userContext, repository, ConfigureStateMachine);

            await stateMachine.Process();

            userContext = await repository.GetUserState(default);
            Assert.Equal(userContext.CurrentState, TestState.Pending);

            stateMachine = new TestStateMachine(userContext, repository, ConfigureStateMachine);
            await stateMachine.Finish();

            userContext = await repository.GetUserState(default);
            Assert.Equal(userContext.CurrentState, TestState.Done);
        }

        private void ConfigureStateMachine(StateMachine<TestState, string> stateMachine)
        {
            stateMachine.Configure(TestState.None)
                .PermitIf(nameof(TestStateMachine.Process), TestState.Pending);

            stateMachine.Configure(TestState.Pending)
                .PermitIf(nameof(TestStateMachine.Finish), TestState.Done);
        }
    }
}