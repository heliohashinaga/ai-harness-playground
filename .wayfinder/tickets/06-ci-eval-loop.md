---
title: CI eval loop
status: closed
assignee: heliohashinaga
labels: [wayfinder:grilling]
blocked-by: [04-harness-wiring-dotnet.md, 05-init-version-pinning.md]
---

## Question

What CI job runs baseline vs candidate against the pinned dotnet target and records quality/cost per run?

Decide trigger and matrix (baseline coder-only vs named candidates like cleaner/planner per `evals/benchmark.md` candidates queue), per-task N and retry cap (max 3 per harness), metrics capture (`task_success, first_attempt_success, retry_rate, human_rate, regression_rate, tokens, cost, latency` → `evals/results/`-style file), promotion rule (when a layer earns default status), and cost guardrails (model choice, timeouts). Blocked by Harness wiring for dotnet + Init version pinning and evolution. HITL grilling; hands a checklist if credentials/provisioning (Task-type work) must happen first.

## Resolution

- Workflow lives in `ai-harness` (e.g. `.github/workflows/eval-dotnet.yml`, runner `evals/run_006-dotnet.py`), `workflow_dispatch` only for v0; checks out playground at `eval-v0` SHA via fetch-tags.
- v0 matrix: baseline coder-only vs coder+cleaner only; planner/reviewer/swarm join later as named experiments.
- N=1 per task (pilot), retry cap 3 with Verdict-as-feedback.
- Record: `evals/results/006-dotnet-<candidate>.md` with benchmark metrics + per-task Verdict; promotion reuses locked rule (fix >=1 baseline-fail at <=2x cost, zero regressions).
- Guardrails: `deepseek-v4-flash` via `opencode`, `LANGSMITH_TRACING=false`, ~20 min/job timeout, `setup-dotnet` 10. Checklist tail: verify provider key exists as GitHub secret before first CI run.
- Decided with human 2026-09-28; (2)-(5) accepted as proposed, (1) ai-harness side.
