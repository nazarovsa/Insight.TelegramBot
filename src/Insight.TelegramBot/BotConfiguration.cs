namespace Insight.TelegramBot
{
    public sealed class BotConfiguration
    {
        public bool UseWebHook { get; set; }

        public string WebHookUrl { get; set; }

        public string Token { get; set; }
    }
}