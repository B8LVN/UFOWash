---
name: Release branch approval
description: Owner's release gate between development/testing and the user-facing main branch.
---

Use one local Unity project and one repository with two branches, not separate project folders. `main` is the branch that goes to users. Use `ufo-wash/cleaning-recovery` for development and testing. Move changes into `main` only when everything is OK and the owner explicitly says OK.

**Why:** The user specified this branch workflow.

**How to apply:** Switch branches inside the existing project repository; do not recommend another clone or a second Unity Hub project as the normal testing workflow. Commit and push development/testing work to the development branch. Do not merge or push those changes into `main` without the owner's explicit approval.
