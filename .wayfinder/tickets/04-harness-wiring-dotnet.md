---
title: Harness wiring for dotnet
status: closed
assignee: heliohashinaga
labels: [wayfinder:research]
blocked-by: []
---

## Question

How does the current Python harness drive and judge an external .NET repo without rewriting the harness?

Surface facts from primary sources only (`src/aiharness/agents/repo_runner.py` worktree flow, `src/aiharness/agents/repo.py`, `scripts/run.ps1`, `scripts/verify.ps1`, `scripts/evaluate.ps1`, `docs/evaluation.md` Verdict format + cheap-first order, `evals/benchmark.md` metrics, `evals/run_005.py` / `archive/run_003.py` invocation): exact commands the harness shells out to, where a `dotnet build/test` gate plugs in, how Evaluator separation is preserved for a non-Python target, and what metrics rows (`task_success, first_attempt_success, retry_rate, human_rate, regression_rate, tokens, cost, latency`) require. AFK research; capture findings to a throwaway `research/harness-wiring-dotnet` note and link it as the resolution asset.

## Resolution

No harness rewrite. Playground must be a local git repo (prerequisite); eval runner calls `run_repo_pipeline` per task-file (single-`target_file` limit → multi-file tasks = multiple calls for v0); `.cs` already flows via explorer auto-discovery; grade with `dotnet build/test` in the coder worktree; judge with fresh-context Evaluator + Verdict YAML; record benchmark metrics to `evals/results/`. Full facts in `research/harness-wiring-dotnet.md`.

Asset: `research/harness-wiring-dotnet.md` (primary sources: `repo_runner.py`, `repo.py`, `explorer.py`, `evaluator.md`, `scripts/run|verify|evaluate.ps1`, `docs/evaluation.md`, `evals/benchmark.md`, `evals/run_005.py`). Resolved AFK 2026-09-28.
