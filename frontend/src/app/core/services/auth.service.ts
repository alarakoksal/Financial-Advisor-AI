import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

export interface RegisterRequest {
  firstName:   string;
  lastName:    string;
  email:       string;
  password:    string;
  dateOfBirth: string;
  currency:    string;
  preferredLanguage: string;
}

export interface LoginRequest {
  email:    string;
  password: string;
}

export interface AuthResponse {
  token:     string;
  expiresAt: string;
  user: {
    id:        string;
    email:     string;
    firstName: string;
    lastName:  string;
  };
}

const TOKEN_KEY = 'vestly_token';
const API       = 'http://localhost:5110/api';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _token = signal<string | null>(localStorage.getItem(TOKEN_KEY));

  readonly isLoggedIn = computed(() => !!this._token());
  readonly token      = computed(() => this._token());

  constructor(private http: HttpClient, private router: Router) {}

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${API}/auth/register`, data).pipe(
      tap(res => this.saveToken(res.token))
    );
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${API}/auth/login`, data).pipe(
      tap(res => this.saveToken(res.token))
    );
  }

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    this._token.set(null);
    this.router.navigate(['/auth/login']);
  }

  private saveToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
    this._token.set(token);
  }
}
