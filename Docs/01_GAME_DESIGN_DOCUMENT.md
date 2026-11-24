# UFO WASH — Game Design Document (GDD)

**Version:** 0.1 — Production baseline draft  
**Status:** Proposed design baseline; creative owner approval required for major scope decisions.  
**Genre:** First-person 3D simulation / job simulator / business management / progression / automation.  
**Primary platform:** Windows PC.  
**Distribution target:** Steam is a future target, not a claim of readiness.  
**Mode:** Single-player first.

---

## 1. High concept

UFO WASH is a first-person simulation about building an alien spacecraft cleaning and servicing business from a tiny, underfunded station into a multi-bay, highly automated intergalactic operation.

The player begins as the owner and only worker. They personally receive UFOs, prepare a bay, operate physical cleaning tools, remove alien grime, rinse and finish each craft, collect payment and reinvest the earnings. As the business grows, the player buys better tools, unlocks new service capabilities, expands the facility, hires employees and builds automated workflows. The player gradually moves from doing every job to designing and managing an operation that can service many spacecraft with less direct intervention.

**Core fantasy:** Start as a one-person UFO cleaner. Become the owner of an efficient intergalactic spacecraft service business.

## 2. Design pillars

### Pillar A — Satisfying hands-on work

The first hours must be enjoyable even before automation. Tools should feel responsive, surfaces should visibly change, sound and effects should communicate contact and progress, and each completed job should provide a clear sense of achievement.

### Pillar B — Visible business growth

Progression changes the physical station: more bays, better equipment, storage, staff spaces, machinery, signage, customer areas and larger spacecraft. The world should communicate the player's success without relying only on UI numbers.

### Pillar C — Meaningful reinvestment

Money unlocks new capabilities and choices: faster or more precise tools, new cleaning processes, added capacity, more complex customer jobs, staff, automation modules and new locations. Avoid upgrades that only add tiny percentage increases without changing play.

### Pillar D — Automation as earned freedom

Automation is a major progression reward. The player identifies bottlenecks, buys/places machines, assigns staff, connects process stages and solves capacity or supply problems. Automation must be visible and understandable in the world; it must not be only a passive income multiplier in a menu.

### Pillar E — Original, coherent sci-fi identity

The game should have a memorable alien service-station identity: varied UFO silhouettes, unusual grime/contamination types, readable equipment, believable industrial spaces and a cohesive art direction. Avoid parodying or copying reference games.

## 3. Design references — what to learn, not copy

### Moon Sweepers

Relevant high-level patterns: an extremely humble start, tactile cleaning work, visible area improvement, carrying/transporting gathered output, selling/delivering, tool and capacity upgrades, depots/vehicles and automation. Its public Steam description describes starting with a toothbrush, collecting and selling lunar dust, upgrading tools/capacity and unlocking cleaning vehicles. Its automation guide recommends proving a small collection→transport→processing→selling route before scaling it. Source links are in the README.

### Schedule I

Relevant high-level patterns: first-person hands-on operations, growing from small-time work to a larger business, using earnings to acquire properties, hiring employees and building increasingly automated chains. Its Steam description explicitly highlights properties, businesses, employees and automated production/distribution. UFO WASH translates the abstract progression pattern into legal, original spacecraft cleaning/service operations.

### Translation into UFO WASH

| Reference principle | UFO WASH expression |
|---|---|
| Do a simple job yourself | Personally clean the first small UFOs |
| Collect/handle output | Manage dirty-water filters, alien residue, used consumables or service by-products only where they add fun |
| Upgrade tools/capacity | Better washers, nozzles, foam systems, hoses, drying tools, storage and bay capacity |
| Build routes/automation | Physical process chain: intake → dock → pre-clean → wash → rinse → dry → inspect → dispatch |
| Buy properties/expand | Add hangars, service bays, land and eventually new stations/locations |
| Hire employees | Hire cleaners, bay attendants, supply operators, inspectors and supervisors |
| Automate operations | Install machines and assign staff to visible tasks with throughput, supply and maintenance constraints |

Do not copy exact UI, assets, maps, characters, names, sound, branding or proprietary designs.

## 4. Setting and player role

The player owns a small spacecraft wash station serving alien pilots, couriers, traders, explorers and other fictional customers. The station is initially rundown and under-equipped. The tone can be playful and slightly eccentric, but the operation should feel tactile and believable enough to support a satisfying job-simulation loop.

The player is not a distant menu-only manager. They can move through the station, interact with machinery, inspect UFOs, use tools, carry supplies, operate controls, place equipment and observe workers.

### Opening chapter — Bay 01 (narrative draft)

**Setting:** Year 2189, the Vesper Belt. Independent pilots depend on small service stations between long-haul routes, while corporate docking and repair fees squeeze the operators who keep those routes alive.

