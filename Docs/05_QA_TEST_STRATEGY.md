# UFO WASH — QA & Test Strategy

## 1. QA philosophy

QA is continuous. A script compiling is not proof that a feature works. A scene opening is not proof that the gameplay is good. A feature is complete only when its acceptance criteria are tested and evidence is recorded.

## 2. Test levels

- **Static review:** code inspection, references, serialization, null/error risks.
- **Automated/unit tests:** economy calculations, job state transitions, inventory rules and other deterministic logic.
- **Integration tests:** job flow across customer, bay, cleaning, reward, inventory and save systems.
- **Unity Editor playtests:** actual controls, animations, collisions, VFX, UI and scene behaviour.
- **Build tests:** Windows build startup, save/load and runtime behaviour outside the Editor.
- **Performance tests:** representative scene and workload, with hardware/settings recorded.
- **User acceptance/playtest:** owner or external tester assesses clarity, feel, enjoyment and progression.

Always state which level was actually performed. Do not call static code review a gameplay test.

## 3. Severity definitions

- **P0 Critical:** project cannot open/build, save corruption/data loss, blocker preventing core loop, severe security/data integrity issue.
- **P1 High:** core gameplay feature broken, progression blocked, repeatable crash, severe visual/input issue affecting normal play.
- **P2 Medium:** feature partially broken, workaround exists, significant usability/presentation defect, non-critical state inconsistency.
- **P3 Low:** minor visual/audio issue, small UI inconsistency, edge case with low impact.

## 4. Core regression areas

After changes to the relevant system, retest:

1. Project opens with correct Unity version and packages.
2. Main scene loads without missing references.
3. Player movement, look and interaction.
4. UFO arrival and bay assignment.
5. Job state transitions.
6. Tool activation and cleaning feedback.
7. Dirt progress and completion criteria.
8. Job completion and payment exactly once.
9. Purchase affordability and upgrade effects.
10. Inventory/supply consumption and restock.
11. Employee assignment and task execution.
12. Automation input/output, blocked states and throughput.
13. Save/load and repeated play sessions.
14. No new Console errors or relevant warnings.
15. Performance in a representative gameplay scenario.

## 5. Feature acceptance template

Every task should define:
- Preconditions.
- Player action or system trigger.
- Expected state change.
- Expected visual/audio/UI feedback.
- Expected economy/inventory effect.
- Negative/invalid cases.
- Regression tests.
- Evidence required (screenshot, recording, log, automated test output or build).

## 6. Bug report minimum fields

Title, severity, build/commit, Unity version, platform, scene, preconditions, reproduction steps, expected result, actual result, frequency, screenshots/video/logs, workaround, suspected cause (clearly marked as hypothesis), fix, regression verification.

## 7. Build acceptance

A build is not accepted until:
- It starts and reaches gameplay.
- Core loop works end-to-end.
- No P0 defects remain.
- No unresolved P1 defects affect the core loop.
- Save/load is tested if included in the milestone.
- Controls and resolution are usable.
- Performance result is recorded.
- Known issues are listed.
- Owner acceptance is recorded.

## 8. Local coating and cleaning feel

- A panel already at its current stage target gives a clear "already treated;
  sweep to an unfinished zone" message and consumes no water or chemical.
- Treating an unfinished zone still changes only the local brush footprint and
  consumes supplies in proportion to time actually applied.
- The HUD progress bar describes the current stage's all-zone gate (pre-rinse,
  foam, final rinse), not overall cleanliness or grade. Verify it advances as
  the remaining zones are treated.
- Base-nozzle cleaning should feel responsive; the purchased fan should retain a
  meaningful coverage advantage. Measure runtime performance and supply use on
  the full customer ship before further tuning.

## 9. Evidence rules

Evidence can include Unity Console screenshots, Editor screenshots, gameplay recordings, build version, commit hash, automated test reports and reproduction notes. Always date evidence and tie it to a specific build or commit when possible.

Do not infer playability or visual quality from source code alone.


## 9. Recovery-pass regression tests

### Contract board
- Open the board only after the UFO docks.
- Confirm three distinct offers, descriptions, contamination profiles and payouts.
- Select each offer and confirm selection feedback.
- Accept one offer; verify the UI closes, cursor relocks and player controls resume.
- Press Escape; verify the board closes without accepting a job.
- Confirm world interaction and movement remain suspended while the board is open.

### Foam application
- Spray one hull area in foam mode.
- Confirm persistent local patches appear at hit locations, not as a full-renderer colour wash.
- Move the nozzle to another panel and confirm coverage appears there too.
- Final-rinse; confirm patches fade/disappear as foam decreases.
- Confirm foam patches do not intercept or block later spray hits.
- Check appearance from normal gameplay distance and close range.

