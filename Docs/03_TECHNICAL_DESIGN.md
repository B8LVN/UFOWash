# UFO WASH — Technical Design Document (TDD)

**Baseline:** Unity 6.6 (6000.6), URP, C#, Windows PC first. Confirm exact patch and package versions from the actual project before changing dependencies.

## 1. Technical objectives

- Support first-person interaction with tools and machinery.
- Represent a UFO cleaning job as clear states and measurable progress.
- Make money, upgrades, staff and automation data-driven where appropriate.
- Keep systems testable and separable; avoid one giant GameManager.
- Support save/load with versioning.
- Build a stable PC vertical slice before porting to Android.
- Keep visual quality and performance in balance on Ryzen 5 5600 / RTX 3060 12GB / 16GB RAM.

## 2. Recommended architecture principles

Use a pragmatic, component-oriented Unity architecture. Do not introduce a framework or dependency without a concrete need.

Suggested layers:

1. **Presentation:** MonoBehaviours for animation, VFX, audio, UI and scene presentation.
2. **Gameplay/application:** job flow, service bay state, cleaning orchestration, economy, inventory, upgrades and automation scheduling.
3. **Definitions/configuration:** ScriptableObjects for static definitions such as UFO types, jobs, equipment, tools, consumables, upgrades and employee roles.
4. **Runtime state:** serializable data models for current jobs, customer state, money, inventory, unlocked content, staff assignments and machine state.
5. **Persistence:** versioned save data, serialization, validation and migration.

A MonoBehaviour should not own unrelated global systems. Prefer explicit references or small service interfaces over hidden global access. Use events where they reduce coupling, and always unsubscribe reliably.

## 3. Proposed system boundaries

The following are logical responsibilities, not a mandate to create every class immediately. Audit existing code and adapt naming/structure rather than duplicating systems.

| System | Responsibility |
|---|---|
| PlayerController | Movement, look, grounding and player state |
| PlayerInteractor | Detect and invoke valid world interactions |
| ToolController | Equipped tool, input, use state and tool feedback |
| UfoDefinition | Static UFO class data: model reference, size, service profile |
| UfoRuntimeState | Current craft condition, dirt state and assigned job |
| Customer/JobService | Job offers, acceptance, job lifecycle and rewards |
| Arrival/QueueService | Spawn/arrival staging and queue capacity |
| ServiceBay | Bay availability, occupancy and stage progression |
| CleaningService | Tool-to-surface interaction and cleaning progress |
| DirtSurface | Surface-specific dirt state and visual representation |
| InventoryService | Supplies and consumable stock |
| EconomyService | Payments, costs, balances and transaction validation |
| UpgradeService | Unlocks, purchases and effect application |
| EmployeeService | Employee roster, roles and task assignments |
| AutomationService | Stage processing, machine capacity and blocked states |
| SaveService | Versioned save/load and validation |
| UI presenters | Display current state; avoid owning core business rules |

## 4. Scene and asset organization

Recommended starting structure (adapt after project audit):

```text
Assets/UfoWash/
  Art/
    Environment/
    Ufos/
    Equipment/
    Props/
    Materials/
    Textures/
  Audio/
    Music/
    SFX/
  Prefabs/
    Scenes/
  ScriptableObjects/
    Scripts/
      Core/
      Player/
      Interaction/
      Customers/
      Jobs/
      Cleaning/
      Economy/
      Inventory/
      Upgrades/
      Employees/
      Automation/
      Save/
      UI/
  UI/
  VFX/
  Tests/
```

Do not mass-move existing assets until the audit identifies current dependencies and references. Unity `.meta` files must be preserved.

## 5. Job state model

A job should have explicit, validated states. Candidate sequence:

`Offered → Accepted → Arriving → Queued → Docking → Preparing → Cleaning → Finishing → Inspection → Completed → Paid → Released`

Not every state needs a separate UI screen. State transitions must prevent paying twice, cleaning the wrong UFO, releasing an incomplete job or processing a craft in two bays at once.

Use clear transition rules and tests for invalid transitions.

## 6. Cleaning/dirt system — prototype before committing

This is a core technical risk. Compare at least two practical approaches on one representative UFO model:

