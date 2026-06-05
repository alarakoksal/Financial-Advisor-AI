import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule }  from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexChart, ApexAxisChartSeries, ApexXAxis, ApexYAxis,
  ApexTooltip, ApexGrid, ApexStroke, ApexFill, ApexAnnotations
} from 'ng-apexcharts';

import { SidebarComponent } from '../dashboard/components/sidebar/sidebar';

@Component({
  selector: 'app-monte-carlo',
  standalone: true,
  imports: [CommonModule, FormsModule, NgApexchartsModule, SidebarComponent],
  templateUrl: './monte-carlo.html',
  styleUrl: './monte-carlo.scss',
})
export class MonteCarloComponent {

  // Inputs
  targetAmount      = 500_000;
  currentSavings    = 50_000;
  monthlyContrib    = 5_000;
  years             = 10;
  annualReturn      = 15;   // % (Türkiye için gerçekçi)
  volatility        = 20;   // % yıllık standart sapma
  trials            = 1000;

  // Results
  successRate: number | null = null;
  medianFinal  = 0;
  p10Final     = 0;
  p90Final     = 0;
  hasResult    = false;
  running      = false;

  // Chart
  series:      ApexAxisChartSeries = [];
  chart:       ApexChart = { type: 'rangeArea', height: 340, toolbar: { show: false }, background: 'transparent', animations: { enabled: false } };
  xaxis:       ApexXAxis = { categories: [], labels: { style: { colors: '#475569', fontSize: '11px', fontFamily: 'Inter' } }, axisBorder: { show: false }, axisTicks: { show: false } };
  yaxis:       ApexYAxis = { labels: { style: { colors: '#475569', fontSize: '11px', fontFamily: 'Inter' }, formatter: (v) => this.fmtShort(v) } };
  stroke:      ApexStroke = { curve: 'smooth', width: [0, 0, 2, 2] };
  fill:        ApexFill = { opacity: [0.15, 0.25, 1, 1] };
  grid:        ApexGrid = { borderColor: 'rgba(255,255,255,0.04)', strokeDashArray: 4 };
  tooltip:     ApexTooltip = { theme: 'dark', y: { formatter: (v) => this.fmt(v) }, style: { fontFamily: 'Inter' } };
  annotations: ApexAnnotations = {};
  colors = ['#6366f1', '#a78bfa', '#4ade80', '#f87171'];

  constructor(private cdr: ChangeDetectorRef) {}

  private normalRandom(): number {
    const u1 = Math.random() || 1e-10;
    const u2 = Math.random();
    return Math.sqrt(-2 * Math.log(u1)) * Math.cos(2 * Math.PI * u2);
  }

  run() {
    this.running = true;
    this.cdr.detectChanges();

    setTimeout(() => {
      const months = this.years * 12;
      const mRate  = this.annualReturn / 100 / 12;
      const mVol   = this.volatility   / 100 / Math.sqrt(12);

      // Store final values per year per trial
      const yearlyValues: number[][] = Array.from({ length: this.years + 1 }, () => []);

      let successCount = 0;

      for (let t = 0; t < this.trials; t++) {
        let balance = this.currentSavings;
        yearlyValues[0].push(balance);

        for (let m = 1; m <= months; m++) {
          const r = this.normalRandom() * mVol + mRate;
          balance = balance * (1 + r) + this.monthlyContrib;
          if (m % 12 === 0) yearlyValues[m / 12].push(balance);
        }
        if (balance >= this.targetAmount) successCount++;
      }

      // Compute percentiles per year
      const pct = (arr: number[], p: number) => {
        const s = [...arr].sort((a, b) => a - b);
        return s[Math.floor(p / 100 * (s.length - 1))];
      };

      const years = Array.from({ length: this.years + 1 }, (_, i) => i);
      const p10  = years.map(y => pct(yearlyValues[y], 10));
      const p25  = years.map(y => pct(yearlyValues[y], 25));
      const p50  = years.map(y => pct(yearlyValues[y], 50));
      const p75  = years.map(y => pct(yearlyValues[y], 75));
      const p90  = years.map(y => pct(yearlyValues[y], 90));

      this.successRate = (successCount / this.trials) * 100;
      this.medianFinal = p50[p50.length - 1];
      this.p10Final    = p10[p10.length - 1];
      this.p90Final    = p90[p90.length - 1];
      this.hasResult   = true;
      this.running     = false;

      const cats = years.map(y => `${y}. Yıl`);

      this.series = [
        { name: 'P10–P25 Aralığı', type: 'rangeArea', data: years.map((_, i) => ({ x: cats[i], y: [p10[i], p25[i]] })) },
        { name: 'P25–P75 Aralığı', type: 'rangeArea', data: years.map((_, i) => ({ x: cats[i], y: [p25[i], p75[i]] })) },
        { name: 'Medyan (P50)',     type: 'line',       data: years.map((_, i) => ({ x: cats[i], y: p50[i] })) },
        { name: 'P90 (İyimser)',    type: 'line',       data: years.map((_, i) => ({ x: cats[i], y: p90[i] })) },
      ];

      this.xaxis = { ...this.xaxis, categories: cats };
      this.annotations = {
        yaxis: [{
          y: this.targetAmount,
          borderColor: '#fbbf24',
          strokeDashArray: 6,
          label: { text: 'Hedef', style: { background: '#fbbf24', color: '#000', fontFamily: 'Inter', fontSize: '11px' } }
        }]
      };

      this.chart = { ...this.chart };
      this.cdr.detectChanges();
    }, 50);
  }

  get successColor(): string {
    if (this.successRate === null) return '#94a3b8';
    if (this.successRate >= 70) return '#4ade80';
    if (this.successRate >= 40) return '#fbbf24';
    return '#f87171';
  }

  get successLabel(): string {
    if (this.successRate === null) return '';
    if (this.successRate >= 70) return 'Yüksek ihtimalli';
    if (this.successRate >= 40) return 'Orta ihtimalli';
    return 'Riskli senaryo';
  }

  fmt(n: number): string {
    return new Intl.NumberFormat('tr-TR', { minimumFractionDigits: 0, maximumFractionDigits: 0 }).format(n) + ' ₺';
  }

  fmtShort(n: number): string {
    if (n >= 1_000_000) return (n / 1_000_000).toFixed(1) + 'M ₺';
    if (n >= 1_000)     return (n / 1_000).toFixed(0) + 'K ₺';
    return n.toFixed(0) + ' ₺';
  }
}
