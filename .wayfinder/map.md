## Destination

A working multi-file, buggy .NET project in `ai-harness-playground`, pinned as the init eval version (v0) and wired to CI, that lets `ai-harness` evals discriminate single-agent vs multi-agent workflows where Hello World cannot.

## Notes

- Domain: `ai-harness` evals (`CONTEXT.md` terms: Task, Agent Session, Harness, Evaluator, Verdict, Retry, Complexity Budget; `evals/benchmark.md` frozen-set rule; `docs/evaluation.md` cheap-first order + Verdict format + metrics).
- Skills every session should consult: `domain-modeling` for terms (eval target vs Task set vs init version), `research` for harness-wiring facts against primary sources (`src/aiharness/agents/repo_runner.py`, `scripts/*.ps1`, `evals/*.md`).
- Standing preferences from charting: working project (not just spec); too-simple is the enemy; goal is to show where multi-agent wins or not; stack is dotnet; multi-file + buggy; working copy stays editable but evals always pin the init version; consumed by CI.
- Tracker: local-markdown (no issue tracker provided). Map is this file. Tickets are `.wayfinder/tickets/*.md`. Claim = set `assignee:` to yourself. `status: open|closed`. Blocking = `blocked-by:` list of ticket files; unblocked = all blockers closed; frontier = open + unblocked + unclaimed.

## Decisions so far

<!-- one line per closed ticket: - [<title>](tickets/<file>): <one-line gist> -->

- [Discriminative task shape](tickets/01-discriminative-task-shape.md): 5 frozen tasks (feature, bugfix, refactor, cross-file break as decider, helper); full candidate queue; fix>=1-at-<=2x-zero-regressions rule; build+test+acceptance signals
- [Dotnet target shape](tickets/02-dotnet-target-shape.md): API/Worker on .NET 10, xUnit, EvalTarget.sln layout serving 3-file ripple; caps <=8 files/~500 LOC/20 tests; gate build+test+diff, SDK 10 via CI
- [Buggy and multi-file seeding](tickets/03-buggy-seeding.md): v0 build-green/test-red (exactly 3 known); B1 Formatter logic, B2 cross-file Dto ripple, B3 DI runtime; hand-seeded, all visible, hidden split deferred
- [Harness wiring for dotnet](tickets/04-harness-wiring-dotnet.md): no harness rewrite; local-git prerequisite, per-file pipeline calls (.cs context works today), dotnet grade in worktree, Evaluator+Verdict unchanged → `research/harness-wiring-dotnet.md`
- [Init version pinning and evolution](tickets/05-init-version-pinning.md): tag `eval-v0` at build time, resolve SHA never HEAD; cosmetic edits free, src/test/task/bug changes force `eval-v1`; SHA in `.wayfinder/eval-pin.txt` + evals protocol, CI fetch-tags
- [CI eval loop](tickets/06-ci-eval-loop.md): workflow in ai-harness (`workflow_dispatch`, playground at eval-v0 SHA); v0 baseline vs coder+cleaner, N=1, retry 3; record `006-dotnet-<candidate>.md`, locked promotion rule; deepseek-v4-flash/opencode, 20-min timeout, setup-dotnet 10, verify secret

## Not yet specified

(none — way is clear; multi-file write support recorded as a future complexity-budget extension in Harness wiring for dotnet, out of v0)



## Out of scope

<!-- work ruled beyond the destination; closed, never graduates -->

- Perf / security / async-at-scale / LLM-judged readability discrimination: explicitly not required for v0 (per Discriminative task shape); returns only if destination is redrawn.
