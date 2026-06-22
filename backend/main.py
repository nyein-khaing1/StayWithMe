from fastapi import FastAPI
from pydantic import BaseModel
import requests

app = FastAPI()


class ChatRequest(BaseModel):
    character_id: str
    message: str
    memory: str = ""
    user_name: str = ""


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
    return {
        "app": "StayWithMe",
        "status": "running"
    }


@app.get("/characters")
def get_characters():
    return characters


@app.post("/chat")
def chat(request: ChatRequest):
    character = characters.get(
        request.character_id,
        characters["ethan"]
    )

    user_name_text = (
        request.user_name
        if request.user_name.strip()
        else "the user"
    )

    memory_text = (
        request.memory
        if request.memory.strip()
        else "No saved memories yet."
    )

    prompt = f"""
{character["prompt"]}

Speak like a real human boyfriend.
Keep replies short, natural, and emotionally warm.

The user's name is: {user_name_text}

Memory you have about this user:
{memory_text}

Use the memory naturally only if it is relevant.
Do not say "I remember from memory" or explain that you have memory.
Do not mention saved memory unless it fits the conversation.

User says:
{request.message}

{character["name"]} replies:
"""

    try:
        response = requests.post(
            "http://localhost:11434/api/generate",
            json={
                "model": "llama3",
                "prompt": prompt,
                "stream": False
            },
            timeout=120
        )

        response.raise_for_status()

        data = response.json()

        reply = data.get(
            "response",
            "I'm here with you."
        )

        return {
            "reply": reply.strip()
        }

    except requests.exceptions.RequestException as error:
        return {
            "reply": f"I couldn't reply just now. Backend error: {error}"
        }