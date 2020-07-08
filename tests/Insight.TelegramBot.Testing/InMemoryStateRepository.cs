using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.State;

namespace Insight.TelegramBot.Testing
{
    public sealed class InMemoryStateRepository : IStateContextRepository<TestState>
    {
        public int Count => _items.Count;

        private readonly Dictionary<long, IStateContext<TestState>> _items =
            new Dictionary<long, IStateContext<TestState>>();

        public Task<IStateContext<TestState>> Get(long telegramId, CancellationToken cancellationToken = default)
        {
            if (_items.TryGetValue(telegramId, out var userContext))
                return Task.FromResult(userContext);

            return Task.FromResult<IStateContext<TestState>>(null);
        }

        public Task Set(IStateContext<TestState> stateContext, CancellationToken cancellationToken = default)
        {
            var context = new StateContextBase<TestState>(stateContext.TelegramId, stateContext.CurrentState);
            if (_items.ContainsKey(stateContext.TelegramId))
            {
                _items[stateContext.TelegramId] = context;
            }
            else
            {
                _items.Add(stateContext.TelegramId, stateContext);
            }

            return Task.CompletedTask;
        }
    }
}