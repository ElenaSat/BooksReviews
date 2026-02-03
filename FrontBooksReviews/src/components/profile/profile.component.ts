import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (authService.currentUser(); as user) {
      <div class="max-w-2xl mx-auto">
        <div class="bg-white rounded-2xl shadow-sm border border-slate-100 overflow-hidden">
          <div class="bg-gradient-to-r from-blue-500 to-indigo-600 h-32"></div>
          
          <div class="px-8 pb-8">
            <div class="relative flex justify-between items-end -mt-12 mb-6">
              <img [src]="user.avatarUrl" [alt]="user.name" class="w-24 h-24 rounded-full border-4 border-white shadow-md bg-white">
              <button (click)="authService.logout(); router.navigate(['/'])" class="mb-1 text-sm text-red-600 hover:text-red-700 font-medium">
                Cerrar Sesión
              </button>
            </div>
            
            <h1 class="text-2xl font-bold text-slate-800">{{ user.name }}</h1>
            <p class="text-slate-500 mb-6">{{ user.email }}</p>
            
            <div class="grid grid-cols-2 gap-4 border-t border-slate-100 pt-6">
              <div>
                <span class="block text-xs font-semibold text-slate-400 uppercase tracking-wider">ID de Miembro</span>
                <span class="text-slate-700 font-mono text-sm">{{ user.id }}</span>
              </div>
              <div>
                 <span class="block text-xs font-semibold text-slate-400 uppercase tracking-wider">Estado</span>
                 <span class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                   Activo
                 </span>
              </div>
            </div>
          </div>
        </div>
        
        <div class="mt-8 text-center text-slate-400 text-sm">
          <p>La función de restablecer contraseña está deshabilitada en esta demo.</p>
        </div>
      </div>
    }
  `
})
export class ProfileComponent {
  authService = inject(AuthService);
  router = inject(Router);
}