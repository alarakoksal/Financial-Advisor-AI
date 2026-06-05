import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule }  from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexChart, ApexAxisChartSeries, ApexXAxis, ApexYAxis,
  ApexTooltip, ApexGrid, ApexPlotOptions, ApexDataLabels
} from 'ng-apexcharts';

import { SidebarComponent }       from '../dashboard/components/sidebar/sidebar';
import { DebtService, Debt }       from '../../core/services/debt.service';

interface StrategyResult {
  totalInterest:  number;
  totalMonths:    number;
  payoffOrder:    string[];
  monthlyData:    { month: number; balance: number }[];
}

interface DebtItem {
  id:             string;
  name:           string;
  balance:        number;
  interestRate:   number;
  minPayment:     number;
}

@Component({
  selector: 'app-debt-strategy',
  standalone: true,
  imports: [CommonModule, FormsModule, NgApexchartsModule, SidebarComponent],
  templateUrl: './debt-strategy.html',
  styleUrl: './debt-strategy.scss',
})
export class DebtStrategyComponent implements OnInit {

  debts:        DebtItem[] = [];
  loading       = true;
  extraPayment  = 1000;
  hasResult     = false;

  snowball: StrategyResult | null = null;
  avalanche: StrategyResult | null = null;
  winner: 'snowball' | 'avalanche' | 'tie' = 'tie';

  series:     ApexAxisChartSeries = [];
  chart:      ApexChart = { type: 'bar', height: 220, toolbar: { show: false }, background: 'transparent' };
  xaxis:      ApexXAxis = { categories: ['Snowball (Küçükten)', 'Avalanche (Yüksek Faiz)'], labels: { style: { colors: '#64748b', fontSize: '12px', fontFamily: 'Inter' } }, axisBorder: { show: false }, axisTicks: { show: false } };
  yaxis:      ApexYAxis = { labels: { style: { colors: '#64748b', fontSize: '11px', fontFamily: 'Inter' }, formatter: (v) => this.fmtShort(v) } };
  tooltip:    ApexTooltip = { theme: 'dark', y: { formatter: (v) => this.fmt(v) }, style: { fontFamily: 'Inter' } };
  grid:       ApexGrid = { borderColor: 'rgba(255,255,255,0.04)', strokeDashArray: 4 };
  plotOptions: ApexPlotOptions = { bar: { borderRadius: 8, columnWidth: '45%' } };
  dataLabels: ApexDataLabels = { enabled: false };
  colors = ['#6366f1', '#f87171'];

  constructor(private svc: DebtService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.svc.getAll().subscribe({
      next: (d) => {
        this.debts = d.map(debt => ({
          id:           debt.id,
          name:         debt.name,
          balance:      debt.remainingAmount,
          interestRate: debt.interestRate,
          minPayment:   debt.monthlyPayment,
        }));
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => { this.loading = false; this.cdr.detectChanges(); },
    });
  }

  calculate() {
    if (this.debts.length === 0) return;

    this.snowball  = this.simulate('snowball');
    this.avalanche = this.simulate('avalanche');

    const sDiff = this.snowball.totalInterest - this.avalanche.totalInterest;
    if (Math.abs(sDiff) < 1) this.winner = 'tie';
    else                     this.winner = sDiff > 0 ? 'avalanche' : 'snowball';

    this.hasResult = true;

    this.series = [{
      name: 'Toplam Faiz',
      data: [
        Math.round(this.snowball.totalInterest),
        Math.round(this.avalanche.totalInterest),
      ],
    }];

    this.cdr.detectChanges();
  }

  private simulate(strategy: 'snowball' | 'avalanche'): StrategyResult {
    // Deep copy
    let items: DebtItem[] = this.debts.map(d => ({ ...d }));
    let totalInterest = 0;
    let month = 0;
    const payoffOrder: string[] = [];
    const monthlyData: { month: number; balance: number }[] = [];

    const MAX = 600; // 50 yıl

    while (items.length > 0 && month < MAX) {
      month++;

      // Sort for strategy
      const sorted = strategy === 'snowball'
        ? [...items].sort((a, b) => a.balance - b.balance)
        : [...items].sort((a, b) => b.interestRate - a.interestRate);

      const target = sorted[0];

      // Apply interest + payments
      const remaining: DebtItem[] = [];

      // Extra payment goes to target
      let extraLeft = this.extraPayment;

      for (const item of items) {
        const monthlyRate = item.interestRate / 100 / 12;
        const interest = item.balance * monthlyRate;
        totalInterest += interest;

        let pay = item.minPayment;
        if (item.id === target.id) pay += extraLeft;

        item.balance = item.balance + interest - pay;

        if (item.balance <= 0) {
          payoffOrder.push(item.name);
          // Freed payment cascades next month (simplified: add to extra)
          extraLeft = Math.max(0, -item.balance) + (item.minPayment - item.minPayment); // recycle
        } else {
          remaining.push(item);
        }
      }

      items = remaining;
      const totalBal = items.reduce((s, d) => s + d.balance, 0);
      monthlyData.push({ month, balance: Math.max(0, totalBal) });
    }

    return { totalInterest, totalMonths: month, payoffOrder, monthlyData };
  }

  get saving(): number {
    if (!this.snowball || !this.avalanche) return 0;
    return Math.abs(this.snowball.totalInterest - this.avalanche.totalInterest);
  }

  get winnerName(): string {
    if (this.winner === 'snowball')  return 'Snowball';
    if (this.winner === 'avalanche') return 'Avalanche';
    return 'Eşit';
  }

  fmt(n: number): string {
    return new Intl.NumberFormat('tr-TR', { minimumFractionDigits: 0, maximumFractionDigits: 0 }).format(n) + ' ₺';
  }

  fmtShort(n: number): string {
    if (n >= 1_000_000) return (n / 1_000_000).toFixed(1) + 'M ₺';
    if (n >= 1_000)     return (n / 1_000).toFixed(0) + 'K ₺';
    return n.toFixed(0) + ' ₺';
  }

  fmtMonths(m: number): string {
    const y = Math.floor(m / 12);
    const mo = m % 12;
    return y > 0 ? `${y} yıl ${mo} ay` : `${mo} ay`;
  }
}
