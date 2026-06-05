import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NgApexchartsModule } from 'ng-apexcharts';
import { forkJoin, of, timeout } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { SidebarComponent }  from './components/sidebar/sidebar';
import { DashboardService, DashboardSummary, FinancialHealth, Notification } from '../../core/services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, DecimalPipe, RouterLink, SidebarComponent, NgApexchartsModule],
  templateUrl: './dashboard.html',
  styleUrl:    './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  summary: DashboardSummary | null = null;
  health:  FinancialHealth  | null = null;
  notifications: Notification[]    = [];
  loading = true;

  // Gauge chart
  gaugeOptions: any = {};

  // Bar chart
  barOptions: any = {};

  constructor(private svc: DashboardService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    forkJoin({
      summary:       this.svc.getSummary().pipe(timeout(8000), catchError(() => of(null))),
      health:        this.svc.getFinancialHealth().pipe(timeout(8000), catchError(() => of(null))),
      notifications: this.svc.getNotifications().pipe(timeout(8000), catchError(() => of([]))),
    }).subscribe(({ summary, health, notifications }) => {
      this.summary       = summary;
      this.health        = health;
      this.notifications = notifications as Notification[];
      if (health) this.buildGauge(health.score);
      if (summary) this.buildBar(summary.monthlyIncome, summary.monthlyExpenses);
      this.loading = false;
      this.cdr.detectChanges();
    });
  }

  private buildGauge(score: number) {
    this.gaugeOptions = {
      series: [score],
      chart:  { type: 'radialBar', height: 280, background: 'transparent' },
      plotOptions: {
        radialBar: {
          startAngle: -135,
          endAngle:    135,
          hollow:      { size: '65%' },
          track:       { background: 'rgba(255,255,255,0.06)', strokeWidth: '100%' },
          dataLabels:  {
            name:  { show: false },
            value: { fontSize: '2.5rem', fontWeight: 800, color: '#fff', offsetY: 10,
                     formatter: (v: number) => Math.round(v).toString() }
          }
        }
      },
      fill: {
        type: 'gradient',
        gradient: { shade: 'dark', type: 'horizontal',
                    gradientToColors: [this.scoreColor(score)],
                    stops: [0, 100] }
      },
      colors:  [this.scoreColorStart(score)],
      stroke:  { lineCap: 'round' },
      theme:   { mode: 'dark' },
    };
  }

  private buildBar(income: number, expenses: number) {
    const net = income - expenses;
    this.barOptions = {
      series: [
        { name: 'Gelir',  data: [income]   },
        { name: 'Gider',  data: [expenses] },
        { name: 'Net',    data: [net]      },
      ],
      chart:  { type: 'bar', height: 220, background: 'transparent', toolbar: { show: false } },
      plotOptions: { bar: { horizontal: false, columnWidth: '40%', borderRadius: 6 } },
      colors:  ['#6366f1', '#f87171', '#4ade80'],
      xaxis:   { categories: ['Bu Ay'], labels: { style: { colors: '#94a3b8' } }, axisBorder: { show: false }, axisTicks: { show: false } },
      yaxis:   { labels: { style: { colors: '#94a3b8' }, formatter: (v: number) => `₺${(v/1000).toFixed(0)}k` } },
      grid:    { borderColor: 'rgba(255,255,255,0.06)', strokeDashArray: 4 },
      legend:  { labels: { colors: '#94a3b8' } },
      tooltip: { theme: 'dark', y: { formatter: (v: number) => `₺${v.toLocaleString('tr-TR')}` } },
      theme:   { mode: 'dark' },
      dataLabels: { enabled: false },
    };
  }

  scoreColor(score: number): string {
    if (score >= 70) return '#4ade80';
    if (score >= 40) return '#fbbf24';
    return '#f87171';
  }

  scoreColorStart(score: number): string {
    if (score >= 70) return '#6366f1';
    if (score >= 40) return '#f59e0b';
    return '#ef4444';
  }

  scoreLabel(score: number): string {
    if (score >= 70) return 'İyi';
    if (score >= 40) return 'Orta';
    return 'Zayıf';
  }

  riskClass(level: string): string {
    const l = level?.toLowerCase();
    if (l === 'low'      || l === 'conservative') return 'risk-low';
    if (l === 'moderate' || l === 'medium')        return 'risk-mid';
    return 'risk-high';
  }

  riskLabel(level: string): string {
    const map: Record<string, string> = {
      low: 'Düşük', conservative: 'Düşük',
      moderate: 'Orta', medium: 'Orta',
      high: 'Yüksek', aggressive: 'Yüksek',
    };
    return map[level?.toLowerCase()] ?? level;
  }

  notifIcon(type: string): string {
    if (type === 'success') return 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z';
    if (type === 'warning') return 'M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z';
    return 'M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z';
  }

  goalProgress(g: DashboardSummary['nearestGoal']): number {
    if (!g || g.targetAmount === 0) return 0;
    return Math.min(100, Math.round((g.currentAmount / g.targetAmount) * 100));
  }

  daysLeft(deadline: string): number {
    return Math.max(0, Math.round((new Date(deadline).getTime() - Date.now()) / 86400000));
  }
}
