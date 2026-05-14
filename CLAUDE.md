# CLAUDE.md

Operating guide for AI coding agents working in this repository.

## Repository summary
`Insight.TelegramBot` is a multi-package C# SDK (NuGet) layered on top of `Telegram.Bot`. It provides update handling (matchers + handlers), polling/webhook hosts, message POCOs + `ITelegramBotClient` extensions, and a fluent `AddTelegramBot(...)` DI builder. Library projects target `netstandard2.1`; tests/samples target `net8.0`. The authoritative project list is `Insight.TelegramBot.sln`.

## Working norms
- Use Serena symbolic tools (`get_symbols_overview`, `find_symbol`, `find_referencing_symbols`, `replace_symbol_body`, `insert_after_symbol`, `replace_content`) for code reads and edits in `.cs` files. Reserve `Read`/`Edit` for non-code files or for tiny in-symbol tweaks where regex replacement is cleaner.
- Folders that exist on disk but are NOT in `Insight.TelegramBot.sln` are stale: `src/Insight.TelegramBot.State`, `src/Insight.TelegramBot.Hosting`, `src/Insight.TelegramBot.Web`, `tests/Insight.TelegramBot.State.Tests`. Do not add code there.
- Central package management is on. Add versions to `Directory.Packages.props`, not to individual `.csproj` files.
- Conventional verification before declaring work done: `dotnet build --configuration Release` then `dotnet test --no-build --configuration Release` from repo root.

## Project knowledge (Serena memories)

Run `list_memories` at session start to confirm what's available.

Mandatory reads — call BEFORE the matching task:

| Task trigger | Required call |
|---|---|
| Structural changes, new modules, moving types between projects | `read_memory("architecture-overview")` |
| Adding/modifying a matcher, handler, or anything touching `HandlingUpdateProcessor` | `read_memory("handling-pipeline")` |
| Adding/modifying any `WithXxx(...)` extension or options builder | `read_memory("di-builder-conventions")` |
| Wiring options or deciding how to throw/swallow exceptions inside the SDK | `read_memory("error-handling-and-options")` |
| Adding a new outbound message POCO or `ITelegramBotClient` send extension | `read_memory("bot-message-and-sending")` |
| Writing or modifying tests | `read_memory("testing-patterns")` |
| Verifying work is done; preparing a PR | `read_memory("build-and-verify")` |

After establishing a new pattern or correcting an outdated one, update with `write_memory(...)`. Do not silently diverge from a memory — fix the memory or escalate the conflict.
