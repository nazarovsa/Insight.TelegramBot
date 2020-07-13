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
			IStateContext<TestState> userContext = new StateContextBase<TestState>(default, TestState.None);
			var repository = new InMemoryStateRepository();
			await repository.Set(userContext);
			Assert.NotEqual(0, repository.Count);

			var stateMachine = new TestStateMachine(userContext, repository, ConfigureStateMachine);

			await Assert.ThrowsAsync<InvalidOperationException>(() => stateMachine.Finish());
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