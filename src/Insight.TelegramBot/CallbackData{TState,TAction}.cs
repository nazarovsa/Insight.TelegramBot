using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.TelegramBot;

public sealed class CallbackData<TState, TAction>
    where TState : struct, Enum
    where TAction : struct, Enum
{
    public const char StateAndActionSeparator = ':';

    public const char ArgsDelimiter = '>';

    public const char ArgSeparator = '|';

    private readonly string[] _args;

    public IReadOnlyCollection<string> Args => _args;

    public TState? State { get; }

    public TAction? Action { get; }

    public CallbackData(TAction action, params string[] args)
    {
        Action = action;
        _args = args;
    }

    public CallbackData(TState state, params string[] args)
    {
        State = state;
        _args = args;
    }

    public override string ToString()
    {
        var stateAndActionMaxLength = 10 + 10 + 3 + (_args.Length - 1);
        var argsLength = 0;
        foreach (var arg in _args)
        {
            argsLength += arg.Length;
        }

        Span<char> span = stackalloc char[stateAndActionMaxLength + argsLength];
        var currentLength = 0;

        Span<char> statePart = stackalloc char[stateAndActionMaxLength];
        if (State.HasValue)
        {
            WriteIntToCharSpan(statePart, Convert.ToInt32(State), out var length);
            statePart[length] = StateAndActionSeparator;
            statePart[++length] = ArgsDelimiter;
            currentLength = length + 1;
        }
        else if (Action.HasValue)
        {
            statePart[0] = StateAndActionSeparator;
            WriteIntToCharSpan(statePart[1..], Convert.ToInt32(Action), out var length);
            statePart[++length] = ArgsDelimiter;
            currentLength = length + 1;
        }
        else
        {
            throw new InvalidOperationException("State or Action should be specified");
        }

        statePart.CopyTo(span);

        for (var i = 0; i < _args.Length; i++)
        {
            var arg = _args[i];
            arg.AsSpan().CopyTo(span[currentLength..]);
            currentLength += arg.Length;

            if (i != _args.Length - 1)
            {
                span[currentLength] = ArgSeparator;
                currentLength++;
            }
        }

        var result = span[..currentLength].ToString();

        return Encoding.UTF8.GetByteCount(result) > 64
            ? throw new ArgumentException("Callback data length should be less than 65 bytes")
            : result;
    }

    public static CallbackData<TState, TAction> Parse(string commandText)
    {
        if (string.IsNullOrWhiteSpace(commandText))
            throw new ArgumentNullException(nameof(commandText));

        var stateAndActionSeparatorIndex = commandText.IndexOf(StateAndActionSeparator);
        var argsDelimiterIndex = commandText.IndexOf(ArgsDelimiter);

        var args = commandText.AsSpan()
            .Slice(argsDelimiterIndex + 1)
            .ToString()
            .Split(ArgSeparator);

        if (stateAndActionSeparatorIndex == 0)
        {
#if NET7_0
            var slice = commandText.AsSpan().Slice(1,
                argsDelimiterIndex - 1);
            var action = (TAction)Enum.Parse(typeof(TAction), slice);
#else
            var actionString = commandText.Substring(1, argsDelimiterIndex - 1);
            var action = (TAction)Enum.Parse(typeof(TAction), actionString);
#endif
            return new CallbackData<TState, TAction>(action, args);
        }
        else
        {
#if NET7_0
            var slice = commandText.AsSpan()[..(argsDelimiterIndex - 1)];
            var state = (TState)Enum.Parse(typeof(TState), slice);
#else
            var stateString = commandText.Substring(0, argsDelimiterIndex - 1);
            var state = (TState)Enum.Parse(typeof(TState), stateString);
#endif
            return new CallbackData<TState, TAction>(state, args);
        }
    }

    private void WriteIntToCharSpan(Span<char> span, int number, out int length)
    {
        var digitsCount = (int)Math.Log10(number) + 1;
        var currentPosition = digitsCount - 1;
        while (number != 0)
        {
            var digit = number % 10;
            span[currentPosition--] = (char)(48 + digit);
            number /= 10;
        }

        length = digitsCount;
    }
}