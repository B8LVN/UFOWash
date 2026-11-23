# UFO WASH — Lessons Learned & Quality Recovery Log

This file records observed failures and verified learning. Do not invent root causes. Mark evidence and confidence.

## Known user-reported problems (root causes not yet verified)

### L-001 — Previous output felt visually poor and too primitive

- **Observation:** The user reported that the game looked too bad/low quality to play.
- **Root cause:** Unknown until current scenes/assets/screenshots are audited.
- **Confidence:** High that the dissatisfaction exists; low/unknown on technical cause.
- **Prevention:** Apply the visual quality bible; use real gameplay screenshots for review; do not label greybox scenes as polished; establish a reference board before production art.
- **Verification needed:** Inspect current build and scene evidence.

### L-002 — Features may have been implemented without a convincing playable loop

- **Observation:** The user reported that the result was not sufficiently playable and that implementation felt like “stupid shit” despite extensive concept instructions.
- **Root cause:** Unknown until code, scene and playtest evidence are audited.
- **Confidence:** High that the current result did not meet expectations; unknown on exact cause.
- **Prevention:** Prioritize one integrated end-to-end job loop; use acceptance criteria and playtest evidence; do not count disconnected scripts/menus as gameplay.
- **Verification needed:** Audit project and test the current loop.

### L-003 — Chat instructions alone did not preserve project context reliably

- **Observation:** The user wants the developer to stop forgetting decisions, issues, improvements and next actions across tasks/conversations.
- **Root cause:** Project memory was not sufficiently externalized into version-controlled, regularly updated files and task records.
- **Confidence:** Medium; verify the workflow history where available.
- **Prevention:** Repository is canonical memory; mandatory read-at-start/update-at-end process; GitHub Issues and `NEXT_TASK.md`; every task ends with a handover.

### L-004 — Concept was mistakenly changed during attempts to fix implementation quality

- **Observation:** An unrelated scrapyard concept was proposed even though the user wanted to keep UFO Wash.
- **Root cause:** Design inspiration was misinterpreted as a request to replace the core concept.
- **Confidence:** High.
- **Prevention:** `PROJECT_CHARTER.md` and D-001 are non-negotiable. References are gameplay/production inspiration only. Never replace UFO Wash without explicit owner approval.

## New lesson template

### L-XXX — Short factual title

- Date:
- Observed behaviour:
- Expected behaviour:
- Evidence (issue/build/commit/screenshot/log):
- Root cause:
- Confidence (High/Medium/Low):
- Fix applied:
- Regression test/prevention rule:
- Verification status:

## Rule

A lesson is not “verified” until the corrective action has been tested. Keep observations, hypotheses and confirmed root causes separate.
