import { HttpInterceptorFn } from '@angular/common/http';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse }
  from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { tap, catchError } from 'rxjs/operators';

@Injectable()
export class CharactersInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      if (req.url.includes('Characters')) {
        console.log(`Intercepted request to: ${req.url}`);
      }

      return next.handle(req).pipe(
        tap((event) => {
          // Перевіряємо, чи це відповідь від сервера
          if (event instanceof HttpResponse && req.url.includes('Characters')) {
            console.log('Intercepted response:', event.body);
          }
        }),
        catchError((error) => {
          console.error('Error intercepted:', error);
          // Обробляємо помилки
          throw error; // або повертаємо Observable з помилкою
        })
      );
    }

}
