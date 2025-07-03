import { BlockUI, NgBlockUI } from 'ng-block-ui';
import {
    HttpErrorResponse,
    HttpEvent,
    HttpHandler,
    HttpHeaders,
    HttpInterceptor,
    HttpRequest,
    HttpResponse
} from '@angular/common/http';
import { Observable, interval, of, throwError } from 'rxjs';
import { catchError, flatMap, map, retryWhen } from 'rxjs/operators';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { Injectable } from '@angular/core';
import { SessionService } from '../framework/web/infrastructure/session/session.service';

// import { ErrorDialogService } from '../error-dialog/errordialog.service';

const ignoreBlockUIStr = 'ignoreBlockUI';
const companyIdStr = 'X-CompanyId';
const loadingTextStr = 'loadingText';
const ignoredUrlFragments = [ignoreBlockUIStr];

export function getHeaderWithBlockUI(): HttpHeaders {
    return (new HttpHeaders()).set(ignoreBlockUIStr, 'True');
}

export function getHeaderWithCompanyId(companyId: number): HttpHeaders {
    return (new HttpHeaders()).set(companyIdStr, `${companyId}`);
}

export function getHeaderWithLoadingText(loadingText: string): HttpHeaders {
    return (new HttpHeaders()).set(loadingTextStr, loadingText);
}

@Injectable()
export class HttpConfigInterceptor implements HttpInterceptor {

    @BlockUI() public blockUI: NgBlockUI | undefined;
    public intervalBetweenRetry = 1000;
    public isErrorMessageShown = false;
    public retryNumber = 2;
    public constructor(
        private sessionService: SessionService,
        private dialogService: CommonDialogService,
    ) {

    }
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
 
        if (this.sessionService.isUserLoggedIn()) {
            const currentUser = this.sessionService.currentUser;
            request = request.clone({
                setHeaders: {
                    'X-YesLaw-Token': `${currentUser?.token}`
                }
            });
        }
        // if (!request.headers.has('Content-Type')) {
        //     request = request.clone({ headers: request.headers.set('Content-Type', 'application/json') });
        // }
        request = request.clone({ headers: request.headers.set('Accept', 'application/json') });
        let canBlock = true;
        // tslint:disable-next-line:prefer-for-of
        for (let index = 0; index < ignoredUrlFragments.length; index++) {
            const element = ignoredUrlFragments[index]?.toUpperCase() ?? '';
            for (const key of request.headers.keys()) {
                if (key.toUpperCase().includes(element)) {
                    canBlock = false;
                    break;
                }
            }

            if (request.urlWithParams.toUpperCase().includes(element)) {
                canBlock = false;
                break;
            }
        }

        let loadingText = 'Loading ...';

        for (const key of request.headers.keys()) {
            if (key.toUpperCase().includes(loadingTextStr.toUpperCase())) {
                // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
                loadingText = request.headers.get(key) || loadingText;
                break;
            }
        }

        if (canBlock) {
            this.blockUI?.start(loadingText);
        }
        // Start blocking
        return next.handle(request).pipe(
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            map((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {
                    this.stopBlockUI(); // Stop blocking
                }
                return event;
            }),
            catchError((error: HttpErrorResponse) => {
                this.stopBlockUI(); // Stop blocking
                if (error.status === 401) {
                    // auto logout if 401 response returned from api
                    this.sessionService.logOut();
                    this.dialogService.errorWithTitle('Unauthorized', 'Session has expired. Please login again');
                } else if (error.status === 500) {
                    this.dialogService.errorWithTitle('HTTP Unhandle Exception', error.message);
                } else {
                    if (!this.isErrorMessageShown) {
                        this.isErrorMessageShown = true;
                        this.dialogService.errorWithTitle('HTTP Unhandle Exception', 'Unable to connect to the Remote API server');
                    }
                }
                // alert(error.error.message);
                return throwError(error);
            }),
            retryWhen((error: unknown) => interval(this.intervalBetweenRetry).pipe(
                flatMap(count => count === this.retryNumber ? throwError(error) : of(count))
            ))

        );
    }

    public stopBlockUI(): void {
        this.blockUI?.stop(); // Stop blocking
    }
}
