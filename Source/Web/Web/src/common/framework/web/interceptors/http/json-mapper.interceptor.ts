import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getListOfAttributeDecorators } from '../../../common/decorators/alias.decorator';
import { tap } from 'rxjs/operators';

@Injectable()
export class JsonMapperInterceptor<T> implements HttpInterceptor {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<T>> {
    return next.handle(req).pipe(
      tap(event => {
        if (event instanceof HttpResponse) {
          // eslint-disable-next-line 
          const resp: HttpResponse<T> = event;
          const body: T | null = resp.body;
          this.recurseBody(body);
        }
      }));
  }

  private renameKeys(obj: { [key: string]: unknown; }, newKeys: { [key: string]: string; }): object {
    const keyValues = Object.keys(obj).map(key => {
      // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
      const newKey = newKeys[key] || key;
      // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
      return { [newKey]: obj[key] };
    });
    // eslint-disable-next-line @typescript-eslint/no-unsafe-return
    return Object.assign({}, ...keyValues);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private recurseBody(body: any): T | undefined {
    if (!body) {
      return;
    }

    // tslint:disable-next-line:forin
    for (const key in body) {
      // eslint-disable-next-line 
      if (body.hasOwnProperty(key)) {
        const sLst = getListOfAttributeDecorators(body as T, key);
        if (!(sLst == null || sLst.length === 0)) {
          // 
        }
        // eslint-disable-next-line
        const element = body[key];
        if (typeof element === 'object') {
          // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
          this.recurseBody(element);
        } else {
          // eslint-disable-next-line
          body = this.renameKeys(body, {});
        }
      }
    }

    return;
  }
}
