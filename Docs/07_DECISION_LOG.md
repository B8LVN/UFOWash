# UFO WASH — Decision Log

Record decisions that affect scope, architecture, design or production. Newest first. Do not silently change an approved decision; add a new entry that supersedes it and explain why.

| ID | Date | Decision | Status | Rationale / notes |
|---|---|---|---|---|
| D-019 | 2026-11-24 | Persist company ledger state in a versioned JSON save with a backup; do not resume an in-progress contract after reload. | Implemented — unverified | Preserve reinvestment across sessions without serializing scene references; damaged data must remain recoverable and must not be silently overwritten. |
| D-018 | 2026-11-24 | Use one local Unity project/repository with two branches: main is user-facing; develop/test on ufo-wash/cleaning-recovery and merge only after the owner's explicit OK. | Approved by owner | Owner rejected separate project copies. Switch branches in the existing project folder; successful checks or a dev push do not authorize merging main. |
| D-017 | 2026-11-24 | Normal owner testing must be Play → main menu → start game, with no generation menu step. | Owner requested; implemented — unverified in Unity | Select the existing main scene automatically and apply temporary service access in Play rather than overwriting the authored scene. |
| D-014 | 2026-11-24 | Prototype UV-independent local coating cells for the existing static hull, using one state for visible dirt/foam and panel summaries. | Implemented — unverified in Unity | Existing procedural saucer has no authored UVs. Locality and weighted totals pass deterministic checks; the technique remains provisional until curved-hull visual/performance comparison. |
| D-015 | 2026-11-24 | Never replace the authored scene merely because builder source was imported; offer a separate quality preview and require explicit confirmation for canonical replacement. | Implemented — unverified in Unity | Preserves owner scene work and keeps source import separate from visual acceptance. |
| D-016 | 2026-11-24 | First equipment purchase unlocks a switchable fan nozzle rather than only hidden numeric power. | Implemented — unverified in Unity | Meaningful reinvestment must visibly change the next cleaning job; retain the narrow footprint as an option. |
| D-012 | 2026-11-23 | Establish a title menu, Story entry, first-shift briefing and accessible How to Play tutorial before gameplay. | Implemented — unverified | Owner explicitly requested a game opening and onboarding instead of dropping directly into an unexplained demo scene. Current narrative is a draft for feedback. |
| D-013 | 2026-11-23 | Generated gameplay scenes must use named top-level Hierarchy groups for world, equipment, vessel, customers, player, systems, lighting and UI. | Implemented — unverified | Scene organization is a maintainability requirement, not optional editor tidiness. Preserve world transforms when grouping roots. |
| D-010 | 2026-11-23 | Prioritize a coherent, playable first customer job over adding disconnected features. | Approved by owner request | Owner explicitly asked for a substantial development pass and a testable result; stop expanding until this pass is playtested. |
| D-011 | 2026-11-23 | Prototype local foam patches, a selectable three-offer contract board and a reactive customer NPC. | Implemented — unverified | These changes directly address owner feedback; visual feel and interaction quality must be accepted in Play Mode. |
| D-001 | 2026-11-23 | The game remains UFO WASH; do not replace it with another simulator. | Approved | Owner explicitly reaffirmed the UFO Wash concept. The production problem is execution quality, depth, progression and automation—not the game's identity. |
| D-002 | 2026-11-23 | Use Moon Sweepers and Schedule I as high-level design references for tactile work, progression, reinvestment, hiring and automation. | Approved | Translate abstract principles into original UFO cleaning/service gameplay. No copying of protected assets, names, UI, levels or exact mechanics. |
| D-003 | 2026-11-23 | First-person 3D is the primary camera direction for the intended experience. | Proposed baseline | Confirm against the existing project and player-feel prototype. |
| D-004 | 2026-11-23 | Windows PC first; Android only after PC gameplay validation. | Approved baseline | Avoid splitting early development across platforms. Steam is a future distribution target. |
| D-005 | 2026-11-23 | Single-player first; multiplayer is not part of initial scope. | Approved baseline | Protect the first playable scope and technical budget. |
| D-006 | 2026-11-23 | Unity 6.6 (6000.6), URP and C# are the current technical baseline. | Approved baseline | Use the exact installed patch and verify package compatibility. Do not upgrade/downgrade without approval. |
| D-007 | 2026-11-23 | Git + GitHub is the canonical version control and project memory system; do not use Unity Version Control in parallel. | Approved | The user selected Git/GitHub. |
| D-008 | 2026-11-23 | Audit the current Unity implementation before major rewrites or new feature development. | Completed for source audit | Repository source was audited before this focused recovery pass. Runtime and visual verification remain outstanding. |
| D-009 | 2026-11-23 | The first vertical slice must prove a satisfying manual cleaning loop, meaningful reinvestment and one small automation payoff. | Proposed baseline | To be approved after current-project audit and prototype scoping. |

## Open decisions

| ID | Question | Needed by | Options / evaluation criteria | Status |
|---|---|---|---|---|
| O-001 | What exact dirt visualization technique? | Core cleaning prototype | Visual clarity, performance, UV quality, authoring cost, multiple craft support | Open |
| O-002 | Which first-person controller/camera implementation? | Player feel prototype | Responsiveness, control, maintainability, Unity 6.6 compatibility | Open |
| O-003 | What is the final art style and asset pipeline? | Station art production | Cohesion, visual quality, affordability, asset license, performance | Open |
| O-004 | What is the first automation payoff? | Vertical-slice scope | Visible, useful, understandable, low-risk implementation | Open |
| O-005 | What exact first job/service sequence? | Cleaning prototype | Fun, tactile, easy to understand, supports progression | Open |
