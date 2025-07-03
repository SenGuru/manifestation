import { Component, Inject, OnInit, DOCUMENT } from '@angular/core';
import { GlobalConfigurationModel, LoggedInUserModel } from 'src/common/models/app-model';

import { CommonDialogService } from '../../services/common-dialog.service';

import { HttpService } from 'src/common/services/app-http-client';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.css'],
    standalone: false
})
export class FooterComponent implements OnInit {
  public currentUser: LoggedInUserModel | undefined | null;
  public tenantEntityName: string = 'Current';
  public todayDate: number | undefined;
  public userToken: string | undefined;
  public siteDetails: GlobalConfigurationModel | undefined;
  public userRoleTypes = PredefinedUserTypes;
  public constructor(private sessionService: SessionService,
    @Inject(DOCUMENT) private document: Document,
    private dialogService: CommonDialogService,
    private httpService: HttpService,) { }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.userToken = this.currentUser?.token;
    this.todayDate = new Date().getFullYear();
    this.siteDetails = new GlobalConfigurationModel();
    this.getSiteConfiguration();
  }

  public getSiteConfiguration(): void {
    const site = this.document.location.host;
    this.httpService.getSiteConfiguration(site, getHeaderWithBlockUI()).subscribe({
      next: result => {
        if (!result.isSuccess) return;
        if (result.model) this.siteDetails = result.model;
      },
      error: err => {
        this.dialogService.error('Unable to get site config details');
      },
      complete: () => null
    });
  }

  public showHelp(): void {
    const height = window.screen.availHeight;
    const width = window.screen.availWidth / 2;
    const left = width;
    const top = 0;

    window.open(`/#/help/auditduo/${this.userToken}`, '_blank',
      "status=no,height=" + height + ",width=" + width + ",resizable=yes,left="
      + left + ",top=" + top + ",screenX=" + left + ",screenY="
      + top + ",toolbar=no,menubar=no,scrollbars=no,location=no,directories=no");
  }

}
