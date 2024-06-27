using System;
using Telegram.Bot.Polling;

namespace Insight.TelegramBot.Polling;

public sealed class PollingOptions
{
    public ReceiverOptions? ReceiverOptions { get; set; }
    
    public TimeSpan PollingTaskCheckInterval { get; set; } = TimeSpan.FromMinutes(1);
        
    public TimeSpan PollingTaskExceptionDelay { get; set; } = TimeSpan.FromMinutes(5);
}