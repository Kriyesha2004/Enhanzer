
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { CommonModule } from '@angular/common';

import { Auth } from '../../../core/services/auth';
import { LoginResponse } from '../../../core/models/auth.model';
//import { LoginResponse } from '../../../core/models/auth.model';
import { Router } from '@angular/router';
import { Session } from '../../../core/services/session';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  loginForm: FormGroup;
  loading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: Auth,
    private session: Session,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }


  onSubmit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading = true;

    const request = {
      email: this.loginForm.value.email.trim(),
      password: this.loginForm.value.password
    };

    this.authService.login(request).subscribe({
      next: (response: LoginResponse) => {
        this.loading = false;

        this.successMessage = response.message;

        console.log('Login response:', response);
        console.log('User locations:', response.locations);

        // Save login session
        this.session.login();

        // Navigate to Purchase Bill page
        this.router.navigate(['/purchase-bill']);
      },

      error: (error) => {
        this.loading = false;

        console.log('HTTP Status:', error.status);
        console.log('Full error:', error);
        console.log('Backend response:', error.error);

        this.errorMessage =
          error.error?.message || 'Login failed. Please try again.';
      }
    });
  }
}
