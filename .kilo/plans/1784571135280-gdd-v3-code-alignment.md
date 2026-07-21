# GDD v3 → Code Alignment Plan

**Goal:** Identify all code changes required to bring the current Unity implementation in line with the GDD v3 spec.

## Current State Summary

The codebase has the command card turn flow working (Sprints 3–6 per ROADMAP.md). `CardDeckManager`, `CardTurnController`, `CardAbilityResolver`, and `TurnManager` implement the Selection → Resolution → Done → Enemy cycle. `GameSetup` builds decks at runtime from army composition. `RunManager` handles basic battle progression and rewards.

## Key Gaps

### 1. Unit Stats (Unit.cs, GameSetup.cs)

**GDD spec:** Each unit has fixed base stats — HP (Leather=1, Bronze=2, Iron=3), Move, Attack damage, Range, and passive abilities.

**Current code:** HP is derived from `ArmorTier` only. Move, base attack damage, and range are NOT stored per-unit. Damage currently comes entirely from card values (`CardAbilityResolver.ResolveAttack` uses card `topValue/bottomValue`). `Unit.GetAttackRange()` has hardcoded switch logic.

**Action:** Add a `UnitStats` struct/class (or ScriptableObject `UnitData`) with `baseMove`, `baseAttackDamage`, `range`, and a list of `PassiveAbility` definitions. Update `Unit.cs` to carry these stats. `CardAbilityResolver` and `AIDirector` must read from unit stats instead of hardcoded values.

**Risk:** Changes damage calculation contract. Need to decide: do card values MODIFY unit base damage, or REPLACE it? The GDD is ambiguous here. Recommend: card value = number of units activated + damage bonus; unit base damage = per-unit damage. Resolve this before implementing.

### 2. Passive Abilities (Unit.cs, CardAbilityResolver.cs, AIDirector.cs)

**GDD spec:** Commander Aura (+1 dmg adjacent), Shield Block (defend once/turn), Brace (+dmg vs charging), Aim (+1 dmg next shot if stationary), Retreat (+1 move when disengaging), War Cry (+1 dmg adjacent), Command (rally exhausted ally), Parthian Shot (move+shoot), Trample (push target).

**Current code:** None implemented. `Unit` has only `HasActivatedThisTurn`.

**Action:** Add a `PassiveAbility` enum and trigger system. `Unit` holds a list of active passives. `TakeDamage`, `Activate`, `GetAttackRange`, etc., consult passives. This is non-trivial but isolated to `Unit.cs` and `CardAbilityResolver.cs`.

### 3. Scenario Objectives (TurnManager.cs, RunManager.cs, GameUIController.cs)

**GDD spec:** 9 objective types: Eliminate, Rescue captives, Escort civilians, Recover livestock, Burn supplies, Defend position, Escape pursuit, Ambush patrol, Breakthrough.

**Current code:** Victory condition is only `unit.IsCommander` death check in `TurnManager.OnAnyUnitDied`.

**Action:** Create `ScenarioObjective` ScriptableObject or serializable class with type, targets, and success condition. `TurnManager` and `RunManager` evaluate objectives each turn. `GameUIController` shows objective text and progress. This is a medium-sized addition (~2–3 new files, updates to 3 existing).

### 4. Expanded Unit Types & Factions (UnitType.cs, GameSetup.cs)

**GDD spec:** David's Company (David, Refugees, Outcasts, Swordsmen, Spearmen, Slingers, Archers, Scouts, Veterans, Mighty Men), Saul's Kingdom, Jonathan's Followers, Philistines, Amalekites, etc.

**Current code:** `UnitType.cs` has 13 entries (David, Swordsman, Spearman, Slinger, Archer, Scout, Refugee, Veteran, Chieftain, Raider, EnemyArcher, EnemyScout, CamelRider). Missing: Outcast, Mighty Men, Abner, Royal Guard, Benjamite Spearmen, Israelite Archers, Officers, Elite Bodyguards, Achish, Heavy Infantry, Champion, Lord of the Philistines, Desert Scout.

**Action:** Expand `UnitType` enum. Add faction data (`Faction` enum: David, Saul, Jonathan, Philistine, Amalekite). Update `GameSetup.CreateCardForUnitType` and `UnitData` to cover new types. For MVP, only David's Company needs to be playable; enemy factions can be minimal.

### 5. Run State Persistence (CardDeckManager.cs, RunManager.cs)

**GDD spec:** Card state (deck/hand/spent/lost) persists between battles. Lost cards stay lost for the entire run unless recovered.

**Current code:** `CardDeckManager.InitializeDeck()` clears all piles and rebuilds from a new `startingCards` list every battle. Lost cards are wiped. `RunManager.SetupNextBattle()` destroys all units and respawns.

**Action:** Add `CardDeckManager.SaveState()` / `LoadState()` (or serialize the 4 lists). `RunManager.StartNewBattle()` should preserve card piles and only add new cards for newly recruited unit types. Remove `InitializeDeck` call between battles; call it only on game start.

### 6. Reward System (RunManager.cs, GameUIController.cs)

**GDD spec:** After battle, choose ONE: (1) Recruit new unit, (2) Upgrade existing unit, (3) Improve equipment (Wood→Bronze→Iron), (4) Gain supplies (heal), (5) Improve Command Deck (add card, upgrade card, recover lost card).

