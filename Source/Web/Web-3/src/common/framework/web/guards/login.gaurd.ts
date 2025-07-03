import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from '../../../services/session/session.service';
import { StringUtils } from '../../common/utils/string.utils';

@Injectable()
export class LoginGuard {

  public constructor(protected router: Router, private sessionService: SessionService) { }

  public canActivate(): boolean {
    if (!StringUtils.isArrNullOrEmpty(this.sessionService.currentUserValue?.token ?? '')) {
      return true;
    } else {
      this.router.navigate(['/auth/login']);
      return false;
    }

  }
}
