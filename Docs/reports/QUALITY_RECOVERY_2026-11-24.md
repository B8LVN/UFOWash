# Cleaning and first-contract quality recovery — 2026-11-24

## Status

**User-reported error follow-up:** the closed upper-shell profile uses inward
triangle winding. Production code now creates an outward-facing runtime mesh copy
and updates its mesh collider only after the owner accepts/starts a contract.
The contract board is expanded/reflowed and suppresses underlying gameplay overlays.
Eighteen deterministic checks now cover the precise legacy shell profile and copy
orientation. These results establish the geometry math only: use the authored
Unity Game view to verify collider visibility, button operation and layout.

**Direct Play update:** normal owner workflow is now **Unity Play → main menu → start the shift**, with no Create/Generate step. `Editor/UfoWashPlayModeEntry.cs` selects the existing menu scene and applies the service-access additions only in memory. Stop removes additions and restores moved props; no scene/material assets are regenerated or saved. The old separate preview command remains only as an optional developer tool. This update is source-checked, not Unity-runtime verified.

**Implemented — unverified in Unity. Not accepted.** No player build is authorized or produced.
The existing scene, existing `.meta` GUIDs, package versions and project version are preserved.
The source changes are an experimental recovery pass, not a claim of polished art or finished gameplay.

## Source changes

- Replaced renderer-wide dirt tint and independent foam spheres with a UV-independent, subdivided coating overlay. Dirt and foam live on the same local cells. Spraying a spot no longer changes distant cells. Panel totals are weighted by exposed triangle area.
- Buried faces are excluded by outward collider checks. This is provisional: authored saucer coverage, window intersections and access to the underside still need actual Unity testing.
- Original hull materials are not rewritten. The URP coating shader renders grime and pale foam; final rinse removes both at the contact footprint. No per-spray objects are spawned.
- Spray and camera acquisition share the same range and obstruction query. Cleaning requires an accepted ship panel, correct stage, supplies and active first-person controls. Menus, focus loss and cursor-relock clicks suppress washing.
- Corrected world-interaction reach to measure from the player rather than the shoulder camera; self-colliders are ignored.
- Supply work is capped to the remaining tank fraction. Wrong-stage/off-contract work is rejected. Existing exactly-once payout, per-panel stage gates and reputation rules remain, with a separate readable receipt.
- The first 200 GC equipment purchase installs a wide fan nozzle. Q switches fan/precision; coverage affects local cleaning and subsequent power upgrades now affect work rate. No persistence was added in this task.
- Added an original synthesized pressure loop with distinct water/foam/rinse pitches and activation/stop fades. This is a prototype tool layer, not a finished station audio pass.
- HUD separates the service overview, per-panel checklist, tool feedback and receipt. Removed the unguarded R-key contract reset.
- Builder source adds restrained drain grates, fluid risers, keep-clear marks and service signage, plus stairs and a three-sided upper-hull service catwalk so local coating is not dependent on reaching hidden roof cells from the ground. Drums move out of the stair approach. Traversal and actual hull visibility remain unverified. Import no longer replaces the scene automatically. A separate quality-preview command preserves the canonical scene and does not replace Build Settings.

## Verification

### Executed

`dotnet run --project Tools/QualityChecks/QualityChecks.csproj`

**14 deterministic C# coating checks passed.** They link the production state class, not a reimplementation.
Coverage: local dirt removal, weighted totals, local foam, untouched neighbors, local rinse,
complete rinse after foam depletion, pre-rinse floor, reset, invalid time/areas and effective power.
These tests do **not** execute Unity physics, rendering, input, career logic, audio or the authored scene.

The .NET runner also parses every game/editor C# source with C# 9 grammar and `UNITY_EDITOR` defined. This catches syntax errors, not Unity API/reference or shader errors.

The earlier source checks covered **24** game/editor files; the direct Play update adds one file, for **25**.
`git diff --exit-code` for tracked scenes, existing metadata, `Packages` and `ProjectSettings` returned **0**.
There are six new metadata GUIDs after the direct Play update; generated Editor/import outputs are ignored.
This is source/preservation evidence only, not successful Unity compilation.

### Editor verification route

