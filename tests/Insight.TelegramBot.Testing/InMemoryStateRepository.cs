using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.State;

namespace Insight.TelegramBot.Testing
{
    public sealed class InMemoryStateRepository : IStateContextRepository<StateContextBase<TestState>, TestState>
    {
        public int Count => _items.Count;

        private readonly Dictionary<long, StateContextBase<TestState>> _items = new();


        public Task Set(StateContextBase<TestState> stateContext, CancellationToken cancellationToken = default)
        {
            var context = new StateContextBase<TestState>(stateContext.TelegramId, stateContext.CurrentState);
            if (_items.ContainsKey(stateContext.TelegramId))
            {
                _items[stateContext.TelegramId] = context;
            }
            else
            {
                _items.TryAdd(stateContext.TelegramId, stateContext);
            }

            return Task.CompletedTask;
        }

        public Task SetState(long telegramId, TestState state, CancellationToken cancellationToken = default)
        {
            var newContext = new StateContextBase<TestState>(telegramId, state);
            _items[telegramId] = newContext;
            return Task.CompletedTask;
        }

        public Task<StateContextBase<TestState>> Get(long telegramId, CancellationToken cancellationToken = default)
        {
            if (_items.TryGetValue(telegramId, out var userContext))
                return Task.FromResult(userContext);

            return Task.FromResult<StateContextBase<TestState>>(null);
        }
    }
}