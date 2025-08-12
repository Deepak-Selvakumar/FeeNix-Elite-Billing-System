import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  // This is a simple mocked auth service. Replace with real HTTP calls.
  async login(email: string, password: string, remember: boolean): Promise<void> {
    // simulate network latency
    await new Promise(r => setTimeout(r, 800));

    // Dummy validation check
    if (email === 'user@example.com' && password === 'password123') {
      // store token
      const token = 'fake-jwt-token';
      if (remember) {
        localStorage.setItem('auth_token', token);
      } else {
        sessionStorage.setItem('auth_token', token);
      }
      return;
    }

    // throw an error that the component will display
    throw new Error('Invalid email or password');
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    sessionStorage.removeItem('auth_token');
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token') || sessionStorage.getItem('auth_token');
  }
}
