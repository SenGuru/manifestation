import { CompanyModel, CompanySummaryModel, LoggedInUserModel } from 'src/common/models/app-model';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CompanyInfoHeaderTabTypes } from 'src/common/models/app-custom-model';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { HttpService } from 'src/common/services/app-http-client';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { Observable } from 'rxjs';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { ReturnModel } from 'src/common/framework/common/models/result/return.model';
import { Router } from '@angular/router';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { SidebarLayoutTypes } from 'src/common/models/app-custom-enums';

@Component({
    selector: 'app-company-header',
    templateUrl: './company-header.component.html',
    styleUrl: './company-header.component.scss',
    standalone: false
})
export class CompanyHeaderComponent implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public layoutTypeId: number | undefined;
  @Input() public companyConfig: boolean = false;
  @Input() public appointmentConfig: boolean = false;
  @Output() public showChangeTab: EventEmitter<CompanyInfoHeaderTabTypes> = new EventEmitter<CompanyInfoHeaderTabTypes>();
  public currentUser: LoggedInUserModel | null = null;
  public companyName: string = '';
  public isSuperAdmin: boolean = false;
  public companyInfo: CompanyModel | undefined;
  public initialUrl: string = '';
  public selectedCompanyConfigScreenType: CompanyInfoHeaderTabTypes | undefined;
  public companyProfilePicture: string | undefined;
   
  public sidebarLayoutType: typeof SidebarLayoutTypes = SidebarLayoutTypes;
  public companySummaryModel: CompanySummaryModel | undefined;
  public constructor(private sessionService: SessionService,
    private httpService: HttpService,
    private companySessionService: CompanySessionService,
    private dialogService: CommonDialogService,
    private routerDataService: RouterDataService,
    private router: Router) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initialUrl = this.sessionService.getInitialUrl();
    const routerParams = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    const routerData = this.routerDataService.getRouterData(this.router.routerState.snapshot.root);
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    this.companyConfig = routerData['companyConfig'];
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    this.appointmentConfig = routerData['appointmentConfig'];
    if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {
      // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
      this.companyId = RouterUtils.getRouterDataNumber(routerParams?.['companyId']) ?? 0;
      this.isSuperAdmin = true;
    } else {
      this.companyId = this.currentUser?.companyId ?? 0;
    }

    const sessionCompany = this.companySessionService.getCurrentCompany();
    if (sessionCompany && sessionCompany?.companyId === this.companyId) {
      this.companyName = sessionCompany?.companyName;
      this.companyInfo = sessionCompany;
    } else {
      this.getCompanyInfo(this.companyId);
    }

    const sessionCompanySummary = this.companySessionService.getCurrentCompanySummary();
    if (!ObjectUtils.isObjNullOrUndefined(sessionCompanySummary) && sessionCompanySummary) {
      this.companySummaryModel = sessionCompanySummary;
    } else {
      this.getCompanySummaries(this.companyId);
    }

    this.getCompanyPictureUrl(this.companyId, this.currentUser?.userProfileGuid ?? "", this.currentUser?.token ?? "");
    if (this.companyConfig) {
      this.changeTab('info');
    } else if (this.appointmentConfig) {
      this.changeTab('appointment-list');
    }
  }

  public getCompanySummaries(companyId: number): void {
    let fnc: Observable<ReturnModel<CompanySummaryModel>>;
    if (this.isSuperAdmin) {
      fnc = this.httpService.getCompanySummaries(companyId);
    } else {
      fnc = this.httpService.getCurrentCompanySummaries();
    }
    fnc.subscribe({
      next: result => {
        if (!result.isSuccess) return this.dialogService.errorWithReturnModel("Unable to get Company Summaries", result);
        if (result.model) {
          this.companySummaryModel = result.model;
          this.companySessionService.setCurrentCompanySummary(result.model);
        }
        return;
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Company Summaries"),
      complete: () => null,
    });
  }

  public getCompanyPictureUrl(companyId: number, profileGuid: string, token: string): void {
    this.companyProfilePicture = this.httpService.getCompanyPictureUrl(companyId, profileGuid, token);
  }

  public getCompanyInfo(companyId: number): void {
    this.httpService.getCompany(companyId).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Company", result);
          return;
        }
        if (result.model) {
          this.companyInfo = result.model;
          this.companySessionService.setCurrentCompany(result.model);
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Company"),
      complete: () => null,
    });
  }

  public changeTab(tab: CompanyInfoHeaderTabTypes): void {
    this.showChangeTab.emit(tab);
    this.selectedCompanyConfigScreenType = tab;
  }

  public backToCompanyListScreen(): void {
    this.router.navigateByUrl(`${this.initialUrl}/companies`);
  }
}
