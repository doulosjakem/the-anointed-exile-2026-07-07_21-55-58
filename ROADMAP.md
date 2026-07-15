# The Anointed Exile — Development Roadmap

> **Goal:** Playable MVP (3 battles + boss, ~30 min run)
> **Engine:** Unity 6 LTS (URP)
> **Language:** C#
> **Target:** iOS (primary)

---

## Sprint 0: Foundation ✅ DONE

- [x] GDD.md — Full Game Design Document
- [x] IDEAS.md — Future concepts log
- [x] Assets/Scripts/HexGrid.cs — 8×8 hex grid with axial coordinates, distance, neighbors, LoS
- [x] Assets/Scripts/Unit.cs — State machine, armor HP, action system
- [x] Assets/Scripts/TurnManager.cs — Turn phases, 2 actions/turn, Overwork mechanic
- [x] Assets/Scripts/AIDirector.cs — Priority-based AI (5 tiers)
- [x] Committed and pushed to GitHub

---

## Sprint 1: Visual Grid & Unit Placement ✅ DONE

**Goal:** See the board and pieces when pressing Play.

### Tasks

#### 1.1 Create HexTileGenerator.cs
- **File:** `Assets/Scripts/HexTileGenerator.cs`
- Procedurally generate flat-top hex meshes at runtime
- Color palette: warm sandy base, slightly darker raised borders
- Subtle color variation between tiles (not a flat carpet)
- Use URP Lit shader for lighting

#### 1.2 Create UnitVisual.cs
- **File:** `Assets/Scripts/UnitVisual.cs`
- Each unit gets a board-game-token look:
  - Small circular base/stand
  - Body shape made from stacked primitives (cylinder, sphere, cube)
  - Player units: warm blues/teals
  - Enemy units: dusty reds/browns
- David: recognizable commander shape (taller, cape-like shoulder pieces)
- Swordsman: blocky with small sword (thin box)
- Spearman: similar with longer spear piece
- Slinger: smaller, crouched shape
- Scout: small, slim shape
- Amalekite variants: same shapes but red/brown palette

#### 1.3 Create GameSetup.cs
- **File:** `Assets/Scripts/GameSetup.cs`
- Spawns initial battle on Start():
  - Player: David + 2 Scouts at one side of grid
  - Enemy: Amalekite Chieftain + 1 Raider + 1 Slinger + 1 Scout at opposite side
- Registers all units with TurnManager
- Positions units at valid hex coordinates

#### 1.4 Camera Setup
- Orthographic camera, top-down angled view (isometric-ish)
- Adjustable zoom for mobile (pinch-to-zoom later)
- Centers on the grid

#### 1.5 Wire into SampleScene
- Replace default scene content
- Add HexGrid, TurnManager, AIDirector, GameSetup to scene
- Press Play → see 8×8 hex grid with units positioned

### Acceptance Criteria
- [ ] 8×8 hex grid visible with colored tiles
- [ ] David + 2 Scouts visible on left side
- [ ] 4 Amalekites visible on right side
- [ ] Camera shows the full board
- [ ] No errors in console

---

## Sprint 2: Selection & Movement ✅ DONE

**Goal:** Tap a unit, see valid moves, move it.

### Tasks

#### 2.1 Unit Selection
- Tap/click a friendly unit → highlight it (glow ring or outline)
- Deselect by tapping empty space or another friendly unit
- Show unit info (name, HP, current state) in a small UI panel

#### 2.2 Movement Visualization
- When a unit is selected, show valid movement hexes
- Movement range = highest Move value in unit's current state actions
- Valid hexes highlighted with translucent green overlay
- Hexes occupied by other units are blocked

#### 2.3 Movement Execution
- Tap a highlighted hex → unit moves there
- Movement consumes the unit's action (calls unit.Act())
- Update TurnManager action count
- If no actions remain, auto-end turn

#### 2.4 Pathfinding
- Simple A* or BFS pathfinding on hex grid
- Path follows valid hexes, avoids obstacles/units
- Animate unit sliding along path (lerp)

### Acceptance Criteria
- [ ] Tap friendly unit → selected with visual indicator
- [ ] Valid move hexes shown in green
- [ ] Tap valid hex → unit moves there
- [ ] Movement consumes action
- [ ] Cannot move through or onto occupied hexes

---

## Sprint 3: Command Card Data System ❌ PENDING

**Goal:** Command Cards exist as data objects with deck, hand, spent, and lost piles.

> **Note:** This is a new sprint replacing the old Sprint 3 (Combat & Turn Flow) which was built around the old 2-actions-per-turn system.

### Tasks

#### 3.1 Define CommandCard ScriptableObject
- **File:** `Assets/Scripts/CommandCard.cs` (ScriptableObject)
- Fields:
  - Card name (string)
  - Top ability name (string)
  - Top ability description (string)
  - Top ability effect (enum: Attack, Move, Buff, etc.)
  - Top ability value (int, e.g. damage or distance)
  - Bottom ability name
  - Bottom ability description
  - Bottom ability effect (enum)
  - Bottom ability value (int)
  - IsLostOnUse (bool) — if true, card goes to Lost pile instead of Spent
  - Card art reference (Sprite)

