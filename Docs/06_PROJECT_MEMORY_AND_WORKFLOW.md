# UFO WASH — Persistent Project Memory & Agent Workflow

## 1. The repository is the memory

ChatGPT conversations are not a reliable permanent project database. The GitHub repository is the canonical source of truth. Every development session must reconstruct context from the repository before acting.

## 2. The five memory anchors

These files must always remain current:

1. `NEXT_TASK.md` — one clearly defined next task.
2. `docs/11_CURRENT_STATUS.md` — verified current state, current milestone and blockers.
3. `docs/07_DECISION_LOG.md` — approved decisions and unresolved decisions.
4. `docs/08_LESSONS_LEARNED.md` — mistakes, root causes and prevention rules.
5. `CHANGELOG.md` — factual record of meaningful changes.

The GDD, TDD, QA strategy and visual bible define the intended product and quality bar.

## 3. Mandatory start-of-task sequence

Every new task/session must:

1. Read `AGENT_START_HERE.md`.
2. Read `NEXT_TASK.md` and confirm the authorized task.
3. Read `CURRENT_STATUS.md`.
4. Read relevant GDD/TDD/visual/QA sections.
5. Read decision log and lessons learned.
6. Inspect recent Git commits, working tree and related files.
7. Read linked GitHub Issues and previous reports.
8. Summarize the current facts and intended scope before implementation.

If the repository is inaccessible, request a ZIP or relevant files. Do not pretend memory or access exists.

## 4. Mandatory end-of-task sequence

Before a task is complete:

- Update the current status.
- Update the next task.
- Update the backlog.
- Add a changelog entry.
- Record new decisions.
- Record meaningful lessons/root causes.
- Update bug/test/asset reports as appropriate.
- Update the GitHub Issue with evidence and remaining risk.
- Commit changes only if actually able and after checking staged files.

## 5. GitHub as a task memory system

Use GitHub Issues for specific tasks and bugs. Use a GitHub Project board for workflow state. Keep a single active primary task unless two tasks are independent.

Every issue must contain:
- Why the task matters.
- Current observed behaviour.
- Expected behaviour.
- Scope and non-scope.
- Dependencies.
- Acceptance criteria.
- Test/evidence requirements.
- Documentation files to update.

Suggested issue states: Backlog, Ready, In Progress, Review / Playtest, Blocked, Done.

Do not close issues because code was generated. Close only after acceptance criteria and evidence are complete and owner acceptance is recorded.

## 6. Preventing forgotten context

At the start of a new ChatGPT conversation, paste `AGENT_TASK_PROMPT.md` and attach/provide repository access. The agent must read the docs and Git history again. Never assume the previous chat's context is present.

At the end of every response, provide a handover summary and the next task. The same summary must be written to the repository.

## 7. Learning from failure

When a feature fails or produces poor quality:

1. Record what was expected and what actually happened.
2. Preserve screenshots/logs/build/commit references.
3. Identify root cause only when supported by evidence.
4. If root cause is uncertain, record hypotheses and confidence.
5. Add a prevention rule or test so the same issue is less likely to recur.
6. Verify whether the corrective change actually fixed the issue.

Do not write vague lessons such as “improve quality.” Write actionable rules, for example: “Do not approve a cleaning effect until a 30-second capture shows dirt disappearing only under the spray cone on the target mesh, with no visible UV seams at the approved camera distance.”

## 8. Status vocabulary

Use only clear states:

- `Not started`
- `In progress`
- `Implemented — unverified`
- `Verified in Editor`
- `Verified in build`
- `Accepted by owner`
- `Blocked`
- `Deprecated`

Do not use `Done` without evidence and acceptance.
