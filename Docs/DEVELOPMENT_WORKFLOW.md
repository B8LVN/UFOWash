# UFO WASH — Development Workflow

## Normal iteration

1. Use the existing Unity project and the `ufo-wash/cleaning-recovery` branch for development and testing. Do not move changes to `main` until the owner explicitly approves.
2. Let Unity finish importing and compiling.
3. Press **Play → main menu → start shift**. Do not run a scene-generation command first.
4. Preserve authored scenes and `.meta` files. Builder/preview tools are optional developer utilities, not player setup steps.

## Play handoff

For a quick owner review, complete a contract from dispatch through inspection and payment, then make a purchase and restart Play to check the saved progress. Report visible behavior and Console errors; source-only checks do not prove a working Unity play session.

## Honest verification labels

Every milestone must distinguish:
- **Committed:** source exists on GitHub.
- **Imported:** Unity has imported the pulled source.
- **Compiled:** Unity reports no compile errors.
- **Play-tested:** the specified gameplay path was completed in Play Mode.
- **Build-tested:** a Windows build was created and launched.

Never infer a later status from an earlier one.
