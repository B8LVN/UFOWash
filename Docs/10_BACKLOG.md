# UFO WASH — Prioritized Backlog

Status must reflect reality. `NEXT_TASK.md` identifies the single task currently authorized.

## P0 — Blocking verification

| ID | Task | Status | Depends on | Notes |
|---|---|---|---|---|
| UW-000 | Audit existing Unity project and establish source baseline | Completed — source audit only | GitHub repository access | Report: `Docs/reports/PROJECT_AUDIT_001.md`. Local Editor/build/playtest evidence remains outstanding. |
| UW-101 | Owner playtest of foam, contract board and customer interaction recovery pass | Ready | Latest main pulled into Unity 6000.6.10f1 | Current authorized task. No feature expansion before results. |

## P1 — First playable vertical slice

| ID | Task | Status | Depends on | Notes |
|---|---|---|---|---|
| UW-100 | Recover the core job loop: local foam, selectable contracts and reactive customer | Implemented — unverified | UW-000 source audit | Code committed; owner must verify visuals, input and end-to-end job flow. |
| UW-001 | Approve vertical-slice scope and visual target from audit | Backlog | UW-000, UW-101 | Owner approval required after seeing the recovery pass. |
| UW-010 | First-person movement/camera feel | Existing source — needs playtest | UW-001 | Tune only from observed feedback. |
| UW-020 | Art-directed starter station and one service bay | Backlog | UW-001 | Current environment is still largely procedural geometry; greybox is not final art. |
| UW-030 | UFO arrival, job request and bay assignment | Partial | UW-010, UW-020 | Ship approach/docking exists; queue and bay assignment do not. |
| UW-040 | Dirt visualization benchmark and local tool interaction | Partial | UW-020 | Local foam prototype committed; dirt remains renderer-level tint. |
| UW-050 | Complete manual cleaning job loop and payout | Partial | UW-030, UW-040 | Stages, supplies and payout exist; owner playtest is required. |
| UW-060 | One meaningful upgrade and progression save | Partial / backlog | UW-050 | Numeric washer upgrade exists; no save/load proof. |
| UW-070 | One small employee task or automation machine | Backlog | UW-050, UW-060 | Choose only after manual loop is accepted. |
| UW-080 | Vertical-slice polish, QA and playtest | Backlog | UW-010–UW-070 | Target: understandable 10–15 minute experience. |

## P2 — Post vertical-slice expansion

- Additional UFO classes and service profiles.
- More tools, chemicals and cleaning methods.
- More service bays and larger facility layouts.
- Employee roles, schedules and task management.
- Additional automation stages and logistics.
- Supplies, maintenance and waste/by-products where they add decisions.
- More advanced contracts, customers and reputation.
- Additional properties/locations.

## P3 — Later / optional

- Android port.
- Multiplayer/co-op.
- Large-scale multi-location simulation.
- Advanced simulation of every worker/machine at high fidelity.
- Extensive narrative/campaign systems.

Do not promote P2/P3 work before the manual job loop is understood, enjoyable and accepted.
