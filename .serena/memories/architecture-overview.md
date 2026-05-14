# Architecture Overview

## When to consult
Before any structural change: adding a new project, moving types between projects, introducing a new abstraction, or changing reference direction.

## Rules
- The SDK is a multi-package NuGet solution. The active projects are exactly those listed in `Insight.TelegramBot.sln` (see lines 6–57). Folders that exist on disk but are **not** in the `.sln` (`src/Insight.TelegramBot.State`, `src/Insight.TelegramBot.Hosting`, `src/Insight.TelegramBot.Web`, `tests/Insight.TelegramBot.State.Tests`) are stale leftovers — do not add code there or reference them.
- Layering, from low to high. Higher layers may reference lower; the reverse is forbidden:
  1. `Insight.TelegramBot` — core abstractions: `IUpdateProcessor`, `BotMessage` hierarchy, `TelegramBotClientExtensions`, `CallbackData<TState>`, `VerticalKeyboardMarkup`. Targets `netstandard2.1`. References only `Telegram.Bot` + `Microsoft.Extensions.DependencyInjection.Abstractions`.
  2. `Insight.TelegramBot.Handling` — the matcher/handler pipeline (`IUpdateMatcher`, `IUpdateHandler`, `IMatchingUpdateHandler<TMatcher>`, `IContextMatchingUpdateHandler<TMatcher>`, `IBeforeExecutionHandler`, `IExceptionFlowHandler`, `HandlingUpdateProcessor`). `netstandard2.1`, `<Nullable>disable</Nullable>`.
  3. `Insight.TelegramBot.Polling` — `TelegramBotPollingWebHost : BackgroundService` + `IPollingExceptionHandler`. `Insight.TelegramBot.WebHook` — `TelegramBotWebHookHost : IHostedService` + `WebHookOptions`. `Insight.TelegramBot.WebHook.Controllers` — `DefaultUpdateController` (`Microsoft.AspNetCore.Mvc`).
  4. `Insight.TelegramBot.DependencyInjection` — `AddTelegramBot(...)` entrypoint + `TelegramBotBuilder` / `TelegramBotClientBuilder` / `TelegramBotOptionsBuilder` / `OptionsBuilderBase<TOptions>`. Nothing above this layer is referenced.
  5. `Insight.TelegramBot.DependencyInjection.Handling|Polling|WebHook|WebHook.Controllers` — thin glue. Each exposes `WithXxx(...)` extension methods on `TelegramBotBuilder` (or `WebHookHostBuilder`) and bridges core + the matching transport/feature package.
- Business logic for consumers (matchers, handlers, message construction) lives outside the SDK. The SDK only provides interfaces, base classes, infrastructure, and DI plumbing. Samples under `samples/Insight.TelegramBot.Samples.*` are reference consumers — they import the SDK exactly the way external apps would.
- `IUpdateProcessor` (`src/Insight.TelegramBot/UpdateProcessors/IUpdateProcessor.cs:7`) is the single seam between transport (polling/webhook) and update handling. Anything that consumes updates must implement it. Only one `IUpdateProcessor` may be registered per container — registration sets `TelegramBotBuilder.UpdateProcessorConfigured = true` and any second attempt throws.

## Canonical examples in this codebase
- `Insight.TelegramBot.sln:6-57` — authoritative project list.
- `src/Insight.TelegramBot/UpdateProcessors/IUpdateProcessor.cs` — the seam interface.
- `src/Insight.TelegramBot.Handling/HandlingUpdateProcessor.cs` — built-in `IUpdateProcessor`, used by `WithHandling(...)`.
- `samples/Insight.TelegramBot.Samples.Domain/SampleUpdateProcessor.cs` — example of a custom `IUpdateProcessor` registered via `WithUpdateProcessor<T>(...)`.

## Anti-patterns
- Don't add a `<ProjectReference>` from a lower-layer project (e.g. `Insight.TelegramBot`) to a higher-layer one (e.g. `Insight.TelegramBot.Handling`). The core stays transport-agnostic and handling-agnostic.
- Don't put ASP.NET MVC dependencies into anything except `Insight.TelegramBot.WebHook.Controllers` / `Insight.TelegramBot.DependencyInjection.WebHook.Controllers`. Polling and core must remain hostable from a console app.
- Don't resurrect the empty `State`/`Hosting`/`Web` folders. If you need similar capability, design a new package and add it to the sln deliberately.
- Don't bypass `IUpdateProcessor` — both `TelegramBotPollingWebHost.HandleUpdateAsync` and `DefaultUpdateController.Post` resolve it from a freshly created scope.

See also: [[handling-pipeline]], [[di-builder-conventions]], [[build-and-verify]].
