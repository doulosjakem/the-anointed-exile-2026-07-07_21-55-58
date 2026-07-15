# Command Cards — Master Catalog

> **Design Philosophy:** Cards answer *"What orders am I giving?"* Each card represents a tactical command David can issue to his warband.

---

## Card Anatomy

Every Command Card has:

```
┌─────────────────┐
│   Card Title     │
├─────────────────┤
│                  │
│   Card Art       │
│   (illustration) │
│                  │
├─────────────────┤
│ TOP ABILITY      │
│ [name] — [desc]  │
├─────────────────┤
│ BOTTOM ABILITY   │
│ [name] — [desc]  │
├─────────────────┤
│ [Lose] tag (if   │
│  card is lost    │
│  after use)      │
└─────────────────┘
```

---

## Starting Deck Pool

The player begins each run with these cards. Total: **10 cards**.

### Flanking Maneuver
| | |
|---|---|
| **Top** | Attack — One unit attacks. +1 Attack if adjacent to an ally. |
| **Bottom** | Move — One unit moves up to 3 spaces. |
| **Lose?** | No |
| **Design note** | Core tactical card. Rewards positioning and adjacency. |

### Forced March
| | |
|---|---|
| **Top** | Move — One unit moves twice. **LOSE this card.** |
| **Bottom** | Move — Two units each move 2 spaces. |
| **Lose?** | Yes (Top ability only) |
| **Design note** | High-risk, high-reward movement. Using the top costs you the card permanently. |

### Volley
| | |
|---|---|
| **Top** | Attack — Up to two ranged units attack. |
| **Bottom** | Move + Attack — One ranged unit attacks after moving. |
| **Lose?** | No |
| **Design note** | Ranged-focused card. Essential for archers and slingers. |

### Hold the Line
| | |
|---|---|
| **Top** | Attack + Buff — One unit attacks. Gains +1 Defense until next turn. |
| **Bottom** | Move — Two adjacent units move 1 space. |
| **Lose?** | No |
| **Design note** | Defensive posture. Great for holding a position. |

### Rally
| | |
|---|---|
| **Top** | Buff — One unit gains +1 Attack this turn. |
| **Bottom** | Move — Move one unit, then that unit may Basic Attack. |
| **Lose?** | No |
| **Design note** | Flexible combat card. Pairs well with any unit type. |

### Shield Wall
| | |
|---|---|
| **Top** | Buff — One adjacent unit gains +2 Defense until next turn. |
| **Bottom** | Attack — One unit attacks with -1 damage but gains +1 Defense. |
| **Lose?** | No |
| **Design note** | Defensive card. Protects key units. |

### Rapid Advance
| | |
|---|---|
| **Top** | Move — One unit moves 3 spaces and may Basic Attack after moving. |
| **Bottom** | Move — One unit moves 2 spaces. Another unit moves 1 space. |
| **Lose?** | No |
| **Design note** | Aggressive positioning card. Press the attack. |

### Coordinated Strike
| | |
|---|---|
| **Top** | Attack — Two units attack the same target. Combine damage. |
| **Bottom** | Attack — One unit attacks with +1 damage. |
| **Lose?** | No |
| **Design note** | Boss-killer. Focus fire is powerful. |

### Reform Ranks
| | |
|---|---|
| **Top** | Move — Move all adjacent units 1 space. |
| **Bottom** | Buff — One exhausted unit refreshes (can act again next turn as normal). |
| **Lose?** | No |
| **Design note** | Utility card. Reshuffles your formation. |

### Desperate Stand
| | |
|---|---|
| **Top** | Attack + Buff — One unit attacks. Then gains +2 Defense and cannot move next turn. **LOSE this card.** |
| **Bottom** | Buff — One unit gains +1 Attack and +1 Defense this turn. |
| **Lose?** | Yes (Top ability only) |
| **Design note** | Last-resort card. Powerful but costly. |

---

## Unlockable Cards (Reward Pool)

These cards can be added to the deck as run rewards.

### Ambush
| | |
|---|---|
| **Top** | Attack — One unit attacks a target that has already acted this turn. +2 damage. |
| **Bottom** | Move — One unit moves 2 spaces. Enemy cannot target this unit until your next turn. |
| **Lose?** | No |

