using Insight.TelegramBot.Testing;
using Xunit;

namespace Insight.TelegramBot.Tests
{
    public sealed class CallbackDataTests
    {
        [Fact]
        public void Should_parse_callback_data()
        {
            var callbackData = new CallbackData<TestState>(TestState.Pending, "arg1", "arg2");
            Assert.NotNull(callbackData);
            Assert.Equal(callbackData.NextState, TestState.Pending);
            Assert.NotEmpty(callbackData.Args);

            var callbackDataStr = callbackData.ToString();
            Assert.Equal(callbackDataStr, "1>arg1|arg2");

            var parsed = CallbackData<TestState>.Parse(callbackDataStr);
            Assert.NotNull(parsed);
            Assert.Equal(parsed.NextState, TestState.Pending);
            Assert.NotEmpty(callbackData.Args);
            Assert.Contains("arg1", parsed.Args);
            Assert.Contains("arg2", parsed.Args);
        }
    }
}