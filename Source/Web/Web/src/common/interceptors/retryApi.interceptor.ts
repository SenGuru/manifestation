import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, interval, of, throwError } from 'rxjs';
import { delay, flatMap, retry, retryWhen, take } from 'rxjs/operators';

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RetryApiInterceptor implements HttpInterceptor {

  private intervalBetweenRetry = 1000;
  private retryNumber = 2;

 public constructor() {
    // 
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
 public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(req).pipe(
      // retryWhen((errors: Observable<HttpErrorResponse>) => interval(this.intervalBetweenRetry).pipe(
      //     flatMap(count => count === this.retryNumber ? throwError(errors) : of(count))
      //   ))
      retry({
        delay: (error, retryCount) => {
          if (retryCount < this.retryNumber) {
            return interval(this.intervalBetweenRetry);
          } else {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-return
            return throwError(() => error);
          }
        }
      })
    );

  }

}
