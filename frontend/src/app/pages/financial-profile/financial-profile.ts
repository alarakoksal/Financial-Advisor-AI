import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { SidebarComponent } from '../dashboard/components/sidebar/sidebar';
import { FinancialProfileService } from '../../core/services/financial-profile.service';

@Component({
  selector: 'app-financial-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, SidebarComponent],
  templateUrl: './financial-profile.html',
  styleUrl: './financial-profile.scss',
})
export class FinancialProfileComponent implements OnInit {
  form!: FormGroup;
  isLoading  = true;
  isSaving   = false;
  hasProfile = false;
  errorMsg   = '';
  successMsg = '';

  constructor(
    private fb: FormBuilder,
    private svc: FinancialProfileService,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      monthlyIncome:   [0, [Validators.required, Validators.min(0)]],
      monthlyExpenses: [0, [Validators.required, Validators.min(0)]],
      rentExpense:     [0, [Validators.min(0)]],
      rentIncome:      [0, [Validators.min(0)]],
      savingsAmount:   [0, [Validators.min(0)]],
      debtAmount:      [0, [Validators.min(0)]],
      currency:        ['TRY'],
    });

    this.svc.get().subscribe({
      next: (data) => {
        this.hasProfile = true;
        this.form.patchValue(data);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.hasProfile = false;
        this.isLoading  = false;
        this.cdr.detectChanges();
      },
    });
  }

  onSubmit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving  = true;
    this.errorMsg  = '';
    this.successMsg = '';

    const data = this.form.value;
    const req  = this.hasProfile ? this.svc.update(data) : this.svc.create(data);

    req.subscribe({
      next: () => {
        this.isSaving   = false;
        this.hasProfile = true;
        this.successMsg = 'Finansal profiliniz başarıyla kaydedildi!';
        this.cdr.detectChanges();
        setTimeout(() => this.router.navigate(['/dashboard']), 1500);
      },
      error: (err) => {
        this.isSaving = false;
        this.errorMsg = err.error?.error ?? 'Bir hata oluştu.';
        this.cdr.detectChanges();
      },
    });
  }

  field(name: string) { return this.form.get(name)!; }
}
