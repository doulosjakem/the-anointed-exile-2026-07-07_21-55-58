import json, os, sys, time, base64, urllib.request, re

sys.path.insert(0, r"D:\the-exile-king")
from review_art_ollama import EXPECTED_PROMPTS

OUTPUT_BASE = r"D:\Jake\ComfyUI_windows_portable\ComfyUI\output\ComfyUI\annointed-exile"
EQUIPMENT_DIR = os.path.join(OUTPUT_BASE, "equipment")

files = sorted([f for f in os.listdir(EQUIPMENT_DIR) if f.endswith(".png")])
newest = files[-3:] if len(files) >= 3 else files

print(f"=== EQUIPMENT REVIEW ({len(newest)} newest images) ===\n")

for fname in newest:
    fpath = os.path.join(EQUIPMENT_DIR, fname)
    prompt_key = fname.split("_")[0].replace("-", "_")
    
    expected = EXPECTED_PROMPTS.get(prompt_key, "N/A")
    
    prompt = f"Rate this image 1-5. Expected: {expected}"
    
    with open(fpath, "rb") as f:
        img_b64 = base64.b64encode(f.read()).decode("utf-8")
    
    payload = {
        "model": "minicpm-v:8b",
        "prompt": prompt,
        "images": [img_b64],
        "stream": False,
        "options": {"temperature": 0.1, "num_ctx": 2048}
    }
    
    try:
        req = urllib.request.Request(
            "http://localhost:11434/api/generate",
            data=json.dumps(payload).encode("utf-8"),
            headers={"Content-Type": "application/json"},
            method="POST"
        )
        with urllib.request.urlopen(req, timeout=120) as resp:
            response = json.loads(resp.read().decode("utf-8")).get("response", "").strip()
    except Exception as e:
        response = f"ERROR: {e}"
    
    matches = re.findall(r'\b([1-5])\b', response)
    rating = matches[0] if matches else "?"
    
    print(f"File: {fname}")
    print(f"Prompt key: {prompt_key}")
    print(f"Rating: {rating}/5")
    print(f"Ollama: {response[:200]}")
    print()
