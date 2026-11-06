import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { PurchaseBill } from './features/purchase-bill/purchase-bill';
import { Dashboard } from './features/dashboard/dashboard';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: '',
        component: Login
    },
    {
        path: 'dashboard',
        component: Dashboard,
        canActivate: [authGuard]
    },
    {
        path: 'purchase-bill',
        component: PurchaseBill,
        canActivate: [authGuard]
    },
    {
        path: '**',
        redirectTo: ''
    }
];
