# Testing Patterns

## When to consult
Before adding or modifying tests under `tests/`.

## Framework
- xUnit (`xunit`, `xunit.runner.visualstudio`) with `Microsoft.NET.Test.Sdk` and `coverlet.collector`. Versions pinned in `Directory.Packages.props` (xunit 2.8.1, Test SDK 17.10.0).
- NSubstitute for mocking (`Substitute.For<T>()`, `.Received(n)`, `.ReturnsForAnyArgs(...)`).
- `Microsoft.Extensions.Logging.Abstractions.NullLogger<T>.Instance` is the canonical logger in tests; never spin up a real `ILoggerFactory`.

## Project layout
- The single active test project for SDK code is `tests/Insight.TelegramBot.Tests` (net8.0, `IsPackable=false`). It references the sample handling project, the core, and the `Insight.TelegramBot.Testing` support project.
- `tests/Insight.TelegramBot.Testing` is shared test-support code (currently `TestState` enum). Add reusable testing helpers (fakes, factory methods) here, not inside the `*.Tests` project.
- Internals of `Insight.TelegramBot.Handling` are exposed to tests via `[assembly: InternalsVisibleTo("Insight.TelegramBot.Tests")]` (`src/Insight.TelegramBot.Handling/Infrastructure/InternalsVisibleTo.cs`). If a test needs another assembly's internals, add a parallel `InternalsVisibleTo.cs` under that project's `Infrastructure/`.
- `tests/Insight.TelegramBot.State.Tests` exists on disk but is NOT in the sln — it's stale, do not modify.

## Conventions
- Test classes are `public class XxxTests` (or `sealed`) under namespace `Insight.TelegramBot.Tests`, file `XxxTests.cs`.
- Use `[Fact]` for single-case tests and `[Theory] + [InlineData(...)]` for parameterised tests (see `WebHookConfigurationTests.Should_return_correct_uri`). Test method names use `snake_case` for behavioural tests (`Should_parse_callback_data_with_args`, `Should_throw_ANE_when_baseUrl_isEmpty`) OR PascalCase `Verb_Scenario_Outcome` for handler/integration tests (`AddTelegramBotHandling_RegistersStartMessageHandlerAndCorrectTypeMap`). Pick the style that matches the file you're editing rather than introducing a third one.
- `ANE` is the project shorthand for `ArgumentNullException` in test names.
- Body is AAA with explicit `// Arrange`, `// Act`, `// Assert` comments for non-trivial tests (`HandlingExtensionsTests`). Trivial single-flow tests may omit the comments (`CallbackDataTests`).
- DI-pipeline tests build a real `ServiceCollection`, call the SDK extension (`services.AddTelegramBotHandling(typeof(StartMessageHandler).Assembly)`), wire any required `ILogger<T>` via `NullLogger<T>.Instance`, substitute injected dependencies via NSubstitute, and call `BuildServiceProvider()`. This is the canonical integration-style approach; don't mock the container.
- For asserting `IUpdateHandler` interactions, drive the real `HandlingUpdateProcessor` end-to-end with a constructed `Update` (see `HandlingExtensionsTests` lines 64–73) and assert via NSubstitute `.Received(n)`.

## Canonical examples in this codebase
- `tests/Insight.TelegramBot.Tests/CallbackDataTests.cs` — straightforward `[Fact]` style, snake_case names.
- `tests/Insight.TelegramBot.Tests/WebHookConfigurationTests.cs` — `[Theory]/[InlineData]` parameterisation; tests for lazy-validation in options POCOs.
- `tests/Insight.TelegramBot.Tests/HandlingExtensionsTests.cs` — DI-pipeline integration test with NSubstitute.

## Anti-patterns
- Mocking `IServiceProvider`/`IServiceCollection`. The existing tests always use real DI containers.
- Real `ILoggerFactory`/`LoggerFactory.Create` in tests — use `NullLogger<T>.Instance`.
- Tests that depend on the `Telegram.Bot` HTTP transport. Existing tests never hit the network — they construct `Update`/`Message`/`CallbackQuery` POCOs directly and route them through `IUpdateProcessor`.
- Adding tests against the stale `Insight.TelegramBot.State.Tests` folder.

See also: [[handling-pipeline]], [[build-and-verify]].
