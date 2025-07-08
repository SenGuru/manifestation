
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DefaultDailyAffirmationTextCrudModalComponent } from '../../../modals/default-daily-affirmation-text-crud-modal/default-daily-affirmation-text-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { DefaultCategoryModel, DefaultCategorySearchCriteria, DefaultDailyAffirmationTextModel, DefaultDailyAffirmationTextSearchCriteria, LoggedInUserModel, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
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
  selector: 'app-default-daily-affirmation-text-list-screen',
  templateUrl: './default-daily-affirmation-text-list-screen.component.html',
  standalone: false,
})
export class DefaultDailyAffirmationTextListScreenComponent extends BaseListComponent<DefaultDailyAffirmationTextModel, DefaultDailyAffirmationTextSearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: DefaultDailyAffirmationTextSearchCriteria = new DefaultDailyAffirmationTextSearchCriteria();

  public currentUser: LoggedInUserModel | null = null;

  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading: boolean = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;
  public defaultCategoryTypeahead$ = new Subject<string>();
  public defaultCategoryTypeaheadLoading: boolean = false;
  public defaultCategorys: DefaultCategoryModel[] = [];
  public selectedDefaultCategory: DefaultCategoryModel | undefined;

  public override columns = [
    {
      text: 'Default Daily Affirmation Text Name',
      align: 'left',
      sortable: true,
      value: 'defaultDailyAffirmationTextName',
      width: '115px',
    },
    {
      text: 'Default Category Name',
      align: 'left',
      sortable: true,
      value: 'defaultCategoryName',
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
    this.titleService.pageTitle = "Default Daily Affirmation Text";
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
    this.getDefaultDailyAffirmationTexts(this.searchModel);
    this.getUsers();
    this.getDefaultCategorys();
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
    this.defaultCategoryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.defaultCategoryTypeaheadLoading = true;
      const defaultCategorySearchCriteria = new DefaultCategorySearchCriteria();
      defaultCategorySearchCriteria.keyword = searchValue;
      return this.httpService.getDefaultCategorys(defaultCategorySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.defaultCategorys = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.defaultCategoryTypeaheadLoading = false,
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
  public getDefaultCategorys(): void {
    const searchModel = new DefaultCategorySearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getDefaultCategorys(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.defaultCategorys = data.model;
      },
      error: (error) => this.dialogService.error('Not Able to get Default Category'),
      complete: () => null,
    });
  }

  public getDefaultDailyAffirmationTexts(searchModel: DefaultDailyAffirmationTextSearchCriteria): void {
 
    this.httpService.getDefaultDailyAffirmationTexts(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Default Daily Affirmation Texts", result);
          return;
        }

        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err) => this.dialogService.error("Unable to get Default Daily Affirmation Texts"),
      complete: () => null,
    });
  }

  public showAddDefaultDailyAffirmationTextModal(): void {
    const modalRef = this.modalService.open(DefaultDailyAffirmationTextCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultDailyAffirmationTextCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Default Daily Affirmation Text';
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultDailyAffirmationTexts(this.searchModel);
    }
  }

  public showViewDefaultDailyAffirmationTextModal(item: DefaultDailyAffirmationTextModel): void {
    const modalRef = this.modalService.open(DefaultDailyAffirmationTextCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultDailyAffirmationTextCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Default Daily Affirmation Text';
    componentInstance.defaultDailyAffirmationTextModel = JSON.parse(JSON.stringify(item)) as DefaultDailyAffirmationTextModel;
  }

  public showUpdateDefaultDailyAffirmationTextModal(item: DefaultDailyAffirmationTextModel): void {
    const modalRef = this.modalService.open(DefaultDailyAffirmationTextCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultDailyAffirmationTextCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Default Daily Affirmation Text';
    componentInstance.defaultDailyAffirmationTextModel = JSON.parse(JSON.stringify(item)) as DefaultDailyAffirmationTextModel;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultDailyAffirmationTexts(this.searchModel);
    }
  }

  public deleteDefaultDailyAffirmationText(item: DefaultDailyAffirmationTextModel): void {
    this.dialogService.confirmWithTitle(`Delete Default Daily Affirmation Text`, `Do you want to delete the Default Daily Affirmation Text - ${item.defaultDailyAffirmationTextName}?`).then(res => {
      if (!res) return;
      this.httpService.markDefaultDailyAffirmationTextAsDeleted(item.defaultDailyAffirmationTextId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Default Daily Affirmation Text");
        this.dialogService.success("Successfully deleted the Default Daily Affirmation Text");
        this.getDefaultDailyAffirmationTexts(this.searchModel);
        return;
      });
    });
  }

  /***
  public enableDefaultDailyAffirmationText(item: DefaultDailyAffirmationTextModel): void {
    this.dialogService.confirmWithTitle(`Enable DefaultDailyAffirmationText`, `Do you want to enable the DefaultDailyAffirmationText - ${item.defaultDailyAffirmationTextName}?`).then(res => {
      if (!res) return;
      this.httpService.enableDefaultDailyAffirmationText(item.defaultDailyAffirmationTextId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the DefaultDailyAffirmationText");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableDefaultDailyAffirmationText(item: DefaultDailyAffirmationTextModel): void {
    this.dialogService.confirmWithTitle(`Disable DefaultDailyAffirmationText`, `Do you want to disable the DefaultDailyAffirmationText - ${item.defaultDailyAffirmationTextName}?`).then(res => {
      if (!res) return;
      this.httpService.disableDefaultDailyAffirmationText(item.defaultDailyAffirmationTextId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the DefaultDailyAffirmationText");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportDefaultDailyAffirmationTextsModal(): void {
    const modalRef = this.modalService.open(DefaultDailyAffirmationTextCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultDailyAffirmationTextCsvFileImportModalComponent;
    componentInstance.title = 'Import DefaultDailyAffirmationTexts';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultDailyAffirmationTexts(this.searchModel);
    }
  }

  exportDefaultDailyAffirmationTextsModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportDefaultDailyAffirmationTextsOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getDefaultDailyAffirmationTexts(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.searchModel.userId = this.selectedUser ? this.selectedUser.userId : null;
    this.searchModel.defaultCategoryId = this.selectedDefaultCategory ? this.selectedDefaultCategory.defaultCategoryId : null;

    this.getDefaultDailyAffirmationTexts(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.userId = null;
    this.searchModel.defaultCategoryId = null;
    this.getDefaultDailyAffirmationTexts(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getDefaultDailyAffirmationTexts(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getDefaultDailyAffirmationTexts(this.searchModel);
  }
}
