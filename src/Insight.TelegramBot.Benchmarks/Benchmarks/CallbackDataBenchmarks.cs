using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;

namespace Insight.TelegramBot.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(BenchmarkOptions))]
[SimpleJob(RuntimeMoniker.Net70, baseline: true)]
[SimpleJob(RuntimeMoniker.Net60)]
public class CallbackDataBenchmarks
{
    private CallbackData<TestState, TestAction> _callbackData;

    private string _callbackDataStr;

    [GlobalSetup]
    public void Setup()
    {
        _callbackData = new(TestState.Done, "arg1", "arg2");
        _callbackDataStr = $"{(int)TestState.Done}:>arg1|arg2";
    }

    [Benchmark]
    public CallbackData<TestState, TestAction> ParseCallbackData()
    {
        return CallbackData<TestState, TestAction>.Parse(_callbackDataStr);
    }

    [Benchmark]
    public string GenerateCallbackDataString()
    {
        return _callbackData.ToString();
    }

    private class BenchmarkOptions : ManualConfig
    {
        public BenchmarkOptions()
        {
            SummaryStyle = SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend);
        }
    }
}