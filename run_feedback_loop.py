"""
Automated generation + review feedback loop.
Generate small batch → review with Ollama → summarize results → suggest prompt fixes → retry.
"""
import argparse
import json
import os
import sys
import time
import subprocess
import shutil

sys.path.insert(0, r"D:\the-exile-king")
from review_art_ollama import EXPECTED_PROMPTS, classify_asset
from run_comfyui_generation import (
    COMFYUI_DIR, COMFYUI_ROOT, COMFYUI_PYTHON, OUTPUT_BASE, CHECKPOINT,
    build_workflow, submit_workflow, move_outputs, resolve_prompt,
    wait_for_comfyui, wait_for_prompt, start_comfyui, stop_comfyui
)

REVIEW_SCRIPT = r"D:\the-exile-king\review_art_ollama.py"
REVIEW_OUTPUT = os.path.join(OUTPUT_BASE, "_review_batch.json")


def start_comfyui():
    cmd = [COMFYUI_PYTHON, "-s", "ComfyUI\\main.py", "--lowvram", "--windows-standalone-build"]
    proc = subprocess.Popen(cmd, cwd=COMFYUI_ROOT, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
    return proc


def generate_batch(queue_items, base_seed=1000):
    proc = start_comfyui()
    try:
        if not wait_for_comfyui():
            print("ERROR: ComfyUI did not start")
            return False

        for idx, item in enumerate(queue_items):
            item_id = item.get("id", f"item-{idx}")
            count = item.get("count", 1)
            prompt_key = item.get("prompt_key", "")
            prefix = item.get("filename_prefix", "ComfyUI")
            subfolder = item.get("output_subfolder", "")
            width = item.get("width", 512)
            height = item.get("height", 512)
            steps = item.get("steps", 4)
            cfg = item.get("cfg", 3)

            print(f"\n[GEN {idx+1}/{len(queue_items)}] {item_id} ({count}x {prompt_key}) ... ", end="", flush=True)

            for i in range(count):
                seed = base_seed + idx * 100 + i
                batch_item = dict(item)
                batch_item["batch_size"] = 1
                workflow = build_workflow(batch_item, seed)
                prompt_id = submit_workflow(workflow)
                wait_for_prompt(prompt_id)
                moved = move_outputs(batch_item, {})
                if not moved:
                    print(f"WARNING: no files moved for seed {seed}")

            print("done")
        return True
    finally:
        proc.terminate()
        try:
            proc.wait(timeout=10)
        except subprocess.TimeoutExpired:
            proc.kill()


def run_review():
    target_dirs = [
        os.path.join(OUTPUT_BASE, "assets", "tiles"),
        os.path.join(OUTPUT_BASE, "equipment"),
        os.path.join(OUTPUT_BASE, "card"),
        os.path.join(OUTPUT_BASE, "card", "universal"),
        os.path.join(OUTPUT_BASE, "ui-elements"),
        os.path.join(OUTPUT_BASE, "player-units"),
        os.path.join(OUTPUT_BASE, "unit-tokens"),
        os.path.join(OUTPUT_BASE, "standees"),
    ]

    existing_images = []
    for d in target_dirs:
        if os.path.isdir(d):
            for f in sorted(os.listdir(d)):
                if f.lower().endswith((".png", ".jpg", ".jpeg")) and "to_" not in f:
                    existing_images.append(os.path.relpath(os.path.join(d, f), OUTPUT_BASE))

    if not existing_images:
        print("No images found to review")
        return []

    from review_art_ollama import lookup_expected_prompt, parse_answers, decide, get_expected_count
    import urllib.request
    import base64

    results = []
    for i, rel in enumerate(existing_images):
        full = os.path.join(OUTPUT_BASE, rel)
        expected, expected_key = lookup_expected_prompt(rel)
        asset_type = classify_asset(expected_key)

        prompt_parts = []
        if expected:
            prompt_parts.append(f"Expected: {expected}")
        prompt_parts.append("Rate this image for a board game: 1=trash, 2=poor, 3=ok, 4=good, 5=great")
        prompt_text = "\n".join(prompt_parts)

        payload = {
            "model": "minicpm-v:8b",
            "prompt": prompt_text,
            "images": [__import__('base64').b64encode(open(full, "rb").read()).decode("utf-8")],
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

        rating = 3
        if response:
            import re
            matches = re.findall(r'\b([1-5])\b', response)
            if matches:
                rating = int(matches[0])

        results.append({
            "filename": rel,
            "expected_prompt_key": expected_key,
            "asset_type": asset_type,
            "rating": rating,
            "raw_response": response[:200]
        })

        if (i + 1) % 10 == 0:
            print(f"  Reviewed {i+1}/{len(existing_images)} ...")

    report = {
        "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
        "total": len(results),
        "ratings": {},
        "by_type": {},
        "images": results
    }

    for r in results:
        rating = r["rating"]
        report["ratings"][str(rating)] = report["ratings"].get(str(rating), 0) + 1
        atype = r["asset_type"] or "unknown"
        if atype not in report["by_type"]:
            report["by_type"][atype] = {"total": 0, "good": 0, "bad": 0}
        report["by_type"][atype]["total"] += 1
        if rating >= 4:
            report["by_type"][atype]["good"] += 1
        else:
            report["by_type"][atype]["bad"] += 1

    with open(REVIEW_OUTPUT, "w", encoding="utf-8") as f:
        json.dump(report, f, indent=2, ensure_ascii=False)

    return results


def summarize_review(results):
    print("\n=== BATCH REVIEW SUMMARY ===")
    if not results:
        print("No results")
        return

    ratings = {}
    for r in results:
        rating = r["rating"]
        ratings[rating] = ratings.get(rating, 0) + 1

    print(f"Total: {len(results)} images")
    for rating in sorted(ratings.keys()):
        pct = ratings[rating] / len(results) * 100
        print(f"  Rating {rating}: {ratings[rating]} ({pct:.0f}%)")

    by_type = {}
    for r in results:
        atype = r["asset_type"] or "unknown"
        if atype not in by_type:
            by_type[atype] = {"good": 0, "bad": 0, "total": 0}
        by_type[atype]["total"] += 1
        if r["rating"] >= 4:
            by_type[atype]["good"] += 1
        else:
            by_type[atype]["bad"] += 1

    print("\nBy asset type:")
    for atype, stats in sorted(by_type.items()):
        pct = stats["good"] / stats["total"] * 100 if stats["total"] > 0 else 0
        print(f"  {atype:15s}: {stats['good']}/{stats['total']} good ({pct:.0f}%)")

    bad_results = [r for r in results if r["rating"] < 4]
    if bad_results:
        print(f"\n=== NEEDS IMPROVEMENT ({len(bad_results)} images) ===")
        for r in bad_results[:10]:
            print(f"  [{r['asset_type']}] {r['filename']} (rating {r['rating']})")
            if r['raw_response']:
                print(f"    Ollama: {r['raw_response'][:120]}")


def suggest_prompt_fixes(results):
    suggestions = {}
    for r in results:
        if r["rating"] >= 4:
            continue
        key = r["expected_prompt_key"]
        if not key:
            continue
        response = r.get("raw_response", "").lower()
        current = EXPECTED_PROMPTS.get(key, key)

        if any(word in response for word in ["person", "people", "human", "hand", "holding"]):
            suggestions[key] = suggestions.get(key, []) + ["add 'isolated, no person, no hands'"]
        if any(word in response for word in ["parchment", "paper", "background", "texture"]):
            if "equipment" in r.get("asset_type", ""):
                suggestions[key] = suggestions.get(key, []) + ["add 'pure white background, cutout'"]
        if any(word in response for word in ["blurry", "low quality", "deformed", "ugly"]):
            suggestions[key] = suggestions.get(key, []) + ["increase quality keywords, reduce negative"]
        if any(word in response for word in ["modern", "not accurate", "wrong"]):
            suggestions[key] = suggestions.get(key, []) + ["strengthen era-lock language"]

    return suggestions


def apply_suggestions(suggestions):
    applied = []
    for key, fixes in suggestions.items():
        if key not in EXPECTED_PROMPTS:
            continue
        current = EXPECTED_PROMPTS[key]
        updated = current
        for fix in fixes:
            if "isolated, no person, no hands" in fix and "isolated" not in updated.lower():
                updated = updated.rstrip(".") + ", isolated, no person, no hands"
            if "pure white background, cutout" in fix and "pure white background" not in updated.lower():
                updated = updated.rstrip(".") + ", pure white background, clean cutout"
            if "increase quality keywords" in fix:
                updated = updated.rstrip(".") + ", high quality, detailed"
            if "strengthen era-lock" in fix:
                updated = updated.rstrip(".") + ", historically accurate bronze age Levantine"
        if updated != current:
            EXPECTED_PROMPTS[key] = updated
            applied.append((key, updated))
    return applied


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--queue", default=r"D:\the-exile-king\generation_queue.json")
    parser.add_argument("--items", type=int, default=3, help="number of queue items to process per cycle")
    parser.add_argument("--start-index", type=int, default=0, help="start index in queue")
    parser.add_argument("--base-seed", type=int, default=1000)
    parser.add_argument("--auto-retry", action="store_true", help="auto-retry bad batches with suggested fixes")
    parser.add_argument("--max-retries", type=int, default=2, help="max retry attempts per batch")
    args = parser.parse_args()

    with open(args.queue, "r", encoding="utf-8") as f:
        queue = json.load(f)

    cycle = 0
    idx = args.start_index
    while idx < len(queue):
        batch = queue[idx:idx + args.items]
        batch_keys = [item.get("prompt_key") for item in batch]
        print(f"\n{'='*60}")
        print(f"CYCLE {cycle+1}: items {idx+1}-{min(idx+args.items, len(queue))} of {len(queue)}")
        print(f"Keys: {batch_keys}")
        print(f"{'='*60}")

        print("\n--- GENERATION ---")
        if not generate_batch(batch, base_seed=args.base_seed + cycle * 1000):
            print("Generation failed, stopping")
            break

        print("\n--- REVIEW ---")
        results = run_review()

        summarize_review(results)

        suggestions = suggest_prompt_fixes(results)
        if suggestions:
            print("\n=== PROMPT FIX SUGGESTIONS ===")
            for key, fixes in suggestions.items():
                print(f"  {key}:")
                for fix in fixes:
                    print(f"    - {fix}")
            print("Raw response samples:")
            for r in results:
                if r["rating"] < 4 and r.get("raw_response"):
                    print(f"  {r['filename']}: {r['raw_response'][:150]}")

            if args.auto_retry:
                print("\n--- APPLYING SUGGESTIONS AND RETRYING ---")
                applied = apply_suggestions(suggestions)
                for key, new_prompt in applied:
                    print(f"  Updated prompt: {key}")
                
                retry_count = 0
                while retry_count < args.max_retries:
                    retry_count += 1
                    print(f"\nRetry attempt {retry_count}/{args.max_retries}...")
                    
                    bad_keys = list(suggestions.keys())
                    retry_items = [item for item in batch if item.get("prompt_key") in bad_keys]
                    
                    if not retry_items:
                        break
                    
                    if not generate_batch(retry_items, base_seed=args.base_seed + cycle * 1000 + retry_count * 500):
                        print("Retry generation failed")
                        break
                    
                    retry_results = run_review()
                    
                    retry_suggestions = suggest_prompt_fixes(retry_results)
                    if retry_suggestions:
                        suggestions = retry_suggestions
                        applied = apply_suggestions(suggestions)
                        for key, new_prompt in applied:
                            print(f"  Updated prompt on retry: {key}")
                    else:
                        print("Retry successful - all images rated 4-5!")
                        suggestions = {}
                        break
                
                if suggestions and retry_count >= args.max_retries:
                    print(f"\nMax retries reached for batch. Moving to next batch.")
                    print("You may want to manually adjust prompts for these keys:")
                    for key, fixes in suggestions.items():
                        print(f"  {key}: {fixes}")
        else:
            print("\nAll images rated 4-5, no fixes needed!")

        idx += len(batch)
        cycle += 1

    print(f"\n{'='*60}")
    print(f"QUEUE COMPLETE")
    print(f"{'='*60}")
    print(f"Processed {idx} of {len(queue)} items")
    print(f"Review report: {REVIEW_OUTPUT}")


if __name__ == "__main__":
    sys.exit(main())
