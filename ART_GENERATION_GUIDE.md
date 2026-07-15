# Art Generation Guide — The Exile King

> **Tool:** ComfyUI + DreamShaper XL Lightning
> **Output folder:** `D:\Jake\ComfyUI\output\exile_king_art\`

---

## Art Direction (Read This First)

**Style:** Hand-painted historical illustration / illuminated manuscript / aged parchment aesthetic.
**NOT realistic photography. NOT fantasy. NOT anime.**
Think: medieval manuscript marginalia, ancient chronicle illustrations, watercolor and ink line art.
Muted earth tones — ochre, umber, faded ochre, parchment tan, faded crimson, charcoal ink.
All figures wear historically accurate bronze age Levantine clothing: linen tunics, leather armor/vests, wool cloaks, sandals. Bronze weapons (short swords, spears with bronze tips, composite bows). No plate armor, no steel, no fantasy elements.

---

## Universal Settings (All Generations)

| Setting | Value |
|---|---|
| **Model** | DreamShaper XL Lightning |
| **Sampler** | DPM++ SDE Karras |
| **Steps** | 4–6 (start with 4, try 6 if results are noisy) |
| **CFG Scale** | 2.5–3.5 (start at 3) |
| **Batch count** | 5 per prompt (generate 5 variants, pick the best) |
| **Resolution** | Varies per batch (see below) |

### Universal Negative Prompt
```
photorealistic, hyperrealistic, realistic skin texture, photograph, cinematic lighting, ray tracing, 3d render, octane render, unity engine, video game screenshot, modern clothing, plate armor, steel armor, fantasy armor, elaborate armor, glowing, neon, bright colors, anime, manga, cartoon, digital art, illustration, signature, watermark, text, logo, ugly, deformed, blurry, low quality, worst quality, bad anatomy, extra limbs, merged body, duplicate, clone, two people, three people, group, crowd, nsfw, gore, blood
```

**Always use this negative prompt. Do not shorten it. It prevents photorealism, fantasy, anachronisms, and artifacts.**

---

## ComfyUI Setup Instructions

### 1. First Time Setup
1. Navigate to `D:\Jake\ComfyUI\`
2. Double-click `run_nvidia_gpu.bat` to start ComfyUI
3. A terminal window opens — wait for it to finish loading (may take 30–60 seconds)
4. Open your browser to `http://127.0.0.1:8188`
5. You should see the ComfyUI node editor with a blank canvas

### 2. Verify the Model
1. Right-click → "Add Node" → "Loaders" → "Load Checkpoint"
2. In the dropdown, you should see **DreamShaper_XL_Lightning.safetensors**
3. If you don't see it, click the refresh icon next to the dropdown
4. If it's still missing, download it from HuggingFace and place in `D:\Jake\ComfyUI\models\checkpoints\`

### 3. Basic Text-to-Image Workflow

Build this node graph:

```
┌─ Load Checkpoint ─────────────────┐
│  model: DreamShaper XL Lightning   │
└──────────┬─────────────────────────┘
           │
           ├──────────────────────────────┐
           │                              │
           ▼                              ▼
┌─ CLIP Text Encode (POS) ──┐   ┌─ CLIP Text Encode (NEG) ──┐
│  prompt: [paste your       │   │  prompt: [paste universal  │
│  positive prompt here]     │   │  negative prompt above]    │
└──────────┬─────────────────┘   └──────────┬─────────────────┘
           │                                 │
           └──────────┬──────────────────────┘
                      │
                      ▼
           ┌─ Empty Latent Image ──────┐
           │  width: 512               │
           │  height: 768              │
           │  batch_size: 5            │
           └──────────┬────────────────┘
                      │
                      ▼
           ┌─ KSampler ───────────────────┐
           │  seed: random                │
           │  steps: 4                    │
           │  cfg: 3                      │
           │  sampler_name: dpmpp_sde     │
           │  scheduler: karras           │
           │  denoise: 1                  │
           └──────────┬───────────────────┘
                      │
                      ▼
           ┌─ VAE Decode ────────────────┐
           └──────────┬──────────────────┘
                      │
                      ▼
           ┌─ Save Image ────────────────┐
           │  filename_prefix: exile_king│
           └─────────────────────────────┘
```

