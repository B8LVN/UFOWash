# UFO WASH — Game Design Document

**Version:** 0.2  
**Status:** Production direction approved; vertical slice in implementation  
**Genre:** First-person work simulation / business management / incremental automation  
**Platform:** Windows PC first; Android only after the PC loop is validated

## 1. High concept

UFO WASH is a first-person sci-fi spaceship cleaning and servicing business simulator. The player inherits a tiny intergalactic wash operation: one cramped bay, unreliable equipment, little working capital and a stream of strange alien customers. The player personally inspects, prepares, cleans and certifies each vessel, then reinvests the proceeds into better tools, machinery, staff and facilities.

The long-term arc is from owner-operator to the owner of a multi-bay, multi-location spacecraft care company. Cleaning remains the identity and the central source of value. Management and automation grow out of the cleaning workflow; they do not replace the premise.

## 2. Product pillars

1. **Tactile work:** tools must feel physical, responsive and satisfying. A job is a sequence of actions on a real vehicle, not a progress bar.
2. **Visible transformation:** contamination, foam, rinse water and restored surfaces must be legible in the world. The player sees exactly what their work changed.
3. **Customer-specific jobs:** ships, contamination, service requirements, deadlines, risk and payout create meaningful variation.
4. **Reinvestment with consequences:** purchases unlock capabilities, throughput or new decisions—not just tiny percentage bonuses.
5. **Delegation as earned progression:** the player first learns the work, then hires specialists and configures machines to take over repeatable tasks.
6. **A cohesive alien service world:** every ship, prop, interface and facility belongs to the same intergalactic industrial design language.

## 3. Player fantasy and tone

The player is a capable owner who turns a neglected service bay into a respected interstellar operation. The tone is playful and slightly strange, but the work and business systems are grounded. Alien customers have distinct needs and personalities; spacecraft are valuable machines with recognizable silhouettes and service quirks.

The player should feel: "I took this filthy ship from a disaster to showroom condition, earned real money, and used that money to make tomorrow's work better."

## 4. Core job loop

**Attract → receive → inspect → quote → prepare → service → certify → collect → reinvest.**

1. A customer books or arrives with a vessel and a service request.
2. The player receives the ship into an available bay and secures it.
3. An inspection reveals ship class, contamination type, damage/fragility, required service stages and quality target.
4. The player accepts a quoted contract. Optional add-ons and rush service trade time and consumables for higher payout.
5. The player prepares the bay: checks water, chemical stock, tool condition and bay readiness.
6. The player performs the ordered work using the correct tool and sequence.
7. A quality inspection evaluates coverage, missed contamination, surface damage and remaining residue.
8. The customer pays. The player receives a breakdown of gross revenue, consumables, operating costs, quality bonus and reputation change.
9. The player spends or saves the proceeds. New jobs, equipment and capacity become available as the business grows.

The first playable slice implements a deliberately small version: accept contract, pre-rinse, foam, final-rinse, consume supplies, refill at a station, complete and earn credits. It is the first proof of the loop, not the full game.

## 5. Hands-on cleaning design

### 5.1 Service stages

The default exterior clean has three ordered stages:

- **Pre-rinse:** break up loose dust, grit and ionized deposits. Water is consumed. This exposes the heavier contamination.
- **Foam application:** coat the vessel in cleaning compound. Chemical stock is consumed. Coverage matters; dry or missed panels reduce final quality.
- **Final rinse:** remove loosened grime and foam. Water is consumed. Rinsing a panel without adequate foam is inefficient and can leave residue.

Later service contracts can add drying, polishing, decontamination, engine intake cleaning, glass treatment and delicate-surface protection. Those are new interactions and equipment requirements, not reskins of the same button.

### 5.2 Tool language

The first tool is a basic ion-water lance. Later tools introduce distinct behavior:
- Fan nozzle: wide coverage, lower impact, slower on stubborn grime.
- Precision nozzle: narrow coverage, high control for vents, seams and delicate panels.
- Foam cannon: rapid chemical coverage, high chemical consumption.
- Rotary polisher: removes oxidation and restores gloss, but can damage unprotected surfaces.
- Deionized rinse: prevents mineral spotting on premium glass and hull coatings.

Tool switching must be quick and readable. A wrong tool should have a comprehensible consequence, not an arbitrary failure.

### 5.3 Surface simulation

Each serviceable panel tracks contamination and stage-specific treatment. The ship is not one global health bar: the player must move around it and treat multiple surfaces. MVP may use a small number of authored surface zones; later ships can have more zones and unique material responses.

