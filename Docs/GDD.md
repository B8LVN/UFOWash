# UFO WASH — Living Game Design Document

**Version:** 2.2 — Vesper Belt Studio Blueprint
**Product target:** Commercial single-player PC game for Steam  
**Engine:** Unity 6.6, URP  
**Perspective:** Third-person planetary exploration and business operation; close first-person view only while actively cleaning the contracted ship  
**Status:** Full-game design target. A playable first shift is a production gate, not the product or scope.

## 1. The game

UFO WASH is a planet-based spacecraft service company simulation. The player arrives at a failing outpost on Vesper, a mineral-rich alien world between established trade lanes. They inherit one wash bay, a damaged pump, overdue station fees and an old service suit. The first customers keep the station alive. Their fees fund land, machines, staff and a reputation that attracts stranger and more valuable ships.

The player explores a continuous third-person alien world, meets pilots, buys and lays out physical facilities, and watches the business change the landscape. At a hull, holding the wash control while aiming at a serviceable panel shifts into close first-person cleaning. Releasing the tool or losing the hull target returns to third person. Late game is an operating company the player designed: staff and machines run visible work orders while the owner handles exceptions, expands the planet-side operation and chooses where to invest.

**Design promise:** every profitable upgrade changes a capability, choice, throughput limit or part of the world. Cleaning remains tactile after the business grows.

### Design grammar

- From **Moon Sweepers**: cleaning physically transforms surfaces; recovered by-products feed processing and automation; upgrades remove repeated labor while preserving a hands-on option.
- From **Schedule I**: the owner walks through a compact world, buys adjoining business plots, recruits workers and grows through visible operational tiers.
- From **Aquapark Tycoon**: visitors have moods and patience, route/layout affects throughput and comfort, and specialized facilities influence satisfaction and revenue.

These are system-level inspirations. UFO WASH has its own world, story, art, characters, names and interface.

### Zero-to-Boss progression

1. **Owner-operator:** accept a dispatch, inspect contamination, clean hull zones in order, certify quality and reinvest.
2. **Station builder:** claim plots, place bays and utilities, route ships and supplies, hire the first worker.
3. **Operations designer:** configure recipes, balance power and fluids, specialize facilities and solve bottlenecks.
4. **Tycoon boss:** operate several sectors, set service prices, staff shifts, handle VIP contracts and expand a planetary network while still able to wash any ship personally.

## 2. Six product pillars

### A. Tactile wash work

A ship has authored cleanable zones, each with its own contamination, foam and quality. The opening recipe is pre-rinse → foam coverage → final rinse. Every zone must pass the current stage before service advances. Water and chemical stock are consumed by active spray time. A ship can earn its base quote at a lower grade, while premium tips and reputation need a better inspection.

Later contamination layers: cosmic dust, engine soot, xenon bio-slime, asteroid barnacles and hardened void crust. Each has a counter-tool and visual response. Planned manual gear: Hydro-Lance → fan/precision nozzles → foam cannon → ultrasonic barnacle peeler → ion-polish buffer. Wrong tools are inefficient or risk damage; correct tools create satisfying local removal. Later service stages add degreasing, drying, polishing, glass care and shield coat.

### B. Planet, ships and visual identity

The setting is Vesper, a twilight frontier planet in the Vesper Belt. Art direction is premium stylized sci-fi industrial: sculpted basalt and glass, readable silhouettes, cool alloy, restrained cyan/teal status light, warm amber hazards, practical hangar lighting and legible wet surfaces. Avoid toy-like placeholder forms and generic military darkness.

**Sectors and land progression**

| Sector | Biome and role | Hazard and counterplay | Prestige unlock |
|---|---|---|---|
| Dust Basin | Starter plateau, regolith apron, inherited pad, relay mast and two small plots | Dust squalls clog filters; cyclone pre-filters protect exposed stock | Independent-operator license |
| Crater Ridge | Elevated basalt shelf for multi-bay traffic and customer plaza | Rockfall closes marked approach lanes; retaining pylons and clear roads protect throughput | Regional fleet contracts |
| Ion Flats | Charged mineral plain for heavy ships, reactor fields and extraction | Ion surges stress grids; grounding pylons and capacitor banks protect machinery | Heavy-vessel certification |
| Glasswake Reach | Twilight canyon of black-glass shelves, luminous brine, suspended shards and skyhook docks | Crystal storms disrupt power/navigation; storm shutters, grounded spires and redundant routing counter them | Vesper Orbital Exchange and galactic expo |

