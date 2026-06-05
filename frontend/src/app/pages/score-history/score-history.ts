import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexChart, ApexAxisChartSeries, ApexXAxis, ApexYAxis,
  ApexTooltip, ApexGrid, ApexStroke, ApexMarkers, ApexFill
} from 'ng-apexcharts';

import { SidebarComponent }       from '../dashboard/components/sidebar/sidebar';
import { DebtService, ScoreHistoryItem } from '../../core/services/debt.service';

@Component({
  selector: 'app-score-history',
  standalone: true,
  imports: [CommonModule, NgApexchartsModule, SidebarComponent],
  templateUrl: './score-history.html',
  styleUrl: './score-history.scss',
})
export class ScoreHistoryComponent implements OnInit {
  history:  ScoreHistoryItem[] = [];
  loading = true;

  series:  ApexAxisChartSeries = [];
  chart:   ApexChart = { type: 'area', height: 320, toolbar: { show: false }, background: 'transparent', animations: { enabled: true, speed: 600 } };
  xaxis:   ApexXAxis = { type: 'category', labels: { style: { colors: '#475569', fontSize: '12px', fontFamily: 'Inter, sans-serif' } }, axisBorder: { show: false }, axisTicks: { show: false } };
  yaxis:   ApexYAxis = { min: 0, max: 100, tickAmount: 5, labels: { style: { colors: '#475569', fontSize: '12px', fontFamily: 'Inter, sans-serif' }, formatter: (v) => v.toFixed(0) } };
  stroke:  ApexStroke  = { curve: 'smooth', width: 3 };
  markers: ApexMarkers = { size: 5, colors: ['#6366f1'], strokeColors: '#05091a', strokeWidth: 2, hover: { size: 7 } };
  fill:    ApexFill    = { type: 'gradient', gradient: { shadeIntensity: 1, opacityFrom: 0.3, opacityTo: 0, stops: [0, 100] } };
  grid:    ApexGrid    = { borderColor: 'rgba(255,255,255,0.04)', strokeDashArray: 4 };
  tooltip: ApexTooltip = {
    theme: 'dark',
    y: { formatter: (v) => `${v} / 100` },
    style: { fontFamily: 'Inter, sans-serif' }
  };
  colors = ['#6366f1'];

  constructor(private svc: DebtService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.svc.getScoreHistory().subscribe({
      next: (h) => {
        this.history = h;
        this.loading = false;
        this.buildChart(h);
        this.cdr.detectChanges();
      },
      error: () => { this.loading = false; this.cdr.detectChanges(); },
    });
  }

  private buildChart(h: ScoreHistoryItem[]) {
    this.series = [{
      name: 'Finansal Skor',
      data: h.map(i => ({
        x: new Date(i.recordedAt).toLocaleDateString('tr-TR', { day: 'numeric', month: 'short' }),
        y: i.score,
      })),
    }];
  }

  get currentScore(): number  { return this.history.at(-1)?.score ?? 0; }
  get firstScore():   number  { return this.history.at(0)?.score ?? 0; }
  get change():       number  { return this.currentScore - this.firstScore; }
  get avgScore():     number  {
    if (!this.history.length) return 0;
    return Math.round(this.history.reduce((s, i) => s + i.score, 0) / this.history.length);
  }
  get maxScore():     number  { return this.history.length ? Math.max(...this.history.map(i => i.score)) : 0; }

  scoreLabel(score: number): string {
    if (score >= 70) return 'İyi';
    if (score >= 40) return 'Orta';
    return 'Riskli';
  }

  scoreColor(score: number): string {
    if (score >= 70) return '#4ade80';
    if (score >= 40) return '#fbbf24';
    return '#f87171';
  }

  formatDate(d: string): string {
    return new Date(d).toLocaleDateString('tr-TR', { day: 'numeric', month: 'long', year: 'numeric' });
  }
}
