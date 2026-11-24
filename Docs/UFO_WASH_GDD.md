# UFO WASH — Master Game Design Document

Version 3.0 · Vesper Belt Studio Blueprint · milestone-gated and agent-ready

Product: commercial single-player PC game for Steam · Engine: Unity 6.6, URP · Camera: third-person world, first-person only while cleaning

## 0. Read this first (rules for the autonomous agent)

1. This document is the source of truth. If code, docs and this file disagree, stop and report the conflict. Do not silently pick one.
2. Work on exactly one milestone at a time (Section 6). A milestone is finished only when every acceptance criterion (AC) in it has passed its stated verification and the Exit Gate is met. Do not start the next milestone early.
3. Status vocabulary, in this order: Designed → Source written → Compiles → EditMode tests pass → Play Mode verified → Screenshot verified → Owner accepted. Never write “done” or “implemented” for anything below Play Mode verified.
4. Every report lists: what changed, which AC IDs moved to which status, the evidence (test names, screenshot paths), and an honest issue list.
5. No standalone player build until the owner asks. Editor compilation, EditMode tests and Play Mode runs are allowed and expected.
6. Do not add scope. Ideas outside the current milestone go to the Parking Lot (Section 10).
7. Assets: original procedural or CC0 (Kenney Space Kit) only. Record source and license in Docs/ASSET\_LOG.md. No paid asset without owner approval.
8. Every number marked \[TUNE\] is a starting value. Keep it in a ScriptableObject or config file, never hard-coded, and log every tuning change in the changelog.

## 1. The game on one page

**Logline.** Inherit a broken wash bay on a twilight alien planet, scrub the galaxy’s strangest ships by hand, and turn their fees into a self-running space-wash empire — without ever losing the satisfaction of the spray.

**Fantasy.** You are the owner who still gets their hands dirty. First a lone cleaner with a damaged pump and overdue fees. Then a station builder. Then a boss whose machines, drones and staff run a visible operation across a planet, while any ship on the lot can still be washed by you, better than any machine can.

**The two loops (equal weight by design).**

- **Hands-on loop (30 seconds to 8 minutes):** inspect → pre-rinse → foam → final rinse → read the report. Tactile, readable, repeatable.
- **Business loop (5 to 60 minutes):** read the dispatch board → choose jobs and prices → build and route facilities → hire and configure staff → expand land → survive storms and outages.

**A 30-minute session:** take two contracts, wash one by hand, notice the pump cannot keep up, spend earnings on a pipe upgrade, watch the queue clear faster, and see a new customer type arrive because reputation crossed a star.

**The five hooks to protect:**

1. **Grime has a story.** Residue layers reveal where a ship has been and hide Finds (lost cargo, pilot graffiti, salvage tags) that you uncover by cleaning.
2. **Every outcome is explained.** After each job a Forensic Report shows a hull heat-map and the exact reasons for grade, tip and reputation change.
3. **The Bottleneck Lens.** One toggle names the factor limiting every machine, pipe and worker.
4. **Owner’s Touch.** Machines give consistent volume. Only you reach the top finish that VIP customers pay for.
5. **A small, dense world.** Four 80 m × 80 m districts that visibly change as the company grows.

**Platform and scope.** Windows PC via Steam, single-player, controller support planned (M10). Target session 20–60 minutes; full campaign 15–25 hours \[TUNE\].

## 2. Design research: what we take and what we avoid

Findings come from public store pages, guides, reviews and player feedback for each game. They describe systems at a high level and are inspiration only. UFO WASH keeps its own world, art, names and interface.

| Game | What it does well | What UFO WASH takes | Trap to avoid |
| --- | --- | --- | --- |
| Moon Sweepers (DragDrop Games) | Cleaning is the entire game. You start with a free toothbrush and an enormous job. Tools differ in width and carry capacity, and upgrade branches multiply them. Vehicles change the feel of play. Belts, robots and a packager automate the dust-selling economy. You can switch between driving, sweeping and base-building at will. Progress is shown as a percentage of the moon cleaned, with exploration flags as side goals. | **R1** a tool ladder where each tier feels like a leap. **R2** hands-on and automation coexist. **R3** a global progress meter (Hull Atlas, Section 4.1). **R4** by-products that feed processing and sale. | Vast empty space. Automation that makes the hands-on loop pointless. |
| Schedule I (TVGS) | Compact world. Properties bought as steps. Staff recruited by role. Customers have preferences, budgets and loyalty, and relationship customers order outside the normal caps. Ranks gate content. Watching daily income grow is the hook. Players report that employee assignment is confusing, broken automation lines are hard to diagnose, and staff caps and station upgrades feel thin. | **R5** customer relationships (Section 4.4). **R6** staff by clear role. **R7** land bought plot by plot. | Opaque automation. Fix: Bottleneck Lens, a per-worker task log, outages that name their cause. |
| Game Dev Tycoon (Greenheart Games) | Early contracts pay cash and research points. Research unlocks new options. Money is the early bottleneck; time and research become the late one. Growth is paced by eras. | **R8** contracts that award Rare Residue and research credits. **R9** a bottleneck that shifts across the game. **R10** a research tree. | Hidden rules and vague feedback. Reviews and players complain that results go unexplained and some combinations feel arbitrary. Fix: Forensic Report and a fully itemized quote. |
| Aquapark Tycoon (Boxelware) | Guests have visible needs such as cleanliness and hunger. Overlays show where trash or hunger concentrates. Zones and per-employee work areas. Attractions come in many customizable sub-types. The tone favors design and happy guests over raw money. | **R11** mood and need overlays. **R12** work-area assignment for staff. **R13** modular facilities with sub-types. **R14** reward good layout design. | Guests and staff getting stuck (reported pathfinding bugs). Thin event variety. Fix: navigation tests are acceptance criteria; hazard and customer events (Section 4.9). |

