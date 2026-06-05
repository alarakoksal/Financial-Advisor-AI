import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Goal {
  id:            string;
  title:         string;
  targetAmount:  number;
  currentAmount: number;
  deadline:      string | null;
  createdAt:     string;
  updatedAt:     string;
}

export interface GoalRequest {
  title:         string;
  targetAmount:  number;
  currentAmount: number;
  deadline:      string | null;
}

const API = 'http://localhost:5110/api';

@Injectable({ providedIn: 'root' })
export class GoalsService {
  constructor(private http: HttpClient) {}

  getAll()                          { return this.http.get<Goal[]>(`${API}/goals`); }
  create(data: GoalRequest)         { return this.http.post<Goal>(`${API}/goals`, data); }
  update(id: string, data: GoalRequest) { return this.http.put<Goal>(`${API}/goals/${id}`, data); }
  delete(id: string)                { return this.http.delete<void>(`${API}/goals/${id}`); }
}
