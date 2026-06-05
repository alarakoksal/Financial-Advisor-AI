# Financial Advisor AI

An AI-powered personal finance management system built as a graduation project. Combines machine learning, large language models, and financial simulation algorithms to provide personalized financial guidance.

## Features

- **Financial Health Score** — Automatically calculated daily score based on income, expenses, debt, and savings rate
- **AI Financial Advice** — GPT-3.5 powered personalized financial recommendations
- **ML Risk Assessment** — Machine learning model that determines investment risk profile from user questionnaire responses
- **Debt Tracker** — Full CRUD for managing debts with payoff date and interest cost projections
- **Debt Strategy Comparator** — Snowball vs Avalanche method simulation with interactive charts
- **Monte Carlo Simulation** — 1,000-trial portfolio projection with percentile bands (P10/P25/P50/P75/P90)
- **Retirement Planner** — Accumulation, early retirement (perpetuity model), and drawdown simulation with inflation adjustment
- **Score History** — Tracks financial health score over time with trend visualization
- **Dark / Dim theme toggle** — Persistent theme preference via localStorage

## Tech Stack

**Frontend**
- Angular 21 (standalone, zoneless)
- ApexCharts (ng-apexcharts) for data visualization
- SCSS with custom design system

**Backend**
- ASP.NET Core 8 Web API
- CQRS pattern with MediatR
- Entity Framework Core + SQL Server
- JWT authentication

**ML Service**
- Python / FastAPI
- scikit-learn (Random Forest classifier)
- Trained on synthetic financial risk profile data

**AI Integration**
- OpenAI GPT-3.5 Turbo for financial advice generation

## Architecture

```
├── frontend/        # Angular SPA
├── backend/         # .NET 8 REST API
│   ├── API          # Controllers, middleware
│   ├── Application  # CQRS handlers, DTOs
│   ├── Domain       # Entities
│   └── Infrastructure # EF Core, external services
└── ml-service/      # FastAPI + scikit-learn
```

## Getting Started

### Prerequisites
- Node.js 18+
- .NET 8 SDK
- Python 3.11+
- SQL Server

### Backend
```bash
cd backend/src/BitirmeProjem.API
cp appsettings.Example.json appsettings.json
# Fill in your DB connection string, OpenAI API key, and JWT secret
dotnet run
```

### ML Service
```bash
cd ml-service
python -m venv .venv && source .venv/bin/activate
pip install -r requirements.txt
python train.py   # trains and saves model.pkl
uvicorn main:app --port 8001
```

### Frontend
```bash
cd frontend
npm install
ng serve
```

App runs at `http://localhost:4200`
