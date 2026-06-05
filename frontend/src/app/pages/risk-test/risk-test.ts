import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { SidebarComponent } from '../dashboard/components/sidebar/sidebar';
import { RiskTestService, RiskQuestion, RiskTestResult } from '../../core/services/risk-test.service';

type Screen = 'intro' | 'quiz' | 'result';

@Component({
  selector: 'app-risk-test',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  templateUrl: './risk-test.html',
  styleUrl: './risk-test.scss',
})
export class RiskTestComponent implements OnInit {
  screen: Screen = 'intro';

  questions:   RiskQuestion[]  = [];
  prevResult:  RiskTestResult | null = null;
  result:      RiskTestResult | null = null;

  currentIdx = 0;
  answers: { questionId: string; optionId: string }[] = [];
  selectedOptionId: string | null = null;

  loading   = true;
  submitting = false;

  protected Math = Math;

  constructor(
    private svc: RiskTestService,
    public router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.svc.getQuestions().subscribe({
      next: (qs) => {
        this.questions = qs;
        this.loading   = false;
        this.cdr.detectChanges();
      },
      error: () => { this.loading = false; this.cdr.detectChanges(); },
    });

    this.svc.getLatest().subscribe({
      next: (r)  => { this.prevResult = r; this.cdr.detectChanges(); },
      error: ()  => {},
    });
  }

  startTest() {
    this.currentIdx      = 0;
    this.answers         = [];
    this.selectedOptionId = null;
    this.screen          = 'quiz';
    this.cdr.detectChanges();
  }

  selectOption(optionId: string) {
    if (this.selectedOptionId) return;
    this.selectedOptionId = optionId;

    const q = this.questions[this.currentIdx];
    this.answers.push({ questionId: q.id, optionId });
    this.cdr.detectChanges();

    setTimeout(() => {
      if (this.currentIdx < this.questions.length - 1) {
        this.currentIdx++;
        this.selectedOptionId = null;
        this.cdr.detectChanges();
      } else {
        this.submitTest();
      }
    }, 520);
  }

  goBack() {
    if (this.currentIdx === 0) { this.screen = 'intro'; this.cdr.detectChanges(); return; }
    this.answers.pop();
    this.currentIdx--;
    this.selectedOptionId = null;
    this.cdr.detectChanges();
  }

  private submitTest() {
    this.submitting = true;
    this.screen     = 'result';
    this.cdr.detectChanges();

    this.svc.submit({ answers: this.answers }).subscribe({
      next: (r) => {
        this.result     = r;
        this.submitting = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.submitting = false;
        this.screen     = 'quiz';
        this.cdr.detectChanges();
      },
    });
  }

  retakeTest() {
    this.answers         = [];
    this.currentIdx      = 0;
    this.selectedOptionId = null;
    this.result          = null;
    this.screen          = 'intro';
    this.cdr.detectChanges();
  }

  get progress(): number {
    return Math.round(((this.currentIdx) / this.questions.length) * 100);
  }

  get currentQuestion(): RiskQuestion {
    return this.questions[this.currentIdx];
  }

  riskLabel(level: string | null): string {
    const map: Record<string, string> = {
      conservative: 'Muhafazakâr', Conservative: 'Muhafazakâr',
      balanced:     'Dengeli',     Balanced:     'Dengeli',
      aggressive:   'Agresif',     Aggressive:   'Agresif',
    };
    return map[level ?? ''] ?? (level ?? '—');
  }

  riskDesc(level: string | null): string {
    const map: Record<string, string> = {
      conservative: 'Riski minimize etmeyi, sermayeyi korumayı tercih edersiniz.',
      Conservative: 'Riski minimize etmeyi, sermayeyi korumayı tercih edersiniz.',
      balanced:     'Orta düzeyde risk alarak büyüme ve güvenliği dengelersiniz.',
      Balanced:     'Orta düzeyde risk alarak büyüme ve güvenliği dengelersiniz.',
      aggressive:   'Yüksek getiri için yüksek riski göze alabilirsiniz.',
      Aggressive:   'Yüksek getiri için yüksek riski göze alabilirsiniz.',
    };
    return map[level ?? ''] ?? '';
  }

  riskColor(level: string | null): string {
    const l = (level ?? '').toLowerCase();
    if (l === 'conservative') return '#4ade80';
    if (l === 'balanced')     return '#fbbf24';
    return '#f87171';
  }

  riskEmoji(level: string | null): string {
    const l = (level ?? '').toLowerCase();
    if (l === 'conservative') return '🛡️';
    if (l === 'balanced')     return '⚖️';
    return '🚀';
  }

  formatDate(d: string): string {
    return new Date(d).toLocaleDateString('tr-TR', { day: 'numeric', month: 'long', year: 'numeric' });
  }
}
