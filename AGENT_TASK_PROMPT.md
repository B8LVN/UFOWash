# Reusable Prompt — Start Every UFO WASH Task

Copy this prompt into the development conversation whenever a new task begins. If the coding agent has repository access, it must inspect and update the repository directly. If it does not have access, it must request the relevant files or a project ZIP and must not pretend it can see GitHub.

---

You are continuing the existing UFO WASH project. Do not treat this as a new project and do not redesign the game from scratch.

**Before doing anything else, read `AGENT_START_HERE.md` and follow it exactly.** Then read `NEXT_TASK.md`, `docs/11_CURRENT_STATUS.md`, the relevant GDD/technical/QA documents, the decision log, lessons learned, recent changelog entries, relevant GitHub Issues and recent commits.

The GitHub repository is the canonical long-term memory. Do not rely on this chat alone. Inspect the actual files and current Git state before proposing changes. Do not overwrite user changes or assume files/scenes/assets exist without verifying them.

### Task

Use the one task currently authorized in `NEXT_TASK.md` and its linked GitHub Issue. If no task is authorized, ask the owner to choose one. Do not silently substitute a different task.

### Required working method

1. Summarize the task, scope, dependencies and acceptance criteria.
2. Inspect the existing implementation and relevant evidence first.
3. Follow `PROJECT_CHARTER.md`, the GDD, technical design, visual quality bible, QA strategy, decision log and lessons learned.
4. Implement a complete, integrated feature or a focused fix—not disconnected scripts or a superficial mock-up.
5. Preserve working functionality and explain any structural change before making it.
6. Use version-compatible Unity 6.6 APIs and packages. Research uncertain/version-sensitive issues using authoritative sources.
7. Provide complete files or a patch with exact paths and Unity setup instructions.
8. Test only what you can genuinely test. Clearly distinguish code inspection, static checks, automated tests, Editor tests and user playtests. Never claim local Unity execution without evidence.
9. Update the repository memory before finishing: at minimum `NEXT_TASK.md`, `docs/11_CURRENT_STATUS.md`, `CHANGELOG.md`, `docs/10_BACKLOG.md` and `docs/13_SESSION_LOG.md`; update `docs/12_IMPROVEMENT_REGISTER.md` and decisions, lessons, asset register and QA records when applicable.
10. Update the matching GitHub Issue with files changed, test evidence and remaining risks. Do not close it until every acceptance criterion is verified and owner acceptance is recorded.

### Quality standard

UFO WASH is an immersive first-person alien spacecraft cleaning and servicing business simulation. The player starts doing the work manually, earns money, upgrades tools and facilities, hires workers, creates visible physical workflows, automates bottlenecks and expands the business. Moon Sweepers and Schedule I are references for satisfying hands-on work, progression, reinvestment, business growth and automation—not sources to copy. Keep the UFO WASH identity.

Do not present primitive cubes, default materials or empty scenes as finished visual work. Greyboxing is allowed only when explicitly labelled as temporary and used to test mechanics. Every interaction should have clear input, feedback, state change and reward/consequence. Money must unlock meaningful new possibilities, not only inflated numbers.

### End-of-task handover

Report:
- Summary of implementation.
- Files created/modified.
- Test evidence and exact test status.
- What remains unverified.
- Known bugs/risks.
- Documentation and GitHub Issue updates.
- Commit/branch details only if actually created.
- The next task now recorded in `NEXT_TASK.md`.

Do not finish until the repository memory has been updated. If you cannot update the repository directly, provide the complete updated Markdown files for me to commit and clearly state that the repository itself was not changed.
