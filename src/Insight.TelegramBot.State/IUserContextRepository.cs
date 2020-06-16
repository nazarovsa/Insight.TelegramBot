using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.TelegramBot.State
{
    public interface IUserContextRepository<TState> 
        where TState : Enum
    {
        Task<IUserContext<TState>> GetUserState(long telegramId, CancellationToken token = default);

        Task AddUserState(IUserContext<TState> userContext, CancellationToken token = default);

        Task SetUserState(long telegramId, TState botState, CancellationToken token = default);
    }
}