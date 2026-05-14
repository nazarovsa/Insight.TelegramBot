# Handling Pipeline (Matchers + Handlers)

## When to consult
When adding/modifying a matcher, a handler, the auto-registration logic, the dispatch order in `HandlingUpdateProcessor`, or anything that consumes `IUpdateHandler`/`IBeforeExecutionHandler`/`IExceptionFlowHandler`.

## Pipeline at a glance
`HandlingUpdateProcessor.HandleUpdate` (`src/Insight.TelegramBot.Handling/HandlingUpdateProcessor.cs:37`) does, in this order, per update:

1. Create a new DI scope (`IServiceProvider.CreateScope()`); every handler/matcher resolution happens through that scope.
2. Enqueue every registered `IUpdateHandler` (unconditional — these run for ALL updates).
3. For each entry in `IUpdateHandlersProvider.TypeMap` (matcher singleton built once at startup) call `MatchesAsync`. If true, resolve every implementation registered for `IMatchingUpdateHandler<TMatcher>` and enqueue them.
4. For each registered `IContextUpdateMatcher` (resolved per scope) call `MatchesAsync`. If true, resolve the corresponding `IContextMatchingUpdateHandler<TMatcher>` and enqueue.
5. Drain the queue. For each handler, first run every `IBeforeExecutionHandler`, then `handler.Handle(update, ct)`. If `Handle` throws, log + run every `IExceptionFlowHandler` with `CancellationToken.None`.

Two flags on `HandlingUpdateProcessorOptions` (`src/Insight.TelegramBot.Handling/HandlingUpdateProcessorOptions.cs`) control rethrow behaviour:
- `ThrowBuildFlowExceptions` — rethrow exceptions from `IUpdateMatcher.MatchesAsync` (default `false`, logs `Warning`; when `true`, logs `Error` and rethrows).
- `ThrowHandlingFlowExceptions` — same for the handler-execution phase. Exception handlers run regardless.

## Rules
- Matchers wired through `IMatchingUpdateHandler<TMatcher>` MUST have a public parameterless constructor — `UpdateHandlersProvider.Initialize` (`src/Insight.TelegramBot.Handling/UpdateHandlersProvider.cs:48`) constructs them via `Activator.CreateInstance` exactly once and caches the instance as a singleton in `TypeMap`. That instance is shared across updates, so keep matcher state immutable / set via protected setters in the ctor (see `StartMessageMatcher`).
- A handler class may implement at most one `IMatchingUpdateHandler<>` and at most one `IContextMatchingUpdateHandler<>` — `RegisterMatchingHandlers` / `RegisterContextMatchingHandlers` in `Insight.TelegramBot.Handling/Infrastructure/ServiceCollectionExtensions.cs` throws `InvalidOperationException` otherwise.
- Use `IContextMatchingUpdateHandler<TMatcher>` only when the matcher needs DI (it's resolved per scope as `IContextUpdateMatcher`). Use `IMatchingUpdateHandler<TMatcher>` when the matcher is pure (no DI). See `ContextMessageMatcher` vs `StartMessageMatcher` for the contrast.
- New matcher base classes that inspect message content go under `Matchers/MessageTypeMatchers` or `Matchers/TextMatchers` and `override MatchesAsync` to first `await base.MatchesAsync(...)` then add their own predicate (see `TextUpdateMatcher.MatchesAsync` at `src/Insight.TelegramBot.Handling/Matchers/TextMatchers/TextUpdateMatcher.cs:19`).
- All handlers are registered with `ServiceLifetime.Transient`; matchers used via `IContextUpdateMatcher` are `Scoped`; `IUpdateHandlersProvider` is `Singleton`. Don't change a lifetime without checking `HandlingUpdateProcessor` — it relies on per-update scope creation.
- `assembly.ExportedTypes` is what the scanner inspects (`TypeExtensions.GetMatchingUpdateHandlersImplementations`). Handler classes therefore must be `public`. Abstract types and interfaces are filtered out.

## Canonical examples in this codebase
- `samples/Insight.TelegramBot.Samples.Handling/StartMessage/StartMessageMatcher.cs` + `StartMessageHandler.cs` — minimal pure matcher + handler pair using `TextEqualsUpdateMatcher`.
- `samples/Insight.TelegramBot.Samples.Handling/ClickButton/ClickButtonMatcher.cs` — `StateCallbackQueryMatcher<HandlingState>` for callback-data routing via `CallbackData<TState>` (`src/Insight.TelegramBot/CallbackData.cs`).
- `samples/Insight.TelegramBot.Samples.Handling/ContextButtonMatcher/ContextMessageMatcher.cs` + `ContextMessageHandler.cs` — context (DI-fed) matcher + handler pair.
- `src/Insight.TelegramBot.Handling/Matchers/TextMatchers/TextUpdateMatcher.cs` — canonical pattern of chained `MatchesAsync` overrides.

## Anti-patterns
- A matcher constructor with parameters used through `IMatchingUpdateHandler<>` — `Activator.CreateInstance` on a parameterless ctor will fail at startup. Use `IContextMatchingUpdateHandler<>` instead.
- Mutable matcher state. Matchers in `TypeMap` are reused across updates; any field written during `MatchesAsync` is a data race.
- Calling `HandlingUpdateProcessor` directly from code that already runs inside a DI scope without realizing it creates a NEW nested scope every call.
- Registering more than one `IUpdateProcessor` (e.g. `WithHandling(...)` and `WithUpdateProcessor<T>(...)` in the same builder). Each setter flips `TelegramBotBuilder.UpdateProcessorConfigured` and the second call throws.

See also: [[di-builder-conventions]], [[error-handling-and-options]], [[testing-patterns]].
