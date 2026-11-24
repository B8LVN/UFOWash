# Quality-recovery checks

## Engine-independent coating checks

Requires .NET 8. From the repository root:

```sh
dotnet run --project Tools/QualityChecks/QualityChecks.csproj
```

This compiles the actual `UfoWashCoatingState.cs` and runs deterministic assertions.
It also parses all game/editor C# files with C# 9 grammar using the SDK's Roslyn libraries.
Outputs go to ignored `generated-artifacts/quality-checks/`.
It is not a Unity API/type compile, shader compile or gameplay check.

## Unity rule fixture

Use licensed **Unity 6000.6.10f1**. Do not open the owner's project in batch mode or generate a player build.
Create an ignored disposable copy containing `Assets/`, `Packages/`, and `ProjectSettings/`.
Then run (replace the two absolute paths with the actual Editor and disposable project):

```sh
"/absolute/path/to/Editor/Unity" \
  -batchmode -nographics \
  -projectPath "/absolute/path/to/disposable-project" \
  -executeMethod UFOWASH.Editor.UfoWashRecoveryChecks.RunBatch \
  -logFile "/absolute/path/to/disposable-project/recovery-checks.log"
```

Do **not** add `-quit`: the entry point enters Play Mode across a domain reload and exits with 0/1 itself.
An external process timeout should stop startup/import hangs; the fixture has a 45-second Play Mode timeout.
The test creates `RecoveryChecks.unity` only in the disposable project. Never merge that scene.
Passing this fixture checks contract rules, not the authored bay, manual controls, visuals or sound.

## Gameplay acceptance

Follow `Docs/reports/QUALITY_RECOVERY_2026-11-24.md`.
Real authored-scene Play Mode screenshots and runtime results are mandatory.
