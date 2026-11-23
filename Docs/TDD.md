# UFO WASH — Technical Design Document (TDD)

**Version:** 0.1 · **Status:** Initial architecture

## Verified baseline (audit reported)
- Unity Editor: 6000.6.10f1
- URP: 17.6.0
- Input System: 1.20.1 installed
- Platform: Windows Standalone x64
- Color space: Linear
- Scripting backend (current target): Mono
- Graphics asset: PC_RPAsset
- Quality profiles: Mobile and PC
- Project path reported: C:\\UnityProjects\\UFOWash

Audit values are settings snapshots, not build verification.

## Goals
Maintainable modular systems; short iteration times; PC-first quality; later Android scalability; minimal external dependencies; safe serialized data; measurable performance.

## Proposed runtime architecture
- **Bootstrap/Core:** initialization, game state and shared services.
- **Player:** movement, camera and input abstraction.
- **Interaction:** ray-based interaction interface and prompt state.
- **Vehicles:** UFO definitions, state and presentation.
- **Cleaning:** dirt state, tool logic and feedback.
- **Economy/Jobs:** job definition, completion evaluation and rewards.
- **Progression:** upgrade definitions and effects.
- **UI:** HUD, prompts, job progress and currency.
- **Persistence:** versioned save data when MVP state requires it.

## Folder convention
Assets/UFO_WASH/{Scenes, Scripts/{Core,Player,Interaction,Vehicles,Cleaning,Economy,UI,Save},Prefabs,Materials,Art,Audio,ScriptableObjects,Documentation}. Inspect existing assets before relocating or overwriting anything.

## Coding conventions
- C#; PascalCase types/methods, camelCase private fields, serialized private fields using [SerializeField].
- Composition and single-responsibility components.
- ScriptableObjects for authored definitions; runtime state must not mutate shared definitions.
- Avoid unjustified singleton dependencies.
- Avoid expensive work and avoidable allocations in per-frame loops.
- No unexplained package additions.

## Input
Use the installed Input System. Isolate input from gameplay logic to support remapping and devices.

## Rendering/performance
URP; establish PC baseline first. Profile before optimizing. Scale shadows, render scale and effects for Mobile. Prefer simple geometry and reusable materials during blockout.

## QA gates
A feature is complete only after compilation, defined test steps and behavior validation. Editor automation must fail safely and avoid destructive overwrite. Runtime/build claims require actual execution.

## Dependency order
Scene foundation → player movement/camera → interaction → UFO/dirt model → spray feedback → job completion/reward → HUD → upgrade → save/load.

## Build targets
Primary Windows 64-bit. Android secondary after gameplay and performance review. Mono for iteration; evaluate IL2CPP for deployment.