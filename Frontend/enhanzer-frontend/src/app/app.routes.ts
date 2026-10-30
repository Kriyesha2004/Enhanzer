import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { PurchaseBill } from './features/purchase-bill/purchase-bill';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: '',
        component: Login
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