import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse }
  from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { tap, catchError } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const token = localStorage.getItem('authToken');

      let cloneReq = req;

      if (token) {
        cloneReq = req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        });
      }

      return next.handle(cloneReq).pipe(
        catchError((error: HttpErrorResponse) => {
          console.error('HTTP Error:', error);
          // Додаткова логіка обробки помилок
          return throwError(error);
        })
      );
    }

}
