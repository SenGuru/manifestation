import { Component, OnInit } from '@angular/core';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CompanyInfoHeaderTabTypes } from 'src/common/models/app-custom-model';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { HttpService } from 'src/common/services/app-http-client';
import { LoggedInUserModel } from 'src/common/models/app-model';
import { NavigationService } from 'src/common/services/navigation/navigation.service';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { Router } from '@angular/router';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-company-config-screen',
    templateUrl: './company-config-screen.component.html',
    styleUrl: './company-config-screen.component.scss',
    standalone: false
})
export class CompanyConfigScreenComponent implements OnInit {

  public selectedCompanyConfigScreenType: CompanyInfoHeaderTabTypes | undefined;
  public showCompanyGenreScreen: boolean = false;
  public canOnlyViewBasicInformation: boolean = false;
  public isViewMode: boolean = false;
  public showCompanyInformationScreen: boolean = false;
  public showStorageInformationScreen: boolean = false;
  public showAppointmentListScreen: boolean = false;
  public showAppointmentCalendarScreen: boolean = false;
  public showCompanyLicensesListScreen: boolean = false;
  public companyId: number = 0;
  public companyName: string | undefined;
  public initialUrl: string = '';
  public currentUser: LoggedInUserModel | null = null;
  public isSuperAdmin: boolean = false;
  public constructor(
    private httpService: HttpService,
    private routerDataService: RouterDataService,
    private sessionService: SessionService,
    private router: Router,
    private dialogService: CommonDialogService,
    private navigationService: NavigationService,
    private companySessionService: CompanySessionService,
  ) {

  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initialUrl = this.sessionService.getInitialUrl();
    const routerParams = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.isSuperAdmin = true;
      // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
      this.companyId = RouterUtils.getRouterDataNumber(routerParams?.['companyId']) ?? 0;
      const sessionCompany = this.companySessionService.getCurrentCompany();
      if (sessionCompany && sessionCompany?.companyId === this.companyId) {
        this.companyName = sessionCompany?.companyName;
      } else {
        this.getCompanyInfo(this.companyId);
      }
    } else {
      this.companyId = this.currentUser?.companyId ?? 0;
      this.companyName = this.currentUser?.companyName;

      if (this.sessionService.isPermissionEnabled() == true) {
        if (this.sessionService.canConfigureCompany()) {
          this.canOnlyViewBasicInformation = false;
          this.isViewMode = false;
        } else if (this.sessionService.canViewCompany()) {
          this.canOnlyViewBasicInformation = true;
          this.isViewMode = true;
        }
        else {
          this.isViewMode = true;
          this.canOnlyViewBasicInformation = true;
          this.navigationService.navigateToHomeScreen();
        }
      }
    }
  }

  public getCompanyInfo(companyId: number): void {
    this.httpService.getCompany(companyId).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Company", result);
          return;
        }
        if (result.model) {
          this.companyName = result.model?.companyName;
          this.companySessionService.setCurrentCompany(result.model);
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Company"),
      complete: () => null,
    });
  }

  public changeTab(tab: CompanyInfoHeaderTabTypes): void {
    this.resetTabs();
    this.selectedCompanyConfigScreenType = tab;
    if (tab == 'info') {
      this.showCompanyInformationScreen = true;
    } else if (tab == 'storage-config') {
      this.showStorageInformationScreen = true;
    } else if (tab == 'appointment-list') {
      this.showAppointmentListScreen = true;
    } else if (tab == 'appointment-calendar') {
      this.showAppointmentCalendarScreen = true;
    } else if (tab == 'company-licenses') {
      this.showCompanyLicensesListScreen = true;
    }
  }

  public resetTabs(): void {
    this.showCompanyGenreScreen = false;
    this.showCompanyInformationScreen = false;
    this.showStorageInformationScreen = false;
    this.showAppointmentListScreen = false;
    this.showAppointmentCalendarScreen = false;
    this.showCompanyLicensesListScreen = false;
  }

  public backToCompanyListScreen(): void {
    this.router.navigateByUrl(`${this.initialUrl}/companies`);
  }
}
