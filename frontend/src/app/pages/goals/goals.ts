import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { SidebarComponent } from '../dashboard/components/sidebar/sidebar';
import { GoalsService, Goal } from '../../core/services/goals.service';

@Component({
  selector: 'app-goals',
  standalone: true,
  imports: [CommonModule, DecimalPipe, ReactiveFormsModule, SidebarComponent],
  templateUrl: './goals.html',
  styleUrl: './goals.scss',
})
export class GoalsComponent implements OnInit {
  goals: Goal[] = [];
  loading  = true;
  showForm = false;
  editingGoal: Goal | null = null;
  deletingId: string | null = null;
  isSaving = false;
  errorMsg = '';

  form!: FormGroup;

  constructor(
    private svc: GoalsService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.buildForm();
    this.loadGoals();
  }

  private buildForm(goal?: Goal) {
    this.form = this.fb.group({
      title:         [goal?.title ?? '', [Validators.required, Validators.maxLength(100)]],
      targetAmount:  [goal?.targetAmount ?? 0, [Validators.required, Validators.min(1)]],
      currentAmount: [goal?.currentAmount ?? 0, [Validators.min(0)]],
      deadline:      [goal?.deadline ? goal.deadline.substring(0, 10) : ''],
    });
  }

  private loadGoals() {
    this.svc.getAll().subscribe({
      next: (goals) => {
        this.goals   = goals;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.cdr.detectChanges();
      },
    });
  }

  openAdd() {
    this.editingGoal = null;
    this.errorMsg    = '';
    this.buildForm();
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(goal: Goal) {
    this.editingGoal = goal;
    this.errorMsg    = '';
    this.buildForm(goal);
    this.showForm = true;
    this.cdr.detectChanges();
  }

  closeForm() {
    this.showForm    = false;
    this.editingGoal = null;
    this.cdr.detectChanges();
  }

  onSubmit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving = true;
    this.errorMsg = '';

    const raw = this.form.value;
    const data = {
      title:         raw.title,
      targetAmount:  +raw.targetAmount,
      currentAmount: +raw.currentAmount,
      deadline:      raw.deadline || null,
    };

    const req = this.editingGoal
      ? this.svc.update(this.editingGoal.id, data)
      : this.svc.create(data);

    req.subscribe({
      next: (saved) => {
        if (this.editingGoal) {
          this.goals = this.goals.map(g => g.id === saved.id ? saved : g);
        } else {
          this.goals = [...this.goals, saved];
        }
        this.isSaving    = false;
        this.showForm    = false;
        this.editingGoal = null;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isSaving = false;
        this.errorMsg = err.error?.error ?? 'Bir hata oluştu.';
        this.cdr.detectChanges();
      },
    });
  }

  confirmDelete(id: string) {
    this.deletingId = id;
    this.cdr.detectChanges();
  }

  cancelDelete() {
    this.deletingId = null;
    this.cdr.detectChanges();
  }

  doDelete(id: string) {
    this.svc.delete(id).subscribe({
      next: () => {
        this.goals      = this.goals.filter(g => g.id !== id);
        this.deletingId = null;
        this.cdr.detectChanges();
      },
      error: () => {
        this.deletingId = null;
        this.cdr.detectChanges();
      },
    });
  }

  progress(g: Goal): number {
    if (!g.targetAmount) return 0;
    return Math.min(100, Math.round((g.currentAmount / g.targetAmount) * 100));
  }

  daysLeft(deadline: string | null): number {
    if (!deadline) return -1;
    return Math.max(0, Math.round((new Date(deadline).getTime() - Date.now()) / 86400000));
  }

  progressColor(pct: number): string {
    if (pct >= 75) return '#4ade80';
    if (pct >= 40) return '#fbbf24';
    return '#6366f1';
  }

  field(n: string) { return this.form.get(n)!; }
}
