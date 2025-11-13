import { Component, OnInit } from '@angular/core';
import { LoggedInUserModel, LoginModel } from 'src/common/models/app-model';

import { EnumViewService } from 'src/common/models/app-enum-view';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { Router } from '@angular/router';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss'],
    standalone: false
})
export class HomeComponent implements OnInit {

  public credentials = new LoginModel();
  private currentUser: LoggedInUserModel | undefined | null;

  public constructor(private router: Router,
    private sessionService: SessionService,
    private enumViewService: EnumViewService,) { }

  public ngOnInit(): void {

    this.currentUser = this.sessionService.getCurrentUserInfo();
    if (this.currentUser && !ObjectUtils.isNullOrUndefined(this.currentUser)) {
      this.enumViewService.navigateToModule(this.currentUser?.userTypeId);
    } else {
      this.router.navigate(['/login']);
    }
  }
}
