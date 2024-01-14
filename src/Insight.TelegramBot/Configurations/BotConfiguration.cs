using System;

namespace Insight.TelegramBot.Configurations
{
    public sealed class BotConfiguration
    {
        public WebHookConfiguration? WebHookConfiguration { get; set; }

        public TimeSpan PollingTaskCheckInterval { get; set; } = TimeSpan.FromMinutes(1);
        
        public string Token { get; set; }
    }
}