**Three rules derived from the research:**

- **Rule A, explain everything.** No hidden multipliers. Every bonus and penalty appears in the quote, the receipt or the Forensic Report.
- **Rule B, debuggable automation.** If a machine or worker is idle, the game says why, in words, within two clicks.
- **Rule C, automation adds volume, never replaces mastery.** The hands-on loop keeps a quality ceiling that machines cannot reach.

## 3. Pillars and the Two-Loop Rule

| Pillar | Meaning |
| --- | --- |
| P1 Tactile wash work | The spray, the foam and the clean reveal feel good in the first minute and the fiftieth hour. |
| P2 Readable business | Money, supply, power and staff are shown as flows the player can follow. |
| P3 Every upgrade changes the world | A purchase changes a capability, a limit or the landscape. No pure-number upgrades. |
| P4 Explained outcomes | The player always knows why they earned what they earned. |
| P5 Owner’s Touch | The owner’s hands remain valuable at every stage. |
| P6 Small, dense world | Four hand-built districts, not a vast empty map. |

**The Two-Loop Rule.** Each loop must feed the other, and neither may dominate.

| The hands-on loop gives the business | The business loop gives the hands-on |
| --- | --- |
| The highest quality ceiling, VIP tips and reputation spikes | Better tools, bays and supplies |
| Finds, Rare Residue and Hull Atlas pages | New ship classes, contamination layers and customers |
| Emergency takeover of any bay or queue | Machines that remove drudgery so the owner picks premium work |
| Personal relationships with named customers | Land, roads and hazards that change what a job looks like |

**Balance guardrails (each is testable in M3 onward):**

- **G1.** A player who never hires staff can still unlock Crater Ridge and Ion Flats. Glasswake Reach requires one automated chain and one VIP certification.
- **G2.** A fully automated bay is capped at grade B (quality ≤ 92%) \[TUNE\]. Only Owner’s Touch can exceed it.
- **G3.** At tier-0 tools a Zorgon Scout takes 6–8 minutes by hand \[TUNE\]; each tool tier reduces that, but never below 2 minutes.
- **G4.** No upgrade may be a bare stat bump. Each one needs a visible or behavioral change recorded in its definition.
- **G5.** The bottleneck moves: early cash, then bay time, then utilities, then staff and routing, then reputation and contracts.

## 4. Core systems

### 4.1 Wash system

A ship has authored cleanable zones. Each zone is a grid of **coating cells** (target 32 × 32 per zone \[TUNE\]). Each cell stores contamination layer and amount, foam amount and a damage flag. Dirt and foam are both drawn locally on the hull. Whole-panel tinting is not acceptable.

**Service stages.** Pre-rinse → foam → final rinse. Every contracted zone must pass the current stage before service advances.

- Pre-rinse: every zone at most 0.58 contamination.
- Foam: every zone at least 0.62 coverage.
- Final rinse: every zone at most 0.15 contamination and 0.10 foam.

**Quality** = 1 − average contracted-zone contamination − 0.18 × average residual foam, clamped to 0–1. Each zone must still pass its own limits.

**Camera rule.** Third person while walking and interacting. Holding the wash control while aiming at a serviceable contract panel enters close first person. Releasing the control or losing the target returns to third person. Spraying outside the contract does nothing and never switches perspective.

**Supplies.** Water and chemical stock are consumed by active spray time only. Refill costs 65 GC when depleted.

**Contamination layers and counters.**

| Layer | Appears from | Counter-tool | Wrong tool |
| --- | --- | --- | --- |
| Cosmic dust, ionic sludge | M1 | Hydro-Lance | none (forgiving) |
| Engine soot, mineral grease | M3 | Fan nozzle plus degreaser | pure pressure smears and wastes supply |
| Xenon bio-slime | M3 | Low pressure plus bio-neutral foam | solvent damages living lacquer |
| Asteroid barnacles | M8 | Ultrasonic barnacle peeler | high pressure risks panel damage |
| Hardened void crust | M9 | Peeler then ion-polish buffer | skipping the peeler wastes the buffer |

**Tool ladder (R1).** Each tier changes how the work feels, not only a number.

