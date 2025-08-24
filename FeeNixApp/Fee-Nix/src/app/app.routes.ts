import { Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from './auth/helpers';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard/dashboard.component';
import { RoleGuard } from './auth/helpers/role.guard'; 

export const routes: Routes = [
    {
        path: '',
        component: DashboardComponent,
        canActivate: [AuthGuard],
        title: 'SELBIllApp | SEL',
        data: { title: 'SELBIllApp' }
    }
    // ,
    // {
    //     path: 'grgeneratedBills/List',
    //     loadComponent: () => import('./Billing/gr-generate-list/gr-generate-list.component').then(m => m.GrGenerateListComponent),
    //     title: 'SEL- Bill Processing | SEL',
    //     canActivate: [AuthGuard],
    //     data: { title: 'SEL' }
    // },
    // {
    //     path: 'grgeneratedlist',
    //     loadComponent: () => import('./Billing/gr-generate-list/gr-generate-list.component').then(m => m.GrGenerateListComponent),
    //     title: 'SEL- Bill Processing | SEL',
    //     canActivate: [AuthGuard],
    //     data: { title: 'SEL' }
    // }
];