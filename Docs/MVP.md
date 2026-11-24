# UFO WASH — MVP VERTICAL SLICE

**Version:** 0.2  
**Objective:** Prove one satisfying, repeatable UFO service job in one bay.

## Player experience

1. Enter a compact sci-fi wash bay in first person.
2. Read the contract terminal and accept the waiting vessel.
3. Use tool modes in order: pre-rinse, foam, final rinse.
4. Move around the ship and treat its separate serviceable panels.
5. Watch contamination and foam coverage change on the actual surfaces.
6. Monitor water and chemical reserves.
7. Refill at the supply station for a visible credit cost.
8. Complete the service, receive payment and accept another contract.

## Included

- Stable first-person movement, camera and interaction.
- One readable UFO silhouette with multiple cleanable surface zones.
- Contract acceptance, customer ship arrival/docking/departure and state.
- Three distinct cleaning stages with stage gating.
- Water and foam chemical consumption.
- Paid supply refill interaction.
- Surface-level dirt and foam feedback.
- Cleanliness and supply HUD.
- Final inspection submission, quality grade, quality bonus and credit payout.
- One meaningful equipment upgrade.
- Repeated contract reset/accept cycle.

## Explicitly excluded

Employees, automation, multiple bays, construction, multiple locations, open world, multiplayer, procedural ships, Android, advanced save/load and final production art.

## Acceptance tests

1. Play Mode starts without blocking Console errors.
2. WASD/mouse movement and E interaction work.
3. Contract cannot be cleaned before acceptance.
4. Pre-rinse reduces contamination and advances the contract at the defined threshold.
5. Foam mode applies visible foam and advances only after adequate coverage.
6. Final rinse removes remaining contamination; wrong stage does not progress the job.
7. Water and chemical levels decrease only when the correct tool is used on a valid surface.
8. Empty supplies prevent further work and display a clear message.
9. Supply station charges the stated price once and restores both resources.
10. Completing the final stage grants the reward exactly once.
11. The next contract can be accepted and completed.
12. No ship or generated material is magenta; all materials use supported URP shaders.
13. A first-time tester can discover the process from the HUD and in-world prompts without developer coaching.

## Current play status

Code is committed to the development/test branch. Unity compilation, Play Mode behavior, visual quality and Windows build are **not yet verified** by this environment. The owner must test the direct Play flow in a licensed Unity Editor; source inspection is not gameplay evidence.
