# Research: harness wiring for dotnet (primary sources only)

Question: how does the Python harness drive and judge an external .NET repo without rewriting the harness?

Sources read: `src/aiharness/agents/repo_runner.py`, `src/aiharness/agents/repo.py`,
`src/aiharness/agents/explorer.py`, `agents/evaluator.md`, `scripts/run.ps1`,
`scripts/verify.ps1`, `scripts/evaluate.ps1`, `docs/evaluation.md`,
`evals/benchmark.md`, `evals/run_005.py`.

## 1. Drive (repo mode, no harness rewrite)

- Entry: `run_repo_pipeline(repo_path, branch, task, target_file, coder_chat/provider/model, base_dir, context_files)` (`repo_runner.py`).
- `resolve_repo` requires the target to be a **local git repo** with the base branch present locally (or `HEAD`); no fetch/clone/push ever (`repo.py`). Implication: playground must be `git init` + committed (owned by Init version pinning ticket) before the harness can touch it.
- Coder gets its own worktree: `git worktree add --force -B agent/coder-<ts> <tmpdir> <base>`; worktree left in temp dir for inspection, cleanable via `repo.cleanup`. Single worktree, single commit, coder-only since TASK-009.
- Code generation: explicit `coder_chat` (tests) or real run via `build_explorer_coder(root, provider, model, target_file, context_files)` — the explorer gathers context then calls `coder.generate_code(task, chat, context)`.
- Explorer (`explorer.py`) is a bounded deterministic scout (no `bind_tools`): explicit `--context` files + auto-discovery (sibling source files in target dir, cap 12, plus README), total cap 9000 chars. `_SOURCE_EXT` **already includes `.cs`**. So .NET files flow into the coder prompt with zero harness change.
- Write-back: `repo_scm.write_file(worktree, target_file, code)` with safe-path guard (relative, no `..`, must stay under worktree), then `commit(worktree, [target_file], "agent(coder): ...")`. Returns `RepoRunResult` (repo, branch, branch_a, commit_c, file_paths, worktree_a).
- **Single-file limit (key finding):** the pipeline writes and commits exactly **one `target_file`** per run. A multi-file .NET task (contract ripple across ~3 files) does NOT fit one call. Options (decision for CI eval loop ticket, not here): one task per file (multiple pipeline calls), or extend the runner with a multi-file write list. Recommend task-per-file for v0 to avoid touching the harness.

## 2. Judge (cheap first, evaluator separation preserved)

- `scripts/run.ps1` is manual: the operator IS the agent (read `agents/agent.md`, follow `docs/workflow.md`, record `.factory/state/current.md`, run verify). No automation to port.
- `scripts/verify.ps1` is Python-only: `uv run ruff check .` + `uv run python -m pytest -q`. **Do not edit it.** The dotnet gate lives in the playground / eval runner: `dotnet build -c Release` + `dotnet test` (exit-zero), mirroring the decisions in Dotnet target shape + Buggy seeding tickets.
- `scripts/evaluate.ps1` pattern: run deterministic gate first, then fresh-context judgement per `agents/evaluator.md`, write `.factory/evaluations/<stem>.yaml`; verdict starts FAIL.
- `docs/evaluation.md` order: build → rules (lint/typecheck, `git diff --stat` sanity) → acceptance (each criterion mapped to test/script output) → LLM judge only for what scripts cannot check. Verdict YAML: `verdict, evidence{build, tests, criteria[{id,status,proof}]}, feedback`. Evaluator is read-only, fresh context, receives Task + diff + test results, never chain-of-thought — stack-agnostic, so it ports to .NET unchanged (feed it `dotnet` output instead of `pytest` output).
- `agents/evaluator.md`: per-criterion PASS only with concrete proof (command output, file+line, observed behavior), else FAIL with reproduction.

## 3. Metrics rows (what CI must record)

- Per `evals/benchmark.md` + `docs/evaluation.md`: `task_success, first_attempt_success, retry_rate, human_rate, regression_rate, tokens_used/tokens, cost, latency` → one file per experiment in `evals/results/`.
- `evals/run_005.py` is the runner template: REAL coder node with provider chats (`coder_default_chat(PROVIDER, MODEL)`, `MODEL=deepseek-v4-flash`, `PROVIDER=opencode`), `LANGSMITH_TRACING=false`, deterministic offline parts (extract fenced code, exec hidden asserts, ruff check), per-task record `{ok, detail, latency_ms, calls, ruff, output}`, plus `provider_api_key` guard. A `run_006-dotnet.py` mirrors this: same coder calls, `grade()` shells `dotnet build/test` in the coder worktree instead of `exec`+`ruff`, same metrics + verdict file.
- Retry (max 3) feeds previous Verdict as feedback; retries resolved by deterministic feedback cost 0 extra tokens in 002 — the thing candidates must beat.

## 4. Wiring answer (no harness rewrite)

1. Playground becomes a local git repo (v0 tag) — harness prerequisite, not a harness change.
2. Eval runner (new file in `ai-harness/evals/`, e.g. `run_006-dotnet.py`) calls `run_repo_pipeline` per task-file against the playground worktree with `target_file` = the task's file and `context_files` = its siblings; `.cs` context works today.
3. Grade via `dotnet build/test` output in the worktree; judge via evaluator pattern (fresh context + Verdict YAML); record benchmark metrics to `evals/results/`.
4. Multi-file tasks = multiple pipeline calls (one per file) for v0; multi-file write support is a later harness extension gated by measured need (complexity budget).
