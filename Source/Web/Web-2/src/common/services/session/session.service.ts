import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Injectable } from '@angular/core';
import { LoggedInUserModel } from '../../models/app-model';
import { Router } from '@angular/router';
import { SessionStorageService } from './session.storage.service';

// import { Storage } from '@ionic/storage';
@Injectable({
  providedIn: 'root'
})
export class SessionService {
  public currentUser: Observable<LoggedInUserModel | null>;
  private currentUserSubject: BehaviorSubject<LoggedInUserModel | null>;
  public constructor(private http: HttpClient,
    private sessionStorageService: SessionStorageService,
    private router: Router) {
    this.currentUserSubject = new BehaviorSubject<LoggedInUserModel | null>((JSON.parse(localStorage.getItem('currentUser') ?? '') as LoggedInUserModel));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): LoggedInUserModel | null {
    return this.currentUserSubject.value;
  }

  public setCurrentUserValue(model: LoggedInUserModel | null): void {
    this.currentUserSubject.next(model);
  }

  public getHeaders(): HttpHeaders {
    // tslint:disable-next-line:new-parens
    const httpHeaders = new HttpHeaders;
    httpHeaders.append('Content-Type', 'application/json');
    const user = this.currentUserValue;
    if (user) {
      httpHeaders.append('Authorization', 'Bearer ' + user.token);
    }
    return httpHeaders;
  }

  public logout(): void {
    // remove user from local storage to log user out
    this.setCurrentUserValue(null);
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

}