- Exact target: Unity **6000.6.10f1**, revision **40e81493c67d**.
- No Editor was initially present on PATH or in the checked installation locations.
- Official Unity release API returned the matching Linux Editor archive (4,132,686,904 bytes; approximately 9.13 GB installed).
- Editor installation and verification outputs belong under ignored `generated-artifacts/`, never in Git or the game deployment.
- Installed required GTK/GL/X11 libraries, then ICU after the first licensing-client startup failed. The corrected isolated import exited **198** with **No valid Unity Editor license found**. No successful Unity source compilation/import or Play Mode occurred.
- Local evidence: ignored `generated-artifacts/unity-import.log` and `generated-artifacts/unity-import-with-icu.log`. No tokens/credentials were requested or used.
- Unblock by activating the exact Editor through an authorized Unity workflow or providing an accessible licensed session. Never bypass licensing; do not send credentials in chat.

### Unity rule-level Play Mode fixture — not yet run

`Assets/UFO_WASH/Editor/UfoWashRecoveryChecks.cs` provides the batch entry point
`UFOWASH.Editor.UfoWashRecoveryChecks.RunBatch`.
Run it only in an isolated copy of `Assets`, `Packages` and `ProjectSettings`.
It creates a disposable two-panel fixture; checks wrong-stage supplies, empty tanks/refill,
each panel's gates, first payout/reputation, duplicate inspection rejection, fan-nozzle purchase,
ship turnaround and a different second contract. This fixture is **not gameplay/visual acceptance**.

## Required authored-scene acceptance

1. Open the project in the exact Editor and retain the existing main scene. Resolve all Console import/compile errors.
2. Run **UFO WASH > QA > VALIDATE ACTIVE GAME SCENE** on the canonical scene.
3. Press Unity **Play**, including from another open scene. Confirm the main menu appears automatically, with no Create command or scene file replacement.
4. Start the shift from the main menu, then accept the routine contract. Confirm the service access appears in the canonical bay during Play; after Stop, confirm the original scene and prop positions are restored.
5. Record a close hull view before/after a short stationary spray. Only the contact region should change; circling the UFO must expose remaining dirt.
6. Clean only one panel; verify every remaining panel still gates pre-rinse. Select foam early: no work or supply charge. Complete all panels, then repeat this check for foam and final rinse.
7. Stop spraying: foam must remain at its original locations. Rinse a different spot: distant foam must remain. Use the automatically added service stairs/decks for canopy/roof access; traverse all steps and bridge without jumping, clipping or camera faults. Observe the saucer, cockpit and underside for holes, seams or inaccessible contamination.
8. Check activation/contact/stop audio, water/foam/rinse character, no lingering particles or loops after menu, pause, disable or focus loss.
9. Check third-person exploration, first-person-only active cleaning, close target acquisition, walls, decorative hull parts, camera collision, mouse sensitivity, Escape/relock, focus loss and UI close while holding LMB.
10. Deplete supplies; verify zero means no work and the final tank fraction grants only proportional work. Refill costs 65 GC once, full tanks cost nothing.
11. Complete inspection, note base/bonus/tip/reputation/credits. Press E repeatedly: payout and completed-job count must not increase again.
12. Buy the 200 GC fan nozzle, compare the visible footprint and next-job coverage, toggle Q both ways. Confirm insufficient funds cannot purchase.
13. Wait for departure/return. Accept the deep-clean offer and finish it without resetting or developer intervention. Verify new contamination/foam reset, supplies retained, correct reward and reputation.
14. Capture **actual Game-view/Play Mode** screenshots: wide starter bay, exploration view, close dirty/part-clean/foamed/rinsed hull, equipment, HUD/receipt and second-contract completion. Record resolution, frame time, coating cell/draw counts and Console state. A fixture screenshot or source rendering is not acceptable evidence.
15. Contract-board focused regression: open dispatch, select the quick contract,
   read the selection text, click Accept. Confirm it closes, modal hides other
   gameplay HUD/subtitles, and the upper saucer shell logs no coating error. Try a
   locked card and confirm the failure/lock state is readable; close and confirm
   gameplay UI returns. Repeat at 1024×576, 1024×768 and 1920×1080.

## Remaining risks / scope boundaries

- Coating geometry density, initial occlusion classification, transparency, CPU work and visual quality need measurements on the authored saucer. Do not finalize this technique without comparing the previous tint baseline and a UV/mask alternative.
- Startup/domain-reload, camera handoff and input timing need real Play Mode evidence.
- No save/load, worker, business expansion, migration, paid assets, browser app or player build was introduced.
- No GitHub Issue was updated: the handoff contains no linked issue URL/number or connected issue-access workflow. Keep any corresponding issue open/Blocked until real acceptance evidence and owner approval exist.
