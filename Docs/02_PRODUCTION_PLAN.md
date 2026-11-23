# UFO WASH — Production Plan

## Production strategy

Build one high-quality, end-to-end vertical slice first. Prove the first-person cleaning loop, earning/reinvestment and one small automation payoff before expanding the simulation. Use milestone exit criteria, not arbitrary deadlines. Do not start a new phase because the code exists; start it when the current phase is verified and accepted.

## Phase 0 — Audit and pre-production

**Goal:** Understand the current project and lock the smallest convincing production target.

Tasks:
- Audit the existing Unity repository, scenes, prefabs, code, assets, build evidence and Git history.
- Establish current verified baseline and list unknowns.
- Confirm Unity 6.6 patch and package versions.
- Approve the GDD and first vertical-slice scope.
- Choose first-person controller/camera approach.
- Prototype and compare dirt visualization approaches.
- Establish visual target with reference board and asset plan.
- Create first task breakdown and QA plan.

Deliverables:
- `docs/reports/PROJECT_AUDIT_001.md`
- Approved GDD decisions and updated decision log.
- Technical architecture baseline.
- Visual reference board / asset plan.
- Vertical-slice backlog with acceptance criteria.

Exit criteria:
- Existing project state is evidenced.
- Key risks and current defects are known or clearly marked unknown.
- First vertical-slice scope is approved.
- No major unknown is being hidden behind assumptions.

## Phase 1 — First-person feel and station foundation

**Goal:** Establish a convincing playable space before adding business systems.

Tasks:
- Player movement, look, interaction and basic controls.
- One well-composed starter station environment.
- One dock/bay with readable navigation.
- Basic UFO presentation and arrival staging.
- Lighting, materials, scale and audio baseline.

Exit criteria:
- Movement and camera feel responsive.
- The station reads as a purposeful alien spacecraft service facility.
- No critical control, collision or scene stability defects.
- Visual review screenshots exist at gameplay eye level and overview.

## Phase 2 — Core cleaning job

**Goal:** Make the central job loop genuinely satisfying.

Tasks:
- Customer/job request and arrival.
- Assign/dock UFO into the bay.
- Inspect service requirements.
- Tool equip/use loop.
- Dirt visualization and removal.
- Foam/treatment/rinse/finish sequence as approved.
- Job quality, completion and payout.

Exit criteria:
- A new tester can complete a job without developer explanation.
- Dirt removal is clearly visible and responds correctly to tool use.
- Job state transitions are reliable.
- Audio/visual feedback makes tool contact and progress clear.
- The complete job loop is testable repeatedly.

## Phase 3 — Economy and meaningful reinvestment

**Goal:** Make the player want to complete another job to unlock a useful improvement.

Tasks:
- Money/reward system.
- Basic job board and customer variety.
- Supply costs/restock if validated as fun.
- Upgrade/shop interaction.
- One or more purchases that visibly or mechanically change play.
- Save/load proof for money and progression.

Exit criteria:
- Rewards and costs are understandable.
- At least one purchase meaningfully changes player capability or throughput.
- Save/load preserves accepted progression and handles invalid/missing data safely.
- No exploit or duplication issue in the tested loop.

## Phase 4 — First automation payoff

**Goal:** Show the transition from individual worker to business owner.

Tasks:
- One employee role OR one automated equipment stage (choose the best first demonstration).
- Physical presence/operation visible in the station.
- A defined input, output and capacity.
- Clear assignment/configuration UI or diegetic interaction.
- A simple status/diagnostic view.

Exit criteria:
- Automation performs its task without manual player input once configured.
- The player can tell what it is doing and why it may be blocked.
- It saves meaningful time or increases capacity.
- It does not break the manual workflow.

## Phase 5 — Vertical slice polish and validation

**Goal:** Deliver a 10–15 minute representative experience.

Tasks:
- Improve station art, lighting, materials, VFX, animation and sound.
- Add onboarding and clear UI feedback.
- Balance the first progression steps.
- Test save/load, repeated jobs, invalid states and performance.
- Capture representative screenshots/video.

Exit criteria:
- All vertical-slice requirements in the GDD are met.
- No open P0 defects; no unresolved P1 defect affecting the core loop.
- Playtest feedback confirms the loop is understandable and has a reason to continue.
- Build and test evidence are recorded.
- Project owner accepts the milestone.

## Phase 6 — Full game expansion (only after vertical-slice approval)

Potential systems:
- More UFO classes and service requirements.
- Multiple bays and station layouts.
- Further equipment and cleaning processes.
- Employee roles, training and task assignment.
- Multi-stage automated lines.
- Supply logistics and waste management where fun.
- New hangars/locations and higher-value contracts.
- Deeper economy, reputation and operational trade-offs.

Each system requires a feature brief, effort/risk assessment, acceptance criteria and owner approval.

## Phase 7 — Release preparation

- Stability, performance and save reliability.
- Settings, accessibility and input support.
- Steam store assets and trailer capture.
- Legal asset/license verification.
- Build/release pipeline and QA matrix.
- Pricing and market research.
- Marketing plan and demo strategy.

Android is considered only after PC gameplay and controls are validated. Multiplayer is not assumed.

## Milestone tracking

Use GitHub Milestones for:
- M0 — Audit & Pre-production
- M1 — First-person Station
- M2 — Core Cleaning Loop
- M3 — Economy & Reinvestment
- M4 — First Automation
- M5 — Vertical Slice

Do not put speculative late-game tasks into the active milestone. Keep the active milestone narrow.
