import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule }  from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexChart, ApexAxisChartSeries, ApexXAxis, ApexYAxis,
  ApexTooltip, ApexGrid, ApexStroke, ApexFill, ApexAnnotations
} from 'ng-apexcharts';

import { SidebarComponent } from '../dashboard/components/sidebar/sidebar';

interface RetirementYear {
  age:     number;
  balance: number;
  contrib: number;
  growth:  number;
}

@Component({
  selector: 'app-retirement',
  standalone: true,
  imports: [CommonModule, FormsModule, NgApexchartsModule, SidebarComponent],
  templateUrl: './retirement.html',
  styleUrl: './retirement.scss',
})
export class RetirementComponent {
  // Inputs
  currentAge        = 30;
  retirementAge     = 60;
  currentSavings    = 100_000;
  monthlyContrib    = 8_000;
  annualReturn      = 15;
  monthlyExpense    = 25_000;   // emeklilikte aylık gider
  inflationRate     = 12;       // yıllık enflasyon %

  // Results
  hasResult      = false;
  retirementBalance = 0;
  yearsOfCoverage   = 0;
  realBalance       = 0;       // enflasyona göre düzeltilmiş
  canRetireAt:  number | null = null;
  timeline:     RetirementYear[] = [];

  series:      ApexAxisChartSeries = [];
  chart:       ApexChart = { type: 'area', height: 300, toolbar: { show: false }, background: 'transparent', animations: { enabled: true, speed: 500 } };
  xaxis:       ApexXAxis = { categories: [], labels: { style: { colors: '#475569', fontSize: '11px', fontFamily: 'Inter' } }, axisBorder: { show: false }, axisTicks: { show: false } };
  yaxis:       ApexYAxis = { labels: { style: { colors: '#475569', fontSize: '11px', fontFamily: 'Inter' }, formatter: (v) => this.fmtShort(v) } };
  stroke:      ApexStroke = { curve: 'smooth', width: [3, 2] };
  fill:        ApexFill   = { type: 'gradient', gradient: { shadeIntensity: 1, opacityFrom: 0.25, opacityTo: 0.02, stops: [0, 100] } };
  grid:        ApexGrid   = { borderColor: 'rgba(255,255,255,0.04)', strokeDashArray: 4 };
  tooltip:     ApexTooltip = { theme: 'dark', y: { formatter: (v) => this.fmt(v) }, style: { fontFamily: 'Inter' } };
  annotations: ApexAnnotations = {};
  colors = ['#6366f1', '#fbbf24'];

  constructor(private cdr: ChangeDetectorRef) {}

  calculate() {
    const years  = this.retirementAge - this.currentAge;
    const mRate  = this.annualReturn / 100 / 12;
    const months = years * 12;

    // Accumulation phase
    let balance = this.currentSavings;
    this.timeline = [];
    this.canRetireAt = null;

    const needAtRetirement = this.monthlyExpense * 12 / (this.annualReturn / 100); // perpetuity

    for (let y = 0; y <= years; y++) {
      if (y > 0) {
        for (let m = 0; m < 12; m++) {
          balance = balance * (1 + mRate) + this.monthlyContrib;
        }
      }
      const age = this.currentAge + y;
      this.timeline.push({ age, balance, contrib: this.monthlyContrib * y * 12, growth: balance - this.currentSavings - this.monthlyContrib * y * 12 });

      // Check if can retire early (balance covers perpetuity need)
      if (!this.canRetireAt && balance >= needAtRetirement && age > this.currentAge) {
        this.canRetireAt = age;
      }
    }

    this.retirementBalance = balance;

    // Inflation-adjusted real value
    const inflFactor = Math.pow(1 + this.inflationRate / 100, years);
    this.realBalance = balance / inflFactor;

    // Drawdown phase: how many years does the balance last?
    const inflAdjExpense = this.monthlyExpense * inflFactor;
    const drawMRate = this.annualReturn / 100 / 12;
    let drawBalance = balance;
    let months2 = 0;
    while (drawBalance > 0 && months2 < 12 * 100) {
      drawBalance = drawBalance * (1 + drawMRate) - inflAdjExpense;
      months2++;
    }
    this.yearsOfCoverage = drawBalance > 0 ? 999 : Math.floor(months2 / 12);

    this.hasResult = true;
    this.buildChart();
    this.cdr.detectChanges();
  }

  private buildChart() {
    const cats    = this.timeline.map(t => `${t.age} yaş`);
    const balData = this.timeline.map(t => Math.round(t.balance));

    this.series = [
      { name: 'Birikim',       data: balData },
    ];

    this.xaxis = { ...this.xaxis, categories: cats };

    this.annotations = {
      xaxis: [{
        x: `${this.retirementAge} yaş`,
        borderColor: '#fbbf24',
        strokeDashArray: 5,
        label: {
          text: 'Emeklilik',
          style: { background: '#fbbf24', color: '#000', fontFamily: 'Inter', fontSize: '11px' }
        }
      }],
      ...(this.canRetireAt ? {
        points: [{
          x: `${this.canRetireAt} yaş`,
          y: this.timeline.find(t => t.age === this.canRetireAt)?.balance ?? 0,
          marker: { size: 7, fillColor: '#4ade80', strokeColor: '#05091a', strokeWidth: 2 },
          label: {
            text: 'Erken Emeklilik',
            style: { background: '#4ade80', color: '#000', fontFamily: 'Inter', fontSize: '10px' }
          }
        }]
      } : {})
    };

    this.chart = { ...this.chart };
  }

  get coverageLabel(): string {
    if (this.yearsOfCoverage >= 999) return 'Sınırsız ♾️';
    return `${this.yearsOfCoverage} yıl`;
  }

  get coverageColor(): string {
    if (this.yearsOfCoverage >= 30)  return '#4ade80';
    if (this.yearsOfCoverage >= 15)  return '#fbbf24';
    return '#f87171';
  }

  get yearsToRetire(): number { return this.retirementAge - this.currentAge; }

  fmt(n: number): string {
    return new Intl.NumberFormat('tr-TR', { minimumFractionDigits: 0, maximumFractionDigits: 0 }).format(n) + ' ₺';
  }

  fmtShort(n: number): string {
    if (n >= 1_000_000) return (n / 1_000_000).toFixed(1) + 'M ₺';
    if (n >= 1_000)     return (n / 1_000).toFixed(0) + 'K ₺';
    return n.toFixed(0) + ' ₺';
  }
}
