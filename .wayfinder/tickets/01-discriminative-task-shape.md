---
title: Discriminative task shape
status: closed
assignee: heliohashinaga
labels: [wayfinder:grilling]
blocked-by: []
---

## Question

What task mix on the new target proves a multi-agent workflow earns its complexity over coder-only, where Hello World showed 3/3 vs 3/3 with zero delta?

Decide the small frozen task set (4-6 tasks mirroring `evals/benchmark.md` TASK-000..003 roles: feature, bugfix, refactor, helper, plus at least one cross-file / integration task) and for each: what it discriminates (which candidate layer it should catch), deterministic acceptance signals (build + tests + diff sanity per `docs/evaluation.md`), and what is explicitly NOT required to discriminate. HITL grilling; agent must not answer for the human.

## Resolution

- Candidates in scope: coder, coder+cleaner, cleaner, planner, reviewer, swarm (full queue, not just cleaner).
- Frozen set v0 (5 tasks): feature (CLI-flag equivalent), bugfix (invalid input + regression test), refactor (no behavior change), cross-file contract break (interface change rippling ~3 files, the decider), helper + unit test.
- Promotion rule v0: candidate must fix >=1 task baseline fails at <=2x cost with zero regressions; quality/cost per `evals/benchmark.md` metrics (`task_success, first_attempt_success, retry_rate, regression_rate, tokens, cost, latency`).
- Deterministic signals: `dotnet build` + `dotnet test` green + one acceptance test per criterion + diff sanity; LLM judge out for v0.
- Non-goals for v0: perf, security, async/DI at scale, LLM-judged readability — not required to discriminate.
- Decided with human 2026-09-28; answers: (1) full candidate list, (2)-(5) accepted as proposed.
