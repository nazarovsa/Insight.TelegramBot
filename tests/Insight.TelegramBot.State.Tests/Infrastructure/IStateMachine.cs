using System.Threading.Tasks;

namespace Insight.TelegramBot.State.Tests.Infrastructure
{
    public interface IStateMachine
    {
        Task Process();

        Task Finish();
    }
}