#### 3.2 Create Card Data Assets
- **File:** `Assets/Resources/CommandCards/`
- Create ScriptableObject assets for all MVP Command Cards:
  - Flanking Maneuver, Forced March, Volley, Hold the Line (from GDD)
  - Plus 4-6 additional cards for variety
  - Total: 8-10 cards in the initial deck pool

#### 3.3 Card Deck Manager
- **File:** `Assets/Scripts/CardDeckManager.cs`
- Manages Deck pile, Hand pile, Spent pile, Lost pile
- Methods:
  - `DrawCards(int count)` — move from Deck → Hand
  - `PlayCard(CommandCard card)` — move from Hand → Played
  - `DiscardToSpent(CommandCard card)` — move to Spent pile
  - `DiscardToLost(CommandCard card)` — move to Lost pile
  - `RefreshDeck()` — shuffle Spent → Deck
  - `GetHand()` — return current hand as List
  - `GetDeckCount()`, `GetSpentCount()`, `GetLostCount()`
- Initialize deck with a starting pool of cards
- Shuffle on game start

#### 3.4 Card Shuffle & Draw Rules
- Start of game: draw 2 cards into hand
- Start of each turn: draw up to 2 cards (hand max = 4)
- If deck is empty, auto-refresh from Spent pile
- Lost cards never shuffle back (permanent loss)

### Acceptance Criteria
- [ ] CommandCard ScriptableObject defined with all fields
- [ ] 8-10 card assets created
- [ ] CardDeckManager manages all 4 piles correctly
- [ ] Draw, play, discard, refresh all work
- [ ] Lost cards stay lost
- [ ] Deck auto-refreshes from Spent when empty

---

## Sprint 4: Command Card UI & Selection ❌ PENDING

**Goal:** See your hand, pick 2 cards, resolve top/bottom.

### Tasks

#### 4.1 Hand Display UI
- **File:** `Assets/Scripts/UI/HandDisplay.cs` (or extend GameUIController)
- Show 4 Command Cards in a horizontal row at the bottom of the screen
- Each card shows: card art, name, top ability text, bottom ability text
- Cards are touch-friendly tap targets (min 44px)
- Selected cards have a highlight/glow border

#### 4.2 Card Selection Flow
- At start of player turn:
  1. Auto-draw (hand fills to 4)
  2. Prompt: "Choose 2 Command Cards"
  3. Player taps first card → it highlights
  4. Player taps second card → it highlights
  5. "Reveal" button appears
  6. Player taps Reveal → both cards animate open

#### 4.3 Top/Bottom Resolution UI
- After reveal, show both cards in larger view
- Top ability of card A + Bottom ability of card B
- Prompt: "Select unit to execute [top ability]" (if targeting needed)
- After first ability resolves: "Select unit to execute [bottom ability]"
- After both resolve: auto-discard to Spent/Lost

#### 4.4 Card Visual States
- Deck count indicator (e.g. "Deck: 8")
- Hand: always visible
- Spent pile indicator ("Spent: 3")
- Lost pile indicator ("Lost: 1") with distinct red styling
- Visual feedback when deck auto-refreshes

### Acceptance Criteria
- [ ] 4-card hand shown on screen
- [ ] Tap to select 2 cards works
- [ ] Reveal animation triggers
- [ ] Top/bottom resolution UI appears
- [ ] Cards auto-discard after resolution
- [ ] Deck, Spent, Lost counters visible

---

## Sprint 5: Card Resolution & Unit Linking ❌ PENDING

**Goal:** Card abilities actually affect units on the grid.

### Tasks

#### 5.1 Card Ability Resolver
- **File:** `Assets/Scripts/CardAbilityResolver.cs`
- Interprets CommandCard effects (Attack, Move, Buff, etc.)
- When an ability is selected:
  - **Attack ability:** show valid attack targets (red highlight), pick one → execute damage
  - **Move ability:** show valid move hexes (green highlight), pick one → execute movement
  - **Multi-target ability:** let player pick multiple valid units
  - **Self-buff:** apply immediately to the selected unit
- Integrates with existing damage/move systems in PlayerInputHandler

#### 5.2 Unit Targeting for Cards
- Some cards target "one unit" — player picks any friendly unit
- Some cards target "adjacent" — auto-filter valid units
- Some cards target "ranged units only" — filter by unit type
- Show valid targets clearly; invalid targets are greyed out

#### 5.3 Card Action Consumption
- Executing a card's top/bottom ability counts as using that action
- After both abilities resolve, turn is done → Enemy Phase
- Units can still use their Basic Move/Basic Attack as fallback if no cards improve them

