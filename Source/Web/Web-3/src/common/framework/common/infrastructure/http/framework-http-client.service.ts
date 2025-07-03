import { BaseHttpClientService } from './base-http-client.service';
import { ConfigService } from '../config/config.service';
import { HttpHeaders } from '@angular/common/http';
import { IStringIndexedObjModel } from '../../models/i-string-indexed-object,model';
import { Injectable } from '@angular/core';
import { ObjectUtils } from '../../utils/object.utils';
import { Observable } from 'rxjs';
import { ReturnListModel } from '../../models/result/return-list.model';
import { ReturnModel } from '../../models/result/return.model';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Injectable({
    providedIn: 'root'
})
export class FrameworkHttpClientService {

    public constructor(
        private baseHttpClientService: BaseHttpClientService,
        private configService: ConfigService,
        private sessionService: SessionService) {
    }

    public getHeader(additionalHeaders?: HttpHeaders): HttpHeaders {
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Accept', 'application/json');
        if (this.sessionService.getCurrentToken() !== null) {
            headers = headers.set('Authorization', `Bearer ${this.sessionService.getCurrentToken()}`);
        }

        if (!ObjectUtils.isObjNullOrUndefined(additionalHeaders)) {
            if (additionalHeaders instanceof HttpHeaders) {
                const newH = additionalHeaders as HttpHeaders;
                for (const hi of newH.keys()) {
                    const objValue = newH.get(hi);
                    if (!ObjectUtils.isNullOrUndefined(objValue)) {
                        headers = headers.set(hi, objValue ?? '');
                    }
                }
            } else {
                const ah = additionalHeaders ?? {};
                for (const header of Object.keys(ah)) {
                    //@ts-expect-error  --- following line expected to have an error
                    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
                    headers = headers.set(header, ah[header] ?? '');
                }
            }
        }
        return headers;
    }

    public getUploadHeaders(additionalHeaders?: HttpHeaders): HttpHeaders {
        let headers = new HttpHeaders();
        // headers = headers.set('Content-Type', 'multipart/form-data');
        headers = headers.set('Accept', 'text/plain');
        if (this.sessionService.getCurrentToken() !== null) {
            headers = headers.set('Authorization', `Bearer ${this.sessionService.getCurrentToken()}`);
        } 

        if (additionalHeaders != null) {
            if (additionalHeaders instanceof HttpHeaders) {
                for (const key of additionalHeaders.keys()) {
                    // @ts-expect-error: This line is expected to have an error
                     
                    headers = headers.set(key, additionalHeaders.get(key));
                }
            } else {
                for (const header of Object.keys(additionalHeaders)) {
                     
                    headers = headers.set(header, additionalHeaders[header]);
                }
            }
        }
        return headers;
    }

    public getInMemory<T>(url: string, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.get<T>(url, this.getHeader(headers));
    }

    public getUrl(url: string, authenticationKey?: string): string {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.getUrl((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, authenticationKey);
    }

    public get<T>(url: string, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.get<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getHeader(headers));
    }

    public simpleGet<T>(url: string, headers?: HttpHeaders): Observable<T> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simpleGet<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getHeader(headers));
    }

    public getWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.getWithParams((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, queryModel, this.getHeader(headers));
    }

    public getUrlWithParams(url: string, queryModel: unknown, authenticationKey?: string): string {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.getUrlWithParams((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, queryModel, authenticationKey);
    }

    public simpleGetWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<T> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simpleGetWithParams((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, queryModel, this.getHeader(headers));
    }

    public getList<T>(url: string, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.getList<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getHeader(headers));
    }

    public simpleGetList<T>(url: string, headers?: HttpHeaders): Observable<T[]> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simpleGetList<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getHeader(headers));
    }

    public getListWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.getListWithParams<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, queryModel, this.getHeader(headers));
    }

    public simpleGetListWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<T[]> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simpleGetListWithParams<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, queryModel, this.getHeader(headers));
    }

    public post<T>(url: string, model: T, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.post<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public simplePost<T>(url: string, model: T, headers?: HttpHeaders): Observable<T> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simplePost<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public postWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.postWithModel<T, X>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public simplePostWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<T> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simplePostWithModel<T, X>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public postList<T>(url: string, model: T, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.postList<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public simplePostList<T>(url: string, model: T, headers?: HttpHeaders): Observable<T[]> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simplePostList<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public postListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.postListWithModel<T, X>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public simplePostListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<T[]> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simplePostListWithModel<T, X>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public put<T>(url: string, model: T, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.put<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public simplePut<T>(url: string, model: T, headers?: HttpHeaders): Observable<T> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simplePut<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public putWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.putWithModel<T, X>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public simplePutWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<T> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simplePutWithModel<T, X>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public putList<T>(url: string, model: T, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.putList<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public simplePutList<T>(url: string, model: T, headers?: HttpHeaders): Observable<T[]> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simplePutList<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public putListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.putListWithModel<T, X>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public simplePutListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<T[]> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simplePutListWithModel<T, X>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, model, this.getHeader(headers));
    }

    public delete<T>(url: string, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.delete<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getHeader(headers));
    }

    public simpleDelete<T>(url: string, headers?: HttpHeaders): Observable<T> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simpleDelete<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getHeader(headers));
    }

    public deleteWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.deleteWithParams<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, queryModel, this.getHeader(headers));
    }

    public simpleDeleteWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<T> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.simpleDeleteWithParams<T>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, queryModel, this.getHeader(headers));
    }

    public postWithUploadWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.postWithModel<T, FormData>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getAsFormData(model), this.getUploadHeaders(headers));
    }

    public postWithUploadListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.postListWithModel<T, FormData>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getAsFormData(model), this.getUploadHeaders(headers));
    }

    public putWithUploadWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.putWithModel<T, FormData>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getAsFormData(model), this.getUploadHeaders(headers));
    }

    public putWithUploadListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        // tslint:disable-next-line: max-line-length
        return this.baseHttpClientService.putListWithModel<T, FormData>((this.configService.serviceConfiguration.baseApiServerUrl ?? '') + this.configService.serviceConfiguration.baseApiServerQueryParam + url, this.getAsFormData(model), this.getUploadHeaders(headers));
    }

    // eslint-disable-next-line
    public getAsFormData(model: any): FormData {
        const formData: FormData = new FormData();
        for (const prop in model) {
            // eslint-disable-next-line
            if (!model.hasOwnProperty(prop)) {
                continue;
            }
            // eslint-disable-next-line
            const element = model[prop];
            if (ObjectUtils.isNullOrUndefined(element)) {
                continue;
            }
            if (element?.constructor.name === 'Number') {
                // eslint-disable-next-line
                formData.set(prop, element);
            }
            else if (element?.constructor.name === 'Date') {
                // eslint-disable-next-line
                formData.set(prop, element.toISOString());
            }
            else if (element?.constructor.name === 'Array') {
                if (element.length === 0) {
                    continue;
                }
                // eslint-disable-next-line
                element.forEach((item: string | Blob) => {
                    // params = params.append(key, item);
                     
                    formData.append(prop, item);
                });
            } else {
                // eslint-disable-next-line
                formData.set(prop, model[prop]);
            }
        }
        return formData;
    }
}
