import { Routes } from '@angular/router';
import { LandingComponent }          from './pages/landing/landing';
import { LoginComponent }            from './pages/auth/login/login';
import { RegisterComponent }         from './pages/auth/register/register';
import { DashboardComponent }        from './pages/dashboard/dashboard';
import { FinancialProfileComponent } from './pages/financial-profile/financial-profile';
import { GoalsComponent }            from './pages/goals/goals';
import { RiskTestComponent }         from './pages/risk-test/risk-test';
import { AdviceComponent }           from './pages/advice/advice';
import { SettingsComponent }         from './pages/settings/settings';
import { DebtsComponent }           from './pages/debts/debts';
import { ScoreHistoryComponent }    from './pages/score-history/score-history';
import { MonteCarloComponent }      from './pages/monte-carlo/monte-carlo';
import { RetirementComponent }      from './pages/retirement/retirement';
import { DebtStrategyComponent }    from './pages/debt-strategy/debt-strategy';
import { authGuard }                from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '',                   component: LandingComponent          },
  { path: 'auth/login',         component: LoginComponent            },
  { path: 'auth/register',      component: RegisterComponent         },
  { path: 'dashboard',          component: DashboardComponent,        canActivate: [authGuard] },
  { path: 'financial-profile',  component: FinancialProfileComponent, canActivate: [authGuard] },
  { path: 'goals',              component: GoalsComponent,            canActivate: [authGuard] },
  { path: 'risk-test',          component: RiskTestComponent,         canActivate: [authGuard] },
  { path: 'advice',             component: AdviceComponent,           canActivate: [authGuard] },
  { path: 'settings',          component: SettingsComponent,         canActivate: [authGuard] },
  { path: 'debts',             component: DebtsComponent,            canActivate: [authGuard] },
  { path: 'score-history',     component: ScoreHistoryComponent,     canActivate: [authGuard] },
  { path: 'monte-carlo',      component: MonteCarloComponent,       canActivate: [authGuard] },
  { path: 'retirement',       component: RetirementComponent,       canActivate: [authGuard] },
  { path: 'debt-strategy',    component: DebtStrategyComponent,     canActivate: [authGuard] },
];
