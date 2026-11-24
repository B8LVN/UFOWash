# Next Production Milestone

**2.0 direction:** Continue the small-planet garage-to-spaceport plan in
`Docs/GARAGE_TO_SPACEPORT_DIRECTION.md`. Direct development is in progress; Unity
testing is still needed for acceptance but is not a prerequisite for source work.

**Owner's branch rule:** One local Unity project/repository, two branches—not two project folders. `main` is user-facing. Develop and test on `ufo-wash/cleaning-recovery`; merge changes into `main` only after the owner explicitly says OK.

## Active handoff — UFO WASH 2.0 direct development

**Task:** Improve the existing Unity game's first-shift experience, service world, and tycoon progression without replacing UFO WASH.

**Status:** In progress on the owner-approved test branch. The current title flow and optional hull-map HUD update are source changes and still need owner-run Unity review.

**Owner:** Continue direct implementation on `ufo-wash/cleaning-recovery`; owner reviews playable results in Unity when available.

**Dependencies:** Preserve authored scenes and existing metadata. Do not wait for Editor access to work on independent source/documentation improvements.

**Next implementation:** Continue the garage/world and business-loop update, using existing CC0 Kenney assets where they fit; record new assets only after verifying their license.

**Download for testing:** the source is on GitHub branch `ufo-wash/cleaning-recovery` in `B8LVN/UFOWash`, not merged into `main`. Close Unity, preserve local edits, run `git fetch origin`, then `git switch ufo-wash/cleaning-recovery` and `git pull --ff-only`. Open Unity 6000.6.10f1 and press Play; no scene-generation command is needed.

Run Git commands inside the existing project's repository folder, not the Windows home folder. Close Unity and preserve uncommitted work before switching branches. Do not clone another project or add a second Unity Hub entry. If `git status` fails inside the actual project root, investigate whether that existing folder contains Git metadata before giving further commands.

An earlier session recorded a Linux Editor download and version check, followed by an import exit **198**: **No valid Unity Editor license found**. Those editor files and logs are not present in this checkout now; `generated-artifacts/` is empty. No licensed Unity Editor session is available here, so gameplay cannot be verified in this workspace.

Source now contains local dirt/foam, tool pressure audio, shared target acquisition, guarded input, a switchable fan purchase, supply limits, a versioned company ledger, a first attendant, persistent starter expansion, and a non-destructive bay preview. Optional test/validator tooling and its standalone reports were removed at the owner's request. The title starts directly, story/help are optional, the hull map toggles with Tab, and the quick contract has a no-penalty rush bonus.

**Normal testing path: press Unity Play → main menu → start the shift. No UFO WASH/Create command is required.** The Editor start-scene hook selects the existing main-menu scene and adds the new service access in memory for that Play session, then cleans it up on Stop. This direct-entry source change still needs licensed Play Mode verification.

Preserve authored scenes and existing `.meta` files. The separate preview generator is an optional developer tool, not an owner setup step. No player build, paid asset or main-branch merge is authorized by this handoff. Keep building on the test branch; owner-run Unity review remains necessary before claiming gameplay acceptance.

---

## Historical handoff (superseded by the active handoff above)

## 2026-11-24 — Imported-project direction

The owner wants continued development toward a Steam-quality UFO WASH inspired by Moon Sweepers, Schedule I and Aquapark Tycoon, not merely a runnable import. Recommended next work is an integrated playable quality-recovery pass before expanding the tycoon systems. This recommendation awaits task selection; no gameplay changes or new runtime verification occurred during import orientation.

This checkout has no Unity Editor command on PATH. Establish an accessible Unity 6000.6.10f1 Editor verification route before claiming gameplay, visual or performance acceptance. Historical references below to an open local Editor or dirty scene are not observations of this environment. Preserve authored scenes and the no-player-build policy.

**Task ID:** UW-104  
**Title:** Contracted hull zones, client satisfaction and service economy  
**Product scope:** Full commercial single-player PC/Steam game. The first shift is one milestone inside that game.  
**Status:** Runtime/editor C# compile checks passed against Unity 6.6 references; Unity import, scene regeneration and gameplay verification pending.

## Work shipped in source this turn

- Bound the career loop to the arriving customer ship’s cleanable surfaces, with a safe scene fallback.
- Require each hull zone to meet pre-rinse, foam and final-rinse targets before stage progression or inspection.
- Restrict cleaning-view activation and spray acceptance to panels on the contracted customer ship.
- Added a readable zone checklist and corrected HUD labels for dispatch, active service, transit and completed inspection.
- Added named client traits, disclosed quality-gated tips, reputation-based dispatch unlocks and customer identity updates.
- Added persistent land-claim gates for Crater Ridge, Ion Flats and Glasswake Reach with credit/reputation requirements, costs and visible sector content toggles.
- Moved contract-surface discovery to `Start` so the customer's `Awake` cache is ready before the career loop binds the active zones.
- Updated runtime discovery calls to current Unity 6 APIs; the runtime compile check now reports no warnings.
- Imported 40 Kenney Modular Space Kit CC0 models and authored a Vesper color-atlas recolor; added those pieces to the generated outpost scene source.
- Fixed contract cards to populate their selected client/brief at open time and expose base pay, tip terms and locked reputation requirements.
- Replaced the old Living GDD with v2.2, including all six pillars, four sectors, six ship classes, facility/utility targets, staff rules, economy and an honest source mapping.

## Next actions

1. Review the source diff and resolve any compile or logic defects without running a player build.
2. Preserve the dirty main scene, refresh the open Unity project, and confirm asset import and `.meta` generation.
3. Regenerate the scene only after its current edits are saved; inspect the modular outpost and expansion gates, then improve per-panel dirt visualization and Vesper art direction.
4. When safe, run the complete first-shift flow in Unity Editor Play Mode and capture current screenshots. Do not create a standalone/player build.
5. Continue the full-game systems roadmap: saveable company ledger, sector plots and construction, utilities, residue processing, staff tasks and visible wash automation.
6. Keep the Living GDD, backlog and changelog synchronized with what is implemented and what is still design-only.

## Owner constraints

- Do not produce or launch a player build until UFO WASH is genuinely playable and the owner explicitly requests one.
- No paid assets without approval.
- Do not ask the owner to provide game design. Make strong in-vision decisions and present evidence.
- Pull/push repository changes only within the owner’s earlier authorization and report commit/push results precisely.
- The target is the full game. A first-shift or vertical-slice gate is not the final product.

## Verification boundary

The code changes above passed runtime and editor C# compilation against Unity 6.6 reference assemblies, but Unity has not imported them into the open project. Play Mode and visual checks remain outstanding. No standalone/player build was created.
