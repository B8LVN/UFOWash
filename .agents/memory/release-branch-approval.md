---
name: Release branch approval
description: Owner's release gate between development/testing and the user-facing main branch.
---

`main` is the branch that goes to users. Use `ufo-wash/cleaning-recovery` for development and testing. Move changes into `main` only when everything is OK and the owner explicitly says OK.

**Why:** The user specified this branch workflow.

**How to apply:** Commit and push development/testing work to the development branch. Do not merge or push those changes into `main` without the owner's explicit approval.
