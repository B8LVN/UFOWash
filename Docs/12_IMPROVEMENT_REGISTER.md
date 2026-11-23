# UFO WASH — Improvement Register

Use this file to track recurring quality gaps, what must improve, what has changed and whether the improvement was actually verified. Do not move an item to Verified based on a code change alone.

## Status definitions

- **Observed:** User or tester reported the issue; root cause may be unknown.
- **Investigating:** Evidence collection/root-cause analysis is underway.
- **Planned:** An approved corrective task exists.
- **Implemented — unverified:** Code/assets changed, but proof is incomplete.
- **Verified:** The corrective change passed the stated verification method.
- **Accepted:** Owner reviewed and accepted the result.
- **Deferred:** Intentionally postponed with a reason.

## Improvement register

| ID | Area | Problem / desired improvement | Evidence / origin | Planned action | Verification method | Status | Related issue |
|---|---|---|---|---|---|---|---|
| IMP-001 | Visual quality | Previous output was reported as visually poor/primitive. | Owner feedback; actual scene still needs inspection. | Audit scenes/assets; establish reference board and visual quality baseline. | Actual gameplay screenshots/recording reviewed against visual bible. | Observed | UW-000 |
| IMP-002 | Playability | Previous output was reported as insufficiently playable. | Owner feedback; exact broken/weak systems not yet verified. | Audit core loop and run repeatable playtest. | Tester completes the job loop without developer intervention. | Observed | UW-000 |
| IMP-003 | Progression | Ensure spending money unlocks genuinely new capabilities, not only small numeric boosts. | GDD pillar. | Validate upgrade choices against player-facing changes. | Before/after gameplay comparison and playtest. | Planned | UW-001 |
| IMP-004 | Automation | Automation must be visible, physical, understandable and diagnosable. | GDD pillar/reference research. | Prototype one small end-to-end automated stage. | Demonstrate inputs, processing, output, throughput and blocked state. | Planned | UW-070 |
| IMP-005 | Project memory | Prevent lost decisions, forgotten defects and repeated mistakes between chats. | Owner request. | Maintain repository memory, issue links, session log and handover every task. | New task begins by reading repository; status and next task updated at handover. | Planned | UW-000 |

## How to add an item

Every item needs a measurable verification method. Avoid vague statements such as “make it better.” State what a tester should see, do or measure, and what evidence is required.

## Improvement completion rule

Keep the original row and update status/evidence. Do not erase failed attempts. Record meaningful improvements in `CHANGELOG.md` and `docs/13_SESSION_LOG.md`.
