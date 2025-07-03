import { CompanyModel, LoggedInUserModel } from 'src/common/models/app-model';
import { Component, OnInit } from '@angular/core';

import { HttpService } from 'src/common/services/app-http-client';
import { Router } from '@angular/router';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-super-admin-summary-header',
    templateUrl: './super-admin-summary-header.component.html',
    styleUrl: './super-admin-summary-header.component.scss',
    standalone: false
})
export class SuperAdminSummaryHeaderComponent implements OnInit {
  public companyProfilePicture: string = '';
  public companyId: number = 0;
  public currentUser: LoggedInUserModel | undefined | null;
  public companyInfo: CompanyModel | undefined;
  public initialUrl: string | undefined;

  public constructor(
    private httpService: HttpService,
    private sessionService: SessionService,
    private router: Router,
  ) {

  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.getCompanyPictureUrl(this.companyId);
  }

  public getCompanyPictureUrl(companyId: number): void {
    this.companyProfilePicture = this.httpService.getCompanyPictureUrl(companyId, this.currentUser?.token);
  }

}
