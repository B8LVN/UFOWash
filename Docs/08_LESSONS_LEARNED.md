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

### L-007 — Visual work and progress must use the same local state

- **Date:** 2026-11-24
- **Observed source behavior:** Dirt was decremented for a whole renderer; separate foam patches all faded from the panel-wide foam number. The advertised power upgrade modified an unused tool field.
- **Root cause / confidence:** High for the source-level mismatch between feedback, accounting and purchased effects; actual appearance remains untested.
- **Correction:** Local area-weighted coating state drives both visible dirt/foam and panel totals; tool power and fan footprint feed that state.
- **Prevention:** Test a stationary spray against untouched neighboring cells, then compare actual gameplay before/after the purchase. Do not approve local feedback based on aggregate percentages.
- **Verification:** 14 deterministic state checks passed; Unity integration/visual acceptance remains blocked.

### L-008 — Local coating raycasts depend on consistent hull winding

- **Date:** 2026-11-24
- **Observation:** Contract acceptance failed during coating creation with `A coating requires exposed surface cells` for the upper saucer shell.
- **Expected:** Each cleanable hull shell should expose outward-facing coating cells.
- **Root cause:** Reconstructed the upper-shell profile and builder triangle order; its closed-mesh signed volume was negative, while the lower shell's order is outward. Confidence high for the geometry math; Unity runtime behavior is not yet verified.
- **Fix:** At runtime, clone and reverse triangles only for inward closed meshes, update associated filter/collider references, and dispose/restore the clone on teardown; do not mutate the scene mesh asset.
- **Regression test:** Deterministic reproduction asserts the exact upper-shell profile is inward, reversed indices are outward, and source indices remain unchanged. Also check flat open QA surfaces are not reversed.
- **Verification status:** Geometry rule checks pass; actual Unity collider hits and visual culling require the owner's licensed Editor.


### L-005 — A whole-renderer foam tint is not readable foam application

- **Date:** 2026-11-23
- **Observation:** Foam existed as a numeric scalar and broad colour blend, but the player could not see a convincing coating where the spray landed.
- **Expected behaviour:** Foam should visibly appear at the nozzle contact point and be removed by final rinse.
- **Root cause:** The previous implementation changed material tint but created no persistent local surface geometry/decals.
- **Confidence:** High for the code-level cause; runtime appearance of the new patch prototype is unverified.
- **Fix applied:** Added local foam patches at the hit point and fade them as foam state decreases.
- **Regression test:** Spray a hull area, move to another area, confirm separate patches; final-rinse and confirm patches fade; verify patches do not block spray.
- **Verification status:** Implemented — unverified.

### L-006 — A character model is not an NPC system

- **Date:** 2026-11-23
- **Observation:** The alien was present in the scene but did not participate in the job.
- **Expected behaviour:** The customer should communicate a request, acknowledge the player and react to job completion.
- **Root cause:** The scene builder instantiated a model without a behaviour component or dialogue interaction.
- **Confidence:** High from source inspection.
- **Fix applied:** Added talk interaction, idle attention, speech bubble and contract-state reactions.
- **Regression test:** Approach the NPC, use E, accept a job and complete it; confirm dialogue and reactions.
- **Verification status:** Implemented — unverified.
