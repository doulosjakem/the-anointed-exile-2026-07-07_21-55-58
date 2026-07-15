# Asset Generation Prompts — The Exile King

> **Art Direction:** Hand-painted historical illustration / illuminated manuscript / aged parchment aesthetic.
> **NOT realistic photography. NOT fantasy. NOT anime.**
> Think: medieval manuscript marginalia, ancient chronicle illustrations, watercolor and ink line art.
> Muted earth tones — ochre, umber, faded ochre, parchment tan, faded crimson, charcoal ink.
> All figures wear historically accurate bronze age Levantine clothing: linen tunics, leather armor/vests, wool cloaks, sandals. Bronze weapons (short swords, spears with bronze tips, composite bows). No plate armor, no steel, no fantasy elements.
> **Recommended tool:** ComfyUI + DreamShaper XL Lightning (see ART_GENERATION_GUIDE.md)

---

## Universal Negative Prompt (all generations)

```
photorealistic, hyperrealistic, realistic skin texture, photograph, cinematic lighting, ray tracing, 3d render, octane render, unity engine, video game screenshot, modern clothing, plate armor, steel armor, fantasy armor, elaborate armor, glowing, neon, bright colors, anime, manga, cartoon, digital art, illustration, signature, watermark, text, logo, ugly, deformed, blurry, low quality, worst quality, bad anatomy, extra limbs, merged body, duplicate, clone, two people, three people, group, crowd, nsfw, gore, blood
```

**Always use this negative prompt. Do not shorten it.**

---

## Prompt Guidance

- **Command card art:** Do NOT prefix with "ONE PERSON ONLY" — these are tactical scenes with multiple figures
- **Unit tokens:** "ONE PERSON ONLY" is fine for these — single figure in a circular medallion
- **Don't skip the negative prompt** — paste the full list every time
- **Batch 5+ per prompt** — pick the best result
- **See ART_GENERATION_GUIDE.md** for full ComfyUI setup, workflow diagrams, and step-by-step instructions

---

## Hex Tile Textures (512×512, tileable)

| Asset | Prompt |
|---|---|
| **Sand tile** | `top-down flat hex tile, sandy desert terrain, warm beige, parchment texture, subtle grain, watercolor wash, board game style, seamless, 512x512` |
| **Rock tile** | `top-down flat hex tile, rocky gravel, gray-brown, stone texture, watercolor wash, board game style, seamless, 512x512` |
| **Grass tile** | `top-down flat hex tile, dry savanna grass, warm green-brown, ink wash, board game style, seamless, 512x512` |

---

## Unit Portrait Prompts (256×256 or 512×512, transparent background)

Generate **waist-up portraits** and composite onto token bases in Unity. Start every prompt with:

> `ONE PERSON ONLY, solo portrait, waist-up,`

### Player Units

Full prompt (unit name + description + suffix). Add negative prompt from the template above.

| Unit | Full Prompt |
|---|---|
| **David** | `ONE PERSON ONLY, young David as a bronze age Israelite fugitive commander, simple linen tunic with leather chest piece, brown wool cloak pinned at shoulder, bronze short sword at his hip, leather sling tucked in his belt, shepherd's staff in hand, determined and watchful expression, standing on a rocky Judean hillside under an overcast sky, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Swordsman** | `ONE PERSON ONLY, young Israelite swordsman, bronze age warrior, simple linen tunic with layered leather vest, worn brown cloak, bronze short sword in hand, small round hide-covered shield on his arm, leather wrapped grip, sturdy sandals, battle-ready stance, alert expression, standing on rocky Judean ground, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Spearman** | `ONE PERSON ONLY, young Israelite spearman, bronze age skirmisher, simple linen tunic with leather shoulder piece, brown cloak tied at neck, long bronze-tipped wooden spear held in both hands, small hide shield slung across his back, knife at his waist, sandals, defensive ready stance, focused expression, standing on a hillside overlooking wilderness valleys, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Slinger** | `ONE PERSON ONLY, young Israelite slinger, bronze age skirmisher, simple linen tunic with leather vest, worn brown cloak, leather sling in hand with pouch at his belt, pouch of smooth stones at his hip, small knife, crouched lightly on the balls of his feet, ready to pivot and throw, alert watchful expression, standing on a rocky slope, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Archer** | `ONE PERSON ONLY, young Israelite archer, bronze age wilderness hunter, simple linen tunic with leather vest, brown cloak, wooden composite bow in hand with arrow nocked, quiver of arrows slung across his back, knife at his waist, sandals, drawing the bow with focused precision, standing on a ridge overlooking the valleys, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Scout** | `ONE PERSON ONLY, young Israelite scout, bronze age wilderness tracker, lean shepherd-skirmisher, simple linen tunic with leather vest, worn brown cloak, sandals, sling at his belt, short spear, small round hide shield slung across his back, knife at his waist, alert watchful expression, standing lightly on rocky Judean hillside overlooking the valleys, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |

### Enemy Units (Amalekites)

| Unit | Full Prompt |
|---|---|
| **Raider** | `ONE PERSON ONLY, Amalekite raider, bronze age nomadic desert warrior, worn red-brown wool cloak wrapped around his body, leather tunic underneath, bronze-tipped spear in hand, curved knife at his belt, weathered and lean face, windblown hair, hardened squinting expression, standing on sandy desert ground with rocky outcrops, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Slinger** | `ONE PERSON ONLY, Amalekite slinger, bronze age nomadic skirmisher, dusty red-brown cloak wrapped loose, leather sling in hand with pouch of stones at his hip, simple leather tunic, barefoot or sandaled, crouched low in a mobile throwing stance, alert predatory expression, standing on sandy desert terrain, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Archer** | `ONE PERSON ONLY, Amalekite mounted archer, bronze age nomadic horseman, dusty red-brown cloak flowing, riding a small hardy desert horse, composite bow drawn with arrow aimed, quiver strapped to the horse's flank, weathered focused expression, horse mid-stride on open desert plain, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Scout** | `ONE PERSON ONLY, Amalekite scout, bronze age desert tracker, lean wind-hardened build, dusty red-brown cloak patched and worn, short javelin in hand, leather sling at his belt, small hide shield slung across his back, sandals, crouched and scanning the horizon, keen narrowed eyes, standing on a rocky desert ridge, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Camel Rider** | `ONE PERSON ONLY, Amalekite camel rider, bronze age desert warrior, dusty red-brown cloak and headwrap, bronze-tipped spear held upright, riding a tall dromedary camel, leather reins in hand, weathered stern expression, camel standing on sandy desert ground with distant mountains, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |
| **Chieftain** | `ONE PERSON ONLY, Amalekite chieftain, bronze age nomadic warlord, dark red-brown wool cloak trimmed with rough wool fringe, leather and bronze chest piece, weathered authoritative face, gray-streaked beard, bronze short sword at his hip, spear in hand, tall headdress wrapped in desert cloth, standing on a rocky outcrop overlooking his warriors, hand-painted historical illustration, watercolor and ink on aged parchment, board game card art, centered composition, family-friendly` |

---

## Equipment & Weapon Prompts (transparent background)

| Asset | Prompt |
|---|---|
| **Bronze sword** | `bronze age short sword, single person holding, hand-painted illustration, watercolor, transparent background` |
| **Leather shield** | `round leather shield, bronze rim, hand-painted illustration, watercolor, transparent background` |
| **Spear** | `bronze-tipped wooden spear, hand-painted illustration, watercolor, transparent background` |
| **Sling** | `leather sling with pouch, hand-painted illustration, watercolor, transparent background` |
| **Bow** | `composite bow, hand-painted illustration, watercolor, transparent background` |
| **Camel** | `dromedary camel, side view, hand-painted illustration, watercolor, transparent background` |

---

## UI Elements (transparent background)

| Asset | Prompt |
|---|---|
| **End Turn button** | `rounded rectangle, aged parchment color, ink border, game UI, flat design, 200x60` |
| **Command card back** | `blank aged parchment card, rectangular, ink border, hand-painted texture, 250x350` |
| **HP bar background** | `thin bar, dark brown ink wash, game UI, 100x10` |
| **HP bar fill** | `thin bar, faded crimson, game UI, 100x10` |
| **Action icon (move)** | `simple sandal footprint, ink drawing style, white on transparent, 32x32` |
| **Action icon (attack)** | `simple bronze sword, ink drawing style, white on transparent, 32x32` |
| **Reward panel** | `aged parchment panel, dark edges, ink border, rounded corners, 400x300` |
| **Card frame template** | `blank rectangular card frame, aged parchment border, ink line art style, top half and bottom half separated by a thin decorative line, space for illustration, 250x350` |

---

## Command Card Art (250×350, ink & parchment)

Generate card art for the top half of each Command Card. Style matches the unit portrait aesthetic.

| Card | Prompt |
|---|---|
| **Flanking Maneuver** | `two soldiers positioned on opposite sides of an enemy, pincer movement diagram, ink drawing on parchment, tactical formation, hand-painted board game card art, 250x350` |
| **Forced March** | `three soldiers running in a line, dust at their feet, urgency and speed, ink drawing on parchment, hand-painted board game card art, 250x350` |
| **Volley** | `two archers aiming upward, arrows in flight arching through the air, ink drawing on parchment, hand-painted board game card art, 250x350` |
| **Hold the Line** | `a shield wall of three soldiers braced for impact, spears angled forward, ink drawing on parchment, hand-painted board game card art, 250x350` |
| **Rally** | `a commander with arm raised, soldiers gathering around, banner or standard, ink drawing on parchment, hand-painted board game card art, 250x350` |
| **Ambush** | `soldiers hidden behind rocks, one signaling silence, surprise attack formation, ink drawing on parchment, hand-painted board game card art, 250x350` |
| **Coordinated Strike** | `two soldiers attacking the same target simultaneously, crossed weapons, ink drawing on parchment, hand-painted board game card art, 250x350` |
| **Reform Ranks** | `disorganized soldiers moving back into formation, regrouping, ink drawing on parchment, hand-painted board game card art, 250x350` |

---

## Generation Protocol

Use this protocol for all art generation in ComfyUI + DreamShaper XL Lightning.

### Universal Settings
- **Model:** DreamShaper XL Lightning
- **Sampler:** DPM++ SDE Karras
- **Steps:** 4–6
- **CFG Scale:** 2–3
- **Batch count:** 5 per prompt (generate 5, pick the best)

### Universal Negative Prompt (always use this)
```
blurry, watermark, text, signature, ugly, deformed, photorealistic, anime, fantasy armor, modern clothing, gore, nsfw, extra limbs, bad anatomy, two people, duplicate, twin, clone, double head, extra body, merged body, malformed weapon
```

### Complete Generation Queue (101 images total)

See `ART_GENERATION_GUIDE.md` for full ComfyUI setup instructions.

#### Batch 1 — Command Card Art (512×768, 5 samples each = 40 images)
| File Prefix | Prompt |
|---|---|
| `card_flanking_maneuver` | `ONE PERSON ONLY, two bronze age soldiers in a pincer movement formation, one on each side attacking a single enemy, tactical diagram style, ink drawing on aged parchment, hand-painted historical illustration, board game card art, centered composition, family friendly` |
| `card_forced_march` | `ONE PERSON ONLY, three bronze age soldiers running urgently in a line, dust kicking up at their feet, momentum and speed, ink drawing on aged parchment, hand-painted historical illustration, board game card art, centered composition, family friendly` |
| `card_volley` | `ONE PERSON ONLY, two bronze age archers aiming upward at an angle, arrows in flight arcing through the air above, ink drawing on aged parchment, hand-painted historical illustration, board game card art, centered composition, family friendly` |
| `card_hold_the_line` | `ONE PERSON ONLY, a shield wall of three bronze age soldiers braced for impact, spears angled forward defensively, ink drawing on aged parchment, hand-painted historical illustration, board game card art, centered composition, family friendly` |
| `card_rally` | `ONE PERSON ONLY, a bronze age commander with arm raised rallying his men, soldiers gathered around a banner, ink drawing on aged parchment, hand-painted historical illustration, board game card art, centered composition, family friendly` |
| `card_shield_wall` | `ONE PERSON ONLY, three bronze age soldiers with interlocking round shields forming a defensive wall, ink drawing on aged parchment, hand-painted historical illustration, board game card art, centered composition, family friendly` |
| `card_coordinated_strike` | `ONE PERSON ONLY, two bronze age soldiers attacking the same target from different angles, weapons crossing, ink drawing on aged parchment, hand-painted historical illustration, board game card art, centered composition, family friendly` |
| `card_desperate_stand` | `ONE PERSON ONLY, a single bronze age soldier holding ground against multiple oncoming enemies, defiant stance, ink drawing on aged parchment, hand-painted historical illustration, board game card art, centered composition, family friendly` |

#### Batch 2 — Card Frame (512×768, 3 samples = 3 images)
| File Prefix | Prompt |
|---|---|
| `card_frame_template` | `blank rectangular card frame, aged parchment background, ornate ink border decoration, top half separated from bottom half by thin decorative line, hand-painted board game card style, 512x768, family friendly` |

#### Batch 3 — Unit Token Icons (256×256, 5 samples each = 40 images)
| File Prefix | Prompt |
|---|---|
| `token_david` | `simple ink silhouette icon of a bronze age king figure with crown and staff, on aged parchment background, board game token style, centered, 256x256, family friendly` |
| `token_swordsman` | `simple ink silhouette icon of a bronze age soldier with short sword and round shield, on aged parchment background, board game token style, centered, 256x256, family friendly` |
| `token_spearman` | `simple ink silhouette icon of a bronze age soldier holding a long spear, on aged parchment background, board game token style, centered, 256x256, family friendly` |
| `token_slinger` | `simple ink silhouette icon of a bronze age slinger with sling raised, on aged parchment background, board game token style, centered, 256x256, family friendly` |
| `token_archer` | `simple ink silhouette icon of a bronze age archer drawing a bow, on aged parchment background, board game token style, centered, 256x256, family friendly` |
| `token_scout` | `simple ink silhouette icon of a bronze age scout running, light clothing, on aged parchment background, board game token style, centered, 256x256, family friendly` |
| `token_chieftain` | `simple ink silhouette icon of a bronze age chieftain with headdress and spear, on aged parchment background, board game token style, centered, 256x256, family friendly` |
| `token_raider` | `simple ink silhouette icon of a bronze age desert raider with curved sword, on aged parchment background, board game token style, centered, 256x256, family friendly` |

#### Batch 4 — Hex Tiles (512×512, 3 samples each = 9 images)
| File Prefix | Prompt |
|---|---|
| `hex_sand` | `top-down flat hex tile, sandy desert terrain, warm beige, parchment texture, subtle grain, watercolor wash, board game style, seamless, 512x512` |
| `hex_rock` | `top-down flat hex tile, rocky gravel, gray-brown, stone texture, watercolor wash, board game style, seamless, 512x512` |
| `hex_grass` | `top-down flat hex tile, dry savanna grass, warm green-brown, ink wash, board game style, seamless, 512x512` |

#### Batch 5 — UI Elements (256×256, 3 samples each = 9 images)
| File Prefix | Prompt |
|---|---|
| `ui_card_back` | `blank aged parchment card, rectangular, ink border, hand-painted texture, game UI, 250x350, transparent background` |
| `ui_hp_bar_bg` | `thin horizontal bar, dark brown ink wash, game UI element, 100x10, transparent background` |
| `ui_hp_bar_fill` | `thin horizontal bar, faded crimson red, game UI element, 100x10, transparent background` |

### Output Naming Convention
Generated files follow this pattern:
```
[type]_[name]_[batch#].png
```
Where batch# = 001, 002, 003, etc.

After selection: rename to `[type]_[name].png` and copy to `Assets/Textures/`.

---

## Tips for Better Results

1. **"ONE PERSON ONLY" at the start** — this is the single most effective fix for duplicate figures
2. **Strong negative prompt every time** — don't skip it, paste the full list
3. **Generate 3-5 versions** of each unit, pick the cleanest
4. **Upscale in Draw Things** if available, then resize to 256×256 in Unity
5. **Output naming convention:**
   - `hex_sand.png`, `hex_rock.png`, `hex_grass.png`
   - `unit_david.png`, `unit_swordsman.png`, `unit_spearman.png`, `unit_slinger.png`, `unit_archer.png`, `unit_scout.png`
   - `enemy_raider.png`, `enemy_slinger.png`, `enemy_archer.png`, `enemy_scout.png`, `enemy_camel.png`, `enemy_chieftain.png`
   - `equip_sword.png`, `equip_shield.png`, `equip_spear.png`, `equip_sling.png`, `equip_bow.png`, `equip_camel.png`
   - `ui_endturn.png`, `ui_overwork.png`, `ui_card_back.png`
6. **Save a prompt log** — note which seed gave the best result for each unit so you can regenerate consistently

---

## Commercial Use

- **SDXL Turbo** is licensed under CreativeML Open RAIL-M — generated outputs are yours to use commercially
- **SDXL v1 8-bit** — same license, same commercial rights
- All code in this project is your property — use it freely
- Unity Personal license is fine until $200K annual revenue

---

## Development Priority

> Don't let perfect art delay gameplay.

1. Get functional art in place (even placeholder colors)
2. Make combat fun
3. Clean up UI
4. Improve art later

The game itself is valuable. The development process is equally valuable.