**How to add nodes:**
- Right-click empty space → "Add Node" → search by name
- Drag from the dot on the right side of one node to the dot on the left side of the next
- For CLIP Text Encode: connect the CLIP output from Load Checkpoint to the CLIP input
- For VAE Decode: connect the VAE output from Load Checkpoint to the VAE input

### 4. Batch Processing (Manual)

1. Paste your prompt into the Positive CLIP Text Encode node
2. Set `batch_size` to 5 in Empty Latent Image
3. Click "Queue Prompt" (or press Ctrl+Enter)
4. Wait for 5 images to generate (~3–15 seconds each on GTX 1060)
5. Review results in `D:\Jake\ComfyUI\output\exile_king_art\`
6. Rename the best one and repeat for the next prompt

### 5. Change Output Folder

In the **Save Image** node:
- Set `filename_prefix` to `exile_king/` — this saves to `D:\Jake\ComfyUI\output\exile_king\`
- Or change the output folder in ComfyUI settings if needed

### 6. After Generation: Importing to Unity

1. Review each batch of 5 images
2. Pick the best 1–2 from each batch
3. Rename to the final name (e.g., `card_flanking_maneuver.png`)
4. Copy into the project: `Assets/Textures/`
5. In Unity, select the imported PNG and set:
   - **Texture Type:** Sprite (2D and UI)
   - **Pixels Per Unit:** 100
   - **Filter Mode:** Point (no filter) — preserves the hand-painted look
   - **Compression:** None

---

## Batch 1: Command Card Art (512×768)

**Settings:** 512×768, steps 4, CFG 3, batch 5

**Style note:** These should read as small tactical diagrams or battle scenes from an illuminated manuscript. NOT character portraits. Think: marginalia in a medieval chronicle showing troop movements.

**Important:** Do NOT prefix with "ONE PERSON ONLY" — these are tactical scenes with multiple figures.

| # | File Prefix | Prompt |
|---|---|---|
| 1 | `card_flanking_maneuver` | `tactical battle diagram in illuminated manuscript style, two bronze age Israelite soldiers attacking an enemy from opposite sides, pincer movement, one holds a bronze short sword and hide-covered shield, the other a bronze-tipped spear, crumpled enemy figure between them, aged parchment background, ink outlines with muted watercolor wash in ochre and faded crimson, hand-painted historical illustration, board game card art, 512x768, family friendly` |
| 2 | `card_forced_march` | `scene in illuminated manuscript style, three bronze age Israelite soldiers running at full speed across rocky ground, dust at their heels, linen tunics and leather vests, wool cloaks billowing behind, spears and shields carried at their sides, expressions of urgency and determination, aged parchment background, ink outlines with muted watercolor wash in ochre and umber, hand-painted historical illustration, board game card art, 512x768, family friendly` |
| 3 | `card_volley` | `scene in illuminated manuscript style, two bronze age archers on a ridge aiming upward at a steep angle, composite bows drawn fully, arrows nocked and ready to release, a third arrow already in flight arcing high above, linen tunics, leather arm bracers, quivers on their backs, aged parchment background, ink outlines with muted watercolor wash in ochre and faded ochre, hand-painted historical illustration, board game card art, 512x768, family friendly` |
| 4 | `card_hold_the_line` | `scene in illuminated manuscript style, three bronze age Israelite soldiers formed in a tight shield wall, round hide-covered shields overlapping, bronze-tipped spears angled outward toward an approaching threat, braced stance, linen tunics, leather chest pieces, grim determined expressions, aged parchment background, ink outlines with muted watercolor wash in umber and faded crimson, hand-painted historical illustration, board game card art, 512x768, family friendly` |
| 5 | `card_rally` | `scene in illuminated manuscript style, a bronze age Israelite commander standing on a rocky outcrop with arm raised rallying his men, several soldiers gathered below looking up at him, a simple cloth banner on a wooden pole held by a soldier beside him, linen tunics, leather armor, wool cloaks, aged parchment background, ink outlines with muted watercolor wash in ochre and brown, hand-painted historical illustration, board game card art, 512x768, family friendly` |
| 6 | `card_coordinated_strike` | `scene in illuminated manuscript style, two bronze age Israelite soldiers converging on a single enemy target from different angles, one lunging with a bronze short sword, the other thrusting a spear, their weapons crossing above the enemy, dust and motion, tactical focus-fire formation, aged parchment background, ink outlines with muted watercolor wash in ochre and faded crimson, hand-painted historical illustration, board game card art, 512x768, family friendly` |
| 7 | `card_desperate_stand` | `scene in illuminated manuscript style, a single bronze age Israelite soldier holding a narrow pass against multiple approaching enemies, shield raised, spear braced, torn cloak, bloodied but defiant, exhausted determined expression, rocky terrain around him, aged parchment background, ink outlines with muted watercolor wash in umber and faded crimson, dramatic composition, hand-painted historical illustration, board game card art, 512x768, family friendly` |
| 8 | `card_reform_ranks` | `scene in illuminated manuscript style, a disordered group of bronze age soldiers regrouping after a retreat, one officer gesturing them back into formation, soldiers turning back toward the battle line, rearranging shields, a broken spear on the ground, tired but regrouping, aged parchment background, ink outlines with muted watercolor wash in ochre and umber, hand-painted historical illustration, board game card art, 512x768, family friendly` |

**Total: 8 prompts × 5 samples = 40 images**

---

## Batch 2: Card Frame Template (512×768)

**Settings:** 512×768, steps 4, CFG 3.5, batch 3

| # | File Prefix | Prompt |
|---|---|---|
| 1 | `card_frame_template` | `blank rectangular playing card, aged parchment background, ornate decorative ink border in dark brown, thin horizontal line dividing the card into top and bottom halves, corner ornaments, medieval manuscript border style, no text, hand-painted board game card, 512x768` |

**Total: 1 prompt × 3 samples = 3 images**

---

## Batch 3: Unit Token Icons (512×512, then downscale)

**Settings:** 512×512, steps 4, CFG 3, batch 5

**Note:** Generating at 512×512 then downscaling to 256×256 in Unity gives better results than generating directly at 256×256.

These are **circular board game tokens** — a single figure in the center of a round medallion, as if stamped onto a clay or wooden token. The figure is a simplified profile silhouette, hand-painted style.

| # | File Prefix | Prompt |
|---|---|---|
| 1 | `token_david` | `circular board game token, hand-painted illustration style, a single bronze age king figure in profile silhouette with a simple crown and a shepherd's staff, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |
| 2 | `token_swordsman` | `circular board game token, hand-painted illustration style, a single bronze age soldier in profile silhouette with a short sword and round shield, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |
| 3 | `token_spearman` | `circular board game token, hand-painted illustration style, a single bronze age soldier in profile silhouette holding a long spear with both hands, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |
| 4 | `token_slinger` | `circular board game token, hand-painted illustration style, a single bronze age soldier in profile silhouette with a leather sling raised overhead, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |
| 5 | `token_archer` | `circular board game token, hand-painted illustration style, a single bronze age archer in profile silhouette drawing a composite bow, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |
| 6 | `token_scout` | `circular board game token, hand-painted illustration style, a single bronze age scout in profile silhouette crouched low in a running pose, light clothing, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |
| 7 | `token_chieftain_amalekite` | `circular board game token, hand-painted illustration style, a single bronze age desert chieftain in profile silhouette with a wrapped headdress and a spear, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |
| 8 | `token_raider_amalekite` | `circular board game token, hand-painted illustration style, a single bronze age desert raider in profile silhouette with a curved sword and small round shield, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |
| 9 | `token_refugee` | `circular board game token, hand-painted illustration style, a single bronze age civilian figure in profile silhouette carrying a bundle on a stick over shoulder, ink stamp style on aged parchment circular medallion, dark ink lines, no background detail, centered composition, family friendly` |

**Total: 9 prompts × 5 samples = 45 images**

---

## Batch 4: Hex Tiles (512×512 tileable)

**Settings:** 512×512, steps 4, CFG 3.5, batch 3

| # | File Prefix | Prompt |
|---|---|---|
| 1 | `hex_sand` | `top-down view of a flat hexagonal tile, sandy desert terrain, warm beige and light brown, subtle parchment-like texture, very fine grain, watercolor wash with soft edges, tileable seamless pattern, board game style, hand-painted texture, no grid lines, 512x512` |
| 2 | `hex_rock` | `top-down view of a flat hexagonal tile, rocky gravel and small stones, gray-brown and warm umber tones, parchment texture overlay, watercolor wash, tileable seamless pattern, board game style, hand-painted texture, no grid lines, 512x512` |
| 3 | `hex_grass` | `top-down view of a flat hexagonal tile, dry savanna grass on hard earth, warm green-brown and ochre tones, dry grass textures, watercolor wash, tileable seamless pattern, board game style, hand-painted texture, no grid lines, 512x512` |

**Total: 3 prompts × 3 samples = 9 images**

---

## Batch 5: UI Elements (512×512, downscale in Unity)

**Settings:** 512×512, steps 4, CFG 3.5, batch 3

Generate at larger size then downscale to target in Unity for better quality.

| # | File Prefix | Prompt | Unity target size |
|---|---|---|---|
| 1 | `ui_endturn_button` | `rounded rectangle button shape, aged warm parchment color, dark ink border outline, flat medieval manuscript style, game UI element, hand-painted texture, isolated on transparent background, family friendly` | 200×60 |
| 2 | `ui_hp_bar_bg` | `thin horizontal bar shape, dark brown ink wash texture, rough hand-painted edges, game UI health bar background, isolated on transparent background, family friendly` | 128×16 |
| 3 | `ui_hp_bar_fill` | `thin horizontal bar shape, faded crimson red ink wash, rough hand-painted edges, game UI health bar fill, isolated on transparent background, family friendly` | 128×16 |
| 4 | `ui_reward_panel` | `large aged parchment panel texture, darker edges, vignette effect, ink border with corner ornaments, rounded rectangle shape, game UI panel, hand-painted texture, isolated on transparent background, family friendly` | 400×300 |

**Total: 4 prompts × 3 samples = 12 images**

---

## Summary: Complete Generation Queue

| Batch | Description | Prompts | Samples Each | Total Images |
|---|---|---|---|---|
| 1 | Command Card Art | 8 | 5 | 40 |
| 2 | Card Frame Template | 1 | 3 | 3 |
| 3 | Unit Token Icons | 9 | 5 | 45 |
| 4 | Hex Tiles | 3 | 3 | 9 |
| 5 | UI Elements | 4 | 3 | 12 |
| **Total** | | **25** | | **109 images** |

Estimated time: **~5–10 minutes total** on GTX 1060 with DreamShaper XL Lightning.

---

## Quick Reference: Final Assets List

After generating and picking the best, these are the final files needed in `Assets/Textures/`:

```
card_flanking_maneuver.png
card_forced_march.png
card_volley.png
card_hold_the_line.png
card_rally.png
card_coordinated_strike.png
card_desperate_stand.png
card_reform_ranks.png
card_frame_template.png

