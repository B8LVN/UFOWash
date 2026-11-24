# UFO WASH — High-visibility visual overhaul research

## Owner's target

Make Bay 01 and its first shift presentable for friend feedback. Keep UFO WASH's
frontier-garage identity: repaired cream/oxidized-orange equipment, basalt and
dust outside, restrained teal instrumentation, warm work lights against a cold
planetary dusk. Avoid the generic polished spaceship interior look and avoid
mixing unmatched asset-store art styles.

The new direction should first replace the player-facing "hero reads": customer
saucer, bay frame and lighting, garage approach, and contract/workbench UI. Smaller
props and distant background dressing come after those establish a coherent scene.
Do not claim that installing more models automatically makes the game high-end.
Materials, lighting, silhouettes, scene composition, scale and UI must be tuned
as one presentation.

## Existing library check

The project already contains Kenney Space Kit (CC0) and Kenney Modular Space Kit
(CC0), thousands of imported source files, authored Vesper textures, and
custom-built ship/garage meshes. Downloading another undirected pack would add
bulk without solving the mismatch. The sources below offer a better selection:
the Quaternius kits for a cohesive stylized baseline, Poly Haven / ambientCG for
real surface response, and a later deliberate ship-model pass.

## Sourced free candidates

Pack details and licenses were checked against their official pages on
2026-11-24. All listed packs are free and do not require a paid purchase:

| Source | Verified contents and license | Best use here | Caution |
|---|---|---|---|
| [Quaternius Modular Sci-Fi MegaKit](https://quaternius.com/packs/modularscifimegakit.html) | 277 modular models; FBX, OBJ, Blend and glTF; the pack page marks CC0 | Build a recognizable, authored garage layout from actual modular parts instead of more isolated primitives | Stylized modular look; do not use every piece or flatten the Vesper identity |
| [Quaternius Ultimate Space Kit](https://quaternius.com/packs/ultimatespacekit.html) | 92 models; FBX, OBJ, Blend and glTF; CC0 | Ship and planetary props if a silhouette improves the current first customer | Inspect the ship silhouettes before choosing; the working UFO needs its own strong identity |
| [Quaternius Stylized Nature MegaKit](https://quaternius.com/packs/stylizednaturemegakit.html) | 116 models; FBX, OBJ, Blend and glTF; CC0 | Selected exterior foliage/rocks for a more inhabited small planet | Use selectively; too much foliage can undermine the barren frontier setting |
| [Poly Haven models](https://polyhaven.com/models) plus [license](https://polyhaven.com/license) | The official license states all models, HDRIs and textures are CC0 | Scanned rock assets, better sky/lighting references, and a few realistic set-dressing elements | PBR realism can clash with stylized modules unless constrained to rocks, ground and equipment |
| [ambientCG materials](https://ambientcg.com/) plus [license](https://ambientcg.com/license) | The official license permits commercial use under CC0 1.0 | Replace flat color on floors, painted panels, rust and concrete with deliberately chosen PBR maps | Textures alone do not replace model silhouettes or lighting |

## Recommended implementation order

1. Tune local spray coverage, no-op feedback and stage-specific progress before
   adding content; expose which hull zones still need work rather than presenting
   one confusing overall cleanliness number.
2. Selectively integrate a few MegaKit pieces into a **separate, previewable**
   garage-layout scene. Preserve the playable and prototype scenes, their metadata,
   and direct Play → main menu flow. Do not replace or regenerate the canonical scene.
3. Integrate a small CC0 material/HDRI set with attribution/license records and
   consistent texel scale; test on the authored bay.
4. Replace the customer UFO's hero silhouette and critical close-up details using
   a cohesive set; keep mesh panels, colliders and cleaning/stage gates intact.
5. Rework the main menu, contract terminal, and cleaning HUD as one readable set.
6. Require owner-run Unity import, Play Mode, and actual Game-view screenshots
   before calling the presentation ready to show friends.

## Current state

- The performance/feedback changes are source edits and pass the deterministic
  tests; there has been no Unity import or Play Mode screenshot yet.
- The five external sources above have not been downloaded or imported. They are
  license-vetted candidates, not a finished art implementation.
- No paid packs, scene replacement, migrations or build/deployment changes were
  made.
