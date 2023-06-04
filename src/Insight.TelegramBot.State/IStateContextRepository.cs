using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.State
{
    public interface IStateContextRepository<TStateContext, TState>
        where TState : Enum
        where TStateContext : IStateContext<TState>
    {
        Task<TStateContext?> Get(long telegramId, CancellationToken cancellationToken = default);

        Task Set(TStateContext stateContext, CancellationToken cancellationToken = default);
        
        Task SetState(long telegramId, TState state, CancellationToken cancellationToken = default);
    }
}