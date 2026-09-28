---
title: Dotnet target shape
status: closed
assignee: heliohashinaga
labels: [wayfinder:grilling]
blocked-by: [01-discriminative-task-shape.md]
---

## Question

What .NET solution lives here as the eval target so the discriminative tasks have something real to bite into?

Decide template and layout (CLI/API/worker, solution + src/test project names), framework + test stack (net version, xUnit, lint/format, nullable), multi-file breakdown (minimum file/module count and dependency direction), build gate equivalent of `scripts/verify.ps1` (`dotnet build + test + format`?), and size ceiling that keeps it maintainable as a fixture. Blocked by Discriminative task shape — the shape must serve the discrimination, not the other way around. HITL grilling.

## Resolution

- Template: API / Worker (not CLI console). Serves the 5 frozen tasks via endpoints/background-service ops instead of flags; cross-file decider ripples through contract -> service -> formatter layers.
- Toolchain: .NET 10, xUnit, nullable enable + implicit usings. Format gate out for v0; gate is build+test only.
- Layout (accepted principle, adapted to API/Worker): `EvalTarget.sln`, `src/EvalTarget/` (Program.cs, Service/Worker, OutputFormatter, RequestOptions/Dto), `tests/EvalTarget.Tests/` (ServiceTests, ApiTests). Enough files for a ~3-file ripple.
- Size ceiling v0: <=8 src files, <=~500 LOC, <=20 tests.
- Build gate: `dotnet build -c Release` + `dotnet test` exit-zero + diff sanity, local + CI (`actions/setup-dotnet`, SDK 10 assumed; CI is verifier since local has no SDK).
- Decided with human 2026-09-28; answers: (1) api/worker, (2) .NET 10, (3)-(5) accepted as adapted.
