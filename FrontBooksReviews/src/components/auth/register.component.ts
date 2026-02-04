import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="max-w-md mx-auto mt-12 bg-white p-8 rounded-2xl shadow-sm border border-slate-100">
      <div class="text-center mb-8">
        <h2 class="text-2xl font-bold text-slate-800">Crear Cuenta</h2>
        <p class="text-slate-500">Únete a la comunidad de amantes de los libros</p>
      </div>

      @if (successMessage()) {
        <div class="mb-6 p-4 bg-green-50 border border-green-200 text-green-700 rounded-xl text-center font-medium animate-bounce">
          {{ successMessage() }}
        </div>
      }

      @if (error()) {
        <div class="mb-6 p-4 bg-red-50 border border-red-200 text-red-700 rounded-xl text-center font-medium">
          {{ error() }}
        </div>
      }

      <form [formGroup]="registerForm" (ngSubmit)="onSubmit()" class="space-y-6">
        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Nombre Completo</label>
          <input type="text" formControlName="name" class="w-full px-4 py-3 rounded-xl border border-slate-200 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none transition-all">
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Correo Electrónico</label>
          <input type="email" formControlName="email" class="w-full px-4 py-3 rounded-xl border border-slate-200 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none transition-all">
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Contraseña</label>
          <input type="password" formControlName="password" class="w-full px-4 py-3 rounded-xl border border-slate-200 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none transition-all">
        </div>

        <button type="submit" [disabled]="registerForm.invalid" class="w-full py-3 bg-slate-900 hover:bg-black text-white rounded-xl font-bold transition-colors shadow-md disabled:opacity-50">
          Crear Cuenta
        </button>

        <div class="text-center text-sm text-slate-500">
          ¿Ya tienes una cuenta? <a routerLink="/login" class="text-blue-600 font-medium hover:underline">Inicia sesión</a>
        </div>
      </form>
    </div>
  `
})
export class RegisterComponent {
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  router: Router = inject(Router);

  registerForm = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  successMessage = signal('');
  error = signal('');

  async onSubmit() {
    this.error.set('');
    if (this.registerForm.valid) {
      const { name, email, password } = this.registerForm.value;
      try {
        const success = await this.authService.register(name!, email!, password!);
        if (success) {
          this.successMessage.set('¡Cuenta creada satisfactoriamente! Redirigiendo al login...');
          setTimeout(() => {
            console.log('Navigating to login...');
            this.router.navigateByUrl('/login');
          }, 3000);
        } else {
          this.error.set('No se pudo crear la cuenta. Por favor verifica los datos.');
        }
      } catch (err) {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 400) {
            this.error.set('Los datos son inválidos o el correo ya está registrado.');
          } else if (err.status === 0) {
            this.error.set('No hay conexión con el servidor.');
          } else {
            this.error.set('Ocurrió un error al procesar el registro.');
          }
        }
      }
    }
  }
}