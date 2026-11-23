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
- [ ] Dirt and foam tint the surface without replacing the original material.
- [ ] No missing scripts, meshes or textures.

## C. Player and tool
- [ ] Play Mode starts.
- [ ] Camera is first-person at eye height.
- [ ] WASD movement and mouse look work.
- [ ] Player collides with floor and bay geometry.
- [ ] Reticle and spray direction align with the washer muzzle.
- [ ] Spray has a forgiving coverage radius and does not clean through walls.
- [ ] Escape unlocks the cursor; click relocks it.

## D. Customer arrival and contracts
- [ ] Customer UFO arrives from the hangar approach and docks at Bay 01.
- [ ] Contract terminal does not accept the job before docking.
- [ ] E at the contract terminal accepts one contract after docking.
- [ ] Contract cannot be accepted twice while active.
- [ ] Contract data shows client, vessel class, contamination and payout.
- [ ] The ship departs after inspection and returns for the next booking.

## E. Cleaning process
- [ ] Spray before accepting a contract does not clean the ship.
- [ ] Mode 1 pre-rinse reduces contamination across individual hull panels.
- [ ] Stage advances only after the pre-rinse threshold.
- [ ] Mode 2 applies visible foam to separate ship panels.
- [ ] Stage advances only after adequate foam coverage.
- [ ] Mode 3 removes dirt and foam.
- [ ] Incorrect tool mode does not advance service.
- [ ] Every required hull surface can be reached with the spray.
- [ ] Dirt/foam changes preserve the hull's original material identity.

## F. Inspection and economy
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
- [ ] Contract 2/3 visibly changes contamination tint (plasma soot / bio residue) while the hull returns to clean base materials.

## G. Automated structural validation
Run UFO WASH > QA > VALIDATE ACTIVE GAME SCENE.
- [ ] Validator reports the expected player, washer, interaction, contract, supply, ship and HUD components.
- [ ] Validator finds at least two washable surfaces.
- [ ] Validator reports no missing materials or missing scripts.
- [ ] Review any non-URP shader warnings.

## Report back
Attach one Play Mode screenshot showing the ship and HUD, plus the first Console error if any. Do not manually change code before reporting a blocking error.
