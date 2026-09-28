# EvalTarget v0 — pinned .NET eval target

API/Worker on .NET 10 + xUnit. Small fixture for `ai-harness` evals that must
discriminate single-agent vs multi-agent workflows where Hello World could not.

## Layout

- `EvalTarget.sln`
- `src/EvalTarget/` — `Program.cs` (minimal API + DI), `Models/` (ReportRequest/ReportResult Dto),
  `Services/` (ReportService, OutputFormatter, EvalOptions), `Worker/` (ReportWorker)
- `tests/EvalTarget.Tests/` — `OutputFormatterTests`, `ReportServiceTests`, `ApiTests`

Caps v0: <=8 src files, <=~500 LOC, <=20 tests. Current: 8 src files, 13 tests.

## Build gate

```powershell
dotnet build -c Release
dotnet test
```

Expected v0 state: **build green, exactly 3 known-red tests** (all tests visible, no hidden split):

1. `OutputFormatterTests.Format_Upper_TrimsAndUpperCases` — B1 Formatter edge case (upper branch skips trim).
2. `ReportServiceTests.CreateReport_TruncatesLongTitleToMaxLength` — B2 cross-file contract (Title not truncated to `EvalOptions.MaxTitleLength`; spans Dto + Options + Service).
3. `ApiTests.Worker_IsRegistered` — B3 DI break (`ReportWorker` exists but is never registered in `Program.cs`; builds fine, worker never runs).

Zero compile-breaks. Any other red is a regression.

## Frozen task set (what this target serves)

Per `.wayfinder/tickets/01-discriminative-task-shape.md`, 5 tasks: feature
(endpoint/background-service op), bugfix (invalid input + regression test),
refactor (no behavior change), **cross-file contract break** (interface change
rippling contract -> service -> formatter, the decider), helper + unit test.

## Pinning

Working copy stays editable. The eval version is the `eval-v0` git tag;
evals resolve tag SHA (mirrored in `.wayfinder/eval-pin.txt`), never HEAD.
Any `src/`/`tests/`/task/bug change forces `eval-v1` + a new experiment file.
