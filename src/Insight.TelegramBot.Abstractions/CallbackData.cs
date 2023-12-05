using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.TelegramBot.Abstractions
{
    public class CallbackData<TState>
        where TState : Enum
    {
        public IReadOnlyCollection<string> Args { get; }

        public TState NextState { get; }

        public CallbackData(TState nextState, params string[] args)
        {
            NextState = nextState;
            Args = args;
        }

        public override string ToString()
        {
            var result = $"{Convert.ToInt32(NextState)}>{string.Join("|", Args)}";

            return Encoding.UTF8.GetBytes(result).Length > 64
                ? throw new ArgumentException("String length should be less than 65 bytes")
                : result;
        }

        public static CallbackData<TState> Parse(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentNullException(nameof(commandText));

            var items = commandText.Split('>');
            var nextState = (TState) Enum.Parse(typeof(TState), items[0]);

            var args = items[^1]
                .Split('|');

            if (args.Length == 0 || args.Length == 1 && string.IsNullOrWhiteSpace(args[0]))
                return new CallbackData<TState>(nextState, Array.Empty<string>());

            return new CallbackData<TState>(nextState, args);
        }
    }
}