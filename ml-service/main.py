import joblib
import numpy as np
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field

app = FastAPI(title="Risk Level ML Service")

try:
    model = joblib.load("model.pkl")
except FileNotFoundError:
    raise RuntimeError("model.pkl bulunamadı. Önce 'python train.py' çalıştırın.")

LABEL_MAP = {1: "Conservative", 2: "Balanced", 3: "Aggressive"}


class PredictRequest(BaseModel):
    question_scores: list[int] = Field(..., min_length=10, max_length=10)
    savings_rate: float = Field(..., ge=0.0)
    debt_to_income: float = Field(..., ge=0.0)
    expense_ratio: float = Field(..., ge=0.0)
    age: int = Field(..., ge=18, le=100)


class PredictResponse(BaseModel):
    risk_level: str


@app.post("/predict", response_model=PredictResponse)
def predict(request: PredictRequest):
    if any(s < 1 or s > 4 for s in request.question_scores):
        raise HTTPException(status_code=400, detail="Soru skorları 1-4 arasında olmalı.")

    features = np.array([
        *request.question_scores,
        request.savings_rate,
        request.debt_to_income,
        request.expense_ratio,
        request.age
    ]).reshape(1, -1)

    prediction = int(model.predict(features)[0])
    return PredictResponse(risk_level=LABEL_MAP[prediction])


@app.get("/health")
def health():
    return {"status": "ok"}