**Player role:** The player inherits the keys to Bay 01, a neglected spacecraft wash station with failing equipment, overdue bills and no staff. The first objective is simple: keep the lights on by completing the first customer contract. The longer ambition is to rebuild the station into a trusted interstellar service business.

**First customer:** Zorp of Orion Traders arrives in a Class-IV scout with asteroid dust and ion residue across the hull. The first shift teaches the player to accept a contract, pre-rinse, apply foam, final-rinse, request inspection and collect payment.

**Opening flow:** Title menu → Story → first-shift briefing → How to Play / field manual → Clock In → playable service bay. The Story and tutorial must remain accessible from the title menu; the first shift should never depend on an external explanation.

This is the first narrative direction introduced for the prototype. Character names, tone and longer story arcs remain subject to owner feedback.

## 5. Core gameplay loops

### 5.1 Job loop (minutes)

1. Review available customer/job request.
2. Accept or prioritize a job.
3. Customer UFO arrives and enters the station queue.
4. Guide/assign the UFO to an available service bay.
5. Inspect the craft and understand its cleaning requirements.
6. Prepare the bay and required supplies.
7. Perform the service: apply cleaning product/foam, remove grime with the appropriate tool, rinse and finish/dry/inspect as the job requires.
8. Track coverage/cleanliness and quality feedback.
9. Complete the job, release the craft and receive payment/reputation/progression reward.
10. Clean/reset the bay and prepare the next job.

### 5.2 Business loop (session)

Complete jobs → receive revenue → pay operating costs/restock supplies → identify bottleneck → purchase a tool, upgrade, employee or machine → increase speed, quality or capacity → accept more valuable/complex jobs.

### 5.3 Automation loop (mid/late game)

Observe a bottleneck → choose a stage to automate → purchase and place a module → connect/configure it → assign staff and/or power/supplies → test a small workflow → monitor throughput and failures → expand only when the route works reliably.

### 5.4 Expansion loop (long term)

Increase reputation/capabilities → qualify for larger spacecraft and contracts → earn more → expand the facility or acquire an additional location → operate more bays and process chains → manage a larger business.

## 6. Customer and UFO design

Each customer craft should be readable and have a reason to feel different. Candidate variation dimensions:

- Size/class: small scout, courier, standard saucer, heavy transport, luxury/exploration craft.
- Exterior form: flat saucer, spherical pod, asymmetrical cargo craft, multi-module vessel.
- Dirt/contamination: ordinary dust, oily residue, crystalline deposits, ionized streaks, biological-looking but fictional slime, heat-baked grime.
- Service requirement: basic wash, heavy clean, delicate surface, emergency turnaround, premium finish, specialized treatment.
- Job value and time pressure: larger/complex jobs pay more but consume capacity, tools or supplies.

Do not implement all variants in the MVP. Start with one base craft and a small number of clearly differentiated job profiles.

## 7. Cleaning interaction design

The cleaning experience is the heart of the game. It must not reduce to holding a button near an object.

Candidate interaction stages:

- Inspect: reveal dirt distribution and service requirements.
- Prepare: choose tool/nozzle/product and set up the bay.
- Apply: foam or treatment visibly coats relevant surfaces.
- Clean: pressure/brush/vacuum action removes dirt with readable feedback.
- Rinse: remove remaining foam/residue.
- Finish: dry/polish/check quality if required.

The player should understand why one tool or process is better for a certain surface/contamination type. Start with one simple but satisfying cleaning process before adding complicated tool combinations.

### Dirt visualization requirement

Dirt removal must be visually obvious at normal gameplay distance and close range. The technical implementation (shader mask, render texture, decals, mesh zones or a hybrid) must be chosen through a tested prototype in Unity 6.6—not assumed from this document. Evaluate quality, performance, authoring complexity and compatibility with different UFO models before committing.

## 8. Progression

### Early game — owner-operated station

- One small location and one service bay.
- One basic cleaning setup.
- A small number of low-risk customer jobs.
- Player personally performs the service.
- Limited supplies, storage and cash.
- First purchases should improve the actual task (e.g. better tool reach/coverage, faster rinse, improved storage or another useful piece of equipment).

### Early-to-mid game — professional station

- More tools and service options.
- More customer/UFO variety.
- Second bay or increased bay capacity.
- Better storage and supply handling.
- First hireable worker who performs a clearly visible task.
- First automation module (one stage of the service workflow).

### Mid game — multi-stage operation

- Several bays with different capabilities.
- Staff roles and schedules.
- Automated prewash/rinse/drying or another proven workflow stage.
- Visible queues and bottlenecks.
- Consumable supply management.
- Larger and specialized spacecraft.
- Contracts that reward throughput, quality or specialization.

### Late game — intergalactic service business

- Larger hangars and specialist equipment.
- Multiple locations (only after one location is genuinely fun and stable).
- Management layer for staff, bays, schedules and supply chains.
- More advanced automation modules and logistics.
- High-value craft and contracts.
- Meaningful trade-offs between capacity, quality, maintenance, wages and supply costs.

