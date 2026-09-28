---
title: Buggy and multi-file seeding
status: closed
assignee: heliohashinaga
labels: [wayfinder:grilling]
blocked-by: [01-discriminative-task-shape.md, 02-dotnet-target-shape.md]
---

## Question

How are bugs and multi-file complexity seeded into the init version so coder-only plausibly stumbles where a second layer could help?

Decide bug count and taxonomy (compile-break vs logic-break vs cross-file contract break vs config/DI break), placement across files, whether bugs are hand-seeded or captured from real runs (cf. `TASK-004-buggy-output-set`), hidden-vs-visible test split, and the rule that keeps the init version buildable-but-failing in known ways. Blocked by Discriminative task shape + Dotnet target shape. HITL grilling.

## Resolution

- v0 ships buildable-but-failing: `dotnet build` green, `dotnet test` red in exactly 3 known tests.
- 3 hand-seeded bugs (no history yet; captured-from-runs starts v1): B1 logic-break in OutputFormatter (edge case), B2 cross-file contract break (Dto/options rename rippling Service + Tests, feeds decider), B3 DI/config break (Worker registration or options binding, builds fine, fails at runtime). Zero compile-breaks.
- Placement: B1 Formatter, B2 Dto + Service + test, B3 Program.cs DI.
- v0 all tests visible; hidden split deferred to CI eval loop. Invariant: build green + exactly the 3 known failures, nothing else red.
- Decided with human 2026-09-28; all 5 accepted as proposed.
