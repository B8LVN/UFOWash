# UFO WASH — Current Project Status

**Last updated:** 2026-11-23  
**Overall state:** Core gameplay recovery pass committed to GitHub; Unity compile and Play Mode verification still outstanding.  
**Current milestone:** M2 — Core Cleaning Loop Recovery.  
**Current task:** UW-101 — Owner playtest of foam, contract board and customer interaction.

## Verified repository baseline

- Repository: `B8LVN/UFOWash`, default branch `main`.
- Unity Editor version: **6000.6.10f1**.
- Packages: URP **17.6.0**, Input System **1.20.1**, uGUI **2.6.0**.
- Scenes present: `UFO_WASH_Playable`, `UFO_WASH_PowerSlice`, `UFO_WASH_Prototype`.
- Kenney Space Kit 2.0 model and sprite files are committed under `Assets/UFO_WASH/Art/Imported/KenneySpaceKit`.
- Included `License.txt` states CC0 and permits commercial use.
- Scene builder selects imported `craft_speederA` spacecraft and `alien` customer when available.
- Main systems exist for first-person movement, cleaning, customer ship movement, contracts, supplies, HUD and washer upgrades.

## Owner-reported state

- The UFO model is now visible in Play Mode.
- The prior foam implementation was not visibly coating the UFO.
- The contract selector looked poor and offered no satisfying job choice.
- The customer model was present but did not meaningfully participate.
- The overall gameplay loop still felt unclear and unsatisfying.

These are owner playtest observations, not inferences from code.

## Source changes in the latest recovery pass

- Added persistent local foam patches at spray impact points; final rinse fades/removes them.
- Added three contract offers with distinct briefs, contamination profiles and payouts.
- Added a full-screen contract selection board with mouse UI input.
- Added an interactive customer NPC with idle attention, dialogue and job-state reactions.
- Paused player movement and world interaction while the contract board is open.
- Added the interactive customer to the generated scene and validator.

## Current implementation status

| Area | Status |
|---|---|
| Imported UFO model | Owner confirms visible |
| First-person controller | Source exists; feel not independently tested |
| Ship arrival/docking | Source exists; runtime behaviour unverified |
| Local foam patches | Implemented in source; visual result unverified |
| Contract board | Implemented in source; button input/layout unverified |
| Customer NPC interaction | Implemented in source; collider/dialogue presentation unverified |
| Job completion and payout | Source exists; full loop unverified |
| Save/load | Not implemented in inspected gameplay source |
| Employees/automation | Not implemented in inspected gameplay source |
| Audio pass | Not implemented in inspected gameplay source |
| Production-ready art pass | Not accepted; builder still creates much of the station from procedural primitive geometry |

## Not yet verified

- Unity compilation after the latest commits.
- Automatic scene refresh after builder script import.
- Foam patch rendering on the imported spacecraft in Play Mode.
- Contract board mouse input and cursor restoration.
- NPC raycast interaction, speech bubble readability and reactions.
- Complete job loop from offer selection through inspection and payout.
- Console cleanliness, Windows build and performance.
- Visual quality at actual gameplay camera height.

## Current blockers

1. Owner must pull latest `main` and run `Docs/QA_TEST_PLAN.md`.
2. Need screenshots/recording of the UFO with foam applied and the contract board open.
3. Need the first Unity Console compile/runtime error if any.

## Working rules

- GitHub repository is the canonical project memory.
- Distinguish code committed from code compiled, runtime-tested and owner-accepted.
- Do not add more disconnected systems before the owner tests this recovery pass.
- Keep UFO WASH; improve execution rather than changing the concept.

## Next action

Complete UW-101 in `NEXT_TASK.md`; report test evidence and update this status before starting another feature.