#### 5.4 Edge Cases
- What if a card ability can't be executed (no valid targets)?
  - Allow player to pick a different card from hand?
  - Or skip and resolve the other half only?
- What if both players and enemies are out of range?
  - Allow pass/mulligan on a half-ability

### Acceptance Criteria
- [ ] Card Attack ability targets and damages enemies
- [ ] Card Move ability moves friendly units
- [ ] Card Buff ability applies stat changes
- [ ] Multi-target cards work (e.g. Volley: 2 ranged units attack)
- [ ] Invalid targets are filtered correctly
- [ ] Turn ends after both card halves resolve

---

## Sprint 6: Updated Turn Flow & Enemy AI ❌ PENDING

**Goal:** Full turn cycle works with card system, AI responds correctly.

### Tasks

#### 6.1 Refactor TurnManager for Card System
- Old TurnManager uses 2-actions-and-Overwork flow → refactor to card-based flow
- New turn phases:
  1. **Draw Phase** — auto-draw to 4
  2. **Selection Phase** — player picks 2 cards
  3. **Resolution Phase** — resolve top/bottom abilities
  4. **Discard Phase** — auto-discard played cards
  5. **Enemy Phase** — AI takes its turn
  6. **Refresh Phase** — advance any per-turn timers
- Remove old Overwork mechanic entirely
- Keep commander death → immediate victory/defeat

#### 6.2 Update Enemy AI
- AI turns remain priority-based (same system, doesn't use cards)
- AI acts with its units directly: move, attack, use abilities
- AI difficulty can scale per battle (more enemies, tougher units)
- AI has no deck — it's a traditional tactical opponent

#### 6.3 Damage Popups & Feedback
- Existing DamagePopup.cs still works — reuse as-is
- Ensure damage from card abilities also shows popups
- Ensure AI damage shows popups

#### 6.4 Turn Indicator UI
- Update existing turn UI to show card flow phases
- "Your Turn: Choose Cards" → "Resolving..." → "Enemy Turn" → "Your Turn"
- Action counter removed (replaced by card selection indicator)

### Acceptance Criteria
- [ ] Full card-based turn cycle works without errors
- [ ] AI takes its turn after player resolves cards
- [ ] Commander death still ends game immediately
- [ ] Damage popups work for both card and AI attacks
- [ ] Turn indicator shows correct phase

---

## Sprint 7: Campaign, Deck Rewards & Polish ❌ PENDING

**Goal:** Full run with deck improvement rewards, victory/defeat screens.

### Tasks

#### 7.1 Deck Improvement Rewards
- After battle victory, reward options include:
  - **Add a new card** to the deck (from a pool of unlockable cards)
  - **Upgrade an existing card** (improve top or bottom ability value)
  - **Recover a Lost card** (move one card from Lost → Deck)
- Integrate with existing RunManager reward picker system

#### 7.2 Victory Screen
- After winning, show Victory screen
- Display: surviving units, cards added/upgraded
- "Continue" button → reward selection
- Reuse existing victory UI infrastructure

#### 7.3 Defeat Screen
- If David dies → Defeat screen
- Show stats (battles won, enemies killed)
- "New Run" button → restart from Battle 1

#### 7.4 Starting Deck Definition
- Define the initial deck pool (8-10 cards) that all runs start with
- Define unlockable cards that can appear as rewards
- Balance: keep the starting deck simple and functional

#### 7.5 Run State & Persistence
- Track which cards were added/upgraded/lost across battles
- Card state (deck/hand/spent/lost) persists between battles in a run
- Lost cards stay lost for the entire run (unless recovered as a reward)

#### 7.6 Battle Progression
- Battle 1: Easy (3 enemies + chieftain)
- Battle 2: Medium (4 enemies + chieftain + 1 elite)
- Battle 3: Hard (5 enemies + chieftain + 2 elites)
- Boss: Unique scenario (e.g., "Survive 6 turns" or "Kill boss unit with 5 HP")

### Acceptance Criteria
- [ ] Deck improvement rewards work (add, upgrade, recover)
- [ ] Victory screen → reward picker → next battle
- [ ] Defeat screen → new run
- [ ] Deck state persists across battles in a run
- [ ] Lost cards stay lost unless recovered
- [ ] 3 battles + boss playable in sequence

---

## Post-MVP (Future Sprints)

These are captured in `IDEAS.md` and are not part of the MVP roadmap:

- Additional Command Cards (enemy-specific, faction-specific)
- Counter-attacks
- Terrain bonuses
- Fog of war
- Campaign map
- Story events
- Additional factions (Philistines, Saul's army)
- The Mighty Men (hero units)
- Equipment crafting
- Morale system
- Sound effects & music
- Leaderboards
- Android / Steam / Web ports

---

## Development Philosophy

> **Finish something fun before making it big.**

Every feature must answer:
> *"Does this make the tactical decisions more interesting?"*

If not, don't build it.