import numpy as np
import joblib
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report

np.random.seed(42)
N = 3000

def generate_samples(n, profile):
    score_range, savings_range, debt_range, expense_range, age_range = profile
    q_scores = np.random.randint(score_range[0], score_range[1] + 1, size=(n, 10))
    savings_rate = np.random.uniform(*savings_range, size=n)
    debt_to_income = np.random.uniform(*debt_range, size=n)
    expense_ratio = np.random.uniform(*expense_range, size=n)
    age = np.random.randint(*age_range, size=n)
    return q_scores, savings_rate, debt_to_income, expense_ratio, age

conservative_profile = ((1, 2), (0.0, 0.10), (0.30, 0.80), (0.70, 0.95), (50, 75))
balanced_profile     = ((2, 3), (0.10, 0.25), (0.10, 0.30), (0.50, 0.70), (35, 55))
aggressive_profile   = ((3, 4), (0.25, 0.60), (0.00, 0.10), (0.30, 0.55), (22, 40))

def build_dataset(n, profile, label):
    q, s, d, e, a = generate_samples(n, profile)
    X = np.column_stack([q, s, d, e, a])
    y = np.full(n, label)
    return X, y

X_c, y_c = build_dataset(N, conservative_profile, 1)
X_b, y_b = build_dataset(N, balanced_profile, 2)
X_a, y_a = build_dataset(N, aggressive_profile, 3)

X = np.vstack([X_c, X_b, X_a])
y = np.concatenate([y_c, y_b, y_a])

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

model = RandomForestClassifier(n_estimators=200, max_depth=12, random_state=42)
model.fit(X_train, y_train)

print(classification_report(y_test, model.predict(X_test),
      target_names=["Conservative", "Balanced", "Aggressive"]))

joblib.dump(model, "model.pkl")
print("Model kaydedildi: model.pkl")
