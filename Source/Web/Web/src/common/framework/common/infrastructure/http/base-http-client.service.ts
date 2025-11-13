import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ReturnListModel } from '../../models/result/return-list.model';
import { ReturnModel } from '../../models/result/return.model';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class BaseHttpClientService {

    public constructor(private httpClient: HttpClient) {
    }

    public getUrl(url: string, accessToken?: string): string {
        let result = url;
        if (accessToken != null && accessToken != undefined) {
            result = result + '?access_token=' + accessToken;
        }
        return result;
    }

    // eslint-disable-next-line
    public getUrlWithParams(url: string, queryModel: any, accessToken?: string): string {
        const qryParam = this.objectToQueryUrl(queryModel)
        let result = url + "?" + qryParam;
        if (accessToken != null) {
            if (qryParam !== '')
                result = result + '&';
            result = result + 'access_token=' + accessToken;
        }
        return result;
    }

    public get<T>(url: string, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        return this.httpClient.get<ReturnModel<T>>(url, { headers },).pipe(map(res => res));
    }

    public simpleGet<T>(url: string, headers?: HttpHeaders): Observable<T> {
        return this.httpClient.get<T>(url, { headers },).pipe(map(res => res));
    }

    public getWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        return this.httpClient.get<ReturnModel<T>>(url,
            { headers, params: this.objectToQueryParameter(queryModel) }).pipe(map(res => res));
    }

    public simpleGetWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<T> {
        return this.httpClient.get<T>(url,
            { headers, params: this.objectToQueryParameter(queryModel) }).pipe(map(res => res));
    }

    public getList<T>(url: string, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        return this.httpClient.get<ReturnListModel<T>>(url, { headers },).pipe(map(res => res));
    }

    public simpleGetList<T>(url: string, headers?: HttpHeaders): Observable<T[]> {
        return this.httpClient.get<T[]>(url, { headers },).pipe(map(res => res));
    }

    public getListWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        return this.httpClient.get<ReturnListModel<T>>(url,
            { headers, params: this.objectToQueryParameter(queryModel) }).pipe(map(res => res));
    }

    public simpleGetListWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<T[]> {
        return this.httpClient.get<T[]>(url,
            { headers, params: this.objectToQueryParameter(queryModel) }).pipe(map(res => res));
    }

    public post<T>(url: string, model: T, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        return this.httpClient.post<ReturnModel<T>>(url, model, { headers }).pipe(map(res => res));
    }

    public simplePost<T>(url: string, model: T, headers?: HttpHeaders): Observable<T> {
        return this.httpClient.post<T>(url, model, { headers }).pipe(map(res => res));
    }

    public postWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        return this.httpClient.post<ReturnModel<T>>(url, model, { headers }).pipe(map(res => res));
    }

    public simplePostWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<T> {
        return this.httpClient.post<T>(url, model, { headers }).pipe(map(res => res));
    }

    public postList<T>(url: string, model: T, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        return this.httpClient.post<ReturnListModel<T>>(url, model, { headers }).pipe(map(res => res));
    }

    public simplePostList<T>(url: string, model: T, headers?: HttpHeaders): Observable<T[]> {
        return this.httpClient.post<T[]>(url, model, { headers }).pipe(map(res => res));
    }

    public postListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        return this.httpClient.post<ReturnListModel<T>>(url, model, { headers }).pipe(map(res => res));
    }

    public simplePostListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<T[]> {
        return this.httpClient.post<T[]>(url, model, { headers }).pipe(map(res => res));
    }

    public put<T>(url: string, model: T, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        return this.httpClient.put<ReturnModel<T>>(url, model, { headers }).pipe(map(res => res));
    }

    public simplePut<T>(url: string, model: T, headers?: HttpHeaders): Observable<T> {
        return this.httpClient.put<T>(url, model, { headers }).pipe(map(res => res));
    }

    public putWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        return this.httpClient.put<ReturnModel<T>>(url, model, { headers }).pipe(map(res => res));
    }

    public simplePutWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<T> {
        return this.httpClient.put<T>(url, model, { headers }).pipe(map(res => res));
    }

    public putList<T>(url: string, model: T, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        return this.httpClient.put<ReturnListModel<T>>(url, model, { headers }).pipe(map(res => res));
    }

    public simplePutList<T>(url: string, model: T, headers?: HttpHeaders): Observable<T[]> {
        return this.httpClient.put<T[]>(url, model, { headers }).pipe(map(res => res));
    }

    public putListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<ReturnListModel<T>> {
        return this.httpClient.put<ReturnListModel<T>>(url, model, { headers }).pipe(map(res => res));
    }

    public simplePutListWithModel<T, X>(url: string, model: X, headers?: HttpHeaders): Observable<T[]> {
        return this.httpClient.put<T[]>(url, model, { headers }).pipe(map(res => res));
    }

    public delete<T>(url: string, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        return this.httpClient.delete<ReturnModel<T>>(url, { headers }).pipe(map(res => res));
    }

    public simpleDelete<T>(url: string, headers?: HttpHeaders): Observable<T> {
        return this.httpClient.delete<T>(url, { headers }).pipe(map(res => res));
    }

    public deleteWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<ReturnModel<T>> {
        return this.httpClient.delete<ReturnModel<T>>(url, {
            headers,
            params: this.objectToQueryParameter(queryModel)
        }).pipe(map(res => res));
    }

    public simpleDeleteWithParams<T>(url: string, queryModel: unknown, headers?: HttpHeaders): Observable<T> {
        return this.httpClient.delete<T>(url, {
            headers,
            params: this.objectToQueryParameter(queryModel)
        }).pipe(map(res => res));
    }

    private objectToQueryUrl(obj: unknown): string {
        let result = "";
        const params = this.objectToQueryParameter(obj);
        const ks = params.keys();
        for (let i = 0; i < ks.length; i++) {
            const k = ks[i] ?? '';
            const ps = params.getAll(k);
            if (!ps) {
                const ps1 = params.get(k);
                if (!ps1) continue;
                result = result + "&" + `${k}=${ps1}`;
            }
            else {
                for (let i = 0; i < ps.length; i++)
                    result = result + "&" + `${k}=${ps[i]}`;
            }
        }
        return result;
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    private objectToQueryParameter(obj: any): HttpParams {
        let params: HttpParams = new HttpParams();
        for (const key in obj) {
            // eslint-disable-next-line
            if (obj.hasOwnProperty(key)) {
                // eslint-disable-next-line
                const element = obj[key];
                if (!element) {
                    if (element === false) {
                        // eslint-disable-next-line
                        params = params.set(key, element);
                    }
                    continue;
                }
                if (element.constructor.name === 'Array') {
                    if (element.length === 0) {
                        continue;
                    }
                    // eslint-disable-next-line
                    element.forEach((item: string | number | boolean) => {
                        // eslint-disable-next-line
                        params = params.append(key, item);
                    });
                } else {
                    // eslint-disable-next-line
                    params = params.set(key, element);
                }
            }
        }
        return params;
    }

}
