# UFO WASH — Visual & Audio Quality Bible

## 1. Quality ambition

The game should look and feel like a deliberately art-directed indie simulation, not a default Unity scene. Visual quality is part of the product and must be designed from the start. A technically functioning scene is not automatically a visually acceptable scene.

## 2. Working art direction

**Proposed direction:** stylized-realistic sci-fi industrial. Use grounded proportions and readable forms, with distinctive alien spacecraft shapes, clean industrial design language, tactile machinery and controlled playful details.

The station should feel used and operational: floor drains, hoses, cable routing, warning markings, service panels, storage, lighting fixtures, workstations, signage, grime and wear where appropriate. Keep visual hierarchy clear; avoid clutter that harms navigation.

This is a proposed direction to validate with a reference board and first environment blockout. The user approves the final direction.

## 3. Visual identity rules

- UFO silhouettes must be identifiable from a distance and distinct from one another.
- Machines must communicate their function through shape, material, movement and status lights.
- Materials need appropriate roughness/metalness variation; avoid flat default grey materials.
- Lighting must guide the player through the bay and highlight the craft being serviced.
- Use a deliberate palette with consistent accent colours for interaction, hazard, machine state and UI.
- Dirt/foam/water effects must be readable and physically coherent enough for the chosen style.
- Keep scale consistent: player, bay doors, spacecraft and equipment must feel believable together.
- Use signage/decals/props to make spaces understandable, not as random decoration.

## 4. Reference-board requirement

Before production art for the starter station, create a visual reference board with separate categories:

1. Overall environment mood and composition.
2. Service-bay architecture.
3. Cleaning machinery and handheld tools.
4. UFO exterior design language.
5. Materials, wear and grime.
6. Lighting and colour palette.
7. UI/HUD direction.
8. Dirt, foam, water and cleaning effects.

For each reference, record the source and what design principle is being used. References are not permission to copy protected assets or compositions.

## 5. Asset pipeline

Each asset must have a purpose, scale, naming convention, source and license record. Prefer consistent packs or custom assets over random asset mixing. Use Blender for custom models where necessary. Inspect polygon count, UVs, texture resolution, pivots, colliders, LOD needs and material count before integrating production assets.

AI-generated images may support mood boards or concept exploration, but they are not automatically production-ready 3D models, textures or rigged assets.

Never download or use assets with unclear licensing. Record all external assets in `docs/09_ASSET_REGISTER.md`.

## 6. First-person feel

- Movement and camera must be responsive and predictable.
- Camera motion should not cause unnecessary nausea.
- Tool aiming must be precise enough for the cleaning task.
- Interaction range and target feedback should be consistent.
- Tool weight/impact can be communicated through animation, audio and restrained camera feedback.
- Avoid excessive camera shake, forced delays and sluggish controls.

## 7. Cleaning feedback

Every cleaning action should communicate:
- Tool activation and shutdown.
- Contact/range validity.
- Water/foam flow and surface response.
- Dirt removal at the location being cleaned.
- Completion of a surface zone or service stage.
- Remaining dirt/foam when relevant.
- Supply usage if the tool consumes resources.

Visual progress must be obvious without relying only on a percentage in the UI. Audio should reinforce pressure, spray, brushing, rinsing, dripping, machine operation and job completion without becoming fatiguing.

## 8. Audio direction

Develop layered sound:
- Ambient station loop: ventilation, distant machinery, subtle alien spaceport activity.
- UFO arrival/departure: engine, hover, docking and hatch sounds.
- Tools: switch, pressure, foam, brush, rinse, dry and refill.
- UI: restrained confirmations, warnings and purchase feedback.
- Automation: machine cycles and clear blocked/fault states.
- Music: optional adaptive layers that support the atmosphere without masking work feedback.

Do not use placeholder beeps for every interaction as final sound design.

## 9. Definition of visual acceptance

A production-quality scene review must include screenshots or a recording from actual gameplay viewpoints, not only a Scene view with editor gizmos. Check:

- First-person screenshot at player eye height.
- Wide view showing the service bay composition.
- Close view of UFO surface and dirt.
- Close view of tools/equipment.
- Lighting and material consistency.
- Scale, navigation and readability.
- No obvious placeholder geometry/materials in areas claimed as finished.

A feature may be marked `Greybox / Temporary` if it is intentionally unfinished. Never label it `Polished` or `Production Ready` until it meets the relevant visual criteria.
