namespace Insight.TelegramBot.Abstractions.Configurations
{
    public sealed class BotConfiguration
    {
        public WebHookConfiguration? WebHookConfiguration { get; set; }
        
        public string Token { get; set; }
    }
}