---
name: Hosted Unity verification
description: Distinguishes installing the exact Linux Editor from an authorized, working Unity verification session.
---

Treat Editor installation, native-library startup, licensing, project import and Play Mode as separate gates.

**Why:** On this hosted Linux checkout, the exact Editor's version command succeeded while project import first failed from missing ICU, then stopped because no valid Unity Editor license was available. Installation alone was not a working gameplay-verification route.

**How to apply:** Consult current production status for evidence and the current blocker. Fix native dependencies before diagnosing licensing; never bypass activation or treat standalone C# checks as Unity compilation. Keep Editor downloads and disposable imports out of Git. Resume runtime/visual acceptance only in an authorized, working exact-version Editor session.
