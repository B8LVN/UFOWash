# UFO WASH — Current Project Status

**Last updated:** 2026-11-24

## Latest design planning — garage to spaceport

Owner requested a major UI/UX, model and world improvement around a small-planet,
low-cost garage start, hiring and visibly upgraded facilities. The proposal and
verified roles of the three existing scenes are recorded in
`Docs/GARAGE_TO_SPACEPORT_DIRECTION.md`. This is design planning, not implemented
art/gameplay. No scenes or Build Settings were changed. Core runtime acceptance
remains outstanding; the owner's Project-window screenshot does not verify it.

## Active quality recovery — latest source state

**Status: Implemented — unverified in Unity; acceptance remains Blocked.**

- **Normal testing path:** press Unity **Play → main menu → start the shift**. No Create/Generate command is required. The Editor start-scene hook selects the existing menu scene and adds service access only in memory for Play, removing it on Stop. This supersedes the earlier manual-preview instructions; licensed runtime verification is still required.
- **GitHub availability:** after the owner connected GitHub, `ufo-wash/cleaning-recovery` was successfully pushed to `B8LVN/UFOWash`. Fetch and switch to that test branch to download the changes; pulling `main` alone will not get them. Main was not updated. Unity runtime acceptance remains blocked.
- Local grime and persistent foam now share area-weighted surface cells; the old whole-renderer dirt tint and separate foam spheres were removed. Original hull materials and scene references are retained.
- Camera and spray share contracted-panel targeting; valid stages/supplies gate active cleaning. Input focus/relock/menu handling and shoulder-camera interaction range were corrected in source.
- Added a switchable wide fan nozzle as the first 200 GC purchase, actual cleaning-power effects, proportional last-drop supply use, stage-aware zone feedback, bounded HUD panels and a separate payout receipt.
- Added original synthesized tool pressure audio. Starter-bay service details and upper-hull stairs/catwalk are source additions applied temporarily on Play, **not** a regenerated or visually reviewed scene. Their presence, traversal and cleanup require real Play Mode verification.
- Import no longer replaces the main scene. The optional separate preview generator moved under Developer Tools; it is not a normal testing step. Existing scenes and existing metadata are unchanged.
- **Source verification:** 14 deterministic coating-state checks and C# 9 syntax checks for 25 game/editor files passed. Tracked scene/material/metadata/package/settings preservation checks passed. These are not Unity API/shader compilation, physics or gameplay tests.
- **Editor access:** downloaded the official Linux Unity 6000.6.10f1 Editor to ignored `generated-artifacts/unity-editor/`; `Editor/Unity -version` reports **6000.6.10f1**. Installed required GTK/GL/X11 libraries and ICU. Initial isolated import timed out because ICU was missing; after correction the licensing client connected, but Unity exited **198**, reporting **No valid Unity Editor license found**. Logs: ignored `generated-artifacts/unity-import.log` and `generated-artifacts/unity-import-with-icu.log`. No successful Unity script compilation/import occurred. The owner checkout was not opened in Unity.
- **Required unblock:** activate this Editor through the owner's licensed Unity workflow, or provide an accessible licensed exact-version session. Do not supply credentials in chat or bypass licensing. Retrying unchanged cannot resolve this gate.
- **Still required:** successful Unity import/compilation, the included two-contract rule fixture, real authored-bay Play Mode with manual controls, gameplay screenshots, audio review and coating/camera performance measurements. No player build was created.
- Detailed changes, evidence boundary and exact acceptance sequence: `Docs/reports/QUALITY_RECOVERY_2026-11-24.md`.
- GitHub Issue update remains unavailable: no linked issue URL/number or connected issue workflow was supplied. Do not claim an issue update or close an issue.

Older sections below are historical and do not supersede this verification boundary.

## Imported checkout orientation — 2026-11-24

- Owner requested development toward a Steam-quality game using Moon Sweepers, Schedule I and Aquapark Tycoon as references, rather than import/run setup.
- Inspected project guidance, canonical design, source organization and cleaning implementation. No gameplay code, scenes, packages or run workflow were changed.
- The cleaning source still uses a whole-surface dirt tint with separate local foam patches; this is not evidence of precise local dirt removal.
- No Unity Editor command was found on PATH in this environment. No fresh Unity import, compilation, Play Mode, screenshot or performance test was performed.
- Earlier local Editor observations below are historical, not confirmation that this environment can control that Editor.
- Recommended next milestone: integrated cleaning/control/presentation recovery with repeat-job and reinvestment acceptance checks. Subsequent work: persistent, visible tycoon progression. Development task selection and Unity verification access remain outstanding.

