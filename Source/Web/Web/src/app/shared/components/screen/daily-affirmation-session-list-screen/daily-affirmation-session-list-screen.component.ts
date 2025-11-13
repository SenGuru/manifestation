
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DailyAffirmationSessionCrudModalComponent } from '../../../modals/daily-affirmation-session-crud-modal/daily-affirmation-session-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { UserModel, UserSearchCriteria, DailyAffirmationSessionModel, DailyAffirmationSessionSearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
import { HttpService } from 'src/common/services/app-http-client';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';
import { TitleService } from 'src/app/shared/services/title.service';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { Router } from '@angular/router';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';
import { SidebarLayoutTypes } from 'src/common/models/app-custom-enums';

@Component({
  selector: 'app-daily-affirmation-session-list-screen',
  templateUrl: './daily-affirmation-session-list-screen.component.html',
  standalone: false,
})
export class DailyAffirmationSessionListScreenComponent extends BaseListComponent<DailyAffirmationSessionModel, DailyAffirmationSessionSearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: DailyAffirmationSessionSearchCriteria = new DailyAffirmationSessionSearchCriteria();

  public currentUser: LoggedInUserModel | null = null;

  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading: boolean = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public override columns = [
    {
      text: 'Daily Affirmation Session Date',
      align: 'left',
      sortable: true,
      value: 'dailyAffirmationSessionDate',
      width: '115px',
    },
    {
      text: 'Daily Affirmation Session Time Spent In Seconds',
      align: 'left',
      sortable: true,
      value: 'dailyAffirmationSessionTimeSpentInSeconds',
      width: '115px',
    },
    {
      text: 'User Name',
      align: 'left',
      sortable: true,
      value: 'userName',
      width: '115px',
    },
    {
      text: 'Added To Feed',
      align: 'left',
      sortable: true,
      value: 'addedToFeed',
      width: '115px',
    },
    {
      text: 'Added To Feed Date',
      align: 'left',
      sortable: true,
      value: 'addedToFeedDate',
      width: '115px',
    },
    {
      text: 'Modified Date',
      align: 'left',
      sortable: true,
      value: 'modifiedDate',
      width: '115px',
    },
    {
      text: 'Modified By User Id',
      align: 'left',
      sortable: true,
      value: 'modifiedByUserFirstName',
      width: '115px',
    },
  ];

  public constructor(
    private dialogService: CommonDialogService,
    public sessionService: SessionService,
    private httpService: HttpService,
    private modalService: NgbModal,
    private selectService: SelectService,
    private crudService: CrudService,
    private titleService: TitleService,
    public enumViewService: EnumViewService,
    private router: Router,
    private routerDataService: RouterDataService,
  ) {
    super();
    this.titleService.pageTitle = "Daily Affirmation Session";
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    const routerParams = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    const routerData = this.routerDataService.getRouterData(this.router.routerState.snapshot.root);
    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    this.layoutTypeId = RouterUtils.getRouterDataNumber(routerData?.['appLayoutTypeId']);
    if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {
      // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
      this.companyId = RouterUtils.getRouterDataNumber(routerParams?.['companyId']) ?? 0;
      if (this.layoutTypeId == SidebarLayoutTypes.CompanyDetail) {
        // this.getCompanyInfo(this.companyId);
      }
      this.isSuperAdmin = true;
    } else {
    }
    this.getDailyAffirmationSessions(this.searchModel);
    this.getUsers();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.userTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.userTypeaheadLoading = true;
      const userSearchCriteria = new UserSearchCriteria();
      userSearchCriteria.keyword = searchValue;
      return this.httpService.getUsers(userSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.users = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.userTypeaheadLoading = false,
    });
  }

  public getUsers(): void {
    const searchModel = new UserSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.users = data.model;
      },
      error: (error) => this.dialogService.error('Not Able to get User'),
      complete: () => null,
    });
  }

  public getDailyAffirmationSessions(searchModel: DailyAffirmationSessionSearchCriteria): void {
    this.httpService.getDailyAffirmationSessions(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Daily Affirmation Sessions", result);
          return;
        }

        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err) => this.dialogService.error("Unable to get Daily Affirmation Sessions"),
      complete: () => null,
    });
  }

  public showAddDailyAffirmationSessionModal(): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Daily Affirmation Session';
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationSessions(this.searchModel);
    }
  }

  public showViewDailyAffirmationSessionModal(item: DailyAffirmationSessionModel): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Daily Affirmation Session';
    componentInstance.dailyAffirmationSessionModel = JSON.parse(JSON.stringify(item)) as DailyAffirmationSessionModel;
  }

  public showUpdateDailyAffirmationSessionModal(item: DailyAffirmationSessionModel): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Daily Affirmation Session';
    componentInstance.dailyAffirmationSessionModel = JSON.parse(JSON.stringify(item)) as DailyAffirmationSessionModel;
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationSessions(this.searchModel);
    }
  }

  public deleteDailyAffirmationSession(item: DailyAffirmationSessionModel): void {
    this.dialogService.confirmWithTitle(`Delete Daily Affirmation Session`, `Do you want to delete the Daily Affirmation Session  }?`).then(res => {
      if (!res) return;
      this.httpService.markDailyAffirmationSessionAsDeleted(item.dailyAffirmationSessionId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Daily Affirmation Session");
        this.dialogService.success("Successfully deleted the Daily Affirmation Session");
        this.getDailyAffirmationSessions(this.searchModel);
        return;
      });
    });
  }

  /***
  public enableDailyAffirmationSession(item: DailyAffirmationSessionModel): void {
    this.dialogService.confirmWithTitle(`Enable DailyAffirmationSession`, `Do you want to enable the DailyAffirmationSession - ${item.dailyAffirmationSessionName}?`).then(res => {
      if (!res) return;
      this.httpService.enableDailyAffirmationSession(item.dailyAffirmationSessionId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the DailyAffirmationSession");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableDailyAffirmationSession(item: DailyAffirmationSessionModel): void {
    this.dialogService.confirmWithTitle(`Disable DailyAffirmationSession`, `Do you want to disable the DailyAffirmationSession - ${item.dailyAffirmationSessionName}?`).then(res => {
      if (!res) return;
      this.httpService.disableDailyAffirmationSession(item.dailyAffirmationSessionId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the DailyAffirmationSession");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportDailyAffirmationSessionsModal(): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCsvFileImportModalComponent;
    componentInstance.title = 'Import DailyAffirmationSessions';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationSessions(this.searchModel);
    }
  }

  exportDailyAffirmationSessionsModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportDailyAffirmationSessionsOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getDailyAffirmationSessions(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.searchModel.userId = this.selectedUser ? this.selectedUser.userId : null;

    this.getDailyAffirmationSessions(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.userId = null;
    this.getDailyAffirmationSessions(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getDailyAffirmationSessions(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getDailyAffirmationSessions(this.searchModel);
  }
}
