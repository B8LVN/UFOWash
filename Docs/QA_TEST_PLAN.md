# UFO WASH — Vertical Slice QA Checklist

Run after pulling the latest main branch and rebuilding the Power Slice scene.

## A. Project and rendering
- [ ] Unity 6000.6.10f1 opens the project.
- [ ] No C# compile errors after script import.
- [ ] URP 17.6 is active.
- [ ] Generated materials show Universal Render Pipeline/Lit in the Inspector.
- [ ] Ship, bay and tool have no magenta materials.
- [ ] No missing scripts or missing mesh warnings.

## B. Player
- [ ] Play Mode starts.
- [ ] Camera is first-person at eye height.
- [ ] WASD movement and mouse look work.
- [ ] Player collides with floor and bay geometry.
- [ ] Reticle and spray direction align.
- [ ] Spray particles originate at the washer muzzle, not in front of the ship.

## C. Contract and cleaning
- [ ] Spray before accepting a contract does not clean the ship.
- [ ] E at contract terminal accepts one contract.
- [ ] Mode 1 pre-rinse removes loose contamination.
- [ ] Stage advances only after the pre-rinse threshold.
- [ ] Mode 2 applies visible foam to separate ship panels.
- [ ] Stage advances only after foam coverage threshold.
- [ ] Mode 3 removes remaining dirt and foam.
- [ ] Incorrect tool mode does not advance service.
- [ ] Completion occurs once and grants one payout.
- [ ] A second contract can be accepted.

## D. Supplies and economy
- [ ] Water decreases during pre-rinse/final rinse.
- [ ] Chemical decreases during foam application.
- [ ] Empty supplies block the matching operation and show a message.
- [ ] E at supply station charges 65 credits once and refills both supplies.
- [ ] Insufficient credits do not refill supplies.
- [ ] Contract reward is not paid more than once.

## E. Usability and performance
- [ ] A first-time player can understand the three-stage process from HUD prompts.
- [ ] All ship panels can be reached with the tool ray.
- [ ] No surface remains impossible to clean because of missing colliders.
- [ ] No repeated Console exceptions during a full contract.
- [ ] Record average FPS and any obvious stutter on the target PC.

## Report back
Attach one screenshot from Play Mode showing the ship and HUD, plus the first Console error if any. Do not change code manually before reporting a blocking error.
