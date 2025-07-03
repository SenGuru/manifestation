import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

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
    private sessionService: SessionService
  ) {

  }

  public canActivate(
    // next: ActivatedRouteSnapshot,
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const currentUser = this.sessionService.currentUser;
    let result = false;

    if (!ObjectUtils.isObjNullOrUndefined(currentUser)) {
      result = true;
    } else {
      result = false;
    }

    // not logged in so redirect to login page with the return url
    if (result === false) {
      this.redirectToAdminLogin();
    }
    return result;
  }

public redirectToAdminLogin(): void {
    this.router.navigate(['/admin/login']);
  }

}
