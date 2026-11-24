# 2026-11-24 — Arrival sequence and compile fix

- Corrected `Sign` to return its generated object, fixing CS0815 at the bay recovery label.
- Customer ships now glide, bank and light their docking beacon during arrival/departure while preserving the existing docked-state gate.

# 2026-11-24 — Starter bay visual overhaul

- Added procedural panel/rock surface materials and a warm/cool lighting rig during Play; existing scene materials/settings are restored on Stop.
- Added scaled Kenney generator, pump, relay and drum models to dress the starter bay.
- Reframed the title screen and rebuilt the gameplay HUD with rounded panels, live supply meters, a selected-tool hotbar and targeting brackets.
- Unity import and visual acceptance remain unverified in this workspace.

# 2026-11-24 — Direct Unity Play entry

- Removed the scene-generation menu step from normal testing: Play automatically opens the existing main-menu scene.
- Service access/detail additions are applied in memory for the Play session and cleaned up on Stop, without regenerating scene or material assets.
- Kept the separate scene generator as an optional developer tool. Licensed Unity runtime verification remains blocked.

# 2026-11-24 — UI entry and optional tooling cleanup

- Added a persistent two-stage Bay Recovery annex: lower water/foam use and a visible Kenney CC0 room expansion.
- Shift start now goes directly into gameplay; Story and How to Play remain optional.
- Added a Tab toggle for the hull checklist and updated the HUD hint.
- Added an optional five-minute, +45 GC express bonus to the quick contract; late completion has no penalty.
- Removed optional standalone checks, Editor validators, QA-plan documents and generated audit/check output.
- Preserved the existing authored scenes, free CC0 assets and test-branch workflow. Unity visuals and gameplay remain unverified here.

# 2026-11-24 — Cleaning and first-contract recovery source

- Replaced whole-panel tint and independent foam objects with a bounded local coating overlay whose area-weighted cells drive panel progress.
- Unified spray/camera targeting; tightened stage/supply/input guards, interaction range and final tank-fraction handling.
- First equipment purchase now unlocks a switchable fan nozzle; cleaning power affects actual work.
- Added original prototype tool pressure audio, readable zone/receipt/tool HUD feedback and removed accidental R-key job reset.
- Builder changes add service-bay detail in a separate preview; asset import no longer overwrites the main scene.
- The former optional source-check tool and Editor rule fixture were removed at the owner's request; neither was used by gameplay.
- Downloaded exact Unity 6000.6.10f1 Editor; version command succeeds. Isolated import/licensing verification remains separately reported in current status. No player build or gameplay screenshots produced; no runtime or visual acceptance claimed.
- Preserved authored scenes, existing metadata, engine and package versions. Updated project status and owner handoff notes.

# 2026-11-24 — Imported-project orientation

- Confirmed the owner wants continued UFO WASH development toward Steam-quality gameplay, not a web conversion or a run-only setup.
- Recorded the current environment's Unity verification limitation and recommended an integrated quality-recovery milestone followed by visible tycoon progression.
- Documentation and task recommendations only; no gameplay implementation, compilation, Editor test or player build performed.

# Changelog

All notable project changes should be recorded here. Use newest entries first. Keep this file factual; distinguish implemented, tested and unverified changes.

## [Unreleased] — Documentation baseline

- Created the initial GitHub-ready UFO WASH documentation structure.
- Established the project charter: UFO WASH remains the game; the improvement target is gameplay depth, presentation, progression and automation.
- Added the initial GDD, production plan, technical design and visual/audio direction; optional test-plan documents were later removed at the owner's request.
- Added persistent agent workflow, task prompt, current status, next task, decision log, lessons learned and asset register.
- Added GitHub issue and pull-request templates.
- No Unity implementation was changed by this documentation pack.

## 2026-11-24 — Persistent company progression and first attendant

- Added a versioned company ledger for credits, reputation, supplies, completed jobs, washer levels, attendant hiring and sector claims. The save rotates a backup, restores a valid backup when possible and blocks writes if all copies are invalid.
- Added a hireable Vesper wash tech for 120 GC, matching the GDD's prototype hiring target. When hired, the tech pre-rinses only the active contract's hull panels and visibly reports idle, working or blocked-on-water status.
- The Play Mode hook and optional scene builder use the included Kenney CC0 astronaut model for the tech when available, with an explicitly temporary primitive fallback. Play injection leaves the authored scene untouched.
- Restored washer upgrades and sector gates from the ledger; preserved earlier sector claims stored in PlayerPrefs.
- Active contracts do not resume after reload; finished company progression does.
- Source implementation only. Unity import, scene regeneration, C# compilation, Play Mode, save corruption tests and visual verification remain outstanding.
- Existing Unity project status remains unverified pending UW-000 audit.