| Tier | Tool | What changes |
| --- | --- | --- |
| T0 | Hydro-Lance (starter) | Narrow jet, finite reach. The first purchase installs a switchable fan nozzle. |
| T1 | Fan and precision nozzles | Wide sweep for panels, fine jet for seams and glass. |
| T2 | Foam cannon | Large coverage patches. Foam must be applied, not just rinsed. |
| T3 | Ultrasonic barnacle peeler | Removes hardened growth that sprays cannot. |
| T4 | Ion-polish buffer | Finishing pass. Required for the 98.5% VIP tip. |

All tools use pressure synthesis and shared targeting. Excess pressure on hot or delicate panels causes damage, which lowers quality and shows on the report.

**Hull Finds and the Hull Atlas (hook 1, R3).** Each ship carries 1–3 Finds \[TUNE\] hidden under grime. A Find is revealed when its cells drop below 0.30 contamination. Types: **Salvage** (credits or Rare Residue), **Story** (a short log line about the ship’s past that links to a named customer), **Atlas decal** (a collectible for the Hull Atlas). Finds are optional and never block payout. The Hull Atlas is a collection book per ship class showing variants found and percent of the Vesper catalog complete. Completing a page grants a cosmetic or a small permanent perk.

**Acceptance criteria — wash system (AC-W):**

- **AC-W1** Spraying a coating cell lowers its contamination in the cells under the nozzle footprint and nowhere else. Verification: EditMode test on the coating grid plus a Play Mode screenshot of a half-cleaned panel.
- **AC-W2** Foam appears only where foam was applied and is removed by the final rinse. Verification: EditMode test plus screenshot.
- **AC-W3** Stage gating: the next stage cannot start while any contracted zone fails the current limit. Verification: EditMode test with fixtures at each limit boundary.
- **AC-W4** Quality equals the formula above for fixture grids (tolerance 0.001). Verification: EditMode test.
- **AC-W5** Supplies decrease only during active spray and the HUD shows exact remaining percent. Verification: Play Mode test.
- **AC-W6** Camera rule holds: enter on hold-and-aim, exit on release or lost target, no switch when spraying off-contract. Verification: Play Mode test with scripted input and a screenshot of each state.
- **AC-W7** Wrong-tool behavior matches the layer table. Verification: EditMode test per row.
- **AC-W8** A Find is revealed at the threshold, can be skipped without penalty, and is recorded in the Atlas save data. Verification: EditMode test plus Play Mode check.

### 4.2 Forensic Inspection Report (hook 2, Rule A)

After inspection the player sees:

1. Grade, quality percent and a pass or fail per zone.
2. A hull heat-map with toggles for **missed coating**, **residual foam**, **damage** and **over-pressure**.
3. Resource use against a par value: water, chemical, time.
4. A ranked list of up to five **causes** (for example “Left rim zone at 0.21 contamination; limit 0.15”), each with a one-line suggestion.
5. The payout breakdown (Section 4.3) and the reputation change.
6. Any Finds revealed.

**Acceptance criteria (AC-F):**

- **AC-F1** Every grade penalty and every missed tip tier is traceable to at least one listed cause. Verification: EditMode test on report generation with fixtures that miss each tier.
- **AC-F2** The heat-map layers match the underlying coating grid. Verification: screenshot comparison on a fixture ship.
- **AC-F3** The report never shows a number that is not also in the receipt or the grid data. Verification: code review plus EditMode consistency test.
- **AC-F4** The report can be re-opened from the ledger for the last 20 jobs. Verification: Play Mode test after save and load.

### 4.3 Economy and reputation

**Currencies.** Galactic Credits (GC) pay wages, utilities, land, structures and supplies. Rare Residue comes from filters and selected ships and is used for advanced formulas and modules or sold to the salvage broker. Training credits promote staff. Research credits unlock tool and facility options (R10).

**Opening state.** 750 GC cash, overdue station fees, one damaged pump, one inherited pad, reputation 2.1 stars \[TUNE\].

**Opening quotes.** 250 / 420 / 600 GC.

**Tips.** 8% of base at 84% quality, 16% at 90%, 35% at 98.5%. The quality bonus is separate.

**Reputation per inspection.** +0.08 at 84%, +0.16 at 92%, +0.25 at 98.5%, −0.15 below 84%, clamped to 1–5. Heavy dispatch unlocks at 2.1 stars and VIP at 3.8 \[TUNE\].

**Receipts** show gross and net separately, with lines for base fee, quality bonus, tip, consumables, wages, utilities and maintenance. Dispatch cards show trait, quote, tip threshold and reputation requirement before the player accepts.

**Synergy cap.** Lounge comfort × premium polish × fast turnaround may stack, with a visible cap and a quote breakdown.

**Acceptance criteria (AC-E):**

- **AC-E1** Tip and reputation tables match the numbers above at every boundary (83.9, 84, 89.9, 90, 98.4, 98.5). Verification: EditMode test.
- **AC-E2** Net equals gross minus all listed costs for fixture jobs. Verification: EditMode test.
- **AC-E3** A first-shift mistake (spoiled supplies) is recoverable from the 750 GC reserve. Verification: scripted Play Mode run that wastes one full tank and still completes the shift.
- **AC-E4** All economy values load from data assets. Verification: grep check shows no hard-coded GC literals in gameplay code.
