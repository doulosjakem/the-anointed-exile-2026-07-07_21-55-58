import json, os, sys, time, base64, urllib.request, re

sys.path.insert(0, r"D:\the-exile-king")
from review_art_ollama import EXPECTED_PROMPTS

OUTPUT_BASE = r"D:\Jake\ComfyUI_windows_portable\ComfyUI\output\ComfyUI\annointed-exile"
EQUIPMENT_DIR = os.path.join(OUTPUT_BASE, "equipment")

target = os.path.join(EQUIPMENT_DIR, "bronze-sword_00009_.png")
if not os.path.exists(target):
    print("bronze-sword_00009_.png not found, checking all bronze-sword files...")
    files = sorted([f for f in os.listdir(EQUIPMENT_DIR) if f.startswith("bronze-sword") and f.endswith(".png")])
    target = os.path.join(EQUIPMENT_DIR, files[-1])
    print(f"Using: {os.path.basename(target)}")

print(f"Reviewing: {os.path.basename(target)}")
print(f"Prompt key: bronze-sword")
print(f"Expected prompt: {EXPECTED_PROMPTS['bronze-sword'][:150]}...")
print()

with open(target, "rb") as f:
    img_b64 = base64.b64encode(f.read()).decode("utf-8")

prompt = f"Rate this image 1-5. Expected prompt: {EXPECTED_PROMPTS['bronze-sword']}"

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
    with urllib.request.urlopen(req, timeout=180) as resp:
        result = json.loads(resp.read().decode("utf-8"))
        response = result.get("response", "").strip()
except Exception as e:
    response = f"ERROR: {e}"

matches = re.findall(r'\b([1-5])\b', response)
rating = matches[0] if matches else "?"

print(f"Rating: {rating}/5")
print(f"Full response:")
print(response)
