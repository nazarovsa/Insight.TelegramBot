using System;
using Insight.TelegramBot.Abstractions;
using Insight.TelegramBot.Testing;
using Xunit;

namespace Insight.TelegramBot.Tests
{
    public sealed class CallbackDataTests
    {
        [Fact]
        public void Should_parse_callback_data_with_args()
        {
            var callbackData = new CallbackData<TestState>(TestState.Pending, "arg1", "arg2");
            Assert.NotNull(callbackData);
            Assert.Equal(TestState.Pending, callbackData.NextState);
            Assert.NotEmpty(callbackData.Args);

            var callbackDataStr = callbackData.ToString();
            Assert.Equal("1>arg1|arg2", callbackDataStr);

            var parsed = CallbackData<TestState>.Parse(callbackDataStr);
            Assert.NotNull(parsed);
            Assert.Equal(TestState.Pending, parsed.NextState);
            Assert.NotEmpty(callbackData.Args);
            Assert.Contains("arg1", parsed.Args);
            Assert.Contains("arg2", parsed.Args);
        }

        [Fact]
        public void Should_parse_callback_data_without_args()
        {
            var callbackData = new CallbackData<TestState>(TestState.Pending);
            Assert.NotNull(callbackData);
            Assert.Equal(TestState.Pending, callbackData.NextState);
            Assert.Empty(callbackData.Args);

            var callbackDataStr = callbackData.ToString();
            Assert.Equal("1>", callbackDataStr);

            var parsed = CallbackData<TestState>.Parse(callbackDataStr);
            Assert.NotNull(parsed);
            Assert.Equal(TestState.Pending, parsed.NextState);
            Assert.Empty(parsed.Args);
        }

        [Fact]
        public void Should_throw_ANE_when_parse_empty_string()
        {
            Assert.Throws<ArgumentNullException>(() => CallbackData<TestState>.Parse(null));
            Assert.Throws<ArgumentNullException>(() => CallbackData<TestState>.Parse(string.Empty));
        }
    }
}