Quality is calculated from:
- Required contamination removed.
- Foam coverage on eligible panels.
- Final-rinse coverage and residue.
- Missed zones and optional detailing.
- Any damage caused by excessive pressure or the wrong tool.

A completion threshold guarantees a clear win condition; higher quality earns a bonus and reputation.

## 6. Job and customer design

A job is a data-defined contract, not a hard-coded sequence of names. Each contract contains:
- Customer profile and ship archetype.
- Service package and required stages.
- Contamination profile and surface difficulty.
- Consumable estimate.
- Target quality and optional deadline.
- Base payout, bonus rules and reputation impact.

Customer archetypes:
- **Courier:** frequent, low-risk, quick turnaround; values reliability.
- **Miner:** abrasive dust and industrial residue; larger hulls, high water use.
- **Diplomat:** delicate finish and strict quality; premium payout, low tolerance for mistakes.
- **Explorer:** biological or unknown contamination; requires specialist chemical later.
- **Military contractor:** engine soot and heat residue; high payout, strict process requirements.

Customers can become repeat clients. Reputation increases booking volume and unlocks premium contracts. Failed deadlines, poor quality or damage reduce trust and can lose a client. Avoid random punishment: requirements and risks must be visible before acceptance.

## 7. Progression and business growth

### Early game — Owner-operator

- One compact bay, one small ship class and one basic tool.
- The player performs reception, inspection, all cleaning stages, inspection and payment.
- Water and chemical stocks are limited; refills cost money.
- First purchases should solve obvious bottlenecks: better nozzle, larger tanks, faster pump or a second service package.
- Early customers are forgiving and teach the cleaning sequence.

**Player question:** "Which job can I complete profitably with the equipment and supplies I have?"

### Mid game — A real service company

- Two or more bays and a customer queue.
- Medium and large ships with different panel layouts.
- Foam cannon, precision nozzle, drying equipment and specialist chemicals.
- Inventory storage, supplier deliveries, maintenance and utility costs.
- Hire a bay technician, wash operator or prep worker.
- Assign workers to a bay, service class and task priority.
- Unlock premium service packages and repeat customer contracts.

**Player question:** "What should I delegate, and what bottleneck is stopping the business from serving more ships?"

### Late game — Intergalactic operations

- Multiple hangars and locations with distinct customer markets.
- Specialized facilities for heavy vessels, luxury craft and hazardous contamination.
- Automated wash gantries, sensor inspection, drying tunnels and chemical dosing.
- Managers coordinate shifts and bay priorities; technicians maintain machines.
- Logistics systems replenish chemicals and move ships between service zones.
- The player manages contracts, capital investment, staff, maintenance and expansion while still being able to step onto the floor and do the work.

**Player question:** "Where should the next capital investment go to increase profit, capacity or market access?"

## 8. Economy

### 8.1 Revenue

Revenue comes from:
- Base service fee by ship class and service package.
- Quality bonus for complete coverage and a clean final inspection.
- Rush premium for accepted deadlines.
- Reputation-driven repeat work and premium customer tiers.
- Later: maintenance plans, fleet contracts and location-specific specialist services.

### 8.2 Costs

Costs must be visible per job and per operating period:
- Water, foam, specialist chemicals and replacement filters.
- Equipment wear, repairs and machine maintenance.
- Employee wages and training.
- Facility utilities and rent/loan costs.
- Logistics and supplier delivery fees.

### 8.3 Investment rule

Every upgrade must unlock a new decision, capability or capacity:
- Larger tanks reduce refill trips and allow larger jobs.
- A better pump reduces active cleaning time but may consume more power.
- A foam cannon increases coverage speed while increasing chemical cost.
- A second bay increases parallel capacity but adds utilities and staffing.
- A worker removes a defined task from the player's workload but has wages and throughput limits.
- An automated gantry increases unattended throughput but requires capital, power, maintenance and a configured recipe.

No endless +2% upgrade ladder. Prefer discrete tiers with visible functional changes.

### 8.4 Pricing and balance targets

Initial tuning targets (to be playtested, not treated as final):
- Early basic contract gross payout: 250–400 credits.
- Consumables: 5–12% of basic contract gross.
- A first useful tool upgrade: approximately 2–3 successful jobs.
- A first hire: approximately 8–12 successful jobs, with wages covered by a modest increase in throughput.
- New bay: a major investment requiring a deliberate saving goal.
- Automated equipment should have a clear break-even estimate shown before purchase.

The player should rarely be unable to act because of money. Offer a lower-paying basic contract and a visible supply cost so a poor decision is recoverable without grinding.

## 9. Employees and automation

