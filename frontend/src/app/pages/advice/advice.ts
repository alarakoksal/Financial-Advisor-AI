import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule }  from '@angular/common';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

import { SidebarComponent }              from '../dashboard/components/sidebar/sidebar';
import { AdviceService, AdviceHistoryItem } from '../../core/services/advice.service';

type Screen = 'intro' | 'generating' | 'result';

@Component({
  selector: 'app-advice',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  templateUrl: './advice.html',
  styleUrl: './advice.scss',
})
export class AdviceComponent implements OnInit {
  screen: Screen = 'intro';

  advice:       string | null = null;
  history:      AdviceHistoryItem[] = [];
  historyLoading = true;
  error: string | null = null;

  constructor(
    private svc: AdviceService,
    private sanitizer: DomSanitizer,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.svc.getHistory().subscribe({
      next: (h) => {
        this.history       = h;
        this.historyLoading = false;
        if (h.length > 0) {
          this.advice = h[0].content;
          this.screen = 'result';
        }
        this.cdr.detectChanges();
      },
      error: () => { this.historyLoading = false; this.cdr.detectChanges(); },
    });
  }

  generate() {
    this.screen = 'generating';
    this.error  = null;
    this.cdr.detectChanges();

    this.svc.generate().subscribe({
      next: (res) => {
        this.advice  = res.advice;
        this.screen  = 'result';
        this.svc.getHistory().subscribe({
          next: (h) => { this.history = h; this.cdr.detectChanges(); },
          error: () => {},
        });
        this.cdr.detectChanges();
      },
      error: () => {
        this.error  = 'Öneri oluşturulurken bir hata oluştu. Finansal profilinizi doldurduğunuzdan emin olun.';
        this.screen = 'intro';
        this.cdr.detectChanges();
      },
    });
  }

  showHistory(item: AdviceHistoryItem) {
    this.advice = item.content;
    this.screen = 'result';
    this.cdr.detectChanges();
  }

  backToIntro() {
    this.advice = null;
    this.screen = 'intro';
    this.cdr.detectChanges();
  }

  get renderedAdvice(): SafeHtml {
    if (!this.advice) return '';
    return this.sanitizer.bypassSecurityTrustHtml(this.parseAdvice(this.advice));
  }

  private parseAdvice(text: string): string {
    const lines = text.split('\n');
    let html = '';
    let inList = false;

    for (const raw of lines) {
      const line = raw.trim();

      if (line === '') {
        if (inList) { html += '</ul>'; inList = false; }
        continue;
      }

      // Bullet point
      if (line.startsWith('- ') || line.startsWith('• ')) {
        if (!inList) { html += '<ul>'; inList = true; }
        html += `<li>${this.inlineFormat(line.slice(2))}</li>`;
        continue;
      }

      if (inList) { html += '</ul>'; inList = false; }

      // Markdown headers
      if (line.startsWith('### ')) {
        html += `<h3>${this.inlineFormat(line.slice(4))}</h3>`;
      } else if (line.startsWith('## ')) {
        html += `<h2>${this.inlineFormat(line.slice(3))}</h2>`;
      } else if (line.startsWith('# ')) {
        html += `<h2>${this.inlineFormat(line.slice(2))}</h2>`;
      } else {
        html += `<p>${this.inlineFormat(line)}</p>`;
      }
    }

    if (inList) html += '</ul>';
    return html;
  }

  private inlineFormat(text: string): string {
    return text
      .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
      .replace(/\*(.+?)\*/g, '<em>$1</em>');
  }

  formatDate(d: string): string {
    return new Date(d).toLocaleDateString('tr-TR', { day: 'numeric', month: 'long', year: 'numeric', hour: '2-digit', minute: '2-digit' });
  }

  formatDateShort(d: string): string {
    return new Date(d).toLocaleDateString('tr-TR', { day: 'numeric', month: 'short' });
  }
}
