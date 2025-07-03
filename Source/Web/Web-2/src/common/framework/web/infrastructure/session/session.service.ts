import { BehaviorSubject, Observable } from 'rxjs';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { Injectable } from '@angular/core';
import { LoggedInUserModel } from '../../../../models/app-model';
import { ObjectUtils } from '../../../common/utils/object.utils';
import { PredefinedUserPermissionTypes } from 'src/common/models/app-enums';
import { Router } from '@angular/router';
import { STORAGE_KEYS } from 'src/common/constants/app.constants';

@Injectable()
export class SessionService {
  public currentUser: LoggedInUserModel | undefined | null;
  private isLoggedIn = false;
  private userLoggedIn = new BehaviorSubject<boolean>(false);

  public constructor(
    private router: Router,
    private enumViewService: EnumViewService,
    private companySessionService: CompanySessionService,
    private dialogService: CommonDialogService,
  ) {
  }

  public setCurrentUser(user: LoggedInUserModel): void {
    this.currentUser = user;
    this.isLoggedIn = true;
  }

  // tslint:disable-next-line: max-line-length
  public setCurrentUserInfo(
    userName: string,
    token: string,
    firstName: string,
    lastName: string,
    userTypeId: number,
    userTypeName: string,
    userId: number,
    companyId: number,
    companyName: string,
    permissions: number[] = []
  ): void {
    this.currentUser = new LoggedInUserModel();
    this.currentUser.userName = userName;
    this.currentUser.token = token;
    this.currentUser.firstName = firstName;
    this.currentUser.lastName = lastName;
    this.currentUser.userTypeId = userTypeId;
    this.currentUser.userTypeName = userTypeName;
    this.currentUser.userId = userId;
    this.currentUser.companyId = companyId;
    this.currentUser.companyName = companyName;
    this.currentUser.permissions = permissions;
    localStorage.setItem(STORAGE_KEYS.CURRENT_USER_LOCAL_STORAGE_NAME, JSON.stringify(this.currentUser));
    this.isLoggedIn = true;
  }

  public clearCurrentUser(): void {
    this.currentUser = null;
    this.isLoggedIn = false;
    localStorage.removeItem(STORAGE_KEYS.CURRENT_USER_LOCAL_STORAGE_NAME);
  }

  public isUserLoggedIn(): boolean {
    // tslint:disable-next-line: max-line-length
    const currentSession = localStorage.getItem(STORAGE_KEYS.CURRENT_USER_LOCAL_STORAGE_NAME) ?? null;
    if (currentSession) {
      if (!ObjectUtils.isNullOrUndefined(currentSession)) {
        this.isLoggedIn = true;
        this.currentUser = JSON.parse(currentSession) as LoggedInUserModel;
      }
    }
    return this.isLoggedIn;
  }

  public getCurrentUserInfo(): LoggedInUserModel | null {
    if (this.isLoggedIn === false || this.currentUser == null) {
      const currentSession = localStorage.getItem(STORAGE_KEYS.CURRENT_USER_LOCAL_STORAGE_NAME) ?? null;
      if (currentSession) {
        if (!ObjectUtils.isNullOrUndefined(JSON.parse(currentSession))) {
          this.isLoggedIn = true;
          this.currentUser = JSON.parse(currentSession) as LoggedInUserModel;
        } else {
          return null;
        }
      } else {
        return null;
      }
    }
    return this.currentUser;
  }

  public getInitialUrl(): string {
    return this.enumViewService.getInititalUrl(this.currentUser?.userTypeId ?? 0);
  }

  public getCurrentToken(): string {
    if (this.isLoggedIn === false || this.currentUser == null || this.currentUser.token == null) {
      return '';
    }
    return this.currentUser.token;
  }

  public getCurrentTokenEncoded(): string {
    if (this.isLoggedIn === false || this.currentUser == null || this.currentUser.token == null) {
      return '';
    }
    return encodeURIComponent(this.currentUser.token);
  }

  public logOut(): void {
    this.clearCurrentUser();
    this.companySessionService.clearAllSessions();
    this.router.navigateByUrl('/login').then(res => {
      window.location.reload();
    });
  }

  public setUserLoggedIn(userLoggedIn: boolean): void {
    this.userLoggedIn.next(userLoggedIn);
  }

  public getUserLoggedIn(): Observable<boolean> {
    return this.userLoggedIn.asObservable();
  }

  public canViewCompany(): boolean {
    let result = false;
    if (ObjectUtils.isObjNullOrUndefined(this.currentUser)) result = false;
    const availablePermissions = this.currentUser?.permissions?.filter(id => (id === PredefinedUserPermissionTypes.ViewCompany || id === PredefinedUserPermissionTypes.ConfigureCompany)) ?? [];
    if (!ObjectUtils.isArrNullOrEmpty(availablePermissions)) {
      result = true;
    } else {
      result = false;
    }
    return result;
  }

  public canConfigureCompany(): boolean {
    let result = false;
    if (ObjectUtils.isObjNullOrUndefined(this.currentUser)) result = false;
    const configurePermission = this.currentUser?.permissions?.find(id => (id === PredefinedUserPermissionTypes.ConfigureCompany));
    if (!ObjectUtils.isNullOrUndefined(configurePermission)) {
      result = true;
    } else {
      result = false;
    }
    return result;
  }

  public setEnableDisablePermission(value: boolean): void {
    localStorage.setItem(STORAGE_KEYS.PERMISSION_ENABLE_DISABLE_OPTION_STORAGE_NAME, JSON.stringify(value));
  }

  public isPermissionEnabled(): boolean {
    const sessionData = localStorage.getItem(STORAGE_KEYS.PERMISSION_ENABLE_DISABLE_OPTION_STORAGE_NAME);
    if (sessionData) {
      return JSON.parse(sessionData) as boolean;
    } else {
      return false;
    }
  }
}
