# Handoff — MVP Sprint Progress

> **Auto-compacting active.** This file tracks what's built and what's next so a fresh context can resume seamlessly.

---

## 🚨 Design Pivot: Command Card System

The battle system has been redesigned from the ground up. See `GDD.md` for the full spec and `ROADMAP.md` for the updated sprint plan.

**Old system:** 2 team actions per turn, unit state machines (Ready/Acted/Exhausted), Overwork mechanic, per-state action tables.

**New system:** Command Card deck (Draw 4 → Choose 2 → Resolve Top/Bottom → Discard), units have fixed base stats enhanced by cards.

**What this means for existing code:**
- `HexGrid.cs` — ✅ Still good, no changes needed
- `HexTileGenerator.cs` — ✅ Still good
- `UnitVisual.cs` — ✅ Still good
- `GameSetup.cs` — ✅ Still good, spawn logic unchanged
- `PlayerInputHandler.cs` — ⚠️ Needs refactor (selection/movement logic can be reused, but action resolution needs card integration)
- `AIDirector.cs` — ✅ Still good, AI doesn't use cards
- `TurnManager.cs` — ❌ Needs significant refactor (new turn phases, remove Overwork, card-based flow)
- `DamagePopup.cs` — ✅ Still good, reuse as-is
- `GameUIController.cs` — ⚠️ Needs update (replace action counter with card selection UI)
- `RunManager.cs` — ⚠️ Needs update (add deck improvement rewards)
- `Unit.cs` — ⚠️ Needs refactor (simplify: remove state machine, focus on base stats + passives)
- `UnitData.cs` — ⚠️ Needs update to match new simplified unit stat blocks
- `MobileInputHandler.cs` — ✅ Still good

**Sprints 0-2 are fully done and the code is committed.** Sprints 3-7 have been rewritten for the new card system.

---

## Latest Commit

`474a0d7` — Sprints 6-7: Unit data definitions, encounter data, mobile input, and final polish

---

## Current Sprint Progress

### ✅ Sprint 0: Foundation
- GDD.md, IDEAS.md, ROADMAP.md
- HexGrid.cs (8×8 hex, axial coords, distance, neighbors, BFS pathfinding)
- Unit.cs (state machine, armor HP, action system, setters) — ⚠️ needs refactor for card system
- TurnManager.cs (turn phases, 2 actions, Overwork mechanic, commander death) — ❌ needs refactor
- AIDirector.cs (priority-based AI — 5 tiers)
- All committed and pushed

### ✅ Sprint 1: Visual Grid & Unit Placement
- HexTileGenerator.cs (procedural hex mesh, sand palette, border rings) — ✅ good
- UnitVisual.cs (board game tokens — different shapes per type, selection ring) — ✅ good
- GameSetup.cs (spawns David + 2 Scouts vs Chieftain + Raider + Slinger + Scout) — ✅ good
- HexGrid.cs updated (visual generation, unit placement, BFS movement) — ✅ good
- Unit.cs updated (setter methods) — ⚠️ needs refactor
- All committed and pushed

### ✅ Sprint 2: Selection & Movement
- PlayerInputHandler.cs — tap detection, unit selection, move/attack highlights, attack execution — ⚠️ needs card integration
- GameUIController.cs — UI framework — ⚠️ needs card UI (hand display, selection, resolution)

### ❌ Sprint 3 (Old): Combat & Turn Flow
- DamagePopup.cs — ✅ Still good, reusable
- Turn cycling & AI turns — needs refactor for card flow
- **Superseded by new Sprint 3 (Command Card Data System)**

### ❌ Sprint 4 (Old): Overwork & Commander Mechanics
- **Removed entirely.** Overwork no longer exists. Commander aura still exists as a unit passive.

### ❌ Sprint 5 (Old): Rewards & Run Structure
- RunManager.cs — ⚠️ partially reusable, needs deck improvement rewards added
- Battle progression structure — ✅ still good

### ❌ Sprint 6 (Old): Unit Data & Balance
- UnitData.cs ScriptableObjects — ⚠️ needs update for simplified stat blocks
- EncounterData.cs — ✅ still good

### ❌ Sprint 7 (Old): Mobile & UI Polish
- MobileInputHandler.cs — ✅ good
- UI scaling configuration — ✅ good

---

## Updated Sprint Plan (from ROADMAP.md)

| Sprint | Focus | Status |
|---|---|---|
| 0 | Foundation (GDD, core scripts) | ✅ Done |
| 1 | Visual grid & unit placement | ✅ Done |
| 2 | Selection & movement | ✅ Done |
| 3 | **Command Card data system** | ❌ PENDING |
| 4 | **Command Card UI & selection** | ❌ PENDING |
| 5 | **Card resolution & unit linking** | ❌ PENDING |
| 6 | **Updated turn flow & enemy AI** | ❌ PENDING |
| 7 | **Campaign, deck rewards, & polish** | ❌ PENDING |

---

## Project File Structure

```
GDD.md                       — Game Design Document (updated for Command Card system)
IDEAS.md                     — Future concepts
ROADMAP.md                   — Sprint-by-sprint plan (updated for card system)
Handoff.md                   — THIS FILE
PROMPTS.md                   — Asset generation prompts (add card art prompts)

Assets/Scripts/
  HexGrid.cs                 — 8×8 hex grid + tile generation + placement + BFS ✅
  HexTileGenerator.cs        — Procedural hex mesh with sand palette ✅
  Unit.cs                    — State machine, armor HP, actions ⚠️ needs refactor
  UnitVisual.cs              — Board game token visuals ✅
  TurnManager.cs             — Turn phases, actions, Overwork ❌ needs refactor
  AIDirector.cs              — Priority-based AI ✅
  PlayerInputHandler.cs      — Tap/click input, selection, movement, attack ⚠️ needs card integration
  GameUIController.cs        — UI ⚠️ needs card UI
  GameSetup.cs               — Spawns initial battle ✅
  DamagePopup.cs             — Floating damage numbers ✅
  RunManager.cs              — Run progression ⚠️ needs deck rewards
  MobileInputHandler.cs      — Pinch-to-zoom, pan ✅
  UnitData.cs                — ScriptableObject definitions ⚠️ needs update
  EncounterData.cs           — Encounter definitions ✅

Assets/Scripts/ (NEW — Sprite 3)
  [CommandCard.cs]           — Card ScriptableObject
  [CardDeckManager.cs]       — Deck/hand/spent/lost piles

Assets/Scripts/UI/ (NEW — Sprint 4)
  [HandDisplay.cs]           — Card hand UI

Assets/Scripts/ (NEW — Sprint 5)
  [CardAbilityResolver.cs]   — Card-to-unit action binding
```

---

## What To Do Next (If Resuming)

### Immediate Next Step
**Start Sprint 3: Command Card Data System**
1. Create `CommandCard.cs` as a ScriptableObject with top/bottom ability fields
2. Create 8-10 card data assets in `Assets/Resources/CommandCards/`
3. Build `CardDeckManager.cs` — deck, hand, spent, lost piles with draw/play/discard/refresh
4. Implement the draw rules: start with 2, draw to 4 each turn, auto-refresh from spent

### Quick Reference: MVP Command Cards (from GDD.md)
| Card | Top | Bottom | Lose? |
|---|---|---|---|
| Flanking Maneuver | Attack (+1 if adjacent to ally) | Move 3 spaces | No |
| Forced March | Unit moves twice | Two units move 2 | Yes (top) |
| Volley | Two ranged units attack | Ranged attack after move | No |
| Hold the Line | Attack (+1 defense) | Move two adjacent 1 | No |
| + 4-6 more cards to design | | | |