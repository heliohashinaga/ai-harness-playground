---
title: Init version pinning and evolution
status: closed
assignee: heliohashinaga
labels: [wayfinder:grilling]
blocked-by: []
---

## Question

What does "can be edited, but always will be the init eval version" mean operationally so evals stay comparable per the frozen-set rule?

Decide the pinning mechanism for v0 (git init + tag/commit, `evals/benchmark.md` frozen reference, snapshot copy?), what edits are allowed on the working copy vs what forces a new named version (v1 + new experiment file, never rewriting v0 results in `evals/results/`), and where the pin lives so both local runs and CI resolve the same bytes. Frontier — answerable without waiting on target shape. HITL grilling.

## Resolution

- Pin: `git init` + initial commit + tag `eval-v0` at build time (not now); evals resolve tag SHA, never HEAD.
- Edit rule: working copy stays editable; docs/comments/format need no new version, any `src/`/`tests/`/task/bug change forces `eval-v1` + new experiment file; `evals/results/` v0 rows never rewritten (frozen-set rule).
- Pin location: tag here, SHA mirrored in `.wayfinder/eval-pin.txt` + referenced from `ai-harness/evals/` protocol; CI resolves tag→SHA via checkout with fetch-tags, identical bytes locally and in CI.
- Decided with human 2026-09-28; all 4 accepted as proposed.
