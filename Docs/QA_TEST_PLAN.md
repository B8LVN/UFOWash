# UFO WASH — Vertical Slice QA Checklist

Run after pulling the latest main branch. The dedicated game scene should open and regenerate automatically when its builder script changes.

## A. Project and scene workflow
- [ ] Unity 6000.6.10f1 opens the project.
- [ ] No C# compile errors after script import.
- [ ] If the scene builder changed in the pull, Unity automatically regenerates and opens UFO_WASH_PowerSlice.
- [ ] If only gameplay scripts changed, the saved scene is retained; no rebuild command is required.
- [ ] The manual fallback is available under UFO WASH > REBUILD GENERATED SCENE (MANUAL FALLBACK).
- [ ] The active scene is Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity.

## B. Rendering and materials
- [ ] URP 17.6 is active.
- [ ] Ship, bay and washer have no magenta materials.
- [ ] Every imported ship renderer material slot uses a supported URP shader.
- [ ] The ship retains metal, glass and accent material differences after cleaning.
- [ ] Dirt changes preserve the original material palette.
- [ ] No missing scripts, meshes or textures.

## C. Player and tool
- [ ] Play Mode starts.
- [ ] Camera is first-person at eye height.
- [ ] WASD movement and mouse look work.
- [ ] Player collides with floor and bay geometry.
- [ ] Reticle and spray direction align with the washer muzzle.
- [ ] Spray has a forgiving coverage radius and does not clean through walls.
- [ ] Escape unlocks the cursor; click relocks it outside UI.

## D. Contract board
- [ ] Customer UFO arrives and docks before offers can be accepted.
- [ ] E at the contract terminal opens a full-screen board with three offers.
- [ ] Each offer has a distinct brief, contamination profile and payout.
- [ ] Clicking a card changes the selected state.
- [ ] Accept starts the selected contract and closes the board.
- [ ] Escape closes the board without accepting.
- [ ] Cursor is visible/unlocked while the board is open.
- [ ] Player movement/look and world interactions are paused while the board is open.
- [ ] Controls resume and cursor relocks after closing.

## E. Customer interaction
- [ ] Alien customer model is visible.
- [ ] Customer turns toward the player and has subtle idle movement.
- [ ] E while aiming at the customer displays dialogue.
- [ ] Speech bubble faces the player and is readable.
- [ ] Customer acknowledges contract acceptance.
- [ ] Customer reacts to successful inspection/completion.

## F. Cleaning process
- [ ] Spray before accepting a contract does not clean the ship.
- [ ] Mode 1 pre-rinse reduces contamination across individual hull panels.
- [ ] Stage advances only after the pre-rinse threshold.
- [ ] Mode 2 creates persistent local foam patches where the nozzle hits.
- [ ] Foam patches are visible at normal gameplay distance and close range.
- [ ] Moving the nozzle creates new patches on separate hull areas.
- [ ] Stage advances only after adequate foam coverage.
- [ ] Mode 3 removes dirt and fades/removes foam patches.
- [ ] Foam patches do not block subsequent spray hits.
- [ ] Incorrect tool mode does not advance service.
- [ ] Every required hull surface can be reached with the spray, including the cockpit.
- [ ] Decorative ship details do not prevent spray reaching an adjacent washable panel.
- [ ] Hangar walls and floor still block spray.

## G. Inspection and economy
- [ ] Final rinse does not automatically submit the job.
- [ ] Contract terminal offers inspection only when dirt and foam are below thresholds.
- [ ] Player can choose when to submit after meeting the minimum standard.
- [ ] Inspection shows a grade and base payout plus quality bonus.
- [ ] Completing the job pays exactly once.
- [ ] Water decreases during pre-rinse/final rinse.
- [ ] Chemical decreases during foam application.
- [ ] Empty supplies block the matching operation and display a clear message.
- [ ] E at supply station charges 65 credits once and refills both resources.
- [ ] Insufficient credits do not refill supplies.
- [ ] Sprayer upgrade increases cleaning rate, spray radius and range; costs rise by level.
- [ ] A second contract can be accepted after the next customer docks.

## H. Automated structural validation
Run UFO WASH > QA > VALIDATE ACTIVE GAME SCENE.
- [ ] Validator reports expected player, washer, interaction, contract, supply, ship, NPC and HUD components.
- [ ] Validator finds at least two washable surfaces.
- [ ] Validator reports no missing materials or missing scripts.
- [ ] Review any non-URP shader warnings.

## Report back
Attach a screenshot of the contract board, a screenshot showing foam on the UFO, a customer dialogue screenshot/clip, and the first Console error/exception if any. Do not manually change code before reporting a blocking error.


## 0. Opening experience and scene organization

- [ ] Title menu appears on entering Play Mode over the paused 3D station.
- [ ] Cursor is visible and UI buttons respond to mouse input.
- [ ] Start First Shift opens the briefing rather than starting gameplay immediately.
- [ ] Story opens and returns to title correctly.
- [ ] How to Play shows movement, interaction, washer controls and service sequence.
- [ ] Continue to Training and Clock In start the simulation.
- [ ] Player movement, look and washer input are enabled after Clock In.
- [ ] The menu does not reappear after Clock In during the same session.
- [ ] The generated scene Hierarchy contains the eight named top-level groups.
- [ ] Grouping preserves the world positions/rotations of the player, UFO, equipment and station props.

## 0.1. Reported Unity exception regression

- [ ] Enter Play Mode without a TypeInitializationException from UfoWashDirtSurface.
- [ ] Use Foam mode and confirm no MaterialPropertyBlock constructor exception appears.
- [ ] Confirm foam patches still render and final-rinse cleanup remains functional.
