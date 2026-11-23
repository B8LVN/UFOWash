# UFO WASH — Development Workflow

## Normal iteration

1. Pull `main` into `C:\\UnityProjects\\UFOWash-Repo`.
2. Let Unity finish importing and compiling.
3. The dedicated generated scene opens automatically when `UfoWashPowerSliceBuilder.cs` has changed.
4. If only gameplay scripts changed, the existing scene is retained; no scene rebuild is needed.
5. Press Play.

## Why the old button existed

The previous environment was assembled by an Editor script and saved as a local scene. A C# builder change did not update that serialized scene, so the developer had to press a menu command to regenerate it. This created a fragile and confusing workflow.

## New scene ownership

- `Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity` is a generated scene.
- `Assets/UFO_WASH/Editor/UfoWashPowerSliceBuilder.cs` is its source of truth.
- When that builder file is reimported, an AssetPostprocessor schedules a refresh after Unity finishes importing assets.
- The refresh recreates the dedicated game scene and opens it as the active scene.
- Gameplay-only C# changes do not invoke the builder and do not destroy or recreate the scene.
- `UFO WASH > REBUILD GENERATED SCENE (MANUAL FALLBACK)` exists only for recovery if the automatic refresh fails.

The generated scene is intentionally local output rather than hand-edited scene content. Do not manually build gameplay features by editing the generated scene; implement them in components and the builder.

## QA

Run `UFO WASH > QA > VALIDATE ACTIVE GAME SCENE` after Unity finishes compiling. This checks required components, active URP, washable surface count, material slots, missing scripts and Build Settings registration.

The validator is a structural check, not a substitute for playing the job. Complete the manual checklist in `QA_TEST_PLAN.md` before accepting a milestone.

## Honest verification labels

Every milestone must distinguish:
- **Committed:** source exists on GitHub.
- **Imported:** Unity has imported the pulled source.
- **Compiled:** Unity reports no compile errors.
- **Play-tested:** the specified gameplay path was completed in Play Mode.
- **Build-tested:** a Windows build was created and launched.

Never infer a later status from an earlier one.
