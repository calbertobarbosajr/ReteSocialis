// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = 'http://localhost:5000/api';
  private readonly tokenKey = 'rs_token';
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  isLogged$ = this.isLoggedInSubject.asObservable();

  readonly login$ = new Subject<void>();
  readonly logout$ = new Subject<void>();

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string) {
    return this.http
      .post<{ token: string }>(`${this.baseUrl}/auth/login`, { username, password })
      .pipe(tap(res => this.setToken(res.token)));
  }

  register(username: string, email: string, password: string) {
    return this.http
      .post<{ token: string }>(`${this.baseUrl}/auth/register`, { username, email, password })
      .pipe(tap(res => this.setToken(res.token)));
  }

  // ✅ corrigido
  getCurrentUser() {
  const token = this.getToken();
  if (!token) throw new Error('Usuário não autenticado');

  const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
  return this.http.get<{ id: string; userName: string; email: string; avatarUrl?: string }>(
    `${this.baseUrl}/users/me`,
    { headers }
  );
}

  setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
    this.isLoggedInSubject.next(true);
    this.login$.next();
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.isLoggedInSubject.next(false);
    this.logout$.next();
    this.router.navigate(['/login']);
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }
} 