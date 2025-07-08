
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DailyAffirmationSessionCategoryCrudModalComponent } from '../../../modals/daily-affirmation-session-category-crud-modal/daily-affirmation-session-category-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { CategoryModel, CategorySearchCriteria, DailyAffirmationSessionModel, DailyAffirmationSessionSearchCriteria, UserModel, UserSearchCriteria, DailyAffirmationSessionCategoryModel, DailyAffirmationSessionCategorySearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
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
  selector: 'app-daily-affirmation-session-category-list-screen',
  templateUrl: './daily-affirmation-session-category-list-screen.component.html',
  standalone: false,
})
export class DailyAffirmationSessionCategoryListScreenComponent extends BaseListComponent<DailyAffirmationSessionCategoryModel, DailyAffirmationSessionCategorySearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: DailyAffirmationSessionCategorySearchCriteria = new DailyAffirmationSessionCategorySearchCriteria();

  public currentUser: LoggedInUserModel | null = null;

  public categoryTypeahead$ = new Subject<string>();
  public categoryTypeaheadLoading: boolean = false;
  public categorys: CategoryModel[] = [];
  public selectedCategory: CategoryModel | undefined;
  public dailyAffirmationSessionTypeahead$ = new Subject<string>();
  public dailyAffirmationSessionTypeaheadLoading: boolean = false;
  public dailyAffirmationSessions: DailyAffirmationSessionModel[] = [];
  public selectedDailyAffirmationSession: DailyAffirmationSessionModel | undefined;
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading: boolean = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public override columns = [
    {
      text: 'Category Name',
      align: 'left',
      sortable: true,
      value: 'categoryName',
      width: '115px',
    },
    {
      text: 'Daily Affirmation Session Name',
      align: 'left',
      sortable: true,
      value: 'dailyAffirmationSessionName',
      width: '115px',
    },
    {
      text: 'Daily Affirmation Session Category Time Spent In Seconds',
      align: 'left',
      sortable: true,
      value: 'dailyAffirmationSessionCategoryTimeSpentInSeconds',
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
    this.titleService.pageTitle = "Daily Affirmation Session Category";
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
    this.getDailyAffirmationSessionCategorys(this.searchModel);
    this.getCategorys();
    this.getDailyAffirmationSessions();
    this.getUsers();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.categoryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.categoryTypeaheadLoading = true;
      const categorySearchCriteria = new CategorySearchCriteria();
      categorySearchCriteria.keyword = searchValue;
      return this.httpService.getCategorys(categorySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.categorys = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.categoryTypeaheadLoading = false,
    });
    this.dailyAffirmationSessionTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.dailyAffirmationSessionTypeaheadLoading = true;
      const dailyAffirmationSessionSearchCriteria = new DailyAffirmationSessionSearchCriteria();
      dailyAffirmationSessionSearchCriteria.keyword = searchValue;
      return this.httpService.getDailyAffirmationSessions(dailyAffirmationSessionSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.dailyAffirmationSessions = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.dailyAffirmationSessionTypeaheadLoading = false,
    });
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

  public getCategorys(): void {
    const searchModel = new CategorySearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getCategorys(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.categorys = data.model;
      },
      error: (error) => this.dialogService.error('Not Able to get Category'),
      complete: () => null,
    });
  }
  public getDailyAffirmationSessions(): void {
    const searchModel = new DailyAffirmationSessionSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getDailyAffirmationSessions(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.dailyAffirmationSessions = data.model;
      },
      error: (error) => this.dialogService.error('Not Able to get Daily Affirmation Session'),
      complete: () => null,
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

  public getDailyAffirmationSessionCategorys(searchModel: DailyAffirmationSessionCategorySearchCriteria): void {
    this.httpService.getDailyAffirmationSessionCategorys(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Daily Affirmation Session Categorys", result);
          return;
        }

        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err) => this.dialogService.error("Unable to get Daily Affirmation Session Categorys"),
      complete: () => null,
    });
  }

  public showAddDailyAffirmationSessionCategoryModal(): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCategoryCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Daily Affirmation Session Category';
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationSessionCategorys(this.searchModel);
    }
  }

  public showViewDailyAffirmationSessionCategoryModal(item: DailyAffirmationSessionCategoryModel): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCategoryCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Daily Affirmation Session Category';
    componentInstance.dailyAffirmationSessionCategoryModel = JSON.parse(JSON.stringify(item)) as DailyAffirmationSessionCategoryModel;
  }

  public showUpdateDailyAffirmationSessionCategoryModal(item: DailyAffirmationSessionCategoryModel): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCategoryCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Daily Affirmation Session Category';
    componentInstance.dailyAffirmationSessionCategoryModel = JSON.parse(JSON.stringify(item)) as DailyAffirmationSessionCategoryModel;
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationSessionCategorys(this.searchModel);
    }
  }

  public deleteDailyAffirmationSessionCategory(item: DailyAffirmationSessionCategoryModel): void {
    this.dialogService.confirmWithTitle(`Delete Daily Affirmation Session Category`, `Do you want to delete the Daily Affirmation Session Category  ?`).then(res => {
      if (!res) return;
      this.httpService.markDailyAffirmationSessionCategoryAsDeleted(item.dailyAffirmationSessionCategoryId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Daily Affirmation Session Category");
        this.dialogService.success("Successfully deleted the Daily Affirmation Session Category");
        this.getDailyAffirmationSessionCategorys(this.searchModel);
        return;
      });
    });
  }

  /***
  public enableDailyAffirmationSessionCategory(item: DailyAffirmationSessionCategoryModel): void {
    this.dialogService.confirmWithTitle(`Enable DailyAffirmationSessionCategory`, `Do you want to enable the DailyAffirmationSessionCategory - ${item.dailyAffirmationSessionCategoryName}?`).then(res => {
      if (!res) return;
      this.httpService.enableDailyAffirmationSessionCategory(item.dailyAffirmationSessionCategoryId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the DailyAffirmationSessionCategory");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableDailyAffirmationSessionCategory(item: DailyAffirmationSessionCategoryModel): void {
    this.dialogService.confirmWithTitle(`Disable DailyAffirmationSessionCategory`, `Do you want to disable the DailyAffirmationSessionCategory - ${item.dailyAffirmationSessionCategoryName}?`).then(res => {
      if (!res) return;
      this.httpService.disableDailyAffirmationSessionCategory(item.dailyAffirmationSessionCategoryId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the DailyAffirmationSessionCategory");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportDailyAffirmationSessionCategorysModal(): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCategoryCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCategoryCsvFileImportModalComponent;
    componentInstance.title = 'Import DailyAffirmationSessionCategorys';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationSessionCategorys(this.searchModel);
    }
  }

  exportDailyAffirmationSessionCategorysModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportDailyAffirmationSessionCategorysOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getDailyAffirmationSessionCategorys(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.searchModel.categoryId = this.selectedCategory ? this.selectedCategory.categoryId : null;
    this.searchModel.dailyAffirmationSessionId = this.selectedDailyAffirmationSession ? this.selectedDailyAffirmationSession.dailyAffirmationSessionId : null;
    this.searchModel.userId = this.selectedUser ? this.selectedUser.userId : null;

    this.getDailyAffirmationSessionCategorys(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.categoryId = null;
    this.searchModel.dailyAffirmationSessionId = null;
    this.searchModel.userId = null;
    this.getDailyAffirmationSessionCategorys(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getDailyAffirmationSessionCategorys(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getDailyAffirmationSessionCategorys(this.searchModel);
  }
}
