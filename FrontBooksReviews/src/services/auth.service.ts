import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/models';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7151/api/Users';

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
      // In this demo, "login" is just finding the user by email
      const users = await lastValueFrom(this.http.get<User[]>(this.apiUrl));
      const user = users.find(u => u.email.toLowerCase() === email.toLowerCase());

      if (user) {
        this._currentUser.set(user);
        localStorage.setItem('bookworm_user', JSON.stringify(user));
        return true;
      }
    } catch (error) {
      console.error('Login failed:', error);
    }
    return false;
  }

  async register(name: string, email: string, password: string): Promise<boolean> {
    if (!email || !name) return false;

    const newUser: User = {
      id: 'u-' + Math.floor(Math.random() * 10000).toString(),
      name: name,
      email: email,
      avatarUrl: `https://ui-avatars.com/api/?name=${encodeURIComponent(name)}&background=0D8ABC&color=fff`
    };

    try {
      await lastValueFrom(this.http.post(this.apiUrl, newUser));
      this._currentUser.set(newUser);
      localStorage.setItem('bookworm_user', JSON.stringify(newUser));
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
