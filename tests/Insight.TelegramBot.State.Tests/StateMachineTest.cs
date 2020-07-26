using System;
using System.Threading.Tasks;
using Insight.TelegramBot.Testing;
using Stateless;
using Xunit;

namespace Insight.TelegramBot.State.Tests
{
    public class StateMachineTest
    {
        [Fact]
        public async Task Should_change_states()
        {
            IStateContext<TestState> userContext = new StateContextBase<TestState>(default, TestState.None);
            var repository = new InMemoryStateRepository();
            await repository.Set(userContext);
            Assert.NotEqual(0, repository.Count);

            var stateMachine = new TestStateMachine(userContext, repository, ConfigureStateMachine);

            await stateMachine.Process();

            userContext = await repository.Get(default);
            Assert.Equal(TestState.Pending, userContext.CurrentState);

            stateMachine = new TestStateMachine(userContext, repository, ConfigureStateMachine);
            await stateMachine.Finish();

            userContext = await repository.Get(default);
            Assert.Equal(TestState.Done, userContext.CurrentState);
        }

        [Fact]
        public async Task Should_throw_IOE_on_wrong_transition()
        {
            var userContext = new StateContextBase<TestState>(default, TestState.None);
            var repository = new InMemoryStateRepository();
            await repository.Set(userContext);
            Assert.NotEqual(0, repository.Count);

            var stateMachine = new TestStateMachine(userContext, repository, ConfigureStateMachine);

            await Assert.ThrowsAsync<InvalidOperationException>(() => stateMachine.Finish());
        }

        [Fact]
        public void Should_throw_ANE_on_null_delegate()
        {
            var userContext = new StateContextBase<TestState>(default, TestState.None);
            var repository = new InMemoryStateRepository();
            var stateMachine = new TestStateMachine(userContext, repository);
            Assert.Throws<ArgumentNullException>(() => stateMachine.Configure(null));
        }

        [Fact]
        public void Should_throw_ANE_on_null_context()
        {
            var repository = new InMemoryStateRepository();
            Assert.Throws<ArgumentNullException>(() => new TestStateMachine(null, repository));
        }

        [Fact]
        public void Should_throw_ANE_on_null_repository()
        {
            var userContext = new StateContextBase<TestState>(default, TestState.None);
            Assert.Throws<ArgumentNullException>(() => new TestStateMachine(userContext, null));
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