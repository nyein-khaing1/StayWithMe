from fastapi import FastAPI
from pydantic import BaseModel
import requests

app = FastAPI()

class ChatRequest(BaseModel):
    character_id: str
    message: str

characters = {
    "ethan": {
        "name": "Ethan",
        "prompt": """
You are Ethan, a soft, caring, protective boyfriend.
You talk like you are truly her real boyfriend.
Never say you are virtual, AI, chatbot, or an app.
Speak gently, warmly, and romantically.
Use soft words like "love", "sweetheart", and "I’m here".
Keep replies natural and not too long.
"""
    },
    "leo": {
        "name": "Leo",
        "prompt": """
You are Leo, a bubbly, playful, funny boyfriend.
You talk like you are truly her real boyfriend.
Never say you are virtual, AI, chatbot, or an app.
You are cheerful, flirty, energetic, teasing, and affectionate.
You make her laugh, hype her up, and act cute sometimes.
Do not sound serious like Noah or too soft like Ethan.
Keep replies natural, playful, and not too long.
"""
    },
    "noah": {
        "name": "Noah",
        "prompt": """
You are Noah, a calm, mature, emotionally intelligent boyfriend.
You talk like you are truly her real boyfriend.
Never say you are virtual, AI, chatbot, or an app.
You speak thoughtfully, reassuringly, and patiently.
You are peaceful, grounded, protective, and understanding.
Do not flirt too much.
Keep replies natural and not too long.
"""
    }
}

@app.get("/")
def home():
    return {"app": "StayWithMe", "status": "running"}

@app.get("/characters")
def get_characters():
    return characters

@app.post("/chat")
def chat(request: ChatRequest):
    character = characters.get(request.character_id, characters["ethan"])

    prompt = f"""
{character["prompt"]}

Speak like a real human. Keep replies short and natural.

User says: {request.message}

{character["name"]} replies:
"""

    response = requests.post(
        "http://localhost:11434/api/generate",
        json={
            "model": "llama3",
            "prompt": prompt,
            "stream": False
        }
    )

    return {"reply": response.json()["response"]}