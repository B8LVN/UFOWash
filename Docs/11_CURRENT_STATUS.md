# UFO WASH — Current Project Status

**Last updated:** 2026-11-23  
**Overall state:** Runtime exception fix, first front-end/story/tutorial pass and generated hierarchy organization committed; Unity compile and Play Mode verification outstanding.  
**Current milestone:** M1/M2 — Station Foundation and Core Cleaning Loop.  
**Current task:** UW-102 — Owner playtest of runtime fix, menu, onboarding and hierarchy.

## Verified repository baseline

- Repository: B8LVN/UFOWash, default branch main.
- Unity Editor version: **6000.6.10f1**.
- Packages: URP **17.6.0**, Input System **1.20.1**, uGUI **2.6.0**.
- Scenes present: UFO_WASH_Playable, UFO_WASH_PowerSlice, UFO_WASH_Prototype.
- Kenney Space Kit 2.0 model and sprite files are committed under Assets/UFO_WASH/Art/Imported/KenneySpaceKit.
- Included License.txt states CC0 and permits commercial use.
- Scene builder selects imported craft_speederA spacecraft and alien customer when available.

## Owner-reported state

- The imported UFO model is visible.
- Foam was not visibly coating the UFO in the previous playtest.
- Contract selection and customer participation were poor.
- The overall station looked like an unfinished greybox, not a believable place.
- The player requested a real game opening: title menu, Story, introduction, tutorial and an understandable scene hierarchy.
- Unity reported a MaterialPropertyBlock..ctor exception originating from a static initializer in UfoWashDirtSurface.

## Latest source changes

- Fixed the reported Unity lifecycle defect by lazily creating the shared foam MaterialPropertyBlock at runtime.
- Added UfoWashFrontEnd.cs with a title menu, Story page, first-shift briefing, How to Play manual and Clock In transition.
- The front end pauses the simulation, exposes the cursor for menu interaction and enables first-person controls only when the shift starts.
- The generated Power Slice now creates the front end automatically.
- Added hierarchy organization into eight named top-level groups for architecture, equipment, vessel, customers, player, systems, lighting and UI.
- Earlier recovery work remains: local foam patch prototype, selectable contracts, customer dialogue, service stages, inspection and payout.

## Current implementation status

| Area | Status |
|---|---|
| Imported UFO model | Owner confirms visible |
| Unity MaterialPropertyBlock exception | Source fix committed; runtime verification outstanding |
| Title menu / Story / briefing / How to Play | Implemented in source; UI interaction unverified |
| Scene hierarchy grouping | Implemented in scene builder; generated result unverified |
| First-person controller | Source exists; feel not owner-accepted |
| Local foam patches | Implemented in source; visual result unverified |
| Contract board | Implemented in source; button input/layout unverified |
| Customer NPC | Implemented in source; interaction/dialogue unverified |
| Full job loop | Source exists; playtest and owner acceptance outstanding |
| Production-ready environment art | Not accepted; current hangar still contains substantial procedural primitive geometry |
| Audio, save/load, employees and automation | Not yet implemented in inspected gameplay source |

## Not yet verified

- Unity compilation after latest commits.
- Menu UI rendering and Input System button clicks.
- Paused-to-running transition and cursor lock restoration.
- Automatic scene regeneration after builder import.
- Correct top-level hierarchy in the generated scene.
- Foam rendering on the imported spacecraft.
- Contract board, customer interaction, full job loop, Console cleanliness, Windows build and performance.

## Working rules

- GitHub repository is canonical project memory.
- Distinguish source committed from compiled, runtime-tested and owner-accepted.
- Do not call the current hangar polished. It needs a separate art-directed environment pass using coherent assets, architectural detail, lighting, decals, props and audio.
- Prioritize a complete understandable first shift over adding unrelated systems.
- Keep UFO WASH; improve execution rather than changing the concept.

## Next action

Complete UW-102 in NEXT_TASK.md. Use the owner's Console and screenshots to fix any compile/runtime defects before the next environment-art and core-loop production pass.


## Local Unity verification (2026-11-23)

- Unity 6.6.10f1 imported the stabilization changes; latest compilation has no C# errors after its log entry. Obsolete API warnings remain.
- Observed the title screen in Play Mode with HUD/signs/dialogue suppressed; stopped the run cleanly.
- Split scene-attached MonoBehaviours into matching files and repaired Power Slice serialized references.
- Foam/gameplay loop still needs fresh end-to-end verification. The station is procedural greybox and needs an art direction pass.


## Camera/dialogue stabilization (2026-11-23)
- Unity compiled the controller, NPC presentation and scene-builder changes without C# errors after the latest compile.
- In Play Mode, verified the opening flow and observed the astronaut in third-person station mode.
- The gun remains hidden in third-person by code; first-person while aiming at a washable hull is not yet verified in Play Mode.
- Alien material fix and dialogue readability change compiled; their final in-game appearance still requires a focused verification pass.
- The world and oversized signage remain greybox quality; this is not a finished demo.
