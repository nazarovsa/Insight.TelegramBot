# Error Handling & Options

## When to consult
When wiring options for a new host/feature, when deciding whether to throw or swallow an exception inside the SDK, or when adding a new exception-handler abstraction.

## Rules — options
- Every feature package that needs configuration ships its own `*Options` POCO (`PollingOptions`, `WebHookOptions`, `HandlingUpdateProcessorOptions`) and its own `*OptionsBuilder` extending `OptionsBuilderBase<TOptions>`.
- Options are bound via `services.Configure<TOptions>(configuration.GetSection(sectionName))` where `sectionName` defaults to `typeof(TOptions).Name`. Override with the second arg when the config section name diverges (`OptionsBuilderBase.FromConfiguration` at `src/Insight.TelegramBot.DependencyInjection/Builders/Base/OptionsBuilderBase.cs:18`).
- Consumers resolve options through `IOptions<TOptions>` (for hosted services, e.g. `TelegramBotPollingWebHost`, `TelegramBotWebHookHost`) or `IOptionsSnapshot<TOptions>` when the value should refresh per scope (e.g. `HandlingUpdateProcessor`).
- Validating an options invariant lazily is acceptable when the value is computed: `WebHookOptions.WebHookUrl` (`src/Insight.TelegramBot.WebHook/WebHookOptions.cs:13`) throws `ArgumentNullException` if `WebHookBaseUrl` or `WebHookPath` is empty. This is covered by `WebHookConfigurationTests` — keep that pattern (throw on access) consistent.
- Default values are set as property initializers in the POCO (`PollingTaskCheckInterval = TimeSpan.FromMinutes(1)`). Don't rely on the configuration provider to supply defaults.

## Rules — exception handling
- Public hosted services log + swallow startup errors so the host process stays up:
  - `TelegramBotWebHookHost.StartAsync` catches `Exception`, logs via `ILogger<TelegramBotWebHookHost>`, returns (`src/Insight.TelegramBot.WebHook/TelegramBotWebHookHost.cs:44`).
  - `TelegramBotPollingWebHost.ExecuteAsync` catches `Exception`, logs `LogError`, awaits `PollingOptions.PollingTaskExceptionDelay`, then continues (`src/Insight.TelegramBot.Polling/TelegramBotPollingWebHost.cs:73`).
- The polling receiver's per-update error path goes through `IPollingExceptionHandler`. The default registration is `NulloPollingExceptionHandler` (no-op), wired via `TryAddSingleton` so user-supplied handlers via `.WithExceptionHandler<T>()` win. The built-in `LoggingPollingExceptionHandler` is a ready-to-use logger implementation.
- Per-handler errors in `HandlingUpdateProcessor.HandleUpdate` go through every registered `IExceptionFlowHandler.Handle(ex, CancellationToken.None)` — note `CancellationToken.None` is intentional so the error pipeline still runs even if the inbound token was cancelled.
- Use `ArgumentNullException(nameof(x))` for constructor null guards on injected services. Pattern is consistent across the codebase (`TelegramBotPollingWebHost`, `LoggingPollingExceptionHandler`, `DefaultUpdateController`, etc.). For null inputs to user-facing methods, also use `ArgumentNullException`. Use `ArgumentException` when the value is wrong but not null (e.g. empty assemblies array in `AddTelegramBotHandling`).
- Builder methods throw `InvalidOperationException` for "already configured / wrong order" errors, not `ArgumentException`. See `TelegramBotBuilder.WithTelegramBotClient` and analogous methods.
- Use `ILogger<TConcreteClass>` (closed generic on the concrete service type) rather than a shared logger. Existing code uses message-template structured logging with `{PascalCase}` placeholders (`logger?.LogDebug("Queued handlers of type {HandlerType}...", typeof(IUpdateHandler).FullName)`).

## Canonical examples in this codebase
- `src/Insight.TelegramBot.Handling/HandlingUpdateProcessor.cs:66-79` — build-flow throw-vs-log toggle driven by `HandlingUpdateProcessorOptions.ThrowBuildFlowExceptions`.
- `src/Insight.TelegramBot.Polling/ExceptionHandlers/LoggingPollingExceptionHandler.cs` — minimal `IPollingExceptionHandler` implementation.
- `src/Insight.TelegramBot.WebHook/WebHookOptions.cs` — options POCO with computed property that validates lazily.
- `tests/Insight.TelegramBot.Tests/WebHookConfigurationTests.cs` — lock-in tests for the lazy-validation pattern.

## Anti-patterns
- Catching `Exception` and silently swallowing without logging — every catch in the codebase either logs or rethrows after letting an exception-handler chain run.
- Throwing from a hosted service's `StartAsync`/`ExecuteAsync` top level — the surrounding service expects them to remain alive across transient errors.
- Reading `IOptions<T>` inside a singleton when scoped/snapshot semantics are needed (or vice versa). `HandlingUpdateProcessor` uses `IOptionsSnapshot<T>` precisely because it resolves per scope.
- Building a new feature with options but no `*OptionsBuilder` deriving from `OptionsBuilderBase<T>`. The base's `Build()` invariant (`OptionsConfigured == true`) catches "forgot to wire configuration" at startup.

See also: [[di-builder-conventions]], [[handling-pipeline]], [[architecture-overview]].
