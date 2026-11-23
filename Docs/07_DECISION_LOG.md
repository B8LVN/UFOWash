# UFO WASH — Decision Log

Record decisions that affect scope, architecture, design or production. Newest first. Do not silently change an approved decision; add a new entry that supersedes it and explain why.

| ID | Date | Decision | Status | Rationale / notes |
|---|---|---|---|---|
| D-001 | 2026-11-23 | The game remains UFO WASH; do not replace it with another simulator. | Approved | Owner explicitly reaffirmed the UFO Wash concept. The production problem is execution quality, depth, progression and automation—not the game's identity. |
| D-002 | 2026-11-23 | Use Moon Sweepers and Schedule I as high-level design references for tactile work, progression, reinvestment, hiring and automation. | Approved | Translate abstract principles into original UFO cleaning/service gameplay. No copying of protected assets, names, UI, levels or exact mechanics. |
| D-003 | 2026-11-23 | First-person 3D is the primary camera direction for the intended experience. | Proposed baseline | Confirm against the existing project and player-feel prototype. |
| D-004 | 2026-11-23 | Windows PC first; Android only after PC gameplay validation. | Approved baseline | Avoid splitting early development across platforms. Steam is a future distribution target. |
| D-005 | 2026-11-23 | Single-player first; multiplayer is not part of initial scope. | Approved baseline | Protect the first playable scope and technical budget. |
| D-006 | 2026-11-23 | Unity 6.6 (6000.6), URP and C# are the current technical baseline. | Approved baseline | Use the exact installed patch and verify package compatibility. Do not upgrade/downgrade without approval. |
| D-007 | 2026-11-23 | Git + GitHub is the canonical version control and project memory system; do not use Unity Version Control in parallel. | Approved | The user selected Git/GitHub. |
| D-008 | 2026-11-23 | Audit the current Unity implementation before major rewrites or new feature development. | Approved | Current implementation has been reported as poor/primitive/unplayable, but actual project evidence must be inspected before assigning root causes. |
| D-009 | 2026-11-23 | The first vertical slice must prove a satisfying manual cleaning loop, meaningful reinvestment and one small automation payoff. | Proposed baseline | To be approved after current-project audit and prototype scoping. |

## Open decisions

| ID | Question | Needed by | Options / evaluation criteria | Status |
|---|---|---|---|---|
| O-001 | What exact dirt visualization technique? | Core cleaning prototype | Visual clarity, performance, UV quality, authoring cost, multiple craft support | Open |
| O-002 | Which first-person controller/camera implementation? | Player feel prototype | Responsiveness, control, maintainability, Unity 6.6 compatibility | Open |
| O-003 | What is the final art style and asset pipeline? | Station art production | Cohesion, visual quality, affordability, asset license, performance | Open |
| O-004 | What is the first automation payoff? | Vertical-slice scope | Visible, useful, understandable, low-risk implementation | Open |
| O-005 | What exact first job/service sequence? | Cleaning prototype | Fun, tactile, easy to understand, supports progression | Open |
