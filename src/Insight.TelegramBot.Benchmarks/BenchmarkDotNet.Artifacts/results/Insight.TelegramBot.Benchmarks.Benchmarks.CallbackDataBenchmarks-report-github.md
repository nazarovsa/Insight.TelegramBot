``` ini

BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.3.1 (a) (22E772610a) [Darwin 22.4.0]
Apple M1 2.40GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.102
  [Host]   : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT SSE4.2
  .NET 6.0 : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT SSE4.2
  .NET 7.0 : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT SSE4.2


```
|                     Method |      Job |  Runtime |     Mean |   Error |  StdDev |        Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|--------------------------- |--------- |--------- |---------:|--------:|--------:|-------------:|--------:|-------:|----------:|------------:|
|          ParseCallbackData | .NET 6.0 | .NET 6.0 | 251.0 ns | 1.90 ns | 1.68 ns | 1.06x slower |   0.01x | 0.1183 |     248 B |  1.19x more |
|          ParseCallbackData | .NET 7.0 | .NET 7.0 | 236.6 ns | 0.28 ns | 0.25 ns |     baseline |         | 0.0329 |     208 B |             |
|                            |          |          |          |         |         |              |         |        |           |             |
| GenerateCallbackDataString | .NET 6.0 | .NET 6.0 | 207.6 ns | 1.06 ns | 1.00 ns | 1.19x faster |   0.01x | 0.0496 |     104 B |  1.00x more |
| GenerateCallbackDataString | .NET 7.0 | .NET 7.0 | 247.9 ns | 0.47 ns | 0.42 ns |     baseline |         | 0.0162 |     104 B |             |
