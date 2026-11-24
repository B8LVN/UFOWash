# UFO WASH — Current Project Status

**Last updated:** 2026-11-24

## Direct development update — 2026-11-24

- Removed the optional test runner, Editor validators, QA-plan/recovery files,
  unused bug-report templates and generated audit/check output. The game does not
  depend on any of them.
- Kept ordinary Unity play instructions and concise acceptance notes; they are not
  a separate QA system. Unity's test framework remains only as an indirect package
  dependency and is not listed as a project dependency or used by game scripts.
- The title now starts a shift directly; story and controls remain optional. The
  hull-zone checklist can be hidden or shown with Tab.
- Quick Decontamination now offers a disclosed +45 GC express bonus for passing
  inspection within five minutes. Missing it has no penalty; the other offers
  remain untimed. This source change is not yet Unity-tested.
- Added a persistent two-tier Bay Recovery purchase: 650 GC saves 15% supply use;
  a further 1,400 GC raises total savings to 30% and expands the annex using
  included Kenney CC0 room models. Purchase, save/reload and rendering remain
  unverified in Unity.
- Added a play-only art pass for textured deck/wall/basalt surfaces, warm/cool
  focal lighting, and scaled Kenney service equipment. The title screen and HUD
  now have stronger framing, resource bars and a selected-tool hotbar. These
  additions are temporary during Play and do not save over the authored scene;
  their actual look still needs a Unity review.
- Existing Kenney Space Kit and Modular Space Kit assets are CC0 and remain the
  current free asset base. No new pack was downloaded or represented as integrated.
- Latest source and documentation edits are on `ufo-wash/cleaning-recovery`.
  Unity visuals and gameplay remain unverified because this workspace has no
  licensed Editor session.

## Owner-confirmed contract fix, cleaning pace and visual asset sourcing

- Owner confirmed that the upper-saucer contract acceptance now works in Unity.
- Cleaning treatment has been tuned to cover more area at a higher work rate. Spray
  on already-treated cells is now a no-op (and costs no water/chemical); the player
  receives a sweep-to-the-next-zone hint. HUD progress now measures progress toward
  the current stage's all-zone gate rather than overall appearance cleanliness,
  which was misleading at around 37%.
- Source changes are present, but actual spray pace and zone progression still need
  a Unity Play Mode retest; no current gameplay screenshot is available here.
- Sourced several additional no-cost, commercially usable asset libraries with
  official CC0 license pages. The most relevant are recorded as candidates in
  `Docs/14_VISUAL_OVERHAUL_ASSET_RESEARCH.md`; none have been downloaded or imported.
- Initial visual direction remains the owner's small-planet frontier garage.
  New art will be a selective, coherent set of hero ship, bay, lighting/materials
  and UI changes—not an indiscriminate asset dump. Current scenes are preserved.

## Latest design planning — garage to spaceport

Owner requested a major UI/UX, model and world improvement around a small-planet,
low-cost garage start, hiring and visibly upgraded facilities. The proposal and
verified roles of the three existing scenes are recorded in
`Docs/GARAGE_TO_SPACEPORT_DIRECTION.md`. This is design planning, not implemented
art/gameplay. No scenes or Build Settings were changed. Core runtime acceptance
remains outstanding; the owner's Project-window screenshot does not verify it.

## User-reported contract board / coating errors — 2026-11-24

- Screenshot showed contract acceptance failing with repeated `A coating requires exposed surface cells` for the upper saucer; board text and gameplay overlays overlapped.
- Reconstructed the exact upper shell from the scene builder and verified its closed triangle winding has negative signed volume. Added runtime-only outward correction on a private mesh copy and associated collider. This alone did not resolve the user's reproduction.
- Follow-up diagnosis: the coating builder's visibility filter raycast every face against all overlapping ship colliders, which could incorrectly classify every subdivided upper-shell triangle as buried. Removed collider-based face rejection; coating cells now come from the panel's own non-degenerate triangles. Active spray still uses contracted-surface targeting and per-cell normal/radius checks.
- Teardown restores existing references and releases the runtime mesh copy; the scene asset is untouched. Failures log once per panel and include mesh vertex/triangle counts if source geometry is empty.
- Resized/reflowed the contract board for responsive scaling, bounded text, added selection/failure feedback, and hid lower-order HUD/subtitle canvases behind the modal.
- Earlier deterministic source checks passed before the optional helper suite was
  removed at the owner's request. They were not Unity runtime tests; colliders,
  successful clicks and rendering remain unverified.
