import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly KEY = 'theme';

  get isDark(): boolean {
    return document.body.classList.contains('light') === false;
  }

  get isLight(): boolean {
    return document.body.classList.contains('light');
  }

  init() {
    const saved = localStorage.getItem(this.KEY);
    if (saved === 'light') this.applyLight();
  }

  toggle() {
    if (this.isLight) this.applyDark();
    else              this.applyLight();
  }

  private applyLight() {
    document.body.classList.add('light');
    localStorage.setItem(this.KEY, 'light');
  }

  private applyDark() {
    document.body.classList.remove('light');
    localStorage.setItem(this.KEY, 'dark');
  }
}