- Surface zones with normalized dirt values and material/texture masks.
- Shader/render-texture or decal-based dirt masks for continuous visual cleaning.
- Hybrid approach if it provides the best visual/performance/authoring balance.

Evaluate:
- Does the surface visibly become clean exactly where the tool is used?
- Are seams/UVs acceptable on curved UFO geometry?
- Is it easy for artists to author new UFOs?
- Does it perform at target resolution?
- Does it support multiple active UFOs?
- Can dirt state be saved/restored if required?
- Can different tools affect different dirt/material types?

Do not commit the project to a technique based only on a theoretical explanation. Build a small benchmark scene and compare screenshots/performance.

## 7. Tool and interaction design

Every interaction should define:
- Valid target and range.
- Player input.
- Preconditions (correct tool, supplies, job/bay state).
- State change.
- Visual feedback.
- Audio feedback.
- UI/reticle feedback.
- Failure feedback.
- Test cases.

Avoid interactions that are merely a button prompt with no meaningful world response.

## 8. Automation architecture

Represent an automated service route as processing stages with explicit inputs, outputs, capacity and state. A candidate route:

`Intake → Dock → Pre-treatment → Wash → Rinse → Dry/Finish → Inspect → Dispatch`

A stage should expose at least:
- Current state (Idle, Running, Blocked, Faulted, Disabled).
- Input queue/required input.
- Output queue/result.
- Processing time/capacity.
- Required supplies/power/worker if applicable.
- Clear reason when blocked.

The first automation prototype should be one machine or one employee task with one clear input/output. Test the route end-to-end before building complex graphs, belts or multiple branches. Avoid premature general-purpose factory simulation.

## 9. Economy and data definitions

Static data candidates:
- `UfoDefinition`
- `JobDefinition`
- `CleaningProfileDefinition`
- `ToolDefinition`
- `ConsumableDefinition`
- `EquipmentDefinition`
- `UpgradeDefinition`
- `EmployeeRoleDefinition`
- `AutomationMachineDefinition`

Runtime data candidates:
- Wallet/balance and transaction history.
- Current job states.
- UFO dirt/condition state.
- Inventory quantities.
- Purchased upgrades/unlocks.
- Employee assignments.
- Machine configuration/state.
- Station expansion state.

Do not put mutable player progression into shared ScriptableObject assets. ScriptableObjects should generally hold definitions/configuration; runtime/save data should hold mutable state.

## 10. Save/load

Save data must be versioned. Include a schema version and safe handling for missing/corrupt/incompatible fields. Save only the state needed to reconstruct progression, not transient scene object references. Add migration logic when the schema changes. Test new save, save/load round-trip, missing file, corrupted file, and old-version migration when implemented.

## 11. Input, UI and packages

Inspect the existing `Packages/manifest.json` before adding packages. Confirm whether the Input System, Cinemachine or other packages are already installed and compatible with the exact Unity 6.6 patch. Do not add packages just because they are popular. Record package additions/removals and reasons in the decision log.

Prefer configurable controls and readable UI. Keep gameplay state out of UI code.

## 12. Performance targets and profiling

Initial development target: smooth 1080p gameplay on the owner's PC (Ryzen 5 5600, RTX 3060 12GB, 16GB RAM). A 60 FPS target is a design goal, not a verified guarantee. Measure in representative scenes and record hardware, resolution, quality settings, frame-time, CPU/GPU bottlenecks and memory use.

Profile before optimizing. Avoid premature optimization, but do not build unbounded update loops, expensive per-frame allocations or uncontrolled real-time material instances. Check draw calls, overdraw, particle counts, texture sizes, shader complexity and active NPC/machine counts as the game scales.

## 13. Testing seams

Core systems should allow deterministic tests where practical. Separate job/economy/state rules from scene presentation enough to test payment, state transitions, inventory consumption and automation throughput without relying entirely on visual playtests.

## 14. Technical unknowns requiring spikes

- Dirt visualization technique.
- First-person controller and interaction implementation.
- UFO arrival/docking staging approach.
- Art pipeline and model/texture standards.
- Employee navigation and task execution reliability.
- Automation simulation granularity.
- Save format/migration strategy.

Record each spike, results and chosen approach in the decision log before committing to a costly path.
