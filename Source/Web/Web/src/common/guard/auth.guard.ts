import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

import { EnumViewService } from '../models/app-enum-view';
import { Injectable } from '@angular/core';
import { ObjectUtils } from '../framework/common/utils/object.utils';
import { Observable } from 'rxjs';
import { SessionService } from '../framework/web/infrastructure/session/session.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  public constructor(
    private router: Router,
    private sessionService: SessionService,
    private enumViewService: EnumViewService,
  ) {

  }

  public canActivate(
    // next: ActivatedRouteSnapshot,
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const currentUser = this.sessionService.currentUser;
    let result = false;
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    const predefinedUserTypeId: number = route.data?.['predefinedUserTypeId'];

    if (!ObjectUtils.isObjNullOrUndefined(currentUser)) {
      if (currentUser && predefinedUserTypeId === currentUser.userTypeId) {
        // logged in so return true
        result = true;
      } else {
        result = false;
      }
    } else {
      result = false;
    }

    // not logged in so redirect to login page with the return url
    if (result === false) {
      if (!ObjectUtils.isNullOrUndefined(currentUser)) {
        this.enumViewService.navigateToModule(currentUser?.userTypeId ?? 0);
      } else {
        this.redirectToAdminLogin();
      }
    }
    return result;
  }

  public redirectToAdminLogin(): void {
    this.router.navigate(['/login']);
  }
}
