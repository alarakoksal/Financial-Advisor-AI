import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Debt {
  id:               string;
  name:             string;
  type:             string;
  totalAmount:      number;
  remainingAmount:  number;
  interestRate:     number;
  monthlyPayment:   number;
  startDate:        string;
  endDate:          string | null;
  remainingMonths:  number;
  totalInterestCost:number;
  paidPercent:      number;
}

export interface DebtRequest {
  name:            string;
  type:            string;
  totalAmount:     number;
  remainingAmount: number;
  interestRate:    number;
  monthlyPayment:  number;
  startDate:       string;
  endDate:         string | null;
}

export interface ScoreHistoryItem {
  score:      number;
  recordedAt: string;
}

const API = 'http://localhost:5110/api';

@Injectable({ providedIn: 'root' })
export class DebtService {
  constructor(private http: HttpClient) {}

  getAll()                    { return this.http.get<Debt[]>(`${API}/debts`); }
  create(d: DebtRequest)      { return this.http.post<Debt>(`${API}/debts`, d); }
  update(id: string, d: DebtRequest) { return this.http.put<Debt>(`${API}/debts/${id}`, d); }
  delete(id: string)          { return this.http.delete(`${API}/debts/${id}`); }

  getScoreHistory()           { return this.http.get<ScoreHistoryItem[]>(`${API}/score-history`); }
}
