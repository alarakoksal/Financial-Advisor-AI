import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface UserProfile {
  firstName:         string;
  lastName:          string;
  email:             string;
  dateOfBirth:       string;
  preferredLanguage: string;
}

export interface UpdateProfileRequest {
  firstName:         string;
  lastName:          string;
  email:             string;
  dateOfBirth:       string;
  preferredLanguage: string;
  currentPassword?:  string | null;
  newPassword?:      string | null;
}

const API = 'http://localhost:5110/api';

@Injectable({ providedIn: 'root' })
export class SettingsService {
  constructor(private http: HttpClient) {}

  getProfile()                         { return this.http.get<UserProfile>(`${API}/users/me`); }
  updateProfile(data: UpdateProfileRequest) { return this.http.put<UserProfile>(`${API}/users/me`, data); }
}
