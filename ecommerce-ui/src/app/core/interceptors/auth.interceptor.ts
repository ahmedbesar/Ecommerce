import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  let request = req;
  if (token) {
    request = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
  }

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      // If we get a 401 Unauthorized and it's not the token endpoint itself
      if (error.status === 401 && !req.url.includes('/connect/token')) {
        return authService.refreshToken().pipe(
          switchMap((res) => {
            // Clone the original request and attach the new token
            const retryReq = req.clone({
              headers: req.headers.set('Authorization', `Bearer ${res.access_token}`)
            });
            return next(retryReq);
          }),
          catchError((refreshError) => {
            // The refresh token failed, logout is already handled inside refreshToken()
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  );
};