Plots are physical places with roads, build pads, hazard overlays and utility connection points. New sectors change the skyline, ambient sound, customer mix and operating constraints.

**World scale:** this is a small, dense planet-side business world, not a vast empty open-world map. Each sector is an approximately 80 m × 80 m walkable district built from 4 m construction cells. The opening Dust Basin yard exposes about 40 m × 48 m and two adjoining claimable pads. A visible road, gate or lift connects the other districts. Crater Ridge targets a 2,500 GC / 2.6-star unlock, Ion Flats 9,000 GC / 3.5 stars, and Glasswake Reach 25,000 GC / 4.2 stars. These are first economy targets for the full game and require balance testing. The gate physically opens and new land, landmarks and building choices become visible; a menu entry alone never counts as world expansion.

**Ship roster**

| Ship class | Silhouette and contamination | Gameplay hook |
|---|---|---|
| Zorgon Scout Saucer | Compact saucer; dust and ionic sludge | Onboarding ship; cockpit dome and rim are separate inspection zones |
| Krell Hauler Barge | Broad modular cargo hull; soot and mineral grease | High water demand; underside/container modules need route planning |
| Vexx Luxury Cruiser | Swept hull and luminous canopy; delicate coating | Low pressure and glass-safe finish required for premium tip |
| Morrowline Bio-Dome Ark | Petal hull and greenhouse pods; fictional pollen film | Bio-neutral wash; aggressive solvent damages living lacquer |
| Tesseract Foldship | Offset rings and articulated panels; crystalline void crust | Stabilize reconfiguring panels before spraying |
| Cinderwing Interceptor | Three heat fins and engine cowl; baked plasma soot | Cooldown before degreasing; excess pressure on hot panels damages finish |

These are design targets. Only ships with verified models and behavior may be called implemented. The current generated scene has one procedural saucer, not six ship assets.

**Asset policy:** create original procedural meshes and textures where useful; curate included Kenney Space Kit assets only where style and license fit; record source and license. No paid asset without owner approval. Verify URP materials in the actual scene. Procedural generation is a production method, not a claim that primitives are finished models.

### C. Construction and facility architecture

Buildings occupy a snap grid of 4 m cells on purchasable plots. Placement checks plot ownership, terrain grade, road access, machine clearance, hazard exposure and utility reach. A facility may look good and still operate poorly if queue roads cross worker routes or pipes take the long way around.

| Structure | Gameplay role and visible upgrade |
|---|---|
| Landing & Wash Bay | Dirt pad → covered bay → automated multi-arch tunnel; capacity and ship-size clearance |
| Bay Recovery Annex | Closed-loop water/foam reclamation lowers supply use; a modular annex is visibly expanded as the station improves |
| Fluid & Chemical Plant | Pressurized silos and mixer produce water/foam/degreaser; exposed pipes show flow |
| Fusion / Solar Hub | Generator, solar field, capacitor bank or crystal reactor; capacity, upkeep and surge resistance trade off |
| Crew Quarters | Trailer, lockers and break room add worker slots and stamina recovery |
| Alien Customer Lounge | Seating, viewing window and service kiosks add waiting capacity, patience and tips |
| Boss Command Office | Tower unlocks pricing, contracts, shift plans and land purchase |
| Residue Reclaimer | Filters used wash fluid into reusable water and Rare Residue; lowers disposal cost |
| Drone Depot | Visible hauling/inspection drones add route capacity and battery/repair upkeep |
| VIP Detailing Annex | Low-pressure enclosure with glass-safe filtration enables luxury finish packages |

Starter utility targets: Fusion Cart 100 kW; wash bay 45 kW; lounge 12 kW; exterior lights 6 kW; automated gantry 35 kW. Pipes have flow capacity and pressure; long or undersized routes reduce throughput. Outages name the overloaded network and affected machines.

### D. Staff, machines and boss management

Workers visibly navigate and perform explicit task loops. Each has role, skill, stamina, shift, wage, morale, work speed, preferred bay and current task. Hiring costs 120 GC plus shift wages. Initial wage targets per 8-hour business shift: Wash Tech 80 GC, Utility Engineer 110 GC, Lounge Host 65 GC, Shift Supervisor 140 GC.

