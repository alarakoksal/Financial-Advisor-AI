import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DashboardSummary {
  fullName:          string;
  riskLevel:         string;
  monthlyIncome:     number;
  monthlyExpenses:   number;
  savingRate:        number;
  debtToIncomeRatio: number;
  expenseRatio:      number;
  goalCount:         number;
  nearestGoal:       { title: string; targetAmount: number; currentAmount: number; deadline: string } | null;
  notificationCount: number;
  latestAdvice:      string | null;
}

export interface FinancialHealth {
  score:            number;
  savingRateScore:  number;
  debtScore:        number;
  cashFlowScore:    number;
  emergencyScore:   number;
}

export interface Notification {
  type:    string;
  message: string;
  value:   number;
}

const API = 'http://localhost:5110/api';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private http: HttpClient) {}

  getSummary()         { return this.http.get<DashboardSummary>(`${API}/dashboard/summary`); }
  getFinancialHealth() { return this.http.get<FinancialHealth>(`${API}/analytics/financial-health`); }
  getNotifications()   { return this.http.get<Notification[]>(`${API}/notifications`); }
}
