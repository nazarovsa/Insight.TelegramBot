using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
	public interface IHostedBot
	{
		void Start();

		void Stop();

		Task ProcessInlineQuery(InlineQuery inlineQuery, CancellationToken cancellationToken = default);

		Task ProcessUpdate(Update message, CancellationToken cancellationToken = default);

		Task ProcessMessage(Message message, CancellationToken cancellationToken = default);

		Task ProcessCallback(CallbackQuery callbackQuery, CancellationToken cancellationToken = default);
	}
}