Skill levels I–III improve one action: coverage rate, maintenance speed, supply yield or customer handling. Promotion costs training credits and time. Morale rises with safe equipment, fair wages, a clean break room, short routes and completed work; it falls with outages, unsafe assignments, long queues and missed breaks. Low morale increases callouts and slows work; it never randomly sabotages a contract. Set shift, home bay, service class and task priority in the staff roster.

Machine work orders define allowed ships, recipe, minimum supply reserve, quality target, queue priority and fallback. Wash Techs prepare/monitor; gantries repeat recipes; engineers refill/repair; hosts route customers; supervisors coordinate queues but never create capacity. Processing competes for actual bay, water, chemical, power, staff and downstream capacity.

### E. UFO customers and incremental economy

Customer identity affects patience, service preference, tip rule, contamination and relationship.

- **Zorp — reliable courier:** routine jobs and a dependable-finish tip.
- **Nyx-7 — scrap prospector:** abrasive deposits and Rare Residue for thorough cleaning.
- **Captain Vex — perfectionist noble:** delicate glass; large premium for near-perfect finish.
- **Archivist Oru — hazard-sensitive pilgrim:** careful handling and safe decontamination build trust.
- **Mira Quell — salvage broker:** fast industrial turnaround; buys recovered residue at a posted price.
- **Rill of the Morrowline — living-ship keeper:** neutral chemistry, patient service and clean brine filter.

**Currencies and reputation**

- Galactic Credits pay wages, utilities, land, structures and purchases.
- Rare Residue comes from contaminated filters and selected ships; craft advanced formulas/modules or sell it.
- Reputation is a visible 1–5 star rating that changes demand and unlocks contracts/sectors. The current prototype starts at 2.1; heavy dispatch unlocks at 2.1, VIP at 3.8. Playtests must tune these values.

Opening quotes: 250 / 420 / 600 GC. Tip rules: 8% of base at 84% quality, 16% at 90%, and 35% at 98.5%. The quality bonus is separate. Inspection changes reputation by +0.08 at 84%, +0.16 at 92%, +0.25 at 98.5%, or −0.15 below 84%, clamped to 1–5. Dispatch cards disclose trait, quote, tip threshold and reputation requirement.

The opening Quick Decontamination contract also offers a transparent, optional express bonus: pass inspection within five minutes for +45 GC. Missing the window only loses that bonus; it never reduces base pay, quality rewards, tips or reputation. The other opening offers have no time limit.

Bay 01 has a two-step recovery investment: the first module costs 650 GC and reduces wash-supply use by 15%; a 1,400 GC second stage replaces it with a larger annex and raises total savings to 30%. Both prices and effects are shown before purchase; these effects are implemented in source but need Unity play testing.

Show gross and net separately. Receipts break out base fee, quality bonus, tip, consumables, wages, utilities and maintenance. Pricing/service packages affect demand and reputation. Lounge comfort × premium polish × fast turnaround can synergize, with a visible cap and quote breakdown.

## 3. First-shift service rules

The opening customer is Zorp of Orion Traders and their Class-IV scout.

1. Review dispatch and accept an unlocked contract.
2. The ship docks. Third person stays active while walking and interacting.
3. Aim and hold wash control on an active contract panel to enter close first person and spray.
4. Pre-rinse every service zone to at most 0.58 contamination.
5. Foam every zone to at least 0.62 coverage.
6. Final-rinse every zone to at most 0.15 contamination and 0.10 foam.
7. Return to dispatch for inspection. Grade, base payout, bonus, tip and reputation are explained.
8. Refill costs 65 GC when water or chemical is depleted. The opening 750 GC cash reserve makes the first mistake recoverable.

Quality = 1 − average contracted-zone contamination − 0.18 × average residual foam, clamped to 0–1. Quality is a summary grade; each individual zone must pass its completion limits. Spraying outside the active ship’s contract has no effect and must not switch perspective.

## 4. Unity implementation and scope truth

**2026-11-24 recovery source update:** localized coating cells replace whole-panel tint and separate foam patches; the first purchase installs a switchable fan nozzle; tool pressure synthesis, shared targeting, supply limits and revised HUD are implemented in source. A versioned company ledger, one hireable first attendant and a persistent starter expansion are also implemented on the test branch. No licensed Unity import, Play Mode or visual pass was available in this workspace. Current acceptance and blockers: `Docs/11_CURRENT_STATUS.md`.

