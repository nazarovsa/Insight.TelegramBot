namespace Insight.TelegramBot.Testing
{
    public enum TestState
    {
        None = 0,
        Pending = 1000,
        Done = 200_000
    }

    public enum TestAction
    {
        None = 0,
        SendPreview = 1000,
        SendNotification = 200_000
    }
}