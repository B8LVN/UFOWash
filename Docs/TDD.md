# UFO WASH — Technical Design Document

**Version:** 0.2  
**Target:** Unity 6000.6.10f1 / URP 17.6 / Input System 1.20.1 / Windows x64

## 1. Technical principles

- Keep gameplay state separate from scene presentation.
- Prefer small components with explicit responsibilities.
- Author content as ScriptableObjects; do not mutate shared definitions at runtime.
- Avoid hidden scene dependencies and avoidable per-frame allocations.
- Make every system inspectable in the Editor and testable in isolation.
- A commit is not a test result. Record compile, Play Mode and build status separately.

## 2. Runtime architecture

### Core and bootstrap
Owns game state, session lifecycle, time, save version and shared service access. No gameplay component should directly own unrelated systems.

### Player
First-person controller, camera, interaction ray, tool selection and input mapping. Tool behavior is separated from input so rebinding and controller support remain possible.

### Vehicles
UfoDefinition data describes ship class, scale, surface zones, fragility, service ports and visual prefab. Runtime UfoInstance owns contamination and service state. Vehicle presentation must not be responsible for economy logic.

### Jobs and customers
ServiceContractDefinition contains customer, vessel, required service recipe, quality target, deadline, consumable estimate and reward formula. ServiceContractRuntime tracks acceptance, stage, quality and completion. Customer profiles provide preferences, patience, reputation and repeat-business behavior.

### Cleaning and equipment
UfoWashDirtSurface is a panel-level contamination/treatment state. Equipment implements a tool contract with mode, range, coverage, pressure, resource consumption and surface compatibility. The current vertical slice uses three stages: pre-rinse, foam and final rinse. Later stages are added as authored recipe steps, not hard-coded branches across unrelated scripts.

### Economy and inventory
Wallet transactions are centralized. Jobs produce a revenue breakdown; consumables, wages, maintenance and utilities are explicit costs. Inventory tracks quantity, capacity, reorder thresholds and supplier lead times.

### Facility and construction
A Facility owns bays, stations, storage and utility capacity. Construction placement validates footprint, access, power and service connections before purchase.

### Employees and automation
Workers are role-based task agents. A task board assigns priorities and constraints. Machines consume inputs, process a recipe and produce outputs at measurable throughput. Staff and machines use the same work-order/task interfaces where practical.

### UI
HUD shows only immediate operational information. Contract terminal, inventory, staff board, machine configuration and financial reports are separate interfaces. UI reads state and issues commands; it does not own authoritative game state.

### Persistence
Versioned save DTOs serialize player, wallet, unlocked content, facility layout, contracts, inventory, workers and machine state. ScriptableObject definitions are referenced by stable IDs, never serialized as mutable runtime assets.

## 3. Current vertical-slice implementation

Existing classes:
- UfoWashFirstPersonController: movement and camera.
- UfoWashPlayerInteraction / UfoWashInteractable: contextual interaction.
- UfoWashSprayTool: spray presentation, raycast and tool-mode selection.
- UfoWashDirtSurface: panel contamination and foam response.
- UfoWashCareerLoop: current contract stage, supplies and payout.
- UfoWashJobBoard: contract acceptance.
- UfoWashSupplyStation: paid refill interaction.
- UfoWashHUD: service stage, cleanliness and supply display.
- UfoWashPowerSliceBuilder: Editor-only scene assembly.

This is transitional prototype architecture. Before content scales, migrate contract, inventory and equipment values to data definitions and separate wallet transactions from UI statics.

## 4. Rendering and asset pipeline

- URP 17.6 is the only supported render pipeline for this project.
- Generated project materials must use Universal Render Pipeline/Lit or a verified URP shader.
- Do not silently fall back to Built-in Standard when URP/Lit cannot be found.
- Imported models must be checked for embedded/read-only material shader compatibility, mesh scale, normals, colliders, pivot and licensing.
- Magenta means the assigned shader is unsupported, missing or failed compilation; changing only material color is not a fix.
- Keep source assets and converted project materials separate where practical.

## 5. Performance targets

Initial PC target: 1080p, stable 60 FPS on Ryzen 5 5600 / RTX 3060 / 16 GB RAM. This is a target, not a verified result. Use URP renderer features conservatively; avoid excessive real-time point lights and transparent particle overdraw. Profile before optimizing.

## 6. Play and release verification

- Import the project in the supported Unity version and resolve compile errors before owner review.
- Press Play → main menu → start shift; complete two contracts, refill and make a purchase.
- Confirm materials render, player collision is usable and the washer only affects its intended target.
- Confirm rewards and purchases occur once and failed purchases leave the wallet unchanged.
- Measure frame time, memory and draw calls before making performance claims.
- Create a Windows build only when the owner asks for one.

## 7. Dependency order

1. Reliable rendering/material pipeline and scene baseline.
2. Player movement and interaction.
3. Surface-level cleaning and feedback.
4. Multi-stage contract and consumables.
5. Payout, quality and repeat jobs.
6. Tool/equipment upgrades.
7. Data-driven contracts and ship variants.
8. Facility capacity, inventory and suppliers.
9. Employees and task assignment.
10. Automation, expansion and save/load.

## 8. Explicit non-goals for the first slice

No multiplayer, open-world travel, multi-location simulation, full employee AI, conveyor logistics, procedural ship generation or Android port. These depend on a fun, stable single-bay loop.
