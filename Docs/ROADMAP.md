# UFO WASH — Development Roadmap

**Version:** 0.2  
**Production rule:** prove the complete single-bay experience before expanding into management and automation.

## Phase 0 — Foundation and direction
- [x] Unity 6000.6 / URP 17.6 / Input System baseline recorded
- [x] UFO WASH identity and expanded progression direction approved
- [x] GDD expanded with service loop, economy, staff, automation and visual direction
- [x] TDD and MVP acceptance criteria updated
- [ ] Validate all project scripts compile in the user's Unity Editor
- [ ] Confirm the active scene, rendering pipeline and imported asset inventory
**Exit gate:** project opens cleanly; one authoritative playable scene and asset workflow are known.

## Phase 1 — First satisfying service job
- [x] Basic first-person movement and contextual interaction
- [x] Contract acceptance and payout loop
- [x] Panel-level contamination state
- [x] Pre-rinse / foam / final-rinse stage logic
- [x] Water and chemical consumption
- [x] Paid supply refill station
- [x] HUD stage and supply readout
- [x] Repair generated material shader assignment to URP/Lit
- [ ] Unity compile and Play Mode QA
- [ ] Fix camera, aiming, collision, surface coverage and usability issues from playtest
- [ ] Confirm the ship is rendered correctly and visually credible
- [ ] Improve cleaning feedback: spray, foam, runoff, sound and finish state
**Exit gate:** a first-time player can complete two consecutive contracts without instructions or blocking bugs; quality and payout are legible.

## Phase 2 — Jobs, customers and economy
- [ ] Data-driven ship archetypes and service contracts
- [ ] Distinct contamination types and surface vulnerabilities
- [ ] Quote/accept flow with optional service add-ons
- [ ] Quality inspection and bonus/malus breakdown
- [ ] Customer patience, reputation and repeat bookings
- [ ] Inventory capacity, suppliers and delivery costs
- [ ] Tool upgrades that unlock new behavior, not just percentage bonuses
**Exit gate:** several jobs create different tactical/economic choices.

## Phase 3 — Facility and capacity
- [ ] Multiple service bays with independent job state
- [ ] Storage, utility capacity and equipment maintenance
- [ ] Build/placement with footprint and connection validation
- [ ] Medium/large ship classes and new service recipes
- [ ] Customer queue and bay scheduling
**Exit gate:** expansion increases throughput while introducing real operating costs.

## Phase 4 — Employees and automation
- [ ] Employee roles, skills, wages and shifts
- [ ] Task board, priorities and bay assignments
- [ ] Worker needs and readable idle/error states
- [ ] Automated gantry and drying equipment with throughput limits
- [ ] Supply thresholds, machine maintenance and exception handling
**Exit gate:** the business completes profitable work without the player manually doing every cleaning task; player decisions still matter.

## Phase 5 — Multi-location management and content
- [ ] Distinct facilities and customer markets
- [ ] Specialist service lines and larger vessels
- [ ] Financial reports, contracts and long-term reputation
- [ ] Save/load, onboarding, accessibility and audio/visual polish
- [ ] Performance profiling and Windows build QA
**Exit gate:** content-complete candidate with stable progression and no critical blockers.

## Phase 6 — Release preparation
- [ ] Steam build, store materials and release checklist
- [ ] Compatibility testing across target PC configurations
- [ ] Optional Android feasibility review after PC validation
**Exit gate:** release candidate passes functional, performance and presentation QA.

## Working policy
- No new system is added without a gameplay reason and acceptance test.
- Do not call greybox geometry finished art.
- Do not add employee/automation complexity before the single-bay loop is enjoyable.
- Track code committed, code compiled, Play Mode tested and build tested as separate states.
