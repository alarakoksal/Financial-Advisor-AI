import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface AdviceHistoryItem {
  id:        string;
  content:   string;
  createdAt: string;
}

const API = 'http://localhost:5110/api';

@Injectable({ providedIn: 'root' })
export class AdviceService {
  constructor(private http: HttpClient) {}

  generate()   { return this.http.get<{ advice: string }>(`${API}/advice`); }
  getHistory() { return this.http.get<AdviceHistoryItem[]>(`${API}/advice/history`); }
}
