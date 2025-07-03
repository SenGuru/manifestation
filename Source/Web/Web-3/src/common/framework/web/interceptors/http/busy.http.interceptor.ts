import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';

import { Injectable } from '@angular/core';
import { LoadingSpinnerService } from '../../components/ui/spinner/loading-spinner.module';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class BusyHttpInterceptor implements HttpInterceptor {

  public constructor(private spinnerService: LoadingSpinnerService) {
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // start our loader here
    this.spinnerService.show();
    return next.handle(req).pipe(
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      tap((event: HttpEvent<any>) => {
        // if the event is for http response
        if (event instanceof HttpResponse) {
          // stop our loader here
          this.spinnerService.hide();
        }
        // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-explicit-any
      }, (err: any) => {
        // if any error (not for just HttpResponse) we stop our loader bar
        this.spinnerService.hide();
      }));
  }
}
