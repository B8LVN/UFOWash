# Next Authorized Task

**Task ID:** UW-102  
**Title:** Owner Playtest — Runtime Fix, Title Menu, Story, Tutorial and Scene Hierarchy  
**Priority:** P0 — blocking verification  
**Status:** Ready for owner playtest  
**Owner:** Game Director / Product Owner  
**Developer:** Lead Developer  
**Implementation authorization:** The menu/onboarding foundation and hierarchy pass are committed. Fix only blocking defects from this playtest before expanding content.

## Objective

Confirm the project enters through a readable game menu, communicates the premise and controls, then hands the player into the first shift. Confirm the scene hierarchy is organized and the reported Unity initialization exception is gone.

## Changes committed

- UfoWashDirtSurface.cs: moved MaterialPropertyBlock creation out of static initialization and into the runtime method that first needs it.
- UfoWashFrontEnd.cs: added a title screen, Story page, first-shift briefing and How to Play field manual.
- UfoWashPowerSliceBuilder.cs: adds the front end to the generated scene and groups scene roots into named world, equipment, vessel, customer, player, systems, lighting and UI sections.
- The menu pauses the shift and keeps the cursor visible; Clock In starts the simulation and enables first-person movement, interaction and spraying.

## Owner test procedure

1. Pull latest main:
   ```powershell
   git -C "C:\UnityProjects\UFOWash-Repo" pull origin main
   ```
2. Wait for Unity to finish compiling and importing. The changed scene builder should automatically regenerate UFO_WASH_PowerSlice.
3. Check the Console before pressing Play. There must be no C# compile errors.
4. Press Play. Confirm the title menu appears over the 3D station backdrop and the game is paused.
5. Open Story. Read the Bay 01 / Vesper Belt introduction. Return to title.
6. Open How to Play. Confirm the controls and five service steps are readable.
7. Start First Shift, read the dispatch briefing, continue to training and Clock In.
8. Confirm WASD/mouse movement works, the cursor is locked, and the UFO/service loop resumes.
9. Confirm the Hierarchy has these top-level groups:
   - 00 // HANGAR ARCHITECTURE
   - 01 // SERVICE EQUIPMENT & STORAGE
   - 02 // CUSTOMER VESSEL
   - 03 // CUSTOMERS & NPC
   - 04 // PLAYER
   - 05 // GAMEPLAY SYSTEMS
   - 06 // LIGHTING & POST PROCESSING
   - 07 // UI & FRONT END
10. Spray in Foam mode and confirm the MaterialPropertyBlock..ctor exception does not return.
11. Continue the contract/cleaning test and report any first Console exception.

## Acceptance criteria

- No UfoWashDirtSurface..cctor / MaterialPropertyBlock..ctor exception.
- No compile errors in the new front-end or builder code.
- Title, Story, Briefing and How to Play pages render in front of the 3D backdrop.
- UI buttons respond to mouse input.
- Simulation remains paused until Clock In.
- Clock In closes the menu, restores game time and enables first-person controls.
- Scene Hierarchy contains the eight named top-level groups.
- Existing contract, foam, customer and job systems remain reachable.

## Evidence to return

- Screenshot of the title menu.
- Screenshot of the Story or How to Play page.
- Screenshot of the organized Hierarchy.
- Screenshot of Console after entering Play Mode.
- Report any first error exactly as shown.

## Known limitations

- This is an initial in-scene front-end foundation, not a finished cinematic title sequence.
- Story text and onboarding are a first narrative draft.
- The hangar still needs a genuine environment-art production pass; grouping primitives does not make them production art.
- No Unity compile, Play Mode or build test has been run by the remote source-editing session.

Do not mark this task complete until the owner has tested and accepted it.
