import { EnumViewService } from 'src/common/models/app-enum-view';
import { Injectable } from '@angular/core';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { Router } from '@angular/router';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  public constructor(private enumViewService: EnumViewService, private router: Router, private sessionService: SessionService) { }

public navigateToHomeScreen(): void {
    if (this.sessionService.getCurrentUserInfo()?.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.router.navigateByUrl(`/${this.enumViewService.getInititalUrl(this.sessionService.getCurrentUserInfo()?.userTypeId ?? 0)}/companies`);
    } else {
      this.router.navigateByUrl(`/${this.enumViewService.getInititalUrl(this.sessionService.getCurrentUserInfo()?.userTypeId ?? 0)}/customers`);
    }
  }
}