## Previous production record

**Product target:** Full commercial single-player PC/Steam game; a first-shift slice is a milestone, not the final product.  
**Current milestone:** UW-104 — contracted hull zones, customer satisfaction and service economy.  
**Build policy:** No Windows/standalone/player build until the game is genuinely playable and the owner requests one.

## Project baseline

- Repository: B8LVN/UFOWash on main.
- Unity Editor version: 6000.6.10f1; URP 17.6.0; Input System 1.20.1; uGUI 2.6.0.
- Main authored scene: Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity.
- Included Kenney Space Kit is CC0; no paid assets have been added.
- The scene builder creates one custom procedural saucer, an imported alien when available, a Vesper Belt panorama, hangar dressing, player, UI, contracts and supply/upgrade stations.
- Kenney Modular Space Kit 1.0 is now imported: 40 CC0 modular station FBX models plus its source atlas/license. A tailored Vesper palette derivative is at Assets/UFO_WASH/Art/Textures/Vesper_ModularSpace_Colormap.png.
- Generated scene source now describes a walkable arrival ramp/outpost and three purchasable sector gates. The scene has not regenerated because Unity currently reports a dirty active scene; the builder skips such a refresh safely.
- Vesper terrain/sky materials and generated textures exist in the repository. The world and hangar remain an early procedural art pass; no production-quality open planet exists yet.

## Previous local observations

An earlier Unity Editor/Windows player observation showed the title screen, story/briefing/tutorial flow, readable customer subtitle and a third-person station view. Those observations predate the newest gameplay edits and do not verify them. A Windows player was built and launched during earlier work; it is not being treated as a deliverable and no further player build is authorized now.

## Source changes in progress

- Career jobs now bind to the arriving ship’s own service panels rather than every globally registered dirt surface.
- Every contracted panel must satisfy each ordered treatment threshold; off-contract surfaces are rejected.
- First-person cleaning view activates only while aiming/holding wash on a contracted ship panel.
- HUD now reports dispatch/service state and panel-by-panel work.
- Customer traits, quality-gated tip rules, reputation gains/losses and reputation-locked contract tiers are added to the source.
- Contract selection cards now refresh their real title, customer, brief, hazard, reward and tip terms after the board opens.
- Living GDD v2.2 replaces [EXPAND] placeholders and defines the commercial game vision and all six design pillars.
- A compact connected world is defined: four 80 m districts, 4 m build cells, a 40 m × 48 m starting yard and staged GC/reputation-gated expansion.
- Crater Ridge, Ion Flats and Glasswake gate purchase/persistence are implemented in source. Runtime and editor C# compilation checks passed against Unity 6.6 references; Unity import and Play Mode remain outstanding.
- Career-loop panel discovery now runs in `Start`, after all scene `Awake` calls, preventing an initialization-order race with the customer's cached surfaces.

## Verification honestly outstanding

- Runtime and editor C# assemblies compiled against Unity 6.6 generated project references with zero errors. The runtime compile had zero warnings; the editor compile reported six existing API deprecation warnings. This is a compiler check, not a live Unity Editor import.
- Unity's open main project did not refresh after the source timestamp change. The new kit models and script still need Unity-generated `.meta` files in the main checkout.
- An isolated batch-mode import was attempted in a disposable project copy and stopped during Unity LicensingClient access-token validation, before asset import. The disposable copy was removed; the owner's main project was not touched by the attempt.
- The Unity window currently has the generated scene marked dirty. The automatic builder intentionally skips a dirty active scene to protect unsaved work; preserve that scene before using the generated-scene menu action.
- The new contract-card rendering, reputation progression, panel checklist, camera handoff and full payout loop have not been tested together in Play Mode.
- Current scene generation after the latest builder changes has not been observed; the checked-in scene file does not yet contain the new outpost and sector gates.
- Multi-sector construction, utilities, save/load, staff AI, automated production, Rare Residue, audio, accessibility and commercial/Steam QA are not yet implemented.
- No current screenshot should be described as a finished or production-ready world.

## Development rules

- Keep code, generated scene, current GDD, backlog and project memory synchronized.
- Preserve owner files and review the entire working-tree diff before commit or push.
- Use included/free assets or original assets; no paid content without approval.
- Keep the owner’s running application untouched. Source compilation is checked; import, scene generation and Play Mode need a safe editor refresh with the dirty scene preserved.
- Continue through all six pillars. Stop/pause only at the owner’s direction or a genuine blocking dependency.

