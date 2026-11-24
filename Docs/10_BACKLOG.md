# UFO WASH — Full-Game Production Backlog

## Latest quality-recovery gate — 2026-11-24

- **Implemented — unverified in Unity:** local area-weighted dirt/foam overlay, tool pressure audio, shared camera/spray targets, guarded input, proportional supplies, switchable fan purchase, stage-aware checklist/receipt and separate starter-bay preview source.
- **Verified outside Unity:** 14 deterministic production coating-state checks. This does not move the P0 gameplay gates to verified.
- **Blocked:** licensed Unity import, two-contract rule fixture, actual authored-scene Play Mode/screenshots and visual/audio/performance review. Exact Editor downloaded and version command succeeded; see current status for licensing retry.
- **Next:** finish this verification gate and resolve defects. Saveable station growth/first-worker work is separately assigned, not implemented here.
- Evidence and acceptance steps: `Docs/reports/QUALITY_RECOVERY_2026-11-24.md`. Older entries below remain historical.

## 2026-11-24 — Import handover

Owner reaffirmed the Steam-game quality target and Moon Sweepers / Schedule I / Aquapark Tycoon references. Recommended first task combines current P0 stabilization with a focused P1 cleaning/control/starter-station recovery, judged by repeat jobs and meaningful reinvestment. Follow with persistent, physical tycoon progression. These are recommendations awaiting task selection, not implemented features. Unity Editor verification access must be established; none was demonstrated in this environment.

**Canonical design:** Docs/GDD.md, version 2.2.

**Product target:** Full commercial PC/Steam game. A vertical slice is a validation milestone, never the delivery target.

**Execution policy:** Progress autonomously across all six pillars; no paid assets without owner approval; no player builds until the game is playable and the owner asks for one.

## P0 — Stabilize current systems and prove the work

| Task | Status | Acceptance evidence |
|---|---|---|
| Bind wash work to the accepted ship and every service panel | Unity-reference C# compile passed; Editor import/play test pending | Each panel separately gates pre-rinse, foam and final rinse |
| Restrict FPS handoff to actively targeted contracted hull | Unity-reference C# compile passed; Play Mode pending | Third person while walking/interacting; first person only while actively cleaning correct ship |
| Customer tips, reputation and contract unlocks | Unity-reference C# compile passed; UI test pending | Disclosed terms; payout receipt and unlocks match quality |
| Full first-shift loop, supplies, grade, customer and repeated dispatch | Partial | Complete two different jobs without reset/developer intervention |
| Current Hierarchy and runtime Console | Needs fresh evidence | Screenshot of scene roots and clean runtime Console |

## P1 — Make Vesper feel like a place and cleaning feel tactile

- Create a coherent Vesper basin surrounding the service yard: terrain silhouette, distinctive sky, landmarks, roads, plot boundaries, ambient life and weather.
- Replace obvious placeholder forms with authored modular hangar architecture, signage, clamps, pumps, piping, service cranes and readable ship silhouettes.
- Improve local grime response so spray clears visible areas rather than tinting a whole zone; add contamination-layer materials, foam/runoff, audio and impact feedback.
- Replace the current debug-like HUD/contract screens with responsive, typographic service interfaces.
- Add exploration landmarks, customer waiting behavior and clear transitions between world movement and close cleaning view.

**Gate:** current Unity screenshots must show a visibly art-directed alien service world, not a greybox called finished.

## P2 — Turn the station into a management game

- Saveable company ledger: GC, reputation, inventory, active contract, unlocks and expenses.
- Contract board with ship/customer archetypes, recipes, deadlines/preferences and transparent gross/net quote.
- Physical land plot ownership, snapping placement, collision/access checks and visible building upgrades.
- Power/fluid network with capacities, routing, load, outages and readable failure recovery.
- Residue Reclaimer and Rare Residue resource/crafting/sales.
- Customer queue, lounge comfort and dispatch/checkout pipeline.
- Staff roster with hiring, wages, shifts, assignments, skills, fatigue, morale and visible NavMesh task loops.

## P3 — Deliver the tycoon fantasy and commercial game

- Automated wash gantry, drone logistics, recipe/work-order configuration, maintenance, throughput and safe fallback.
- Additional bays, buildings, six authored ship classes, alien customer archetypes, contamination types and specialist contracts.
- Four playable Vesper sectors, weather hazards, land expansion and late-game prestige fleet traffic.
- Boss office for prices, service tiers, shift plans, sector acquisition and network bottlenecks.
- Audio, accessibility, input settings, save migration, performance and Steam-facing QA/presentation.
- Repeated owner-facing review of actual screenshots/gameplay; only the owner decides when the game is playable enough for a player build.

A task is complete only with checked implementation and evidence. Code written, scene-generated or committed is not runtime verification.
