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

## 8. Evidence rules

Evidence can include Unity Console screenshots, Editor screenshots, gameplay recordings, build version, commit hash, automated test reports and reproduction notes. Always date evidence and tie it to a specific build or commit when possible.

Do not infer playability or visual quality from source code alone.
