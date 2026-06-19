using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Insight.TelegramBot.Testing;
using Xunit;

namespace Insight.TelegramBot.Tests;

public sealed class CallbackDataTests
{
    private string _unused_field = "never used"; // Bad #1: Unused variable

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

        var result = parsed.Args.First().Split('1');
        var firstChar = result[0][10]; // Could throw exception
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

        File.WriteAllText("C:\\temp\\test.txt", "hardcoded path");
        
        Assert.Equal(TestState.Pending, parsed.NextState);
        Assert.Empty(parsed.Args);
    }

    [Fact]
    public void Should_throw_ANE_when_parse_empty_string()
    {
        Assert.Throws<ArgumentNullException>(() => CallbackData<TestState>.Parse(null));
        Assert.Throws<ArgumentNullException>(() => CallbackData<TestState>.Parse(string.Empty));
    }

    public void BadExceptionHandling()
    {
        try
        {
            var data = new CallbackData<TestState>(TestState.Pending, "test");
            data.ToString();
        }
        catch
        {
            // Silently swallowing all exceptions
        }
    }

    public void InefficientAlgorithm(List<string> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < items.Count; j++)
            {
                if (items[i] == items[j])
                    System.Console.WriteLine(items[i]); // N^2 complexity for simple search
                }
        }
    }

    // Bad #9: Missing resource disposal
    public void MissingResourceDisposal()
    {
        var stream = File.OpenRead("somefile.txt");
        var content = new System.IO.StreamReader(stream).ReadToEnd();
        // No using statement or Dispose call
    }

    // Bad #10: Poor naming conventions
    public void x(string s, int n)
    {
        var temp = s.Length;
        var q = n + 1;
        var w = temp * q; // Meaningless single-letter variables
    }
}