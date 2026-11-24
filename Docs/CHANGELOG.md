# 2026-11-24 — Direct development and optional QA cleanup

- Added a two-tier, saved Bay Recovery purchase with real supply savings and
  visible Kenney CC0 annex growth; Unity behavior and visuals remain unverified.
- Removed the optional standalone test runner, Editor validators, QA-plan/recovery
  documents, unused bug-report templates and generated check/audit output. None
  were runtime dependencies; ordinary play verification remains necessary.
- Simplified the title flow so starting a shift is immediate while story/manual
  pages remain optional. Added a Tab toggle for the hull checklist.
- Added a disclosed 5-minute, +45 GC express bonus to Quick Decontamination;
  missing it does not reduce other rewards or reputation.
- Updated project handoff documents and the setup helper to match the new workflow.
- Kept the changes on the approved test branch; Unity visual/gameplay review still
  requires the owner's licensed Editor.

# 2026-11-24 — Cleaning and station-growth source update

Local area-weighted dirt/foam, guarded camera/spray/input, supply-fraction limits,
switchable fan purchase, effective power, readable service HUD/receipt and original
prototype tool audio are implemented in source. The test branch also includes a
versioned company ledger, a hireable wash tech and persistent starter expansion.
Builder adds temporary Play-mode bay details and does not replace authored scenes.

Source-level checks passed, but no licensed Unity import, Play Mode run or gameplay
screenshots were produced in this workspace. The owner can test the branch directly.

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

**Verification status:** source committed; Unity import, Play Mode, visuals and Windows build were not verified here.


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
- Added an optional Editor scene validator; it was later removed because it was not needed by gameplay.

**Verification status:** committed source only. Unity compilation, automatic scene refresh callback, Play Mode, visual result and build remain unverified locally.


## 2026-11-23 — Job feedback and technical pass

- Added three visibly distinct contamination profiles that tint the authored hull material and clear back to the original ship finish.
- Kept the vertical slice honest about its single currently authored UFO class rather than advertising nonexistent ship prefabs.
- The optional Editor validator for scene wiring was later removed at the owner's request; gameplay does not depend on it.
- Replaced per-frame scene searches for cleanable surfaces with an active-surface registry.
- Expanded the washer upgrade to increase cleaning power, spray coverage radius and working range.
- Tuned customer approach distance to keep the ship clear of the rear bulkhead.

**Verification status:** source only; Unity compilation, callback behavior, Play Mode and Windows build still need owner verification.


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
- The earlier structural validator was extended to cover the interactive customer, then removed as optional tooling.

**Verification status:** source was committed. Unity compilation, Play Mode, foam visibility, UI input, NPC interaction and Windows build remain unverified. Owner playtest is the next gate.


## 2026-11-23 — Opening experience and scene organization

- Fixed the Unity exception caused by constructing a MaterialPropertyBlock in a static field initializer. The shared block is now lazily created from the runtime rendering path.
- Added a title menu over the paused 3D station, with Story, Start First Shift and How to Play entry points.
- Added a first-shift narrative briefing introducing Bay 01, the Vesper Belt, the station's financial situation and the first customer dispatch.
- Added a field manual covering movement, interaction, washer modes and the first service sequence.
- Clock In starts the simulation, restores the first-person controls and hides the menu.
- Organized generated scene roots into eight named Hierarchy groups to make the project navigable.

**Verification status:** source committed only. Unity compile, scene regeneration, UI clicks, pause/resume, hierarchy output and Play Mode remain unverified. The environment is still a greybox/temporary art pass and is not being represented as finished.


## 2026-11-23 — Local Unity stabilization check

- Split scene-attached MonoBehaviours into matching files and repaired current Power Slice script references.
- Suppressed world labels, HUD and customer dialogue while the title screen is open.
- Observed a clean title screen in Unity 6.6.10f1 Play Mode; stopped cleanly.

**Verification:** latest compile had no errors after its log entry. Obsolete API warnings remain; full job loop and foam rendering are unverified; hangar is still greybox.


