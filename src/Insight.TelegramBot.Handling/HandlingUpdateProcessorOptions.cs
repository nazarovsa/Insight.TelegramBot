namespace Insight.TelegramBot.Handling;

public sealed class HandlingUpdateProcessorOptions
{
    /// <summary>
    /// If true exception inside one of handlers throws exception from <see cref="HandlingUpdateProcessor"/>.
    /// </summary>
    public bool ThrowHandlingFlowExceptions { get; set; } = false;

    public bool ThrowBuildFlowExceptions { get; set; } = false;

    public bool ExecuteHandlersAtSameAsyncContext { get; set; } = false;
}
