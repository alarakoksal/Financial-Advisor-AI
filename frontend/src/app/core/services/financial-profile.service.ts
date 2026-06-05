import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface FinancialProfileDto {
  monthlyIncome:   number;
  monthlyExpenses: number;
  rentExpense:     number;
  rentIncome:      number;
  savingsAmount:   number;
  debtAmount:      number;
  currency:        string;
}

const API = 'http://localhost:5110/api';

@Injectable({ providedIn: 'root' })
export class FinancialProfileService {
  constructor(private http: HttpClient) {}

  get()                              { return this.http.get<FinancialProfileDto>(`${API}/financial-profile`); }
  create(data: FinancialProfileDto)  { return this.http.post<FinancialProfileDto>(`${API}/financial-profile`, data); }
  update(data: FinancialProfileDto)  { return this.http.put<FinancialProfileDto>(`${API}/financial-profile`, data); }
}