token_david.png
token_swordsman.png
token_spearman.png
token_slinger.png
token_archer.png
token_scout.png
token_chieftain_amalekite.png
token_raider_amalekite.png
token_refugee.png

hex_sand.png
hex_rock.png
hex_grass.png

ui_endturn_button.png
ui_hp_bar_bg.png
ui_hp_bar_fill.png
ui_reward_panel.png
```

Some of the old filenames from PROMPTS.md (like `unit_david.png`, `enemy_raider.png`, `equip_sword.png`, `ui_overwork.png`) are **deprecated** by the Command Card pivot. The new token system replaces unit portraits for MVP.

---

## Tips for Best Results

1. **Do not use "ONE PERSON ONLY" for command card art** — these are tactical scenes with multiple figures
2. **Always paste the full negative prompt** — don't shorten it, it's tuned to prevent specific DreamShaper issues
3. **If images come out too dark** — raise CFG to 3.5 or steps to 6
4. **If images come out too saturated** — lower CFG to 2.5
5. **If images have artifacts/extra limbs** — increase steps to 6, keep CFG at 3
6. **Save seeds of good results** — the seed number appears in the image metadata. Write it down so you can regenerate if needed.
7. **Pick the best, don't settle** — with 5 samples per prompt, you should get at least 1–2 usable ones
8. **Downscale tokens in Unity** — generate at 512×512, set Pixels Per Unit to match the board scale
9. **Check the parchment style** — if DreamShaper drifts toward photorealism, add "watercolor, ink outlines, not realistic, not photograph" to the positive prompt