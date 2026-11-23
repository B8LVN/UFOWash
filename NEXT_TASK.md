# Next Authorized Task

**Task ID:** UW-000  
**Title:** Audit the Existing UFO WASH Unity Project and Establish a Verified Baseline  
**Priority:** P0 — blocking  
**Status:** Ready for audit  
**Owner:** Lead Developer  
**Implementation authorization:** Audit only. Do not begin major feature work until the audit report is delivered and reviewed.

## Objective

Establish what actually exists in the current Unity project, why the current result has been judged too poor/primitive/unplayable, and what must be improved. This is an evidence-based audit, not a concept rewrite.

## Required inputs

- Current Unity project repository or a ZIP of the project (excluding Library/Temp/Logs/Obj).
- Current screenshots or gameplay recording, if available.
- Current Console errors/warnings, if available.
- Existing relevant GitHub Issues and recent commits.

If any input is unavailable, list it as missing. Do not invent findings.

## Scope

1. Verify Unity Editor version and packages.
2. Inspect project structure, scenes, prefabs, scripts, materials, models, lighting, UI and input.
3. Identify what is functional, partially functional, broken, placeholder-only or unknown.
4. Evaluate first-person controller, camera, interaction feel, UFO arrival/servicing, cleaning feedback, economy, progression and automation against the GDD.
5. Evaluate visual quality and asset cohesion against `docs/04_VISUAL_AUDIO_QUALITY_BIBLE.md`.
6. Review code architecture, maintainability, errors, performance risks and save/data design.
7. Identify the root causes of poor results where evidence permits; assign confidence (High/Medium/Low).
8. Separate critical fixes from redesign, polish and future scope.
9. Recommend the smallest sequence of changes that can produce a convincing playable vertical slice.

## Deliverable

Create `docs/reports/PROJECT_AUDIT_001.md` containing:

- Evidence reviewed and missing evidence.
- Verified project baseline.
- Feature-by-feature implementation status.
- Visual quality assessment.
- Gameplay quality assessment.
- Technical/code assessment.
- Bugs and blockers ranked by severity.
- Root-cause hypotheses with confidence and evidence.
- Keep / improve / replace recommendations.
- Proposed vertical-slice recovery plan.
- Exact next implementation task and acceptance criteria.

## Acceptance criteria

- No unverified claim is presented as fact.
- Findings link to relevant files, screenshots, logs or scenes wherever possible.
- Every major issue has impact, evidence and recommended action.
- Existing working functionality is identified and protected.
- The report does not replace or change the UFO WASH concept.
- The next task is clearly recorded in `NEXT_TASK.md` and `docs/10_BACKLOG.md`.

## Out of scope

- Rewriting the project before the audit.
- Replacing UFO WASH with another game.
- Building unrelated features.
- Claiming a fix has been tested without evidence.
