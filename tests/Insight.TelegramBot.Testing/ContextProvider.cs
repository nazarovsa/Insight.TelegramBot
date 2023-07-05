using System.Threading;

namespace Insight.TelegramBot.Testing;

public static class ContextProvider<T>
{
    public static void SetContext(T context)
    {
        _context.Value = context;
    }

    public static T? GetContext()
    {
        return _context.Value;
    }

    private static AsyncLocal<T> _context = new AsyncLocal<T?>();
}