Employees are not passive percentage buffs. Each worker has:
- Role and permitted task set.
- Skill level and work speed.
- Fatigue/shift availability.
- Wage and training cost.
- Assigned bay, task priority and service restrictions.

Initial roles:
- **Prep technician:** secures incoming ships, connects service ports and stages supplies.
- **Wash operator:** performs pre-rinse, foam and final-rinse on assigned ships.
- **Detail specialist:** handles polish, glass and delicate finish.
- **Maintenance technician:** maintains pumps, gantries and filters.
- **Shift supervisor:** coordinates work priorities and reduces idle time; does not magically create capacity.

Automation is configured through readable work orders:
- Which bay/ship classes are allowed.
- Which service recipe to run.
- Minimum supply reserve before starting.
- Quality target versus throughput priority.
- What to do when supplies are low, a machine is damaged or a job needs manual attention.

Machines have throughput, operating cost, maintenance and failure states. A machine cannot exceed the capacity of its upstream supply or downstream bay. Employees and machines should complement one another: staff prepare, monitor, repair and handle exceptions while automation performs repeatable work.

## 10. Visual direction

**Art direction:** premium stylized sci-fi industrial, not toy-like primitive geometry and not generic dark military sci-fi.

- Palette: graphite and deep navy structure, cool brushed metal, restrained cyan/teal status lighting, warm amber hazard accents, clean white service markings.
- Shapes: strong readable silhouettes, rounded industrial housings, modular panels, exposed hoses, clamps, rails, gantries and service ports.
- UFOs: each class needs a distinct silhouette, landing gear, windows, engine details, panel breaks and readable contamination zones.
- Materials: physically plausible metal, coated ceramic, glass, rubber, wet surfaces and grime. Dirt should layer over—not simply replace—the underlying material.
- Lighting: practical hangar fixtures, readable key/fill/rim lighting, restrained bloom, contact shadows and reflection where performance allows.
- UI: compact industrial service interface, clear contract card, tool mode, supply levels, stage checklist, quality feedback and credits. Avoid debug-like text blocks.
- Audio: pump startup, nozzle pressure, foam hiss, water impact, drainage, tool switching, ship engine ambience, customer arrival and satisfying certification/payment cues.

Temporary blockout geometry is allowed for layout tests only. It must be labelled honestly and replaced before any quality milestone is called an art pass.

### Asset sourcing policy
Prefer original or commercially usable assets with explicit licenses. Kenney Space Kit is CC0 and includes over 150 spaceport, ship, character and prop models. Its low-poly style can support a coherent stylized direction if used consistently, but imported materials must be converted/verified for URP and models must be curated rather than selected by a loose filename guess. Do not claim an asset is in the repository until its files are actually present and verified.

## 11. MVP vertical slice

The MVP is one polished, repeatable job in one bay:
- Stable first-person movement and interaction.
- One authored, non-pink UFO with several individually cleanable zones.
- Contract acceptance and readable service requirements.
- Three-stage pre-rinse / foam / final-rinse workflow.
- Water and chemical consumption plus a paid refill station.
- Visible contamination and foam feedback.
- Completion, quality result, payout and repeat contract.
- One meaningful equipment purchase.
- Reliable reset and save-ready data boundaries.

Out of scope until this loop is enjoyable: multiple locations, employee AI, automated production lines, multiplayer, open-world exploration, elaborate construction and Android.

## 12. Acceptance and quality gates

A milestone is accepted only when:
1. It compiles in the target Unity version.
2. It launches in Play Mode without blocking errors.
3. The player can complete the loop without developer instructions.
4. Every interaction has clear feedback and a failure state.
5. Purchases and consumables affect the economy correctly.
6. A second job can be accepted and completed.
7. A first-time tester can explain what to do and why the next purchase matters.
8. Screenshots/video from the actual Unity build support visual-quality claims.

The developer must distinguish committed code, locally compiled code, and playtested behavior. A GitHub commit is not proof of a working Unity build.

## 13. Design references (principles only)

- **Moon Sweepers:** manual cleaning creates visible world change; collection/delivery creates a satisfying conversion of labor into money; tools and capacity improve the work; vehicles, depots, belts and robots progressively replace repetitive labor. UFO WASH adapts this as panel cleaning, supply logistics, service bays and wash automation.
- **Schedule I:** a hands-on opening grows into a configurable business; workers take over specific jobs; properties and equipment unlock capacity; customer preferences and timing make planning meaningful. UFO WASH adapts this as contracts, customer reputation, specialist service roles and configurable wash recipes.

We do not copy either game's setting, content, UI, characters or exact systems. We borrow the clarity of their progression structures.
