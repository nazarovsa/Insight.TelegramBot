namespace Insight.TelegramBot.DependencyInjection;

public sealed class TelegramBotOptions
{
    public string Token { get; set; } = null!;

    public string? BaseUrl { get; set; }
}