### Counter-Charge
| | |
|---|---|
| **Top** | Move + Attack — Move one unit 2 spaces toward an enemy, then attack with +1 damage. |
| **Bottom** | Buff — One unit gains +1 Attack against adjacent enemies. |
| **Lose?** | No |

### Skirmish
| | |
|---|---|
| **Top** | Move + Attack — One unit moves 1 space, attacks, then moves 1 space (hit-and-run). |
| **Bottom** | Move — One unit moves 2 spaces. |
| **Lose?** | No |

### War Cry
| | |
|---|---|
| **Top** | Buff — All adjacent allies gain +1 Attack this turn. |
| **Bottom** | Attack — One unit attacks. No damage bonus. |
| **Lose?** | No |

### Disengage
| | |
|---|---|
| **Top** | Move — One unit moves 3 spaces. Cannot be targeted by attacks of opportunity. |
| **Bottom** | Heal — One unit recovers 1 HP (cannot exceed max HP). |
| **Lose?** | No |

### Overwatch
| | |
|---|---|
| **Top** | Attack — One ranged unit attacks any enemy that moves within range this turn. |
| **Bottom** | Buff — One unit gains +2 range this turn. |
| **Lose?** | No |

---

## Card Types by Effect

### Attack
| Card | Top | Bottom |
|---|---|---|
| Flanking Maneuver | Attack (+1 adjacent) | — |
| Volley | Two ranged attack | Attack after move |
| Hold the Line | Attack (+1 defense) | — |
| Desperate Stand | Attack (+heavy buff, lose) | — |
| Coordinated Strike | Two vs one (combined) | Attack (+1 dmg) |
| Ambush | Attack (+2 vs acted) | — |
| Counter-Charge | Move + attack (+1) | — |
| Skirmish | Hit-and-run | — |
| War Cry | — | Basic attack |
| Overwatch | Ranged reacts to move | — |

### Move
| Card | Top | Bottom |
|---|---|---|
| Flanking Maneuver | — | Move 3 |
| Forced March | Move twice (lose) | Two units move 2 |
| Hold the Line | — | Two adjacent move 1 |
| Rally | — | Move + basic attack |
| Rapid Advance | Move 3 + basic attack | Move 2 + another move 1 |
| Reform Ranks | All adjacent move 1 | — |
| Counter-Charge | Move 2 + attack | — |
| Skirmish | Hit-and-run | Move 2 |
| Disengage | Move 3 (safe) | — |

### Buff / Defense
| Card | Top | Bottom |
|---|---|---|
| Hold the Line | +1 Defense after attack | — |
| Rally | +1 Attack this turn | — |
| Shield Wall | +2 Defense to adjacent | Attack with +1 Defense |
| Desperate Stand | +2 Defense, can't move | +1 Atk & +1 Def |
| Reform Ranks | — | Refresh exhausted unit |
| War Cry | All adjacent +1 Attack | — |
| Disengage | — | Heal 1 HP |
| Overwatch | — | +2 Range |

### Special (Lose on Use)
| Card | Ability |
|---|---|
| Forced March (Top) | Move twice |
| Desperate Stand (Top) | Attack + heavy buff, can't move next turn |

---

## Deck Rules Summary

| Rule | Details |
|---|---|
| **Starting hand** | 2 cards |
| **Draw per turn** | Up to 2 (hand fills to 4) |
| **Cards chosen per turn** | 2 |
| **Resolve** | Top of card A + Bottom of card B |
| **After resolve** | Both cards → Spent pile |
| **Lose on use** | Card → Lost pile (instead of Spent) |
| **Lost recovery** | Only via run reward or special ability |
| **Empty deck** | Auto-refresh from Spent pile |
| **Starting deck size** | 10 cards |
| **Max hand size** | 4 cards |

---

## Card Ability Icons (Future)

For visual clarity, each ability type should have a small icon on the card:

| Ability | Icon Concept |
|---|---|
| Attack | Crossed swords |
| Move | Sandal footprint |
| Buff / Defense | Shield |
| Heal | Red crescent / drop |
| Multi-target | Two figures |
| Lose | Skull / broken seal |