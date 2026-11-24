# UFO WASH — Garage to spaceport

## Status and owner direction

Design expansion, not implemented gameplay or accepted final art.
The owner requested a major UI/UX and 3D/world improvement: begin on a small
planet, wash UFOs in a low-cost garage, hire help, and visibly improve the garage
as upgrades are purchased. This refines the canonical Vesper/GDD direction.
One local project, two Git branches; main changes still require explicit approval.

## Core promise

Turn a patched-together roadside UFO wash into a recognizable planetary service
destination. Keep the original garage as part of the growing property so progress
has a visible reference point. Growth adds capabilities and operating choices,
not just larger buildings or percentage buffs.

## World and visual direction

- Vesper is a small inhabited frontier planet, not an empty infinite space map.
  Start with a compact playable garage district and a convincing planetary horizon;
  a fully traversable spherical planet is not required.
- Build a memorable arrival route: beacon ridge → dusty landing apron → garage
  → customer waiting corner → exit lane. Walkways must not cross ship approaches.
- Surround it with basalt outcrops, hardy alien scrub, an antenna, utility shed,
  neighboring freight stop and distant settlement lights. Keep expansion plots
  visible from the starting garage as future goals.
- Palette: worn cream wall panels, oxidized orange equipment, dark basalt,
  dusty blue/teal metal and restrained cyan indicators. Warm interior task lights
  against a cool twilight exterior. Avoid saturated emissive strips everywhere.
- Model tactile construction: doors with tracks, replaceable wall panels, pipe
  couplings, drains, wheels, hose reels, grilles and brackets. Poverty should read
  as repairs and mismatched equipment, not missing art.

## Visible progression ladder (proposed)

| Stage | What the player buys/enables | What physically changes |
|---|---|---|
| Inherited garage | One small scout, manual washer, limited stock | Patched roof, portable tank, hand-painted sign, battered dispatch terminal |
| Reliable local wash | Fan nozzle, filtration and better lighting | New nozzle on the tool, fixed pipe run, clean workbench, repaired fixtures |
| Professional garage | Better supply capacity and room for a first worker | Proper service desk, worker locker, organized chemical rack and safer service platforms |
| Two-bay yard | Concurrent customers and scheduled work | Side wall opens into a second bay; separate approach/exit lanes and waiting area |
| Service depot | Physical automation and specialist contracts | Gantry rails, treatment machinery, utility plant and inspection station |
| Planetary landmark | Larger ships, premium service and sector expansion | Tall hangar, distinctive signage, customer plaza and an active service yard |

Do not grant all upgrades at once. Offer staged choices: quicker coverage versus
less refill downtime versus capacity. Reveal total cost and tangible benefit
before purchase. Preserve access/collision, customer routes and save compatibility
when a garage module changes.

Hiring must visibly remove work, not provide passive income. Coordinate with the
separately assigned station-persistence/first-worker work rather than implementing
a competing employee or save system.

## Proposed first session

1. Main menu shows the actual small garage at rest.
2. New game starts at the garage door with a short actionable first customer request.
3. Inspect and wash a small courier; brief contextual hints explain only the current stage.
4. Receive a clear payment breakdown and learn the supply cost.
5. Buy a nozzle or a practical garage repair; see the installed change immediately.
6. Wash a second customer and notice the benefit.
7. Reveal the next goal: reliable supplies and enough sustained income to support help.

No forced multi-page story/tutorial sequence on every start. Story and full controls
remain available. Continue resumes existing progress when persistence supports it;
never show a nonfunctional Continue button or imply saves exist before verification.

## UI/UX overhaul

### Main menu
Garage backdrop; New Game, Continue when a valid save exists, Settings, Credits,
and Quit. Put Story/Help in secondary navigation. Explicit confirmation before
replacing a save. The garage backdrop can reflect progression later.

### Working HUD
Keep the ship central. Compact current objective/stage; water and chemical gauges
near tool feedback; reticle provides contact, range and blocked-action feedback.
Show the aimed panel's progress prominently. Expand the full panel checklist on
demand rather than covering the screen continuously. Use labels/icons as well as color.

