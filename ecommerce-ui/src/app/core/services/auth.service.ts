import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { TokenResponse, UserSession } from '../models/auth.model';

const SESSION_KEY = 'currentUser';

function parseJwtPayload(token: string): Record<string, any> {
  try {
    const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    const json = atob(base64);
    return JSON.parse(json);
  } catch {
    return {};
  }
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _isLoggedIn = signal<boolean>(this.checkSession());

  readonly isLoggedIn = this._isLoggedIn.asReadonly();

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string): Observable<TokenResponse> {
    const body = new URLSearchParams();
    body.set('grant_type', 'password');
    body.set('client_id', environment.clientId);
    body.set('client_secret', environment.clientSecret);
    body.set('username', username);
    body.set('password', password);
    body.set('scope', 'openid profile email roles ecommerce.api');

    const headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

    return this.http
      .post<TokenResponse>(`${environment.identityApiUrl}/connect/token`, body.toString(), { headers })
      .pipe(
        tap((res) => this.storeSession(res, username)),
        catchError((err) => {
          const message = err?.error?.error_description || 'Login failed. Please check your credentials.';
          return throwError(() => new Error(message));
        })
      );
  }

  logout(): void {
    sessionStorage.removeItem(SESSION_KEY);
    this._isLoggedIn.set(false);
    this.router.navigate(['/login']);
  }

  getCurrentUser(): UserSession | null {
    const raw = sessionStorage.getItem(SESSION_KEY);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as UserSession;
    } catch {
      return null;
    }
  }

  getToken(): string | null {
    return this.getCurrentUser()?.accessToken ?? null;
  }

  private storeSession(res: TokenResponse, username: string): void {
    const payload = parseJwtPayload(res.access_token);
    const roles: string[] = Array.isArray(payload['role'])
      ? payload['role']
      : payload['role']
      ? [payload['role']]
      : [];

    const session: UserSession = {
      username: payload['name'] || payload['preferred_username'] || username,
      email: payload['email'] || '',
      accessToken: res.access_token,
      refreshToken: res.refresh_token,
      expiresAt: Date.now() + res.expires_in * 1000,
      roles
    };

    sessionStorage.setItem(SESSION_KEY, JSON.stringify(session));
    this._isLoggedIn.set(true);
  }

  private checkSession(): boolean {
    const user = this.getCurrentUser();
    if (!user) return false;
    return user.expiresAt > Date.now();
  }
}
