import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { SidebarComponent } from '../dashboard/components/sidebar/sidebar';
import { SettingsService, UpdateProfileRequest } from '../../core/services/settings.service';
import { AuthService } from '../../core/services/auth.service';
import { ThemeService } from '../../core/services/theme.service';

type Section = 'profile' | 'security' | 'preferences' | 'danger';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  templateUrl: './settings.html',
  styleUrl: './settings.scss',
})
export class SettingsComponent implements OnInit {
  activeSection: Section = 'profile';

  loading  = true;
  saving   = false;
  success: string | null = null;
  error:   string | null = null;

  // Profile form
  firstName   = '';
  lastName    = '';
  email       = '';
  dateOfBirth = '';
  preferredLanguage = 'tr';

  // Security form
  currentPassword = '';
  newPassword     = '';
  confirmPassword = '';
  showCurrent = false;
  showNew     = false;
  showConfirm = false;

  // Danger zone
  deleteConfirmText = '';
  deletingAccount   = false;

  readonly sections: { id: Section; label: string; icon: string }[] = [
    { id: 'profile',     label: 'Profil Bilgileri',  icon: 'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z' },
    { id: 'security',    label: 'Güvenlik',           icon: 'M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z' },
    { id: 'preferences', label: 'Tercihler',          icon: 'M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z M15 12a3 3 0 11-6 0 3 3 0 016 0z' },
    { id: 'danger',      label: 'Tehlike Bölgesi',   icon: 'M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z' },
  ];

  constructor(
    private svc:    SettingsService,
    public auth:    AuthService,
    public theme:   ThemeService,
    private router: Router,
    private cdr:    ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.svc.getProfile().subscribe({
      next: (p) => {
        this.firstName         = p.firstName;
        this.lastName          = p.lastName;
        this.email             = p.email;
        this.dateOfBirth       = p.dateOfBirth;
        this.preferredLanguage = p.preferredLanguage;
        this.loading           = false;
        this.cdr.detectChanges();
      },
      error: () => { this.loading = false; this.cdr.detectChanges(); },
    });
  }

  setSection(s: Section) {
    this.activeSection = s;
    this.success = null;
    this.error   = null;
    this.cdr.detectChanges();
  }

  saveProfile() {
    this.saving  = true;
    this.success = null;
    this.error   = null;
    this.cdr.detectChanges();

    const req: UpdateProfileRequest = {
      firstName:         this.firstName,
      lastName:          this.lastName,
      email:             this.email,
      dateOfBirth:       this.dateOfBirth,
      preferredLanguage: this.preferredLanguage,
    };

    this.svc.updateProfile(req).subscribe({
      next: () => {
        this.saving  = false;
        this.success = 'Profil bilgileri başarıyla güncellendi.';
        this.cdr.detectChanges();
      },
      error: (e) => {
        this.saving = false;
        const msg = e?.error?.message ?? '';
        this.error = msg === 'EMAIL_ALREADY_EXISTS'
          ? 'Bu e-posta adresi zaten kullanılıyor.'
          : 'Profil güncellenirken bir hata oluştu.';
        this.cdr.detectChanges();
      },
    });
  }

  savePassword() {
    if (!this.newPassword || !this.currentPassword) {
      this.error = 'Lütfen tüm alanları doldurun.';
      this.cdr.detectChanges();
      return;
    }
    if (this.newPassword !== this.confirmPassword) {
      this.error = 'Yeni şifreler eşleşmiyor.';
      this.cdr.detectChanges();
      return;
    }
    if (this.newPassword.length < 6) {
      this.error = 'Yeni şifre en az 6 karakter olmalıdır.';
      this.cdr.detectChanges();
      return;
    }

    this.saving  = true;
    this.success = null;
    this.error   = null;
    this.cdr.detectChanges();

    const req: UpdateProfileRequest = {
      firstName:         this.firstName,
      lastName:          this.lastName,
      email:             this.email,
      dateOfBirth:       this.dateOfBirth,
      preferredLanguage: this.preferredLanguage,
      currentPassword:   this.currentPassword,
      newPassword:       this.newPassword,
    };

    this.svc.updateProfile(req).subscribe({
      next: () => {
        this.saving          = false;
        this.success         = 'Şifreniz başarıyla güncellendi.';
        this.currentPassword = '';
        this.newPassword     = '';
        this.confirmPassword = '';
        this.cdr.detectChanges();
      },
      error: (e) => {
        this.saving = false;
        const msg = e?.error?.message ?? '';
        this.error = msg === 'INVALID_CURRENT_PASSWORD'
          ? 'Mevcut şifreniz hatalı.'
          : 'Şifre güncellenirken bir hata oluştu.';
        this.cdr.detectChanges();
      },
    });
  }

  savePreferences() {
    this.saving  = true;
    this.success = null;
    this.error   = null;
    this.cdr.detectChanges();

    const req: UpdateProfileRequest = {
      firstName:         this.firstName,
      lastName:          this.lastName,
      email:             this.email,
      dateOfBirth:       this.dateOfBirth,
      preferredLanguage: this.preferredLanguage,
    };

    this.svc.updateProfile(req).subscribe({
      next: () => {
        this.saving  = false;
        this.success = 'Tercihleriniz kaydedildi.';
        this.cdr.detectChanges();
      },
      error: () => {
        this.saving = false;
        this.error  = 'Tercihler kaydedilirken bir hata oluştu.';
        this.cdr.detectChanges();
      },
    });
  }

  get canDeleteAccount(): boolean {
    return this.deleteConfirmText === 'HESABIMI SİL';
  }

  deleteAccount() {
    // Logout for now — real delete would need a backend endpoint
    this.auth.logout();
  }

  get initials(): string {
    return (this.firstName[0] ?? '') + (this.lastName[0] ?? '');
  }

  get fullName(): string {
    return `${this.firstName} ${this.lastName}`.trim();
  }

  get passwordStrength(): { label: string; color: string; width: number } {
    const p = this.newPassword;
    if (!p) return { label: '', color: '', width: 0 };
    let score = 0;
    if (p.length >= 8)  score++;
    if (/[A-Z]/.test(p)) score++;
    if (/[0-9]/.test(p)) score++;
    if (/[^A-Za-z0-9]/.test(p)) score++;
    return [
      { label: 'Çok Zayıf', color: '#ef4444', width: 20 },
      { label: 'Zayıf',     color: '#f97316', width: 40 },
      { label: 'Orta',      color: '#fbbf24', width: 60 },
      { label: 'Güçlü',     color: '#4ade80', width: 80 },
      { label: 'Çok Güçlü', color: '#22c55e', width: 100 },
    ][score];
  }
}