### Dispatch and customer experience
Readable work-order cards: ship silhouette, contamination, required treatment,
expected supplies, payout and tip rules. Show acceptance, arrival and bay status
explicitly. Later customers differ through silhouette, accessible panel layout,
dirt pattern and service demands—not just a different text label.

### Upgrades and management
Workshop panel with before/after appearance, exact cost, prerequisites and practical
effect. Owned items are clearly different from affordable and locked items.
Compact employee cards: current job, wage, assignment and blocked reason. A deeper
management tablet is useful only once multiple bays/workers exist.

### Payment
Brief receipt with base, bonus, tip and actual recorded costs; distinguish revenue
from net income. Do not invent expense lines that the economy does not track.

### Interaction rules
Consistent confirmation/back controls, modal input isolation, legible type,
UI scaling, color-independent status and comfortable camera transitions.
Third-person exploration remains; first-person is for active cleaning only.

## 3D production priorities

1. A complete small garage kit with opening doors, wall/roof variants and a
   cleanable floor/service apron.
2. One polished scout with intentional accessible panels, convincing cockpit,
   landing gear and exhaust. Verify cleaning geometry, not just beauty renders.
3. Washer viewmodel, fan attachment, supply tank, terminal and hose hardware:
   the objects seen most often and closest to camera.
4. Consistent worker/customer proportions and a minimal useful animation set.
5. A small reusable environment kit and two visible garage upgrade variants.
6. Additional ship classes and large facilities only after the first kit works.

Use original or clearly licensed free assets; no paid assets without approval.
Require consistent texel density, scale, material palette, collision and LOD strategy.
Create a reference board before final assets. Validate actual eye-level Unity
screenshots and before/after upgrade captures—not only concept images.

## What the three existing scenes are

Source inspection:
- `UFO_WASH_Prototype`: original environment blockout, parking pad, camera and
  player start marker. Its foundation builder creates an early layout.
- `UFO_WASH_Playable`: later simple playable prototype with primitive ship,
  movement, washer, HUD and upgrade station. Its builder is already under LEGACY.
- `UFO_WASH_PowerSlice`: current main-menu/service-bay implementation and automatic
  Play entry. The other two are not successive levels or three planets.
- Checked-in Build Settings additionally enable the Unity `SampleScene` and old
  Playable scene. The owner's local Build Settings were modified, so do not assume
  that file exactly matches their local editor.

## Proposed scene cleanup

For the next milestone, retain one obvious authoritative gameplay scene and its
integrated main menu. Avoid a bootstrap/persistent-services/additive-world split
until there is a demonstrated need.

- Ultimately give the main scene a clear production name such as `Vesper_Garage`.
- Archive the two prototypes under a clearly marked Legacy folder; do not delete them.
- Keep only the canonical entry scene enabled in the normal build scene list.
- Update hard-coded paths, builders, validators, tests and metadata references in
  the same change. Preserve GUIDs and verify both direct Play and asset references.
- Convert temporary Editor-only service details into authored prefabs/scene
  content using a licensed Editor; normal gameplay should not depend on an Editor
  builder or require generation commands.
- Use prefabs for garage modules, ship panels and service props so upgrades replace
  controlled parts rather than regenerate the entire world.
- Keep development/test scenes outside the normal game path and label them clearly.

No scene has been moved, renamed, removed or regenerated by this planning pass.

## Delivery order and acceptance gates

1. **Finish core-loop verification and scene clarity.** Direct Play, two jobs,
   supply gates, exactly-once payout, purchase and clean Console; safe scene cleanup.
2. **Establish one visual target.** Garage exterior/interior, scout, washer and
   menu/HUD composition shown in real Unity gameplay views.
3. **Implement the focused art and UI replacement.** Complete one garage, one ship,
   dispatch, cleaning HUD, receipt and upgrade preview with functional states.
4. **Make growth visible.** Two garage upgrade states wired to actual owned upgrades.
   Integrate the separately assigned persistence/worker work.
5. **Expand the yard.** Second bay and later services after the manual loop and
   first worker are verified.

The major overhaul is a multi-milestone direction, not a claim that the current
cleaning task includes the entire commercial game. Licensing still blocks local
Unity runtime verification in the hosted environment.