## 2026-11-23 — Camera and NPC readability pass
- Set the player camera to third-person for station exploration and gated first-person view on an accepted wash job while aiming at a washable hull.
- Hide camera-mounted washer model in third-person and restore the spacer body; show the washer only in cleaning view.
- Apply safe teal/dark materials to imported alien renderer slots and lower/enlarge customer dialogue for easier reading.

**Verification:** Unity compilation completed without C# errors; title/briefing/tutorial flow and third-person gameplay were observed. First-person transition, NPC final materials/dialogue in scene, full cleaning job and visuals need further verification.

## 2026-11-23 — Contract depth, world blueprint and full-game scope

- Replaced the old Living GDD baseline with v2.1: full commercial PC/Steam scope, third-person planetary exploration, first-person only for contracted hull cleaning, four Vesper sectors, six UFO classes, facilities, utilities, staff wages/morale, customer traits, economy, progression and source-to-design truth table.
- Bound contract quality and stage progression to the customer ship’s own cleanable zones. Each panel must meet pre-rinse, foam and final-rinse criteria; unrelated surfaces cannot progress the job.
- Limited the close cleaning camera to an active contracted ship target and added a hull-zone service checklist to the HUD.
- Added named client traits, quality-gated tips, reputation thresholds, reputation-gated contract cards and NPC identity updates.
- Corrected contract card data refresh so the live board fills the selected client and job terms after opening.
- No player build was made during this milestone. The newest C# changes remain uncompiled/unverified in Unity until a safe editor check.

**Verification status:** source/doc edits only; no current Unity compile or full Play Mode run. The commercial game remains in active development.

## 2026-11-23 — Vesper outpost art and compact planet world

- Added Kenney Modular Space Kit 1.0: 40 free modular FBX pieces. Its CC0 license is stored beside the models in the repository.
- Authored a personalized graphite/steel, teal and amber derivative of the kit’s original color atlas; original source atlas and license are preserved.
- Extended the generated scene source with a walkable ramped causeway, pressure-gate opening, modular arrival gate/corridor/rooms, utility spine and approach beacons. The generated scene refresh now skips a dirty active scene rather than replacing unsaved editor work.
- Clarified the full-game world as four compact, connected 80 m districts with visible land gates, claimable 4 m plots and staged expansion costs. This is a dense business world, not a huge empty planet.
- Added three persistent land claim gates: Crater Ridge (2,500 GC / 2.6 stars), Ion Flats (9,000 GC / 3.5 stars) and Glasswake Reach (25,000 GC / 4.2 stars). A successful purchase opens the physical gate and reveals its sector content.

**Verification status:** CC0 files and derivative texture are present and inspected. Scene and sector code is written; because Unity currently has a dirty scene open, the automatic scene refresh was intentionally guarded. The new scene dressing and gate behavior have not yet been generated or visually verified in Unity. No player build was made.

## 2026-11-23 — Compact-world progression and Unity-reference compile check

- Bumped the Living GDD to v2.2 and locked the inspiration mapping: tactile layered washing, walkable owner-led business expansion, and customer/layout/throughput management.
- Added three physical land-claim gates with GC and reputation requirements, persistent unlocks and visible claimed-room content in the generated-scene source.
- Added the CC0 Kenney modular station kit and original Vesper palette to the outpost scene source. The world is framed as four adjoining 80 m districts rather than an oversized empty map.
- Bound service orders to the docked ship's zones and moved discovery to `Start` so Unity's `Awake` order cannot accidentally bind the global grime list.
- Compiled runtime and editor C# assemblies against Unity 6.6 generated project references: zero errors; the runtime compile had zero warnings and the editor compile reported six existing deprecation warnings.
- Did not produce a player build. Unity Editor import, generated-scene refresh, Play Mode, screenshots and in-game asset appearance remain unverified. Isolated batch-mode import was blocked during LicensingClient token validation.
