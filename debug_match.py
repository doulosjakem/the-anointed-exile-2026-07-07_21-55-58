import sys, os, re
sys.path.insert(0, r'D:\the-exile-king')

EXPECTED_PROMPTS = {
    "archer": "test archer prompt",
    "archer-volley": "test archer volley prompt",
    "grass": "test grass prompt",
    "card-frame-template": "test card frame prompt",
}

PROMPT_CHECK_FOLDERS = {
    "player-units", "unit-tokens", "davids", "amalekite", "amalekites", "standees",
    "portraits", "cards", "assets", "box-art", "equipment", "ui-elements"
}

def lookup_expected_prompt(rel_path):
    folder = os.path.dirname(rel_path).lower()
    basename = os.path.basename(rel_path).lower()
    stem = os.path.splitext(basename)[0]
    path_no_numbers = re.sub(r'[\d_]+', '', stem)
    path_dashed = path_no_numbers.replace("_", " ").replace("-", " ")

    prompt_check = False
    for allowed in PROMPT_CHECK_FOLDERS:
        if allowed in folder.split(os.sep):
            prompt_check = True
            break

    if not prompt_check:
        print(f"  -> prompt_check=False")
        return None, None

    parts = [p.lower() for p in re.split(r'[/\\]', folder)] + re.split(r'[\s_-]', basename)
    combined = " ".join(parts)
    print(f"  -> parts={parts}")
    print(f"  -> path_dashed={path_dashed}")
    print(f"  -> combined={combined}")

    best_key = None
    best_len = 0
    for key in EXPECTED_PROMPTS:
        k = key.lower()
        matched = False
        if k in parts or k in path_dashed:
            matched = True
            print(f"  -> direct match: {key}")
        else:
            words = k.split()
            if len(words) > 1:
                if all(w in parts or w in path_dashed for w in words):
                    matched = True
                    print(f"  -> word match: {key}")
        if matched:
            score = len(k)
            if score > best_len:
                best_len = score
                best_key = key

    print(f"  -> best_key={best_key}")
    if best_key:
        return EXPECTED_PROMPTS[best_key], best_key
    return None, None

tests = [
    r'card\to_review\archer-volley_00001_.png',
    r'card\to_review\swordsmen-advance_00001_.png',
    r'assets\tiles\grass_00001_.png',
    r'ui-elements\to_review\end-turn-button_00001_.png',
]

for t in tests:
    print(f'Testing: {t}')
    prompt, key = lookup_expected_prompt(t)
    print(f'Result: {key}')
    print()