- Unity verification is still blocked by unavailable Editor licensing. Next: owner tests the new update on `ufo-wash/cleaning-recovery` and shares any new Console error or screenshot.

## Current implementation — latest source state

**Status: Implemented — unverified in Unity; acceptance remains Blocked.**

- **Normal testing path:** press Unity **Play → main menu → start the shift**. No Create/Generate command is required. The Editor start-scene hook selects the existing menu scene and adds service access only in memory for Play, removing it on Stop. This supersedes the earlier manual-preview instructions; licensed runtime verification is still required.
- **GitHub availability:** cleaning recovery and station growth are pushed to `ufo-wash/cleaning-recovery`. `main` remains unchanged.
- Local grime and persistent foam now share area-weighted surface cells; the old whole-renderer dirt tint and separate foam spheres were removed. Original hull materials and scene references are retained.
- Camera and spray share contracted-panel targeting; valid stages/supplies gate active cleaning. Input focus/relock/menu handling and shoulder-camera interaction range were corrected in source.
- Added a switchable wide fan nozzle as the first 200 GC purchase, actual cleaning-power effects, proportional last-drop supply use, stage-aware zone feedback, bounded HUD panels and a separate payout receipt.
- Added original synthesized tool pressure audio. Starter-bay service details and upper-hull stairs/catwalk are source additions applied temporarily on Play, **not** a regenerated or visually reviewed scene. Their presence, traversal and cleanup require real Play Mode verification.
- Import no longer replaces the main scene. The optional separate preview generator moved under Developer Tools; it is not a normal testing step. Existing scenes and existing metadata are unchanged.
- **Source checks:** earlier deterministic coating/geometry and C# syntax checks passed before the owner requested removal of the optional test tooling. Those checks were not Unity import, shader, physics or gameplay tests.
- **Editor access:** an earlier session recorded downloading the official Linux Unity 6000.6.10f1 Editor and an import exit **198**: **No valid Unity Editor license found**. The Editor files and logs are not present in this checkout; `generated-artifacts/` is currently empty. No successful Unity script compilation/import occurred here, and the owner checkout was not opened in Unity.
- **Required unblock:** activate this Editor through the owner's licensed Unity workflow, or provide an accessible licensed exact-version session. Do not supply credentials in chat or bypass licensing. Retrying unchanged cannot resolve this gate.
- **Still required:** successful Unity import/compilation, two consecutive contracts in authored-bay Play Mode, owner review of visuals/audio and a save/reload check. No player build was created.
- GitHub Issue update remains unavailable: no linked issue URL/number or connected issue workflow was supplied. Do not claim an issue update or close an issue.

Older sections below are historical and do not supersede this verification boundary.

## Imported checkout orientation — 2026-11-24

- Owner requested development toward a Steam-quality game using Moon Sweepers, Schedule I and Aquapark Tycoon as references, rather than import/run setup.
- Inspected project guidance, canonical design, source organization and cleaning implementation. No gameplay code, scenes, packages or run workflow were changed.
- The cleaning source still uses a whole-surface dirt tint with separate local foam patches; this is not evidence of precise local dirt removal.
- No Unity Editor command was found on PATH in this environment. No fresh Unity import, compilation, Play Mode, screenshot or performance test was performed.
- Earlier local Editor observations below are historical, not confirmation that this environment can control that Editor.
- Recommended next milestone: integrated cleaning/control/presentation recovery with repeat-job and reinvestment acceptance checks. Subsequent work: persistent, visible tycoon progression. Development task selection and Unity verification access remain outstanding.

## Station growth implementation — 2026-11-24

