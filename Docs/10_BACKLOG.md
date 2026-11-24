# UFO WASH — Full-Game Production Backlog

## Latest source status — 2026-11-24

- **Implemented — unverified in Unity:** local area-weighted dirt/foam overlay, tool pressure audio, shared camera/spray targets, guarded input, proportional supplies, switchable fan purchase, stage-aware checklist/receipt and separate starter-bay preview source. The title now starts directly; story/help are optional and the hull map toggles with Tab.
- **Also implemented — unverified in Unity:** versioned company save, first hireable attendant, persisted washer level and starter land claim.
- **Unity blocker:** this workspace has no valid Unity Editor license, so import, Play Mode, visuals, audio balance and save/reload behavior were not verified here.
- **Next:** continue direct source development on `ufo-wash/cleaning-recovery`; ask the owner for Unity review only when a new testable milestone is ready.

## 2026-11-24 — Import handover

Owner reaffirmed the Steam-game quality target and Moon Sweepers / Schedule I / Aquapark Tycoon references. The next direction is a small Vesper garage that grows through repeat jobs, visible upgrades and useful staff. Unity Editor verification remains unavailable in this workspace.

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

## P1 — Make Vesper feel like a place and cleaning feel tactile

- Create a coherent Vesper basin surrounding the service yard: terrain silhouette, distinctive sky, landmarks, roads, plot boundaries, ambient life and weather.
- Replace obvious placeholder forms with authored modular hangar architecture, signage, clamps, pumps, piping, service cranes and readable ship silhouettes.
- Improve local grime response so spray clears visible areas rather than tinting a whole zone; add contamination-layer materials, foam/runoff, audio and impact feedback.
- Continue replacing debug-like HUD/contract screens with responsive, typographic service interfaces; direct entry and the optional hull-map toggle are the first pass.
- Add exploration landmarks, customer waiting behavior and clear transitions between world movement and close cleaning view.

**Gate:** current Unity screenshots must show a visibly art-directed alien service world, not a greybox called finished.

## P2 — Turn the station into a management game

- **Implemented — unverified:** Versioned company save for credits, reputation, water/chemical, completed jobs, washer upgrades, the first hired wash tech and land claims. Active contracts reset on reload; Unity round-trip and corrupt-file recovery tests remain outstanding.
- **Implemented — unverified:** A 120 GC wash tech hire performs automatic pre-rinse on the active contracted hull zones and exposes working/idle/water-blocked states. Recurring wages are not simulated. Unity Play Mode, affordability and supply recovery checks remain outstanding.
- **Implemented — unverified:** Quick Decontamination offers +45 GC for passing inspection within five minutes. The deadline is disclosed, other contracts have no timer, and missing it has no payout penalty.

- Extend the company ledger with inventory beyond wash supplies, expenses and safe migration. Active contracts intentionally reset on reload; serializing their live hull state remains future work.
- Expand the contract board with more ship/customer archetypes, service recipes, optional preferences and a transparent gross/net quote.
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
- Audio, accessibility, input settings, save migration, performance and Steam-facing presentation.
- Repeated owner-facing review of actual screenshots/gameplay; only the owner decides when the game is playable enough for a player build.

A task is complete only with checked implementation and evidence. Code written, scene-generated or committed is not runtime verification.
