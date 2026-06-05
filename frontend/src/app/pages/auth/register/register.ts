import { Component, OnDestroy, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';

function passwordMatch(control: AbstractControl): ValidationErrors | null {
  const pw  = control.get('password')?.value;
  const cpw = control.get('confirmPassword')?.value;
  return pw && cpw && pw !== cpw ? { mismatch: true } : null;
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl:    './register.scss'
})
export class RegisterComponent implements AfterViewInit, OnDestroy {
  @ViewChild('canvas') canvasRef!: ElementRef<HTMLCanvasElement>;

  form: FormGroup;
  showPassword        = false;
  showConfirmPassword = false;
  isLoading           = false;
  currentStep         = 1;
  totalSteps          = 2;

  private animFrame!: number;
  private particles: Particle[]      = [];
  private shootingStars: ShootingStar[] = [];
  private glowOrbs: GlowOrb[]        = [];
  private time = 0;

  errorMessage = '';

  constructor(private fb: FormBuilder, private router: Router, private auth: AuthService) {
    this.form = this.fb.group({
      firstName:       ['', [Validators.required, Validators.minLength(2)]],
      lastName:        ['', [Validators.required, Validators.minLength(2)]],
      email:           ['', [Validators.required, Validators.email]],
      dateOfBirth:     ['', Validators.required],
      password:        ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required],
      currency:        ['TRY', Validators.required],
      language:        ['tr',  Validators.required],
    }, { validators: passwordMatch });
  }

  ngAfterViewInit() { this.initCanvas(); }
  ngOnDestroy()     { cancelAnimationFrame(this.animFrame); }

  get step1Valid() {
    return ['firstName','lastName','email','dateOfBirth'].every(k => this.form.get(k)?.valid);
  }

  nextStep() { if (this.step1Valid) this.currentStep = 2; else this.markStep1(); }
  prevStep() { this.currentStep = 1; }

  onSubmit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isLoading    = true;
    this.errorMessage = '';

    const { confirmPassword, language, ...rest } = this.form.value;
    this.auth.register({ ...rest, preferredLanguage: language }).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => {
        this.isLoading    = false;
        this.errorMessage = err.error?.message ?? 'Kayıt başarısız. Lütfen tekrar deneyin.';
      }
    });
  }

  private markStep1() {
    ['firstName','lastName','email','dateOfBirth'].forEach(k => this.form.get(k)?.markAsTouched());
  }

  f(name: string) { return this.form.get(name)!; }
  get passwordMismatch() { return this.form.hasError('mismatch') && this.f('confirmPassword').touched; }

  // ── Canvas (login ile aynı sistem) ──────────────────────────
  private initCanvas() {
    const canvas = this.canvasRef.nativeElement;
    const ctx    = canvas.getContext('2d')!;
    const resize = () => {
      canvas.width  = window.innerWidth;
      canvas.height = window.innerHeight;
      this.buildParticles(canvas.width, canvas.height);
      this.buildGlowOrbs(canvas.width, canvas.height);
    };
    resize();
    window.addEventListener('resize', resize);
    const draw = () => {
      this.time++;
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      this.drawGlowOrbs(ctx, canvas.width, canvas.height);
      this.drawShootingStars(ctx, canvas.width, canvas.height);
      this.drawParticles(ctx, canvas.width, canvas.height);
      this.animFrame = requestAnimationFrame(draw);
    };
    draw();
  }

  private buildParticles(w: number, h: number) {
    this.particles = Array.from({ length: 90 }, () => ({
      x: Math.random() * w, y: Math.random() * h,
      vx: (Math.random() - 0.5) * 0.5, vy: (Math.random() - 0.5) * 0.5,
      r: Math.random() * 2 + 0.5, alpha: Math.random() * 0.6 + 0.2,
      pulse: Math.random() * Math.PI * 2,
    }));
  }

  private buildGlowOrbs(w: number, h: number) {
    const colors = [[37,99,235],[99,102,241],[14,165,233],[139,92,246]];
    this.glowOrbs = colors.map(c => ({
      x: Math.random() * w, y: Math.random() * h,
      r: Math.random() * 180 + 100,
      vx: (Math.random() - 0.5) * 0.3, vy: (Math.random() - 0.5) * 0.3,
      color: c, alpha: Math.random() * 0.06 + 0.04, phase: Math.random() * Math.PI * 2,
    }));
  }

  private drawGlowOrbs(ctx: CanvasRenderingContext2D, w: number, h: number) {
    this.glowOrbs.forEach(o => {
      o.x += o.vx; o.y += o.vy;
      if (o.x < -o.r) o.x = w + o.r; if (o.x > w + o.r) o.x = -o.r;
      if (o.y < -o.r) o.y = h + o.r; if (o.y > h + o.r) o.y = -o.r;
      const pulse = Math.sin(this.time * 0.01 + o.phase) * 0.02 + 1;
      const grad  = ctx.createRadialGradient(o.x, o.y, 0, o.x, o.y, o.r * pulse);
      grad.addColorStop(0, `rgba(${o.color},${o.alpha})`);
      grad.addColorStop(1, `rgba(${o.color},0)`);
      ctx.beginPath(); ctx.arc(o.x, o.y, o.r * pulse, 0, Math.PI * 2);
      ctx.fillStyle = grad; ctx.fill();
    });
  }

  private drawParticles(ctx: CanvasRenderingContext2D, w: number, h: number) {
    this.particles.forEach(p => {
      p.x += p.vx; p.y += p.vy;
      if (p.x < 0 || p.x > w) p.vx *= -1;
      if (p.y < 0 || p.y > h) p.vy *= -1;
      p.pulse += 0.02;
      const a = p.alpha * (0.7 + 0.3 * Math.sin(p.pulse));
      const halo = ctx.createRadialGradient(p.x, p.y, 0, p.x, p.y, p.r * 4);
      halo.addColorStop(0, `rgba(148,163,184,${a * 0.4})`);
      halo.addColorStop(1, `rgba(148,163,184,0)`);
      ctx.beginPath(); ctx.arc(p.x, p.y, p.r * 4, 0, Math.PI * 2); ctx.fillStyle = halo; ctx.fill();
      ctx.beginPath(); ctx.arc(p.x, p.y, p.r, 0, Math.PI * 2);
      ctx.fillStyle = `rgba(200,215,240,${a})`; ctx.fill();
    });
    for (let i = 0; i < this.particles.length; i++)
      for (let j = i + 1; j < this.particles.length; j++) {
        const dx = this.particles[i].x - this.particles[j].x;
        const dy = this.particles[i].y - this.particles[j].y;
        const d  = Math.sqrt(dx*dx + dy*dy);
        if (d < 130) {
          ctx.beginPath();
          ctx.moveTo(this.particles[i].x, this.particles[i].y);
          ctx.lineTo(this.particles[j].x, this.particles[j].y);
          ctx.strokeStyle = `rgba(148,163,220,${0.18*(1-d/130)})`; ctx.lineWidth = 0.7; ctx.stroke();
        }
      }
  }

  private drawShootingStars(ctx: CanvasRenderingContext2D, w: number, h: number) {
    if (this.time % 140 === 0)
      this.shootingStars.push({
        x: Math.random()*w*0.6, y: Math.random()*h*0.4,
        len: Math.random()*100+50, speed: Math.random()*7+5,
        alpha: 1, angle: Math.PI/4+(Math.random()-0.5)*0.3,
      });
    this.shootingStars = this.shootingStars.filter(s => s.alpha > 0);
    this.shootingStars.forEach(s => {
      s.x += Math.cos(s.angle)*s.speed; s.y += Math.sin(s.angle)*s.speed; s.alpha -= 0.02;
      const grad = ctx.createLinearGradient(s.x-Math.cos(s.angle)*s.len, s.y-Math.sin(s.angle)*s.len, s.x, s.y);
      grad.addColorStop(0, `rgba(200,215,255,0)`); grad.addColorStop(1, `rgba(200,215,255,${s.alpha})`);
      ctx.beginPath(); ctx.moveTo(s.x-Math.cos(s.angle)*s.len, s.y-Math.sin(s.angle)*s.len); ctx.lineTo(s.x, s.y);
      ctx.strokeStyle = grad; ctx.lineWidth = 1.5; ctx.stroke();
      ctx.beginPath(); ctx.arc(s.x, s.y, 2, 0, Math.PI*2); ctx.fillStyle = `rgba(255,255,255,${s.alpha})`; ctx.fill();
    });
  }
}

interface Particle     { x:number; y:number; vx:number; vy:number; r:number; alpha:number; pulse:number; }
interface ShootingStar { x:number; y:number; len:number; speed:number; alpha:number; angle:number; }
interface GlowOrb      { x:number; y:number; r:number; vx:number; vy:number; color:number[]; alpha:number; phase:number; }
