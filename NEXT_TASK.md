# Next Authorized Task

**Task ID:** UW-101  
**Title:** Owner Playtest — Foam Application, Contract Board and Customer Interaction  
**Priority:** P0 — blocking verification  
**Status:** Ready for owner playtest  
**Owner:** Game Director / Product Owner  
**Developer:** Lead Developer  
**Implementation authorization:** No new feature expansion until this recovery pass is compiled and playtested. Fix only blocking regressions found during the test.

## Objective

Verify the latest integrated gameplay recovery pass in the owner's local Unity 6.6.10f1 project. The goal is to establish whether the player can understand and enjoy one customer job, not to add more disconnected systems.

## What changed

- Persistent foam patches are spawned at the spray contact point and fade during final rinse.
- The contract terminal opens a three-offer selection board.
- The alien customer idles, faces the player, displays dialogue, can be spoken to and reacts to job acceptance/completion.
- Player movement and world interaction pause while the contract board is open.
- The generated scene attaches the customer interaction to the imported alien or fallback character.

## Owner test procedure

1. Pull latest `main`:
   ```powershell
   git -C "C:\\UnityProjects\\UFOWash-Repo" pull origin main
   ```
2. Wait for Unity to finish importing and compiling. Do not press the manual scene rebuild command unless the Console reports the automatic builder failed.
3. Press Play in `UFO_WASH_PowerSlice`.
4. Wait for the UFO to dock.
5. Walk to the contract terminal and press E. Confirm the three offers appear.
6. Select each offer and confirm the selected card changes. Accept one offer.
7. Spray the UFO in foam mode (2). Confirm persistent foam patches appear exactly where the spray hits.
8. Switch to final rinse (3). Confirm the foam patches fade/disappear as the surface rinses.
9. Walk to the alien customer and press E. Confirm dialogue appears and the customer reacts to accepting/completing the contract.
10. Complete the rinse/inspection flow and verify payout happens once.
11. Check the Unity Console for compile errors, exceptions and relevant warnings.

## Acceptance criteria

- No C# compile errors after import.
- Contract board opens, all three offers can be selected, and Accept closes the board and starts the chosen contract.
- Mouse clicks work while the board is open; player movement/look remain suspended until it closes.
- Foam is clearly visible on the hull at spray impact points from normal gameplay distance.
- Final rinse visibly removes/fades the foam.
- Foam patches do not block subsequent spray hits.
- Customer can be targeted and spoken to; dialogue is readable and customer reactions match job state.
- Existing UFO arrival, spray, stage progression, inspection and payout still work.
- No new Console exceptions.

## Evidence to return

- Screenshot of the contract board.
- Screenshot of foam visibly applied to the UFO.
- Screenshot or short clip of customer dialogue/reaction.
- First Console error/exception if any.
- Brief verdict: what worked, what felt bad, and what should be changed next.

## Out of scope

- New ship classes.
- Employee hiring or automation.
- Save/load expansion.
- More tool modes.
- Major environment rewrite.

Do not mark this task complete until the owner has tested and accepted the result.
