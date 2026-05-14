# DI Builder Conventions

## When to consult
When adding a new `WithXxx(...)` extension on `TelegramBotBuilder`/`WebHookHostBuilder`/`PollingHostBuilder`, when introducing a new options class, or when exposing a new package's wiring as DI.

## Entry point
- Public API root: `services.AddTelegramBot(builder => ...)` (`src/Insight.TelegramBot.DependencyInjection/Infrastructure/ServiceCollectionExtensions.cs:9`). It creates one `TelegramBotBuilder`, runs the user callback, and returns `IServiceCollection` (NOT the builder — chaining is done inside the callback).
- All feature-specific packages contribute by extending `TelegramBotBuilder` with `WithXxx(...)` extension methods. Examples: `WithPolling`, `WithWebHook`, `WithHandling`. `WithDefaultUpdateController` extends `WebHookHostBuilder` instead, because it composes with `WithWebHook`.

## Rules
- The `TelegramBotBuilder` carries internal flags that enforce mutual exclusion / single-configuration:
  - `PollingConfigured` and `WebHookConfigured` — flipped by `WithPolling` / `WithWebHook`; the other extension throws if either is already set (`src/Insight.TelegramBot.DependencyInjection.Polling/TelegramBotBuilderExtensions.cs:11`, `src/Insight.TelegramBot.DependencyInjection.WebHook/TelegramBotBuilderExtensions.cs:10`).
  - `UpdateProcessorConfigured` — flipped by `WithUpdateProcessor<T>(...)` AND by `WithHandling(...)`. Calling both, or either twice, throws `InvalidOperationException`.
  - When you add a new `WithXxx(...)` that registers an alternative `IUpdateProcessor` or another transport, use these same flags. New mutually-exclusive concepts get their own internal flag.
- Inner builders (`TelegramBotClientBuilder`, `TelegramBotOptionsBuilder`, `PollingHostBuilder`, `WebHookHostBuilder`, `PollingOptionsBuilder`, `WebHookOptionsBuilder`) follow the same shape:
  - Constructor takes `IServiceCollection` (or the parent builder's `Services`), null-checked with `throw new ArgumentNullException(nameof(services))`.
  - Public fluent `WithXxx(...)` methods return `this`.
  - An internal `Build()` method is called by the parent after the user callback runs. `Build()` is where `services.Add/TryAdd` happens — never inside the `With*` setters that just record state.
  - A `WithXxx` that may only be called once stores a private nullable field (`_telegramBotClientBuilder`, `_pollingOptionsBuilder`, etc.) and throws `InvalidOperationException("...already configured")` on re-entry.
- Service registrations:
  - `ITelegramBotClient` is registered as a `ServiceDescriptor` via `_services.TryAdd` so user overrides win. Default lifetime is `Singleton`; configurable through `TelegramBotClientBuilder.WithLifetime` (`src/Insight.TelegramBot.DependencyInjection/Builders/TelegramBotClientBuilder.cs`).
  - Options classes register via `services.Configure<TOptions>(configuration.GetSection(sectionName))` where `sectionName` defaults to `typeof(TOptions).Name` (`OptionsBuilderBase.FromConfiguration`).
  - Hosted services register via `_services.AddHostedService<...>()` inside `Build()` of the relevant host builder (`PollingHostBuilder.Build`, `WebHookHostBuilder.Build`).
- New options classes MUST derive their builder from `OptionsBuilderBase<TOptions>` (`src/Insight.TelegramBot.DependencyInjection/Builders/Base/OptionsBuilderBase.cs`). It enforces the `OptionsConfigured` invariant — `Build()` throws if no `FromConfiguration(...)` (or future `FromValue(...)`-style) call ran. Note: the `FromValue`/custom-options builder support was intentionally removed (commit b2f6ff5); don't reintroduce it without revisiting that decision.
- Default exception handlers register through `TryAddSingleton` so a `WithExceptionHandler<...>()` user-supplied one wins (`PollingHostBuilder.Build`, line 45 — `TryAddSingleton<IPollingExceptionHandler, NulloPollingExceptionHandler>`).

## Canonical examples in this codebase
- `src/Insight.TelegramBot.DependencyInjection/Builders/TelegramBotBuilder.cs` — root builder + mutual-exclusion flags.
- `src/Insight.TelegramBot.DependencyInjection.Polling/PollingHostBuilder.cs` — inner builder pattern with `Build()` and `TryAddSingleton` fallback.
- `src/Insight.TelegramBot.DependencyInjection/Builders/Base/OptionsBuilderBase.cs` — shared options-builder base.
- `samples/Insight.TelegramBot.Samples.SimpleHostedBot/Startup.cs` — end-to-end usage of the fluent API.

## Anti-patterns
- Doing `services.Add(...)` inside a `WithXxx(...)` setter instead of in `Build()`. Registrations must run exactly once per builder, after the user callback.
- Using `services.Add` where `services.TryAdd` is the established pattern (e.g. for `ITelegramBotClient`, default exception handlers). It silently breaks user overrides.
- Returning `IServiceCollection` from inner builder methods. Inner builders return `this`/`builder` for fluent chaining; only the outer `AddTelegramBot` returns `IServiceCollection`.
- Forgetting to set the relevant `*Configured` flag — a second invocation of your new `WithXxx` will then silently double-register services.

See also: [[architecture-overview]], [[handling-pipeline]], [[error-handling-and-options]].
