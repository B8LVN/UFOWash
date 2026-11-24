# UFO WASH — Current Project Status

**Last updated:** 2026-11-23  
**Product target:** Full commercial single-player PC/Steam game; a first-shift slice is a milestone, not the final product.  
**Current milestone:** UW-104 — contracted hull zones, customer satisfaction and service economy.  
**Build policy:** No Windows/standalone/player build until the game is genuinely playable and the owner requests one.

## Project baseline

- Repository: B8LVN/UFOWash on main.
- Unity Editor version: 6000.6.10f1; URP 17.6.0; Input System 1.20.1; uGUI 2.6.0.
- Main authored scene: Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity.
- Included Kenney Space Kit is CC0; no paid assets have been added.
- The scene builder creates one custom procedural saucer, an imported alien when available, a Vesper Belt panorama, hangar dressing, player, UI, contracts and supply/upgrade stations.
- Kenney Modular Space Kit 1.0 is now imported: 40 CC0 modular station FBX models plus its source atlas/license. A tailored Vesper palette derivative is at Assets/UFO_WASH/Art/Textures/Vesper_ModularSpace_Colormap.png.
- Generated scene source now describes a walkable arrival ramp/outpost and three purchasable sector gates. The scene has not regenerated because Unity currently reports a dirty active scene; the builder skips such a refresh safely.
- Vesper terrain/sky materials and generated textures exist in the repository. The world and hangar remain an early procedural art pass; no production-quality open planet exists yet.

## Previous local observations

An earlier Unity Editor/Windows player observation showed the title screen, story/briefing/tutorial flow, readable customer subtitle and a third-person station view. Those observations predate the newest gameplay edits and do not verify them. A Windows player was built and launched during earlier work; it is not being treated as a deliverable and no further player build is authorized now.

## Source changes in progress

- Career jobs now bind to the arriving ship’s own service panels rather than every globally registered dirt surface.
- Every contracted panel must satisfy each ordered treatment threshold; off-contract surfaces are rejected.
- First-person cleaning view activates only while aiming/holding wash on a contracted ship panel.
- HUD now reports dispatch/service state and panel-by-panel work.
- Customer traits, quality-gated tip rules, reputation gains/losses and reputation-locked contract tiers are added to the source.
- Contract selection cards now refresh their real title, customer, brief, hazard, reward and tip terms after the board opens.
- Living GDD v2.2 replaces [EXPAND] placeholders and defines the commercial game vision and all six design pillars.
- A compact connected world is defined: four 80 m districts, 4 m build cells, a 40 m × 48 m starting yard and staged GC/reputation-gated expansion.
- Crater Ridge, Ion Flats and Glasswake gate purchase/persistence are implemented in source. Runtime and editor C# compilation checks passed against Unity 6.6 references; Unity import and Play Mode remain outstanding.
- Career-loop panel discovery now runs in `Start`, after all scene `Awake` calls, preventing an initialization-order race with the customer's cached surfaces.

## Verification honestly outstanding

- Runtime and editor C# assemblies compiled against Unity 6.6 generated project references with zero errors. The runtime compile had zero warnings; the editor compile reported six existing API deprecation warnings. This is a compiler check, not a live Unity Editor import.
- Unity's open main project did not refresh after the source timestamp change. The new kit models and script still need Unity-generated `.meta` files in the main checkout.
- An isolated batch-mode import was attempted in a disposable project copy and stopped during Unity LicensingClient access-token validation, before asset import. The disposable copy was removed; the owner's main project was not touched by the attempt.
- The Unity window currently has the generated scene marked dirty. The automatic builder intentionally skips a dirty active scene to protect unsaved work; preserve that scene before using the generated-scene menu action.
- The new contract-card rendering, reputation progression, panel checklist, camera handoff and full payout loop have not been tested together in Play Mode.
- Current scene generation after the latest builder changes has not been observed; the checked-in scene file does not yet contain the new outpost and sector gates.
- Multi-sector construction, utilities, save/load, staff AI, automated production, Rare Residue, audio, accessibility and commercial/Steam QA are not yet implemented.
- No current screenshot should be described as a finished or production-ready world.

## Development rules

- Keep code, generated scene, current GDD, backlog and project memory synchronized.
- Preserve owner files and review the entire working-tree diff before commit or push.
- Use included/free assets or original assets; no paid content without approval.
- Keep the owner’s running application untouched. Source compilation is checked; import, scene generation and Play Mode need a safe editor refresh with the dirty scene preserved.
- Continue through all six pillars. Stop/pause only at the owner’s direction or a genuine blocking dependency.

