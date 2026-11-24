# GitHub Project Setup — UFO WASH

This guide configures GitHub as the team's task board and persistent work memory.

## 1. Repository

Create or use the private repository for the Unity project. Upload this documentation pack to the repository root while preserving existing `Assets/`, `Packages/`, `ProjectSettings/` and `.meta` files. Do not replace the current Unity project with this documentation pack.

Suggested repository name: `UFO-Wash`.

## 2. Create a GitHub Project

Create a Project named **UFO WASH — Production Board** and link it to the repository.

Use these workflow statuses:

- Backlog
- Ready
- In Progress
- Review / Playtest
- Blocked
- Done

Suggested fields:

- Priority: P0 / P1 / P2 / P3
- Area: Gameplay / Art / UI / Economy / Automation / Architecture / Performance / Documentation
- Milestone: M0 / M1 / M2 / M3 / M4 / M5
- Build or commit: text field, where useful

## 3. Labels

Create the following labels:

**Type:** `type:feature`, `type:bug`, `type:tech-debt`, `type:research`, `type:art`, `type:documentation`  
**Priority:** `priority:P0`, `priority:P1`, `priority:P2`, `priority:P3`  
**Area:** `area:gameplay`, `area:art`, `area:ui`, `area:economy`, `area:automation`, `area:architecture`, `area:performance`

## 4. Milestones

Create milestones:

- M0 — Audit & Pre-production
- M1 — First-person Station
- M2 — Core Cleaning Loop
- M3 — Economy & Reinvestment
- M4 — First Automation
- M5 — Vertical Slice

Do not create a milestone for late-game expansion until the vertical slice has been accepted.

## 5. Issue discipline

Every meaningful task must have an Issue. The Issue must include the objective, evidence/current state, scope, non-scope, dependencies, acceptance criteria, test evidence and docs to update.

The `NEXT_TASK.md` file must point to the one authorized next task and its Issue number/link. The Project board is the visual queue; `NEXT_TASK.md` is the single-task handoff. Keep both synchronized.

When a task starts, move its issue to In Progress. When implementation is ready but needs local testing or owner feedback, move it to Review / Playtest. If required evidence is missing, move it to Blocked and explain why. Close only after acceptance criteria are verified and owner acceptance is recorded.

## 6. Pull requests and commits

Use a branch for substantial features or risky fixes. Small documentation-only changes can be committed directly if the repository policy allows it. Reference the Issue in the PR. Include screenshots/recording for visual or gameplay changes and test evidence in the PR template.

## 7. Canonical memory rule

GitHub issues, commits and versioned Markdown files are the durable memory. ChatGPT chat history is supporting context only. At the beginning of each task, the agent must read the repository memory. At the end, it must update it.
