import json
import urllib.request
import time

workflow = {
    "3": {
        "class_type": "KSampler",
        "inputs": {
            "seed": 12345,
            "steps": 4,
            "cfg": 3,
            "sampler_name": "dpmpp_sde",
            "scheduler": "karras",
            "model": ["4", 0],
            "positive": ["6", 0],
            "negative": ["7", 0],
            "latent_image": ["5", 0],
            "denoise": 1
        }
    },
    "4": {
        "class_type": "CheckpointLoaderSimple",
        "inputs": {
            "ckpt_name": "dreamshaperXL_sfwLightningDPMSDE.safetensors"
        }
    },
    "5": {
        "class_type": "EmptyLatentImage",
        "inputs": {
            "width": 512,
            "height": 512,
            "batch_size": 1
        }
    },
    "6": {
        "class_type": "CLIPTextEncode",
        "inputs": {
            "text": "test",
            "clip": ["4", 1]
        }
    },
    "7": {
        "class_type": "CLIPTextEncode",
        "inputs": {
            "text": "test",
            "clip": ["4", 1]
        }
    },
    "8": {
        "class_type": "VAEDecode",
        "inputs": {
            "samples": ["3", 0],
            "vae": ["4", 2]
        }
    },
    "9": {
        "class_type": "SaveImage",
        "inputs": {
            "filename_prefix": "test_gen",
            "images": ["8", 0]
        }
    }
}

payload = {"prompt": workflow}
data = json.dumps(payload).encode("utf-8")
req = urllib.request.Request(
    "http://127.0.0.1:8188/prompt",
    data=data,
    headers={"Content-Type": "application/json"},
    method="POST"
)

try:
    with urllib.request.urlopen(req, timeout=30) as resp:
        result = json.loads(resp.read().decode("utf-8"))
        print(f"Submission response: {json.dumps(result, indent=2)}")
        prompt_id = result.get("prompt_id")
        print(f"Prompt ID: {prompt_id}")
        
        # Poll queue and history
        for i in range(20):
            time.sleep(5)
            try:
                q = urllib.request.urlopen("http://127.0.0.1:8188/queue", timeout=5)
                queue = json.loads(q.read().decode("utf-8"))
                print(f"[{i*5}s] Queue running: {len(queue.get('queue_running', []))}, pending: {len(queue.get('queue_pending', []))}")
                
                h = urllib.request.urlopen(f"http://127.0.0.1:8188/history/{prompt_id}", timeout=5)
                history = json.loads(h.read().decode("utf-8"))
                print(f"[{i*5}s] History keys: {list(history.keys())}")
                if prompt_id in history:
                    entry = history[prompt_id]
                    print(f"[{i*5}s] Entry: {json.dumps(entry, indent=2)[:500]}")
            except Exception as e:
                print(f"[{i*5}s] Poll error: {e}")
except Exception as e:
    print(f"Submission error: {e}")
