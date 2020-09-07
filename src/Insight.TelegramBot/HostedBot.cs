using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Insight.TelegramBot
{
    public abstract class HostedBot : Bot, IHostedBot
    {
        protected BotConfiguration Config { get; }

        protected HostedBot(BotConfiguration config, ITelegramBotClient client)
            : base(client)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public virtual void Start()
        {
            if (Config.WebHookConfiguration.UseWebHook)
                throw new InvalidOperationException("For webhook bots use TelegramBotWithHookWebHost");

            Client.OnMessage += async (s, e) => await ProcessMessage(e.Message);
            Client.OnCallbackQuery += async (s, e) => await ProcessCallback(e.CallbackQuery);
            Client.OnUpdate += async (s, e) => await ProcessUpdate(e.Update);
            Client.OnInlineQuery += async (s, e) => await ProcessInlineQuery(e.InlineQuery);
            Client.OnReceiveError += async (s, e) => await ProcessReceiveError(e.ApiRequestException);
            Client.StartReceiving();
        }

        public virtual void Stop()
        {
            Client.StopReceiving();
        }

        public abstract Task ProcessInlineQuery(InlineQuery inlineQuery, CancellationToken cancellationToken = default);

        public abstract Task ProcessUpdate(Update message, CancellationToken cancellationToken = default);

        public abstract Task ProcessMessage(Message message, CancellationToken cancellationToken = default);

        public abstract Task ProcessCallback(CallbackQuery callbackQuery,
            CancellationToken cancellationToken = default);

        public abstract Task ProcessReceiveError(ApiRequestException exception,
            CancellationToken cancellationToken = default);
    }
}