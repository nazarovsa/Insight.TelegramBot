using System.Threading.Tasks;

namespace Insight.TelegramBot.Testing
{
    public interface IStateMachine
    {
        Task Process();

        Task Finish();
    }
}