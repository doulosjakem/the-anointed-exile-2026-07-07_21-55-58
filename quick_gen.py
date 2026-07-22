import subprocess, time, json, os, shutil, urllib.request

COMFYUI_DIR = r"D:\Jake\ComfyUI_windows_portable\ComfyUI"
COMFYUI_ROOT = r"D:\Jake\ComfyUI_windows_portable"
COMFYUI_PYTHON = r"D:\Jake\ComfyUI_windows_portable\python_embeded\python.exe"
OUTPUT_BASE = os.path.join(COMFYUI_DIR, r"output\ComfyUI\annointed-exile")
BASE_URL = "http://127.0.0.1:8188"

with open(r"D:\the-exile-king\quick_batch.json", "r", encoding="utf-8") as f:
    queue = json.load(f)

UNIVERSAL_NEGATIVE = "photorealistic, hyperrealistic, realistic skin texture, photograph, 3d render, modern clothing, plate armor, steel armor, chainmail, fantasy armor, longbow, long sword, greatsword, crossguard, medieval helmet, horned helmet, knight, crusader, anime, manga, cartoon, text, logo, ugly, deformed, blurry, low quality, person, people, human, hands, fingers, body, figure, face, background, scenery, aged parchment, board game card art"
POSITIVE_SUFFIX = ", isolated single object centered on pure white background, clean cutout, hand-painted historical illustration, watercolor and ink, no background, no person, no hands, family friendly, NOT medieval, NOT fantasy, NOT European"

EXPECTED_PROMPTS = {}
sys_path_added = False

def ensure_sys_path():
    global sys_path_added
    if not sys_path_added:
        import sys
        sys.path.insert(0, r"D:\the-exile-king")
        sys_path_added = True

def load_prompts():
    ensure_sys_path()
    from review_art_ollama import EXPECTED_PROMPTS as EP
    global EXPECTED_PROMPTS
    EXPECTED_PROMPTS = EP

def resolve_prompt(prompt_key):
    if prompt_key in EXPECTED_PROMPTS:
        return EXPECTED_PROMPTS[prompt_key] + POSITIVE_SUFFIX
    return prompt_key.replace("-", " ") + POSITIVE_SUFFIX

def build_workflow(item, seed):
    positive = resolve_prompt(item["prompt_key"])
    negative = UNIVERSAL_NEGATIVE
    width = item.get("width", 512)
    height = item.get("height", 512)
    prefix = item.get("filename_prefix", "ComfyUI")
    
    return {
        "3": {"class_type": "KSampler", "inputs": {"seed": seed, "steps": item.get("steps", 4), "cfg": item.get("cfg", 3), "sampler_name": "dpmpp_sde", "scheduler": "karras", "model": ["4", 0], "positive": ["6", 0], "negative": ["7", 0], "latent_image": ["5", 0], "denoise": 1}},
        "4": {"class_type": "CheckpointLoaderSimple", "inputs": {"ckpt_name": "dreamshaperXL_sfwLightningDPMSDE.safetensors"}},
        "5": {"class_type": "EmptyLatentImage", "inputs": {"width": width, "height": height, "batch_size": 1}},
        "6": {"class_type": "CLIPTextEncode", "inputs": {"text": positive, "clip": ["4", 1]}},
        "7": {"class_type": "CLIPTextEncode", "inputs": {"text": negative, "clip": ["4", 1]}},
        "8": {"class_type": "VAEDecode", "inputs": {"samples": ["3", 0], "vae": ["4", 2]}},
        "9": {"class_type": "SaveImage", "inputs": {"filename_prefix": prefix, "images": ["8", 0]}}
    }

def submit_workflow(workflow):
    payload = {"prompt": workflow}
    data = json.dumps(payload).encode("utf-8")
    req = urllib.request.Request(f"{BASE_URL}/prompt", data=data, headers={"Content-Type": "application/json"}, method="POST")
    with urllib.request.urlopen(req, timeout=30) as resp:
        return json.loads(resp.read().decode("utf-8")).get("prompt_id")

def wait_for_queue_empty(timeout=300):
    start = time.time()
    last_running = None
    while time.time() - start < timeout:
        try:
            req = urllib.request.Request(f"{BASE_URL}/queue", method="GET")
            with urllib.request.urlopen(req, timeout=10) as resp:
                q = json.loads(resp.read().decode("utf-8"))
                running = q.get("queue_running", [])
                pending = q.get("queue_pending", [])
                
                if running != last_running:
                    last_running = running
                
                if not running and not pending:
                    return True
        except Exception:
            pass
        time.sleep(3)
    return False

def move_outputs(item):
    prefix = item.get("filename_prefix", "ComfyUI")
    subfolder = item.get("output_subfolder", "")
    dest_dir = os.path.join(OUTPUT_BASE, subfolder)
    os.makedirs(dest_dir, exist_ok=True)
    
    comfy_output = os.path.join(COMFYUI_DIR, "output")
    files = sorted([f for f in os.listdir(comfy_output) if f.startswith(prefix)])
    moved = []
    for f in files:
        src = os.path.join(comfy_output, f)
        dest = os.path.join(dest_dir, f)
        if not os.path.exists(dest):
            shutil.move(src, dest)
            moved.append(os.path.relpath(dest, OUTPUT_BASE))
    return moved

def main():
    load_prompts()
    
    print("=== QUICK BATCH GENERATION ===")
    print(f"Items: {len(queue)}")
    print(f"Output: {OUTPUT_BASE}")
    print()
    
    for idx, item in enumerate(queue):
        item_id = item.get("id", f"item-{idx}")
        count = item.get("count", 1)
        prefix = item.get("filename_prefix", "ComfyUI")
        
        print(f"[{idx+1}/{len(queue)}] {item_id} ({count} images) ... ", end="", flush=True)
        
        # Generate existing files count
        dest_dir = os.path.join(OUTPUT_BASE, item.get("output_subfolder", ""))
        existing = len([f for f in os.listdir(dest_dir) if f.startswith(prefix)]) if os.path.isdir(dest_dir) else 0
        
        for i in range(count):
            seed = 1000 + idx * 100 + i
            workflow = build_workflow(item, seed)
            prompt_id = submit_workflow(workflow)
            
        if wait_for_queue_empty():
            moved = move_outputs(item)
            print(f"done ({len(moved)} files, {existing} already existed)")
        else:
            print("TIMEOUT waiting for generation")
    
    print("\n=== COMPLETE ===")

if __name__ == "__main__":
    import sys
    main()