The exact unlock order and economy values must be playtested and balanced. Do not invent final prices or progression timings before a working prototype exists.

## 9. Employees and automation

### Employee principles

Employees are not merely a passive income multiplier. They should have visible jobs and operate relevant equipment or stations. Candidate roles:

- Bay attendant: guides craft into a bay and prepares the area.
- Cleaner: operates a defined cleaning stage.
- Supply operator: restocks consumables and handles waste/by-products.
- Inspector: checks service quality and releases completed craft.
- Supervisor: improves coordination/capacity across a section, if useful and understandable.

Start with one employee role and one task. Evaluate pathfinding, animation, task assignment and reliability before expanding.

### Automation principles

Automation should be a physical and readable process. Candidate stages:

`Arrival/Queue → Docking → Pre-treatment → Wash → Rinse → Dry/Finish → Inspection → Dispatch`

Not every stage needs to be automated. The player chooses which bottleneck to solve. Machines should have understandable inputs, outputs, capacity and constraints.

Possible constraints: water/product supply, power, machine capacity, maintenance, blocked output, worker availability, bay availability and customer queue. Introduce only constraints that create interesting decisions—not busywork.

A working automation route must be tested end-to-end on a small scale before adding branches or multiple machines. A stalled route should expose where the process stopped.

## 10. Economy principles

Revenue can be influenced by job class, craft size/condition, service quality, completion time and contract bonus. Expenses may include supplies, wages, maintenance, utilities and property costs. Introduce costs gradually and ensure the player understands them.

Every purchase should answer at least one question:
- What new action does this enable?
- What bottleneck does it remove?
- What capacity does it add?
- What new customer/job becomes available?
- What task can now be delegated or automated?

Avoid false depth from many tiny percentage upgrades. Avoid making manual play obsolete too early; manual work should remain useful for premium/special jobs or intervention, while automation handles volume.

## 11. MVP / first vertical slice

The first vertical slice should prove the fantasy in a compact, polished setting—not contain the entire business simulation.

### Required vertical-slice experience

- First-person movement and stable camera.
- One coherent, art-directed starter station with one service bay.
- One customer arrival/queue flow.
- One UFO model with visible dirt.
- One complete cleaning job that has satisfying tool interaction and visible dirt removal.
- Clear job completion and payment.
- A basic money display and job/shift feedback.
- At least one meaningful purchase that changes the next job.
- A small, demonstrable automation upgrade or employee task that previews the long-term business fantasy. Keep it to one stage and one simple workflow.
- A save/load proof for money and purchased progression, if this does not derail the core loop.

### Explicitly out of first vertical-slice scope

- Multiplayer/co-op.
- Multiple planets or huge open world.
- Many locations.
- Large staff roster or complex schedules.
- Fully simulated factory economy.
- Dozens of UFO classes.
- Large tech tree.
- Mobile port.
- Complex narrative/campaign.

### Vertical-slice playtest goal

A new tester should be able to play approximately 10–15 minutes and understand: receive a UFO, clean it, earn money, buy an upgrade and see how the station or workflow improves. The basic automation preview should make the player curious about expanding the business.

## 12. UX and accessibility principles

- Clear interaction prompts and readable controls.
- Remappable controls where feasible.
- Adjustable sensitivity and FOV.
- Readable UI at 1080p and common aspect ratios.
- Do not communicate critical dirt/service state only through colour.
- Provide clear feedback for job state, supplies, bay status, money and automation status.
- Avoid excessive UI overlays that fight the first-person view.

## 13. Risks and open design questions

These are intentionally unresolved until prototyping:

- What exact cleaning mechanic feels best: freehand nozzle aiming, surface-zone tool interaction, or a hybrid?
- Which dirt-visualization method gives a strong result without excessive GPU/authoring cost?
- Is a fully first-person workflow comfortable when managing multiple bays, or is a diegetic/tablet management interface needed later?
- Which service stages are fun to perform and which are better automated?
- How many simultaneous UFOs can be managed without the station becoming confusing?
- How should automation remain legible and diagnosable?
- What art style gives a distinct identity within our available production resources?

Resolve these through focused prototypes and playtests. Record approved decisions in `docs/07_DECISION_LOG.md`.

## 14. Definition of a good feature

A feature is not good merely because it compiles. It must:

- Support one of the design pillars.
- Be understandable in play.
- Have clear player input and feedback.
- Integrate with existing game systems.
- Have an intentional visual/audio presentation appropriate to its stage.
- Pass its acceptance criteria and regression checks.
- Be documented and reproducible.

> **Archived baseline:** this earlier draft is retained for project history. The current canonical design is [Docs/GDD.md](GDD.md), version 2.1. Its full-game scope, third-person planetary exploration and first-person-only cleaning rule supersede this document.
