# Build & Verify

## When to consult
Before declaring a change complete; before opening a PR; whenever you need to know the exact command set CI runs.

## Commands (all run from repo root: `D:\Work\open-source\Insight.TelegramBot`)
- Restore: `dotnet restore`
- Build: `dotnet build --configuration Release` — verified to succeed on the current branch (0 errors, ~49 NU1507 / package-source-mapping warnings that are environmental, unrelated to code).
- Tests: `dotnet test --no-build --configuration Release` — verified to pass (13/13 in `tests/Insight.TelegramBot.Tests`, ~125 ms).
- CI runs the same in `.github/workflows/dotnet.yml` jobs: `dotnet restore` → `dotnet build --configuration Release --no-restore` → `dotnet test --no-restore --verbosity normal`. The workflow installs both .NET 8.0.302 and 7.0.410 SDKs.
- The legacy `run-tests.sh` script in repo root iterates `tests/*` and runs `dotnet test` for any folder ending in `.Tests`. It's there for local convenience; CI does not invoke it. Currently only `Insight.TelegramBot.Tests` matches.

## Target frameworks
- Library projects (`src/Insight.TelegramBot*`) target `netstandard2.1`. Some of them also use C# 12 via `<LangVersion>12</LangVersion>` in `Directory.Packages.props`. Don't lower or raise `TargetFramework` without an explicit reason — `netstandard2.1` is what makes this SDK consumable from .NET Framework / Xamarin / etc.
- Tests, samples, and `Insight.TelegramBot.Testing` target `net8.0`.
- Central package management is on (`Directory.Build.props` sets `ManagePackageVersionsCentrally=true`). Add new versions to `Directory.Packages.props` — never put a `Version=` on a `<PackageReference>` inside an individual csproj.

## What "done" looks like
1. `dotnet build --configuration Release` — `Build succeeded. 0 Error(s)`. Net-new NU1507 warnings are not introduced.
2. `dotnet test --no-build --configuration Release` — all tests pass.
3. The change does not add files under the stale `src/Insight.TelegramBot.State|Hosting|Web` or `tests/Insight.TelegramBot.State.Tests` folders.
4. New public APIs are added to / extend an existing builder (`TelegramBotBuilder`, `WebHookHostBuilder`, `PollingHostBuilder`) where appropriate rather than introducing a new entry point.

## Canonical files
- `.github/workflows/dotnet.yml` — CI definition.
- `Directory.Packages.props` — central package versions, `LangVersion`.
- `Directory.Build.props` — central-package-management flag.
- `run-tests.sh` — legacy local test runner.

## Anti-patterns
- Skipping `--no-build` on the test command when you've already run `dotnet build` — wastes time and risks rebuilding with different flags.
- Adding `<PackageReference Include="X" Version="Y" />` instead of declaring `Version` in `Directory.Packages.props`.
- Committing changes that turn the NU1507 warning into errors via `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` without first cleaning up the local NuGet sources — would break dev environments.

See also: [[architecture-overview]], [[testing-patterns]].