### Customer NPC
- Approach the imported alien/fallback character.
- Confirm the NPC can be targeted and interacted with using E.
- Confirm speech bubble is readable and faces the player.
- Accept a contract and confirm customer acknowledgement.
- Complete inspection and confirm customer reaction.

### Verification status
These tests are required in the owner's Unity Editor. Source changes and static inspection do not count as passing them.


## 10. Opening experience and scene structure

Before accepting the first playable chapter, verify the complete entry flow:

- Title menu appears before the player receives control.
- Story and How to Play are accessible without starting a shift.
- First-shift briefing establishes the immediate customer objective.
- UI navigation works through the Input System UI module at the target resolutions.
- Time remains paused until the player explicitly clocks in.
- Clock In closes the front end, restores time and enables player input.
- The generated scene uses named top-level Hierarchy groups and preserves world transforms.

The first narrative and tutorial are draft content. Owner feedback is required before treating the story as final.

## 11. Unity native-object lifecycle regression

Unity native rendering objects such as MaterialPropertyBlock must not be constructed from static field initializers or MonoBehaviour constructors. Create them from a valid Unity runtime/editor lifecycle method or lazily at the point of use. When a static-initialization exception is reported, add a regression check that enters Play Mode and exercises the affected rendering path, not merely a source-text check.

## 12. Local-coating recovery gate

The 2026-11-24 local-cell implementation supersedes section 9's foam-patch mechanism.
Run `dotnet run --project Tools/QualityChecks/QualityChecks.csproj` for deterministic coating rules only.
Run `UFOWASH.Editor.UfoWashRecoveryChecks.RunBatch` in a disposable exact-version Unity project for the two-contract rule fixture.
Neither replaces manual camera/input, authored-saucer visibility/access, sound, performance or screenshot acceptance.
The complete positive/negative checklist and evidence requirements are in `reports/QUALITY_RECOVERY_2026-11-24.md`.

### 12.1 Contract board and upper-saucer failure regression

- Enter Play, clock in, wait for the UFO, open dispatch, select Quick Decontamination
  and press Accept. Confirm the board closes and one contract starts.
- Confirm the upper saucer shell produces coating cells without Console errors.
- Select Deep Hull Clean, verify its card feedback; inspect locked VIP and accept behavior.
- Confirm modal hides lower-order HUD/customer subtitle canvases while open, and
  restores them on close and successful acceptance.
- Repeat at 16:9 and a narrower aspect ratio; no clipped, overlapping or off-panel
  copy. Verify buttons and input work.
- Source rules include `UfoWashHullWinding.IsInward` and are exercised by the
  .NET deterministic fixture. This does not replace Unity Play Mode acceptance.

## 13. Company ledger and first attendant

The following Unity Editor tests remain required; source inspection is not a pass:

- Fresh start: receive 750 GC, 2.1 reputation, full water and chemical, zero completed jobs, no hired attendant and no washer upgrades.
- Hire affordability: below 120 GC, hiring is denied without changing balance or save; at 120 GC or above, the tech is hired once and the exact cost is deducted.
- Attendant work: accept a contract and confirm only contracted panels are pre-rinsed; the service stage advances only after all zones meet the existing threshold.
- Blocked state: exhaust water and confirm the tech reports blocked, stops changing dirt, and resumes after a successful refill.
- Washer upgrade: buy an affordable upgrade, confirm power/coverage/reach increase once, save, and verify those values and the fan nozzle restore after restarting Play Mode.
- Land claim: buy an eligible sector expansion and verify its barrier/content state and cost persist after restarting.
- Save/load round-trip: complete a job, restart Play Mode, and compare credits, reputation, remaining supplies, completed-job count, washer level, hired state and unlocked land.
- Active-job boundary: interrupt a contract and restart; confirm the unfinished contract is not resumed, no payout is duplicated, and saved company progress remains.
- Corrupt primary save: with a valid backup, confirm backup recovery is visible and the damaged primary is preserved; with both files invalid, confirm saving is visibly disabled and neither file is overwritten.
- Missing save: confirm a new company starts with defaults and can create a save after a purchase or completed job.

For Editor QA, inspect `Application.persistentDataPath/UfoWash/company-save.json` and its `.bak` backup. Copy test files before destructive corruption checks. The service and attendant are injected for Play Mode only; do not regenerate or overwrite the authored scene to test them.

No Unity import, Play Mode run or filesystem corruption test was performed in this checkout because a licensed Unity Editor is unavailable.
