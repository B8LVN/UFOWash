# Mandatory Agent Start Procedure

**Read this before every development session. No exceptions.**

## Step 1 — Load the repository's memory

Read these files in this order:

1. `PROJECT_CHARTER.md`
2. `NEXT_TASK.md`
3. `Docs/11_CURRENT_STATUS.md`
4. `Docs/01_GAME_DESIGN_DOCUMENT.md`
5. `Docs/02_PRODUCTION_PLAN.md`
6. `Docs/03_TECHNICAL_DESIGN.md`
7. `Docs/04_VISUAL_AUDIO_QUALITY_BIBLE.md`
8. `Docs/05_QA_TEST_STRATEGY.md`
9. `Docs/06_PROJECT_MEMORY_AND_WORKFLOW.md`
10. `Docs/07_DECISION_LOG.md`
11. `Docs/08_LESSONS_LEARNED.md`
12. `Docs/10_BACKLOG.md`
13. Relevant GitHub Issues, recent commits, changelog entries and files related to the assigned task.

Do not rely on ChatGPT conversation history as the only source of truth. The repository is the canonical project memory.

## Step 2 — Inspect the real project state

Before editing anything:

- Inspect `git status`, recent commits and the current branch.
- Inspect the actual Unity project structure and relevant files.
- Check whether there are uncommitted user changes. Never overwrite them.
- Check `ProjectVersion.txt`, `Packages/manifest.json` and `packages-lock.json` where available.
- Check the current scenes, prefabs, scripts, materials and assets related to the task.
- Read linked bug reports, screenshots, logs and previous audit reports.
- Distinguish verified facts from assumptions. Never claim to have inspected files that were not provided or accessible.

## Step 3 — Confirm task and scope

Read the single task in `NEXT_TASK.md` and its GitHub Issue.

If there is no clear task, ask the project owner which task to authorize. Do not invent a large task.

Before implementation, state briefly:
- What the task is intended to achieve.
- What existing systems/files it touches.
- What will not be changed.
- Acceptance criteria.
- Risks/dependencies.

Do not start a second feature while a P0 bug or a blocked acceptance criterion affects the current milestone.

## Step 4 — Implement and verify

- Follow the GDD, technical design and approved decisions.
- Prefer focused, integrated changes over disconnected scripts.
- Preserve existing functionality unless the task explicitly changes it.
- Do not silently replace assets or remove functionality.
- Provide exact file paths and complete files/patches.
- Test what can actually be tested. If Unity Editor execution is unavailable, say so and provide exact local test steps.
- Never mark a feature verified merely because code was generated or compilation is assumed.

## Step 5 — Mandatory end-of-task memory update

Before calling a task complete, update the relevant repository records:

- `Docs/11_CURRENT_STATUS.md` — current verified state and blockers.
- `NEXT_TASK.md` — one clear next task, owner and dependencies.
- `CHANGELOG.md` — meaningful changes and test status.
- `Docs/10_BACKLOG.md` — status, priority and next work.
- `Docs/12_IMPROVEMENT_REGISTER.md` — problems and verified improvements.
- `Docs/13_SESSION_LOG.md` — session history and handover.
- `Docs/07_DECISION_LOG.md` — only when a decision was made or changed.
- `Docs/08_LESSONS_LEARNED.md` — when a mistake, root cause or reusable lesson was discovered.
- `Docs/09_ASSET_REGISTER.md` — when assets are added, removed or replaced.
- QA reports / test cases / bug records — when relevant.

Update the GitHub Issue with implementation summary, files changed, evidence, test results and remaining risks. Append a session entry to `Docs/13_SESSION_LOG.md` and update relevant rows in `Docs/12_IMPROVEMENT_REGISTER.md`. Close it only when all acceptance criteria are verified and the project owner accepts it. Otherwise leave it open or move it to Review/Blocked.

## Step 6 — Final handover

Finish with a concise handover:

1. What changed.
2. Files created/modified.
3. What was actually tested and the results.
4. What could not be tested and why.
5. Known issues or risks.
6. Documentation updated.
7. Commit/branch/Issue reference, if actually created.
8. Exact next task.

## Absolute prohibitions

- Do not change the game from UFO WASH.
- Do not restart or rewrite the project without an audit and explicit approval.
- Do not make the game more generic to simplify implementation.
- Do not call primitive greybox art “polished.”
- Do not claim a build, test, GitHub upload, commit or issue update happened unless it actually happened.
- Do not let chat memory replace repository memory.
- Do not finish a task without updating `NEXT_TASK.md` and `Docs/11_CURRENT_STATUS.md`.
