namespace Insight.TelegramBot.Hosting.Options
{
    public sealed class TelegramBotOptions
    {
        public WebHookOptions? WebHook { get; set; }

        public PollingOptions Polling { get; set; } = new();

        public string Token { get; set; }
    }
}