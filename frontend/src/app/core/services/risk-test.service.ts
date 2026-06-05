import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface RiskOption {
  id:           string;
  optionTextTr: string;
  optionTextEn: string;
  score:        number;
}

export interface RiskQuestion {
  id:             string;
  orderIndex:     number;
  questionTextTr: string;
  questionTextEn: string;
  options:        RiskOption[];
}

export interface RiskTestResult {
  id:          string;
  totalScore:  number;
  riskLevel:   string;
  mlRiskLevel: string | null;
  completedAt: string;
}

export interface SubmitRequest {
  answers: { questionId: string; optionId: string }[];
}

const API = 'http://localhost:5110/api';

@Injectable({ providedIn: 'root' })
export class RiskTestService {
  constructor(private http: HttpClient) {}

  getQuestions()  { return this.http.get<RiskQuestion[]>(`${API}/risk-test/questions`); }
  getLatest()     { return this.http.get<RiskTestResult>(`${API}/risk-test/result/latest`); }
  submit(data: SubmitRequest) { return this.http.post<RiskTestResult>(`${API}/risk-test/submit`, data); }
}