- Owner supplied the existing `ufo-wash/cleaning-recovery` test branch and confirmed that branch is the intended route for Unity testing. Station-growth changes were integrated there while retaining the branch's cleaning, fan-nozzle and Play Mode workflow.
- Added a schema-versioned company JSON ledger for credits, reputation, supplies, completed jobs, washer levels, attendant hiring and purchased sector claims. It writes a backup, restores a valid backup when the primary is damaged, and visibly blocks saving if no valid copy exists.
- Added a 120 GC Vesper wash tech, matching the GDD's prototype hiring target, who automatically pre-rinses only active contracted hull panels, stops at the service-stage boundary and reports idle/working/water-blocked status. The builder uses the included Kenney CC0 astronaut model when available and a primitive fallback otherwise. Recurring shift wages are not yet simulated.
- Added save/load wiring for washer upgrades and sector expansion content. Legacy sector claims in PlayerPrefs are imported when the ledger loads.
- Unfinished contracts intentionally reset on restart; company progress and supplies persist.
- Implementation is unverified. No licensed Unity import, Unity API compilation, Play Mode, save round-trip or visual check was available in this checkout. The Play Mode hook temporarily injects the save service and attendant without modifying the authored scene; the optional generated preview also includes them.

## Previous production record

**Product target:** Full commercial single-player PC/Steam game; a first-shift slice is a milestone, not the final product.  
**Current milestone:** UW-104 — contracted hull zones, customer satisfaction and service economy.  
**Build policy:** No Windows/standalone/player build until the game is genuinely playable and the owner requests one.

## Project baseline

- Repository: B8LVN/UFOWash; `main` is user-facing and `ufo-wash/cleaning-recovery` is the owner-approved test branch.
- Unity Editor version: 6000.6.10f1; URP 17.6.0; Input System 1.20.1; uGUI 2.6.0.
- Main authored scene: Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity.
- Included Kenney Space Kit is CC0; no paid assets have been added.
- The scene builder creates one custom procedural saucer, an imported alien when available, a Vesper Belt panorama, hangar dressing, player, UI, contracts, supply/upgrade stations and the new Vesper service tech.
- Kenney Modular Space Kit 1.0 is now imported: 40 CC0 modular station FBX models plus its source atlas/license. A tailored Vesper palette derivative is at Assets/UFO_WASH/Art/Textures/Vesper_ModularSpace_Colormap.png.
- Generated scene source describes a walkable arrival ramp/outpost and three purchasable sector gates. The checked-in scene has not been refreshed in this environment; the builder has a guard that skips regeneration when its active scene contains unsaved edits.
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
- A versioned save service, save-backed upgrades/sector claims and one bounded pre-rinse attendant are implemented in source but have not been imported or tested in Unity.

## Verification honestly outstanding

- Runtime and editor C# assemblies compiled against Unity 6.6 generated project references with zero errors. The runtime compile had zero warnings; the editor compile reported six existing API deprecation warnings. This is a compiler check, not a live Unity Editor import.
- Unity import has not been run. The two new progression scripts include Unity metadata, but their component references and runtime behavior have not been imported or checked.
- An isolated batch-mode import was attempted in a disposable project copy and stopped during Unity LicensingClient access-token validation, before asset import. The disposable copy was removed; the owner's main project was not touched by the attempt.
- Older notes mention a dirty Unity scene; this checkout does not provide current access to that Editor state. Preserve any unsaved scene before using the generated-scene menu action.
- The new contract-card rendering, reputation progression, panel checklist, camera handoff and full payout loop have not been tested together in Play Mode.
- Current scene generation after the latest builder changes has not been observed; the checked-in scene file does not yet contain the new outpost, sector gates or attendant.
- New save/load, attendant and land-claim integration still need Unity import and Play Mode verification, including backup recovery. Full construction, utilities, additional staff AI, automated production, Rare Residue, audio, accessibility and commercial/Steam presentation remain future work.
- No current screenshot should be described as a finished or production-ready world.

## Development rules

- Keep code, generated scene, current GDD, backlog and project memory synchronized.
- Preserve owner files and review the entire working-tree diff before commit or push.
- Use included/free assets or original assets; no paid content without approval.
- Keep the owner’s running application untouched. Previous source compile reports apply only to earlier source; current progression changes have not been compiled. Import, scene generation and Play Mode need a safe Editor refresh with any dirty scene preserved.
- Continue through all six pillars. Stop/pause only at the owner’s direction or a genuine blocking dependency.

