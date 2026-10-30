import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class Session {
    private readonly sessionKey = 'enhanzer_logged_in';

    login(): void {
        sessionStorage.setItem(this.sessionKey, 'true');
    }

    logout(): void {
        sessionStorage.removeItem(this.sessionKey);
    }

    isLoggedIn(): boolean {
        return sessionStorage.getItem(this.sessionKey) === 'true';
    }
}