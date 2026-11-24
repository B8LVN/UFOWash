# Next Production Milestone

**New design direction:** See `Docs/GARAGE_TO_SPACEPORT_DIRECTION.md` for the
owner-requested small-planet garage start, UI/3D overhaul, visible growth and
proposed safe scene cleanup. The document is a proposal, not a completed overhaul;
current cleaning/runtime acceptance remains the immediate implementation gate.

**Owner's branch rule:** One local Unity project/repository, two branches—not two project folders. `main` is user-facing. Develop and test on `ufo-wash/cleaning-recovery`; merge changes into `main` only after the owner explicitly says OK.

## Active handoff — cleaning and first-contract verification

**Task:** Finish the integrated quality-recovery milestone in the existing Unity game.

**Status:** Source implemented; Unity runtime/visual acceptance **Blocked** pending a working licensed Editor session.

**Owner:** Developer with Unity 6000.6.10f1 Editor access; product owner reviews gameplay evidence.

**Dependencies:** successful Editor licensing/import; preserved authored scene; actual Play Mode and screenshots.

**Next single action:** Establish the working Editor session and run the complete acceptance sequence in `Docs/reports/QUALITY_RECOVERY_2026-11-24.md`. Fix any discovered defects before declaring this milestone complete.

**Download for testing:** the source is on GitHub branch `ufo-wash/cleaning-recovery` in `B8LVN/UFOWash`, not merged into `main`. Close Unity, preserve local edits, run `git fetch origin`, then `git switch ufo-wash/cleaning-recovery` and `git pull --ff-only`. Open Unity 6000.6.10f1 and press Play; no scene-generation command is needed.

Run Git commands inside the existing project's repository folder, not the Windows home folder. Close Unity and preserve uncommitted work before switching branches. Do not clone another project or add a second Unity Hub entry. If `git status` fails inside the actual project root, investigate whether that existing folder contains Git metadata before giving further commands.

The exact Linux Editor has been downloaded into ignored `generated-artifacts/unity-editor/`; its version command succeeds. After installing required Linux libraries and ICU, the isolated import exited **198**: **No valid Unity Editor license found**. Activate the exact Editor or provide an accessible licensed session; do not send credentials in chat. No further import retry is useful until licensing changes.

Source now contains local dirt/foam, tool pressure audio, shared target acquisition, guarded input, a switchable fan purchase, supply limits, revised HUD and a non-destructive bay-preview generator. Fourteen engine-independent coating checks pass. Neither these checks nor the new two-contract Unity rule fixture count as authored-scene gameplay acceptance.

**Normal testing path: press Unity Play → main menu → start the shift. No UFO WASH/Create command is required.** The Editor start-scene hook selects the existing main-menu scene and adds the new service access in memory for that Play session, then cleans it up on Stop. This direct-entry source change still needs licensed Play Mode verification.

Preserve `Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity` and existing `.meta` files. The separate preview generator is now a developer-only optional tool, not an owner setup step. No player builds, paid assets, migrations, browser rewrite or additional feature expansion belongs to this handoff. Saveable station progression and the first worker are separately assigned work, not part of this recovery pass.

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
