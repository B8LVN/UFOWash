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
