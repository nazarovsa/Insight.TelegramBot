using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.State;

namespace Insight.TelegramBot.Testing
{
    public sealed class InMemoryStateRepository : IUserContextRepository<TestState>
    {
        public int Count => _items.Count;

        private readonly Dictionary<long, IUserContext<TestState>> _items = new Dictionary<long, IUserContext<TestState>>();

        public Task<IUserContext<TestState>> GetUserState(long telegramId, CancellationToken token = default)
        {
            if (_items.TryGetValue(telegramId, out var userContext))
                return Task.FromResult(userContext);

            return Task.FromResult<IUserContext<TestState>>(null);
        }

        public Task AddUserState(IUserContext<TestState> userContext, CancellationToken token = default)
        {
            _items.Add(userContext.TelegramId, userContext);
            return Task.CompletedTask;
        }

        public async Task SetUserState(long telegramId, TestState botTestState, CancellationToken token = default)
        {
            var context = new UserContextBase<TestState>(telegramId, botTestState);
            if (_items.ContainsKey(telegramId))
            {
                _items[telegramId] = context;
            }
            else
            {
                await AddUserState(context, CancellationToken.None);
            }
        }
    }
}