import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule }  from '@angular/forms';

import { SidebarComponent }          from '../dashboard/components/sidebar/sidebar';
import { DebtService, Debt, DebtRequest } from '../../core/services/debt.service';

@Component({
  selector: 'app-debts',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  templateUrl: './debts.html',
  styleUrl: './debts.scss',
})
export class DebtsComponent implements OnInit {
  debts:   Debt[] = [];
  loading  = true;
  showForm = false;
  saving   = false;
  error:   string | null = null;

  editingId: string | null = null;
  deletingId: string | null = null;

  readonly types = [
    { value: 'mortgage',    label: 'Konut Kredisi' },
    { value: 'auto',        label: 'Taşıt Kredisi' },
    { value: 'personal',    label: 'İhtiyaç Kredisi' },
    { value: 'credit_card', label: 'Kredi Kartı' },
    { value: 'other',       label: 'Diğer' },
  ];

  // Form fields
  fName            = '';
  fType            = 'personal';
  fTotalAmount     = 0;
  fRemainingAmount = 0;
  fInterestRate    = 0;
  fMonthlyPayment  = 0;
  fStartDate       = '';
  fEndDate         = '';

  constructor(private svc: DebtService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.svc.getAll().subscribe({
      next: (d) => { this.debts = d; this.loading = false; this.cdr.detectChanges(); },
      error: ()  => { this.loading = false; this.cdr.detectChanges(); },
    });
  }

  openAdd() {
    this.editingId = null;
    this.fName = ''; this.fType = 'personal';
    this.fTotalAmount = 0; this.fRemainingAmount = 0;
    this.fInterestRate = 0; this.fMonthlyPayment = 0;
    this.fStartDate = ''; this.fEndDate = '';
    this.error = null;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(d: Debt) {
    this.editingId       = d.id;
    this.fName           = d.name;
    this.fType           = d.type;
    this.fTotalAmount    = d.totalAmount;
    this.fRemainingAmount = d.remainingAmount;
    this.fInterestRate   = d.interestRate;
    this.fMonthlyPayment = d.monthlyPayment;
    this.fStartDate      = d.startDate;
    this.fEndDate        = d.endDate ?? '';
    this.error = null;
    this.showForm = true;
    this.cdr.detectChanges();
  }

  closeForm() { this.showForm = false; this.cdr.detectChanges(); }

  onSubmit() {
    if (!this.fName || !this.fTotalAmount || !this.fMonthlyPayment || !this.fStartDate) {
      this.error = 'Lütfen zorunlu alanları doldurun.';
      this.cdr.detectChanges();
      return;
    }

    this.saving = true;
    this.error  = null;
    this.cdr.detectChanges();

    const req: DebtRequest = {
      name:            this.fName,
      type:            this.fType,
      totalAmount:     this.fTotalAmount,
      remainingAmount: this.fRemainingAmount,
      interestRate:    this.fInterestRate,
      monthlyPayment:  this.fMonthlyPayment,
      startDate:       this.fStartDate,
      endDate:         this.fEndDate || null,
    };

    const obs = this.editingId
      ? this.svc.update(this.editingId, req)
      : this.svc.create(req);

    obs.subscribe({
      next: () => {
        this.saving   = false;
        this.showForm = false;
        this.svc.getAll().subscribe({ next: (d) => { this.debts = d; this.cdr.detectChanges(); } });
      },
      error: () => {
        this.saving = false;
        this.error  = 'Bir hata oluştu.';
        this.cdr.detectChanges();
      },
    });
  }

  confirmDelete(id: string) { this.deletingId = id; this.cdr.detectChanges(); }
  cancelDelete()            { this.deletingId = null; this.cdr.detectChanges(); }

  doDelete(id: string) {
    this.svc.delete(id).subscribe({
      next: () => {
        this.debts     = this.debts.filter(d => d.id !== id);
        this.deletingId = null;
        this.cdr.detectChanges();
      },
      error: () => { this.cdr.detectChanges(); },
    });
  }

  typeLabel(type: string): string {
    return this.types.find(t => t.value === type)?.label ?? type;
  }

  typeColor(type: string): string {
    const map: Record<string, string> = {
      mortgage:    '#6366f1',
      auto:        '#fbbf24',
      personal:    '#38bdf8',
      credit_card: '#f87171',
      other:       '#94a3b8',
    };
    return map[type] ?? '#94a3b8';
  }

  get totalDebt(): number    { return this.debts.reduce((s, d) => s + d.remainingAmount, 0); }
  get totalMonthly(): number { return this.debts.reduce((s, d) => s + d.monthlyPayment, 0); }

  fmt(n: number): string {
    return new Intl.NumberFormat('tr-TR', { minimumFractionDigits: 0, maximumFractionDigits: 0 }).format(n);
  }

  payoffDate(d: Debt): string {
    if (!d.remainingMonths) return '—';
    const date = new Date();
    date.setMonth(date.getMonth() + d.remainingMonths);
    return date.toLocaleDateString('tr-TR', { month: 'long', year: 'numeric' });
  }
}
