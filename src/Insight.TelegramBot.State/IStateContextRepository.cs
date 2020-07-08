using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.State
{
    public interface IStateContextRepository<TState> 
        where TState : Enum
    {
        Task<IStateContext<TState>> Get(long telegramId, CancellationToken cancellationToken = default);

        Task Set(IStateContext<TState> stateContext, CancellationToken cancellationToken = default);
    }
}