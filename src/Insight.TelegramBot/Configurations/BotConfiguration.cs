using System;

namespace Insight.TelegramBot.Configurations
{
    public sealed class BotConfiguration
    {
        public WebHookConfiguration? WebHookConfiguration { get; set; }

        public TimeSpan PollingTaskCheckInterval { get; set; } = TimeSpan.FromMinutes(1);
        
        public TimeSpan PollingTaskExceptionDelay { get; set; } = TimeSpan.FromMinutes(5);

        public string Token { get; set; }
    }
}