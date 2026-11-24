# UFO WASH — Project Audit 001

**Date:** 2026-11-23  
**Repository:** `B8LVN/UFOWash` — `main`  
**Audit type:** GitHub source and project-structure audit  
**Status:** Source audit complete; local Unity verification outstanding.

## Evidence reviewed

- Repository tree and recent commits, including the owner-added Kenney Space Kit and project documentation.
- `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`.
- Three scenes: Playable, PowerSlice and Prototype.
- First-person, interaction, cleaning, spray, career, HUD, upgrade, builder, importer and validator source.
- Kenney Space Kit `License.txt`.
- Owner report: imported ship is now visible; foam is not visibly coating it; contract selector looks poor; customer character is inert; gameplay remains unclear and unsatisfying.

### Missing evidence
- Current Unity Console output.
- Current Play Mode screenshots/recording.
- Local Git working-tree status and any unpushed files.
- Unity compile, Play Mode and Windows build performed during this audit.

## Verified project baseline

| Area | Repository evidence |
|---|---|
| Unity | 6000.6.10f1 |
| URP | 17.6.0 |
| Input System | 1.20.1 |
| uGUI | 2.6.0 |
| Scenes | Playable, PowerSlice, Prototype |
| Imported art | Kenney Space Kit 2.0, FBX/DAE models and PNG sprites |
| Asset license | Included license states CC0 and permits commercial use |
| UFO/customer models | Builder selects `craft_speederA` and `alien` when present |
| Gameplay systems | First-person movement, spray, surface cleaning, ship arrival, contract/supply interactables, HUD and washer upgrade |
| Verification | The optional Editor validator was removed; it was not a gameplay dependency |

The customer ship, contract board and supply station classes are defined inside `UfoWashCareerLoop.cs`; they are not missing, despite not having separate files.

## Baseline feature assessment before the recovery pass

| System | Source status | Assessment |
|---|---|---|
| First-person movement | Implemented | Feel/collision unverified |
| UFO arrival | Implemented | Linear approach/dock/depart; no queue or bay assignment |
| Customer character | Static model | No dialogue or job-state behaviour |
| Contract selection | Default accept/submit | No offer comparison or meaningful selection UI |
| Cleaning stages | Implemented | Pre-rinse, foam, final rinse |
| Dirt feedback | Renderer-level tint | No local dirt mask |
| Foam feedback | Scalar plus colour tint | No persistent local coating |
| Spray feedback | Transient particles | No persistent foam application |
| Economy | Basic credits, payouts and supplies | Balancing/repeated-job flow unverified |
| Upgrade | One washer upgrade | Numeric power/range/coverage changes only |
| HUD | Runtime uGUI Text | Functional readouts, not finished presentation |
| Interaction prompt | OnGUI | Basic prompt, not a contract selector |
| Save/load | Not found in inspected gameplay source | No persistence proof |
| Employees/automation | Not found in inspected gameplay source | Long-term business fantasy not yet represented |
| Audio | No gameplay audio implementation found in inspected scripts | No authored tool/ship/customer sound feedback in reviewed code |
| Environment | Procedural builder plus imported models | Much of the hangar remains primitive geometry |

## Main problems and causes

### P1 — Foam does not read as foam
Old `UfoWashDirtSurface.Process` changed a numeric foam value; `ApplyColor` blended a tint across the renderer. No persistent coating was created at the nozzle contact point.

**Impact:** Player cannot read where foam landed or what must be rinsed.  
**Action:** Add local persistent foam patches at the hit point and fade/remove them during final rinse. Treat this as a prototype until visually accepted.

### P1 — Contract interaction has no meaningful choice
Old `UfoWashJobBoard.Interact` accepted the default job; feedback was a basic prompt.

**Impact:** No job comparison, customer context or meaningful decision.  
**Action:** Add multiple offers with a clear brief, contamination profile and payout; pause player controls while the board is open.

### P1 — Customer character is scenery
The builder instantiated the alien model/fallback without a behaviour component.

**Impact:** Character communicated no request or reaction.  
**Action:** Add idle attention, player-facing dialogue, talk interaction and reactions to job acceptance/completion. This is not a full animated NPC system.

### P1 — Cleaning is too abstract
The process advanced based on time spent spraying renderer-level surfaces and global averages.

**Impact:** Player changes broad tints and values rather than feeling contact-based treatment.  
**Action:** Improve spatial feedback first; then benchmark local dirt decals/masks. Do not add more tool modes until the basic loop feels good.

### P2 — Progression is thin
One upgrade changes numeric washer stats; save/load, employees and automation were not found in inspected gameplay scripts.

**Action:** Prove one enjoyable job and one purchase that changes play before adding automation.

### P2 — Visual/audio cohesion is incomplete
Hangar is largely procedural primitive geometry; no gameplay audio system was found in inspected scripts.

**Action:** Art-direct one bay with coherent props, decals, materials, lighting and sound after the core loop is accepted.

## Keep / improve / defer

**Keep:** UFO WASH identity, first-person service fantasy, imported Kenney spacecraft/alien, URP baseline, working arrival/docking and basic job/economy logic.

**Improve now:** Local foam feedback, contract selection, customer interaction, point-of-contact cleaning feedback and a guided first job.

**Defer:** More ship classes, large tech tree, multiple bays, employee rosters, complex automation and save expansion until the manual job is accepted.

## Recovery pass committed after baseline review

- `UfoWashDirtSurface.cs`: local foam patches at spray hit point; final rinse fades/removes them.
- `UfoWashSprayTool.cs`: forwards hit point, normal and spray radius.
- `UfoWashCareerLoop.cs`: three offers with different contamination/payouts and customer notifications.
- `UfoWashContractTerminalUI.cs`: selectable uGUI contract board using Input System UI input.
- `UfoWashCustomerNpc.cs`: idle attention, speech bubble, talk interaction and job-state reactions.
- `UfoWashFirstPersonController.cs` / `UfoWashPlayerInteraction.cs`: suspend movement/world interaction while board is open.
- `UfoWashPowerSliceBuilder.cs`: attaches NPC behaviour to imported alien or fallback.
- The optional structural validator described during this audit was later removed; gameplay does not depend on it.

**Status:** source committed; Unity compile/runtime unverified.

## Next recovery sequence

1. Owner playtests the exact pass and reports Console errors/screenshots/video.
2. Fix P0/P1 regressions.
3. Tune foam patch scale/density/opacity/rinse fade from actual screenshots.
4. Make the first contract and customer request self-explanatory.
5. Improve dirt localization.
6. Add authored audio and art-direct the starter bay.
7. Only then add a meaningful purchase and one physical automation payoff.

## Conclusion

The source audit found concrete causes that align with the owner's feedback: foam was only a renderer tint, contracts offered no choice, and the alien was only a model. The recovery pass addresses those three gaps in source, but is not yet a verified playable improvement. Local compile and owner playtest are the next gate.
