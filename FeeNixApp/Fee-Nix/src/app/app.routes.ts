import { Routes } from '@angular/router';
import { LoginComponent } from './MasterComponents/login/login.component';
import { CreateAccountComponent } from './MasterComponents/create-account/create-account.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'create-account', component: CreateAccountComponent },
  { path: '**', redirectTo: '/login' }
];