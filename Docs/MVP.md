# UFO WASH — Minimum Viable Product

**Version:** 0.1 · **Status:** Scope baseline

## Objective
Prove one satisfying end-to-end loop: player enters a wash bay, approaches one dirty UFO, uses a spray tool, sees dirt visibly reduce, completes service and earns credits.

## Included
- One compact navigable wash facility.
- First-person movement and mouse-look.
- Basic collision and interaction.
- One UFO with a dirt state.
- One spray tool with aim and range.
- Dirt reduction from tool use.
- Immediate visual cleaning feedback (simple material/overlay feedback is acceptable in prototype).
- Job completion criteria and credit reward.
- Minimal HUD and interaction prompt.
- One upgrade with a measurable effect.
- Reliable launch in Unity.

## Excluded
Employees/AI staffing; multiple maps/open world; complex customer traffic; large vehicle catalog; advanced construction; automation/idle systems; multiplayer; monetization/premium currency; final art and extensive customization.

## Acceptance criteria
1. Play Mode starts without critical errors.
2. Movement and camera work.
3. Interaction prompt appears in range and disappears out of range.
4. Spray affects only valid targets in range and aim.
5. Dirt value decreases with visible feedback.
6. Job completion triggers once at threshold.
7. Reward grants exactly once and is displayed.
8. Upgrade validates funds and applies its effect.
9. Core loop can be repeated/reset.
10. No known blocking console errors.

## Implementation order
A. Project audit and baseline scene
B. Player controller
C. Interaction framework
D. UFO dirt model
E. Spray tool/feedback
F. Jobs and economy
G. HUD
H. Upgrade
I. Playtest, bug fixes and build validation

## Success metric
A first-time tester understands the interaction without external explanation and chooses to clean a second UFO because the action/feedback/reward loop is satisfying.