| GDD system | Unity implementation | State |
|---|---|---|
| Hull contamination and local foam feedback | Assets/UFO_WASH/Scripts/Cleaning/UfoWashDirtSurface.cs | Source implemented; foam visibility and per-zone tuning still need Unity visual verification |
| Contract-only panels and staged zone gates | UfoWashCareerLoop.cs, UfoWashCustomerShip.cs | C# compile check against Unity 6.6 references passed; Editor import and Play Mode unverified |
| Third-person exploration / aimed cleaning view | UfoWashFirstPersonController.cs | C# compile check passed; camera behavior still needs Play Mode verification |
| Trait tips, reputation unlocks and NPC identity | UfoWashCareerLoop.cs, UfoWashCustomerNpc.cs, UfoWashContractTerminalUI.cs | C# compile check passed; UI and receipt behavior unverified |
| Title, story, briefing and field guide | UfoWashFrontEnd.cs | Source exists; earlier local run observed, needs art and interaction polish |
| Vesper vista, modular outpost and palette | UfoWashPowerSliceBuilder.cs, KenneyModularSpaceKit, Art/Textures | Editor C# compile check passed; CC0 source kit and Vesper recolor are present; asset import, scene regeneration and visual check pending |
| Sector land claims and gate unlocks | UfoWashSectorGate.cs, UfoWashProgressSave.cs, UfoWashPowerSliceBuilder.cs | Three physical gates are authored; JSON save integration is source-implemented; Play Mode restoration is unverified |
| Company ledger and washer upgrades | UfoWashProgressSave.cs, UfoWashCareerLoop.cs, UfoWashUpgradeStation.cs | Versioned JSON, backup recovery and progression restoration implemented in source; Unity round-trip and corruption cases unverified |
| First attendant | UfoWashWashAttendant.cs, UfoWashPowerSliceBuilder.cs | Hireable for 120 GC, matching the GDD target; pre-rinses contracted zones and reports water blockage; temporary Play Mode injection and task-loop behavior unverified |
| Bay recovery investment | UfoWashBayRecoveryStation.cs, UfoWashProgressSave.cs, UfoWashPowerSliceBuilder.cs | Two persistent tiers reduce actual water/chemical use and reveal CC0 Kenney annex modules; Unity persistence and appearance unverified |
| Physical building placement and utility routing | None | Designed, not implemented; sector claim currently unlocks land only |
| Full staff AI, work orders and automation | None beyond the bounded attendant prototype | Full navigation, rosters, wages, work-order configuration and automated lines remain design/future work |
| Ship roster, chemistry damage, Rare Residue and full multi-sector progression | Partial or absent | Future production work; only basic sector purchase persistence is implemented |

The present Windows player output is not the product. Do not create or launch another player build until the full game is ready and the owner requests it. Unity source import/compilation and Editor Play Mode checks are allowed; state exactly what they verify.

## 5. Full-game production roadmap

1. Complete a truthful first shift: zone feedback, tool gating, supplies, inspection, tips, reputation and repeat contracts.
2. Author the Vesper Basin: modular hangar, custom hull panels, terrain, props, readable light, audio and modern menu/HUD.
3. Add saved company state, booking/queue, financial ledger, sector plots and physical construction.
4. Add utility networks, residue processing, first visible employee and automated gantry chain.
5. Expand sectors, ship classes, traits, contracts and boss management.
6. Finish accessibility, controller support, sound, performance, save migration and commercial/Steam release requirements.

Code written or a player launched is not proof of completion. Each milestone needs clean Unity compilation, an Editor Play Mode run, current-scene screenshots and an honest issue list.

## 6. Living GDD changelog

- **v1.0–v1.1:** Initial planetary spacecraft wash and company-growth premise.
- **v2.0:** Zero-to-Boss progression, service stages, initial buildings, ship/customer/staff concepts and six-pillar framework.
- **v2.1:** Locked the commercial full-game target and camera rule; defined four Vesper sectors/hazards, six ship classes, three specialist facilities, physical utility/build grid, staff hiring/wages/skills/morale/shifts, customer traits, exact tips/reputation gates, first-shift zone thresholds and Unity implementation map. Replaced all outstanding [EXPAND] prompts with game-ready targets.
- **v2.2:** Carried the Moon Sweepers / Schedule I / Aquapark Tycoon inspiration into a compact, contiguous Vesper business world; added three physical sector claims, a CC0 modular outpost kit with a custom Vesper palette, per-zone service progression and customer/reputation rewards. Fixed contract-panel initialization ordering. Unity-reference C# compile checks passed; actual Editor import, scene generation and Play Mode remain pending.

