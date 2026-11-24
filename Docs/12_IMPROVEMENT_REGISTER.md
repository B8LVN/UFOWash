# Import orientation — 2026-11-24

- Owner still reports the game's quality as unacceptable. This is owner feedback, not a new runtime diagnosis.
- Source inspection confirms whole-surface dirt tint remains a limitation; local foam patches do not prove local dirt removal.
- Recommended improvement: integrated cleaning, controls and starter-station recovery with actual Unity evidence before visual acceptance. Status: proposed, not implemented.

# UFO WASH — Improvement Register

Use this file to track recurring quality gaps, what must improve, what has changed and whether the improvement was actually verified. A code change alone is never verification.

## Status definitions

- **Observed:** User/tester reported the issue; root cause may be unknown.
- **Investigating:** Evidence collection/root-cause analysis is underway.
- **Planned:** An approved corrective task exists.
- **Implemented — unverified:** Code/assets changed, but proof is incomplete.
- **Verified:** The corrective change passed the stated verification method.
- **Accepted:** Owner reviewed and accepted the result.
- **Deferred:** Intentionally postponed with a reason.

## Improvement register

| ID | Area | Problem / desired improvement | Evidence / origin | Planned action | Verification method | Status | Related task |
|---|---|---|---|---|---|---|---|
| IMP-001 | Visual quality | Previous output was reported as visually poor/primitive. | Owner feedback; source audit found procedural primitive geometry in much of the hangar. | Art-direct one coherent bay with real props, decals, materials, lighting and sound. | Gameplay screenshots/video reviewed against visual bible. | Observed / Investigating | UW-020 |
| IMP-002 | Playability | The job loop still feels unclear and unsatisfying. | Owner's latest Play Mode report. | Test recovery pass; then tune manual job loop from observed evidence. | New tester completes one job without developer explanation and wants to do another. | Observed | UW-101 / UW-050 |
| IMP-003 | Progression | Spending must unlock genuinely new capabilities, not only small numeric boosts. | GDD pillar; old power value did not affect surface work. | First 200 GC purchase installs a switchable wide fan nozzle; power now changes work rate. | Before/after gameplay comparison and two-contract Play Mode test. | Implemented — unverified in Unity | UW-060 |
| IMP-004 | Automation | Automation must be visible, physical, understandable and diagnosable. | GDD pillar; no employee/automation implementation found in audited gameplay scripts. | Prototype one end-to-end stage after manual loop acceptance. | Demonstrate inputs, processing, output, throughput and blocked state. | Planned | UW-070 |
| IMP-005 | Project memory | Prevent lost decisions, defects and next actions between sessions. | Owner request and new documentation pack. | Keep repo canonical; update task, status, changelog and session log every pass. | New task starts from repo; handover is current. | Implemented — unverified | Workflow |
| IMP-006 | Cleaning feedback | Foam was only a whole-renderer tint; later patches faded globally. | Source audit; owner reported no visible foam. | Foam now persists on the same local cells as dirt and rinses only at contact. | Play Mode: persistent coverage, distant foam unchanged by local rinse, no blocked spray. | Implemented — unverified in Unity | UW-100 / UW-101 |
| IMP-007 | Contract selection | Player could accept only the default job from a basic prompt. | Source audit of old `UfoWashJobBoard.Interact`; owner said selector looked bad. | Add three selectable offers with brief, contamination and payout. | Board opens, cards select, acceptance starts selected job and closes UI. | Implemented — unverified | UW-100 / UW-101 |
| IMP-008 | Customer presence | Alien model was scenery and did not participate in the job. | Scene builder spawned model without NPC behaviour; owner report. | Add talk interaction, idle attention and accepted/completed reactions. | Player can talk; customer reacts to contract state; dialogue is readable. | Implemented — unverified | UW-100 / UW-101 |
| IMP-009 | Dirt localization | Dirt reduction affected an entire renderer rather than a local footprint. | Source audit of `UfoWashDirtSurface`; deterministic cell-state checks passed. | Experimental UV-independent coating overlay with local dirt/foam and area-weighted summaries. | Authored-saucer Play Mode closeups, access/seam review and performance comparison against masks. | Implemented — unverified in Unity | UW-040 |
| IMP-011 | Opening experience | The project drops directly into a bare prototype without a game identity, story context or tutorial. | Owner explicitly requested title menu, Story, introduction and tutorial. | Add title menu, first-shift briefing and How to Play flow before enabling player control. | Owner verifies all pages, button input, pause/resume and completes the first shift without external explanation. | Implemented — unverified | UW-102 |
| IMP-012 | Scene maintainability | Generated scene Hierarchy is difficult to understand and navigate. | Owner feedback; builder created many unrelated root objects. | Group scene roots into eight stable named subsystem containers while preserving world transforms. | Owner inspects generated Hierarchy and confirms objects are discoverable and grouped logically. | Implemented — unverified | UW-102 |
| IMP-010 | Audio feedback | Tool, ship and customer feedback lacked audio. | Source inspection. | Added original prototype pressure loop with water/foam/rinse character and flow/stop control; ship/ambience pass remains future work. | Listen in Play Mode; verify no flow on rejected work and no lingering loop on disable/menu. | Tool layer implemented — unverified; broader pass planned | UW-080 |
| IMP-013 | Progress persistence | Credits, supplies and upgrades can be lost on restart; corrupt files must not silently overwrite progress. | Source audit; PlayerPrefs covered land claims only. | Add a versioned company ledger with backup recovery and explicit blocked-save status. | Restart round-trip, missing/corrupt primary and invalid backup tests in Unity. | Implemented — unverified | Progression milestone |
| IMP-014 | First staff payoff | No worker performs a useful visible task in the starter service loop. | Source audit; staff were design-only. | Add a hireable tech that pre-rinses contracted hull zones and reports water blockage. | Verify 120 GC hiring cost, zone limits, stage gating, empty-water stop and refill recovery in Play Mode. | Implemented — unverified | Progression milestone |

## Completion rule

Keep original rows and update status/evidence; do not erase failed attempts. Record meaningful changes in `Docs/CHANGELOG.md` and `Docs/13_SESSION_LOG.md`.


## 2026-11-23 — Camera/NPC readability follow-up
- Issue: player camera could enter first-person on any mouse hold and the camera child tool stayed visible in third-person; alien materials and speech placement were poor.
- Change: gate close-up on accepted-job washable-surface aim; toggle body and tool renderers by mode; normalize alien materials; lower and enlarge dialogue canvas.
- Verification: compiled in Unity; observed opening and third-person player. First-person transition, alien material and dialogue final appearance still need focused visual verification.
