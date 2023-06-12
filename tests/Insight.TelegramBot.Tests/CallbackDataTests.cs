using System;
using Insight.TelegramBot.Testing;
using Xunit;

namespace Insight.TelegramBot.Tests
{
    public sealed class CallbackDataTests
    {
        [Fact]
        public void Should_generate_correct_state_callback_data_with_args()
        {
            // Arrange
            var callbackData = new CallbackData<TestState, TestAction>(TestState.Pending, "arg1", "arg2");
            Assert.NotNull(callbackData);
            Assert.Null(callbackData.Action);
            Assert.Equal(TestState.Pending, callbackData.State);
            Assert.NotEmpty(callbackData.Args);

            // Act
            var callbackDataStr = callbackData.ToString();

            // Assert
            Assert.Equal($"{(int)TestState.Pending}:>arg1|arg2", callbackDataStr);
        }

        [Fact]
        public void Should_parse_state_data_with_args()
        {
            // Arrange
            var callbackData = $"{(int)TestState.Pending}:>arg1|arg2";

            var parsed = CallbackData<TestState, TestAction>.Parse(callbackData);
            Assert.NotNull(parsed);
            Assert.Null(parsed.Action);
            Assert.Equal(TestState.Pending, parsed.State);
            Assert.NotEmpty(parsed.Args);
            Assert.Contains("arg1", parsed.Args);
            Assert.Contains("arg2", parsed.Args);
        }

        [Fact]
        public void Should_parse_state_data_with_one_arg()
        {
            // Arrange
            var callbackData = $"{(int)TestState.Pending}:>arg1";

            var parsed = CallbackData<TestState, TestAction>.Parse(callbackData);
            Assert.NotNull(parsed);
            Assert.Null(parsed.Action);
            Assert.Equal(TestState.Pending, parsed.State);
            Assert.NotEmpty(parsed.Args);
            Assert.Contains("arg1", parsed.Args);
        }

        [Fact]
        public void Should_generate_correct_action_callback_data_with_args()
        {
            // Arrange
            var callbackData = new CallbackData<TestState, TestAction>(TestAction.SendNotification, "arg1", "arg2");
            Assert.NotNull(callbackData);
            Assert.Equal(TestAction.SendNotification, callbackData.Action);
            Assert.NotEmpty(callbackData.Args);

            // Act
            var callbackDataStr = callbackData.ToString();

            // Assert
            Assert.Equal($":{(int)TestAction.SendNotification}>arg1|arg2", callbackDataStr);
        }

        [Fact]
        public void Should_parse_action_callback_data_with_args()
        {
            // Arrange
            var callbackData = $":{(int)TestAction.SendNotification}>arg1|arg2";

            var parsed = CallbackData<TestState, TestAction>.Parse(callbackData);
            Assert.NotNull(parsed);
            Assert.Equal(TestAction.SendNotification, parsed.Action);
            Assert.NotEmpty(parsed.Args);
            Assert.Contains("arg1", parsed.Args);
            Assert.Contains("arg2", parsed.Args);
        }

        [Fact]
        public void Should_parse_callback_data_with_args()
        {
            var callbackData = new CallbackData<TestState>(TestState.Pending, "arg1", "arg2");
            Assert.NotNull(callbackData);
            Assert.Equal(TestState.Pending, callbackData.NextState);
            Assert.NotEmpty(callbackData.Args);

            var callbackDataStr = callbackData.ToString();
            Assert.Equal("1000>arg1|arg2", callbackDataStr);

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
            Assert.Equal("1000>", callbackDataStr);

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