**Current code:** `GenerateRewardOptions()` offers Recruit, Upgrade (+1 HP), Heal all, Recover Lost Card. No equipment system. No card add/upgrade rewards.

**Action:** Expand `GenerateRewardOptions` to match GDD. Add `EquipmentTier` (Wood/Bronze/Iron) to `Unit` or `UnitData`. Add card upgrade logic (increase `topValue`/`bottomValue` or `maxActivations`). Add "Add Card" reward (pick from unlocked cards not yet in deck).

### 7. Battle Progression & Enemy Composition (GameSetup.cs, RunManager.cs)

**GDD spec:** Battle 1: 3 enemies + chieftain (Easy). Battle 2: 4 enemies + chieftain (Medium). Battle 3: 5 enemies + chieftain + elite (Hard). Boss: unique scenario.

**Current code:** `enemyCount = 2 + battleNumber` (3, 4, 5). No elite units, no chieftain guaranteed, no boss-specific scenario logic.

**Action:** Replace `GetEnemyForBattle` with encounter tables per battle number. Ensure chieftain is always included. Add elite unit variants (Veteran Swordsman, etc.) for Battle 3+. `SpawnBossBattle` should accept a scenario definition rather than being hardcoded.

### 8. Deck Building Rules (GameSetup.cs)

**GDD spec:** 1 copy per unit type brought, up to a max of 2 copies per card type.

**Current code:** `BuildInitialDeck` adds 1 card per player unit. No max-2 enforcement.

**Action:** Track card counts in `BuildInitialDeck`. If a card already has 2 copies, skip adding a third. This is a small logic change.

### 9. Card Targeting (CardTurnController.cs, CardAbilityResolver.cs)

**GDD spec:** Player picks which units execute each half. If no valid targets, allow skip/mulligan.

**Current code:** `CardAbilityResolver.FindAttackTarget` auto-picks nearest enemy. `CardTurnController.PromptForSelection` shows "skipping" if no valid units but doesn't let player pick a different card.

**Action:** Expose target selection for attacks. When `valid.Count == 0`, allow player to return to hand and pick different cards instead of auto-skip.

### 10. Commander Aura & Activation Token (Unit.cs, CardAbilityResolver.cs)

**GDD spec:** David provides adjacent allies +1 damage. Each unit activates only once per player turn (activation token).

**Current code:** `Unit.Activate()` sets `hasActivatedThisTurn`. No aura calculation. `CardTurnController` places activation via `resolver.ResolveAttack` calling `unit.Activate()`.

**Action:** Add `GetAdjacentAllies()` helper. In damage resolution, check for adjacent David and apply +1. This is straightforward.

### 11. Fatigue Timing Verification (TurnManager.cs, CardDeckManager.cs)

**GDD spec:** Start with 2 cards. First turn draw 2 more. Each subsequent turn draw up to 2 to refill to 4. Then lose 1 random card.

**Current code:** `InitializeDeck` deals 2. `StartPlayerTurn` calls `DrawToHandSize(4)` then `ApplyFatigue()`. This is correct.

**Action:** No change needed, but verify first turn behavior in `GameBootstrap` / `GameSetup` (ensure `StartPlayerTurn` is called after `InitializeDeck`).

### 12. Victory / Defeat Screens (GameUIController.cs, RunManager.cs)

**GDD spec:** Victory screen shows surviving units and cards added/upgraded. Defeat screen shows stats (battles won) and "New Run".

**Current code:** `GameUIController.OnGameOver` shows a basic "Victory!" / "Defeat" panel with "New Run". No stats, no run summary.

**Action:** Expand game over panel to show run stats. Add victory screen between battle end and reward picker. Wire defeat to full restart with stats.

## Out of Scope for MVP

- Multi-faction player control (Jonathan, Philistines) — can be data-only additions for now
- Counter-attacks
- Terrain bonuses / Fog of war
- Voice acting / Cutscenes
- Multiplayer
- Campaign map
- Morale system

## Recommended Implementation Order

1. **Unit stats refactor** — unblock damage accuracy and passives
2. **Passive ability framework** — Commander Aura is highest priority
3. **Run state persistence** — required for meaningful runs
4. **Battle progression fix** — correct enemy compositions
5. **Reward expansion** — equipment + card upgrades
6. **Scenario objectives** — medium complexity, adds variety
7. **Victory/defeat polish** — UI only, low risk
8. **Card targeting UX** — small UX improvement
9. **New unit types** — data work, can be gradual

## Resolved Decisions

1. **Damage formula:** Unit base damage + card bonus/modifier. Card `topValue`/`bottomValue` represent bonus damage (or activation count), not total damage. `CardAbilityResolver.ResolveAttack` will read `unit.baseAttackDamage + cardValue`.

## Remaining Open Questions

1. **Equipment system:** Per-unit or per-type? GDD says "Improve equipment (Wood → Bronze → Iron)". Recommend per-unit with `equipmentTier` field on `UnitTemplate`.
2. **Card upgrade:** What does upgrading a card do? Recommend increase `topValue`/`bottomValue` by 1 and unlock higher `maxActivations`.
3. **Scenario objectives for MVP:** All 9 types, or subset? Recommend MVP = Eliminate, Defend, Escape, Rescue (4 types).
