# UFO WASH — Changelog

## 2026-11-23 — Playable service-loop pass

- Repaired generated material creation so existing project materials are reassigned to URP/Lit instead of retaining incompatible shaders.
- Replaced the previous over-the-shoulder camera with a first-person camera and dedicated washer viewmodel.
- Moved the spray emitter to the first-person tool muzzle.
- Added three ordered cleaning modes: pre-rinse, foam application and final rinse.
- Added panel-level foam state and visible color response alongside contamination reduction.
- Added water and chemical consumption, paid supply refill and service-stage progression.
- Added service stage and supply readouts to the HUD.
- Expanded GDD, TDD, MVP and roadmap around the UFO WASH business fantasy and acceptance gates.

**QA status:** source committed; Unity compile, Play Mode, visuals and Windows build still require local verification. No claim of tested behavior is made here.


## 2026-11-23 — Workflow, customer and inspection pass

- Added automatic regeneration of the dedicated Power Slice scene when the scene-builder source itself is reimported after a pull. The generated scene opens automatically; gameplay-script-only changes do not rebuild it.
- Renamed the manual command as a fallback instead of a required step.
- Disabled a competing legacy prototype auto-builder.
- Replaced single-slot imported-ship material assignment with per-slot URP-safe material assignment to prevent leftover pink submaterials.
- Changed dirt/foam rendering to MaterialPropertyBlock tinting so the ship's base material identity is retained instead of replaced by a flat brown/cyan material.
- Added customer ship arrival, docking, departure and return.
- Added a final inspection action at the contract terminal; the player chooses when to submit after meeting cleanliness/residue thresholds.
- Added inspection grades and a quality-based payout bonus.
- Added a small physical spray radius for more forgiving cleaning.
- Added an Editor structural QA validator.

**QA status:** committed source only. Unity compilation, automatic scene refresh callback, Play Mode, visual result and build remain unverified locally.


## 2026-11-23 — Job feedback and technical pass

- Added three visibly distinct contamination profiles that tint the authored hull material and clear back to the original ship finish.
- Kept the vertical slice honest about its single currently authored UFO class rather than advertising nonexistent ship prefabs.
- Added an Editor validator for scene wiring, active URP, material shader compatibility, missing scripts and washable surface count.
- Replaced per-frame scene searches for cleanable surfaces with an active-surface registry.
- Expanded the washer upgrade to increase cleaning power, spray coverage radius and working range.
- Tuned customer approach distance to keep the ship clear of the rear bulkhead.

**QA status:** committed source only; Unity compilation, callback behavior, Play Mode and Windows build still need local verification.


## 2026-11-23 — Final interaction corrections

- Made the cockpit an actual washable surface in the procedural fallback ship.
- Changed spray hit handling so decorative ship hardware does not block the washable hull behind it.
- Kept environment colliders as physical blockers; the washer still cannot clean through the hangar.


## 2026-11-23 — Core gameplay recovery pass

- Added local persistent foam patches at the actual spray contact point; final rinse fades and removes the patches.
- Passed spray hit position, normal and radius into the surface treatment so foam follows nozzle contact rather than tinting the entire renderer.
- Added three selectable service offers with different briefs, contamination profiles and payouts.
- Added a full-screen contract selection board built with uGUI and the Input System UI module.
- Suspended first-person movement and world interaction while the contract board is open; Escape closes the board.
- Added an interactive customer NPC with idle attention, a world-space speech bubble, talk interaction and reactions to contract acceptance/completion.
- Attached the customer behaviour to the imported Kenney alien or procedural fallback.
- Extended the structural validator to require the interactive customer.

**QA status:** source committed to main. Static source review only; Unity compilation, Play Mode, foam visibility, UI input, NPC interaction and Windows build remain unverified. Owner playtest is the next gate.
