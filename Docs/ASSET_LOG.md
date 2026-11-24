# UFO WASH — Asset Log

**Rule 7 (GDD Section 8):** Original procedural or CC0 (Kenney Space Kit) only. Record source and license here. No paid asset without owner approval.

## Asset Entries

| Asset | Source | License | Location | Used For | Status |
|-------|--------|---------|----------|----------|--------|
| Kenney Space Kit | Kenney.nl (https://kenney.nl/assets/space-kit) | CC0 1.0 Universal | Assets/UFO_WASH/Art/Imported/KenneySpaceKit/ | Environment, equipment, ship models | Imported |
| BlenderModel.glb | Generated via Blender MCP | Original (procedural) | Assets/Generated/Imported/BlenderModel.glb | Prototype geometry | Placeholder |
| UfoWashLocalCoating.shader | Original (Vesper Belt Studio) | Proprietary (studio) | Assets/UFO_WASH/Scripts/Cleaning/UfoWashLocalCoating.shader | Dirt + foam overlay rendering | Implemented |
| LegacyRuntime.ttf | Unity built-in | Unity license (Editor use) | Built-in | HUD text, UI text | Implemented |
| Rounded HUD panel | Procedural (generated at runtime) | Original | Assets/UFO_WASH/Scripts/UI/UfoWashHUD.cs | HUD panel chrome | Implemented |

## Tuning Values (Rule 8)

Every `[TUNE]` number in the GDD must live in a ScriptableObject or config file, never hard-coded.

| Value | GDD Reference | Current Location | Status |
|-------|--------------|------------------|--------|
| Opening cash: 750 GC | 4.3 | UfoWashProgressSave.CreateNewData(), UfoWashCareerLoop.Start() | Hard-coded — needs ScriptableObject |
| Refill cost: 65 GC | 4.3 | UfoWashCareerLoop.RefillSupplies(), UfoWashSupplyStation | Hard-coded — needs ScriptableObject |
| Opening quotes: 250/420/600 GC | 4.3 | UfoWashCareerLoop.OfferPayouts | Hard-coded — needs ScriptableObject |
| Tip: 8% @ 84%, 16% @ 90%, 35% @ 98.5% | 4.3 | UfoWashCareerLoop.OfferTipRates/Quality | Hard-coded — needs ScriptableObject |
| Rep: +0.08 @ 84%, +0.16 @ 92%, +0.25 @ 98.5%, −0.15 <84% | 4.3 | UfoWashCareerLoop.CompleteJob() | Hard-coded — needs ScriptableObject |
| Rep clamp: 1–5 | 4.3 | UfoWashCareerLoop | Hard-coded |
| Pre-rinse limit: 0.58 | 4.1 | UfoWashCareerLoop.AdvanceStageIfReady() | Hard-coded |
| Foam coverage: 0.62 | 4.1 | UfoWashCareerLoop.AdvanceStageIfReady() | Hard-coded |
| Final rinse: dirt ≤0.15, foam ≤0.10 | 4.1 | UfoWashCareerLoop.CanSubmit | Hard-coded |
| Quality foam coefficient: 0.18 | 4.1 | UfoWashCareerLoop.Quality | Hard-coded |
| Automated bay cap: 92% | G2 | Not yet implemented | TBD |
| Tool tiers: T0–T4 costs | 4.6 | UfoWashUpgradeStation | Partially implemented |

## Change Log

| Date | Change | Reason | Author |
|------|--------|--------|--------|
| 2026-11-24 | Initial asset log created | Rule 7 compliance | Agent |
