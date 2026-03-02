import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

export type UserRole = 'Admin' | 'Buyer' | 'Seller';

export interface AuthUser {
  userName: string;
  role: UserRole;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly storageKey = 'ecomm_auth_user';
  private readonly isBrowser: boolean;
  private memoryStore: Record<string, string> = {};

  constructor(private router: Router, @Inject(PLATFORM_ID) platformId: Object) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  private getItem(key: string): string | null {
    if (this.isBrowser && typeof window !== 'undefined' && window.localStorage) {
      return window.localStorage.getItem(key);
    }
    return this.memoryStore[key] ?? null;
  }

  private setItem(key: string, value: string): void {
    if (this.isBrowser && typeof window !== 'undefined' && window.localStorage) {
      window.localStorage.setItem(key, value);
    } else {
      this.memoryStore[key] = value;
    }
  }

  private removeItem(key: string): void {
    if (this.isBrowser && typeof window !== 'undefined' && window.localStorage) {
      window.localStorage.removeItem(key);
    } else {
      delete this.memoryStore[key];
    }
  }

  get currentUser(): AuthUser | null {
    const raw = this.getItem(this.storageKey);
    return raw ? (JSON.parse(raw) as AuthUser) : null;
  }

  get isLoggedIn(): boolean {
    return !!this.currentUser;
  }

  get role(): UserRole | null {
    return this.currentUser?.role ?? null;
  }

  login(userName: string, role: UserRole): void {
    const user: AuthUser = { userName, role };
    this.setItem(this.storageKey, JSON.stringify(user));
  }

  logout(): void {
    this.removeItem(this.storageKey);
    this.router.navigate(['/']);
  }
}
