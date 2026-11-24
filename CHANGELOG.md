# 2026-11-24 — Direct Unity Play entry

- Removed the scene-generation menu step from normal testing: Play automatically opens the existing main-menu scene.
- Service access/detail additions are applied in memory for the Play session and cleaned up on Stop, without regenerating scene or material assets.
- Kept the separate scene generator as an optional developer tool. Licensed Unity runtime verification remains blocked.

# 2026-11-24 — Cleaning and first-contract recovery source

- Replaced whole-panel tint and independent foam objects with a bounded local coating overlay whose area-weighted cells drive panel progress.
- Unified spray/camera targeting; tightened stage/supply/input guards, interaction range and final tank-fraction handling.
- First equipment purchase now unlocks a switchable fan nozzle; cleaning power affects actual work.
- Added original prototype tool pressure audio, readable zone/receipt/tool HUD feedback and removed accidental R-key job reset.
- Builder changes add service-bay detail in a separate preview; asset import no longer overwrites the main scene.
- Added 14 passing engine-independent C# coating checks and an unexecuted Unity Play Mode rule fixture for two contracts, supplies, payout, reputation and purchase.
- Downloaded exact Unity 6000.6.10f1 Editor; version command succeeds. Isolated import/licensing verification remains separately reported in current status. No player build or gameplay screenshots produced; no runtime or visual acceptance claimed.
- Preserved authored scenes, existing metadata, engine and package versions. Updated production and QA handoff records.

# 2026-11-24 — Imported-project orientation

- Confirmed the owner wants continued UFO WASH development toward Steam-quality gameplay, not a web conversion or a run-only setup.
- Recorded the current environment's Unity verification limitation and recommended an integrated quality-recovery milestone followed by visible tycoon progression.
- Documentation and task recommendations only; no gameplay implementation, compilation, Editor test or player build performed.

# Changelog

All notable project changes should be recorded here. Use newest entries first. Keep this file factual; distinguish implemented, tested and unverified changes.

## [Unreleased] — Documentation baseline

- Created the initial GitHub-ready UFO WASH documentation structure.
- Established the project charter: UFO WASH remains the game; the improvement target is gameplay depth, presentation, progression and automation.
- Added GDD, production plan, technical design, visual/audio quality bible and QA strategy.
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
