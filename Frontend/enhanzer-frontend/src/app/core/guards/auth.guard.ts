import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Session } from '../services/session';

export const authGuard: CanActivateFn = () => {
    const session = inject(Session);
    const router = inject(Router);

    if (session.isLoggedIn()) {
        return true;
    }

    return router.createUrlTree(['/']);
};