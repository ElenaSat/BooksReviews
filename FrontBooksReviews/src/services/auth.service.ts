import { Injectable, inject, signal, computed } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/models';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Users';

  // Signal to hold the current authenticated user
  private _currentUser = signal<User | null>(null);

  currentUser = computed(() => this._currentUser());
  isAuthenticated = computed(() => !!this._currentUser());

  constructor() {
    // Attempt to restore session from local storage
    const stored = localStorage.getItem('bookworm_user');
    if (stored) {
      try {
        this._currentUser.set(JSON.parse(stored));
      } catch (e) {
        console.error('Failed to parse user session');
      }
    }
  }

  async login(email: string, password: string): Promise<boolean> {
    try {
      const result = await lastValueFrom(this.http.post<User>(`${this.apiUrl}/login`, { email, password }));

      if (result) {
        this._currentUser.set(result);
        localStorage.setItem('bookworm_user', JSON.stringify(result));
        return true;
      }
    } catch (error) {
      console.error('Login failed:', error);
    }
    return false;
  }

  async register(name: string, email: string, password: string): Promise<boolean> {
    if (!email || !name || !password) return false;

    const registerData = {
      id: 'u-' + Math.floor(Math.random() * 10000).toString(),
      name: name,
      email: email,
      password: password,
      avatarUrl: `https://ui-avatars.com/api/?name=${encodeURIComponent(name)}&background=0D8ABC&color=fff`
    };

    try {
      await lastValueFrom(this.http.post(this.apiUrl, registerData, {
        responseType: 'text'
      }));
      return true;
    } catch (error) {
      console.error('Registration failed:', error);
      return false;
    }
  }

  logout() {
    this._currentUser.set(null);
    localStorage.removeItem('bookworm_user');
  }
}
