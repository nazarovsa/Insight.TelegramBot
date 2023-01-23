namespace Insight.TelegramBot.Handling;

public sealed class HandlingUpdateProcessorOptions
{
    /// <summary>
    /// If true exception inside one of handlers throws exception from <see cref="HandlingUpdateProcessor"/>.
    /// </summary>
    public bool ThrowFlowExceptions { get; set; } = false;
}
