import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="max-w-md mx-auto mt-12 bg-white p-8 rounded-2xl shadow-sm border border-slate-100">
      <div class="text-center mb-8">
        <h2 class="text-2xl font-bold text-slate-800">Bienvenido de nuevo</h2>
        <p class="text-slate-500">Inicia sesión en tu cuenta de BookWorm</p>
      </div>

      <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" class="space-y-6">
        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Correo Electrónico</label>
          <input type="email" formControlName="email" class="w-full px-4 py-3 rounded-xl border border-slate-200 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none transition-all">
          @if (loginForm.get('email')?.touched && loginForm.get('email')?.invalid) {
            <p class="text-red-500 text-xs mt-1">Por favor ingresa un correo válido.</p>
          }
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Contraseña</label>
          <input type="password" formControlName="password" class="w-full px-4 py-3 rounded-xl border border-slate-200 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none transition-all">
          @if (loginForm.get('password')?.touched && loginForm.get('password')?.invalid) {
            <p class="text-red-500 text-xs mt-1">La contraseña es obligatoria.</p>
          }
        </div>

        <button type="submit" [disabled]="loginForm.invalid" class="w-full py-3 bg-blue-600 hover:bg-blue-700 text-white rounded-xl font-bold transition-colors shadow-md shadow-blue-200 disabled:opacity-50">
          Iniciar Sesión
        </button>

        @if (error) {
          <div class="p-3 bg-red-50 text-red-600 rounded-lg text-sm text-center border border-red-100 animate-shake">
            {{ error }}
          </div>
        }

        <div class="text-center text-sm text-slate-500">
          ¿No tienes una cuenta? <a routerLink="/register" class="text-blue-600 font-medium hover:underline">Regístrate</a>
        </div>
      </form>
    </div>
  `,
  styles: [`
    .animate-shake {
      animation: shake 0.5s cubic-bezier(.36,.07,.19,.97) both;
    }
    @keyframes shake {
      10%, 90% { transform: translate3d(-1px, 0, 0); }
      20%, 80% { transform: translate3d(2px, 0, 0); }
      30%, 50%, 70% { transform: translate3d(-4px, 0, 0); }
      40%, 60% { transform: translate3d(4px, 0, 0); }
    }
  `]
})
export class LoginComponent {
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  router: Router = inject(Router);
  error = '';

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  async onSubmit() {
    if (this.loginForm.valid) {
      this.error = '';
      const { email, password } = this.loginForm.value;
      try {
        const success = await this.authService.login(email!, password!);
        if (success) {
          this.router.navigate(['/']);
        } else {
          this.error = 'Credenciales inválidas';
        }
      } catch (err) {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            this.error = 'Correo o contraseña incorrectos.';
          } else if (err.status === 0) {
            this.error = 'No se pudo conectar con el servidor. Revisa tu conexión.';
          } else {
            this.error = 'Ocurrió un error inesperado. Inténtalo de nuevo.';
          }
        } else {
          this.error = 'Ocurrió un error desconocido.';
        }
      }
    }
  }
}