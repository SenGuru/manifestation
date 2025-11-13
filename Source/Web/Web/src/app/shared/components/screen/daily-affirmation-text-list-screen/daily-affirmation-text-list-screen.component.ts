
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DailyAffirmationTextCrudModalComponent } from '../../../modals/daily-affirmation-text-crud-modal/daily-affirmation-text-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { CategoryModel , CategorySearchCriteria ,UserModel , UserSearchCriteria , DailyAffirmationTextModel, DailyAffirmationTextSearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
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
  selector: 'app-daily-affirmation-text-list-screen',
  templateUrl: './daily-affirmation-text-list-screen.component.html',
  standalone: false,
})
export class DailyAffirmationTextListScreenComponent extends BaseListComponent<DailyAffirmationTextModel, DailyAffirmationTextSearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: DailyAffirmationTextSearchCriteria = new DailyAffirmationTextSearchCriteria();

  public currentUser: LoggedInUserModel | null = null;

  public categoryTypeahead$ = new Subject<string>();
  public categoryTypeaheadLoading:boolean = false;    
  public categorys: CategoryModel[] = [];
  public selectedCategory: CategoryModel | undefined;
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading:boolean = false;    
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public override columns = [
    {
      text: 'Daily Affirmation Text Value',
      align: 'left',
      sortable: true,
      value: 'dailyAffirmationTextValue',
      width: '115px',
    },
    {
      text: 'Category Name',
      align: 'left',
      sortable: true,
      value: 'categoryName',
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
    this.titleService.pageTitle = "Daily Affirmation Text";
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
  this.getDailyAffirmationTexts(this.searchModel);
	    this.getCategorys();
	    this.getUsers();
    this.initTypeAheads();
  }
 
 public initTypeAheads(): void {
      this.categoryTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
        this.categoryTypeaheadLoading = true;
        const categorySearchCriteria = new CategorySearchCriteria();
        categorySearchCriteria.keyword = searchValue;
        return this.httpService.getCategorys(categorySearchCriteria, getHeaderWithBlockUI());
      }),untilDestroyed(this)).subscribe({
        next: (data) => {
          if(data.model) this.categorys = data.model;
      },  
      error: err => {
       //
      },
      complete: () =>  this.categoryTypeaheadLoading = false,});
      this.userTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
        this.userTypeaheadLoading = true;
        const userSearchCriteria = new UserSearchCriteria();
        userSearchCriteria.keyword = searchValue;
        return this.httpService.getUsers(userSearchCriteria, getHeaderWithBlockUI());
      }),untilDestroyed(this)).subscribe({
        next: (data) => {
          if(data.model) this.users = data.model;
      },  
      error: err => {
       //
      },
      complete: () =>  this.userTypeaheadLoading = false,});
  }

      public getCategorys(): void {
        const searchModel = new CategorySearchCriteria();
        searchModel.sortAscending = false;
        searchModel.sortBy = 'createdDate';
        this.httpService.getCategorys(searchModel).subscribe({
        next: (data) => {
          if (!data.isSuccess)
            return;
          if(data.model) this.categorys = data.model;
        },
        error: (error) => this.dialogService.error('Not Able to get Category'),
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
          if(data.model) this.users = data.model;
        },
        error: (error) => this.dialogService.error('Not Able to get User'),
        complete: () => null,
        });
      }
  
 public getDailyAffirmationTexts(searchModel: DailyAffirmationTextSearchCriteria): void {
    this.httpService.getDailyAffirmationTexts(searchModel).subscribe({
      next: result => {
      if(!result.isSuccess) {
        this.dialogService.errorWithReturnModel("Unable to get Daily Affirmation Texts", result);
        return;
      }

      if(result.model) {
       this.listData = result.model;
       this.totalItems = result.totalRecords ??  0;
      }
     },
    error: (err) => this.dialogService.error("Unable to get Daily Affirmation Texts"),
    complete: () => null,
  });
  }

  public showAddDailyAffirmationTextModal(): void {
    const modalRef = this.modalService.open(DailyAffirmationTextCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationTextCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Daily Affirmation Text';
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationTexts(this.searchModel);
    }
  }

  public showViewDailyAffirmationTextModal(item: DailyAffirmationTextModel): void {
    const modalRef = this.modalService.open(DailyAffirmationTextCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationTextCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Daily Affirmation Text';
    componentInstance.dailyAffirmationTextModel = JSON.parse(JSON.stringify(item)) as DailyAffirmationTextModel;
  }

  public showUpdateDailyAffirmationTextModal(item: DailyAffirmationTextModel): void {
    const modalRef = this.modalService.open(DailyAffirmationTextCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationTextCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Daily Affirmation Text';
    componentInstance.dailyAffirmationTextModel = JSON.parse(JSON.stringify(item)) as DailyAffirmationTextModel;
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationTexts(this.searchModel);
    }
  }

  public deleteDailyAffirmationText(item: DailyAffirmationTextModel): void {
   this.dialogService.confirmWithTitle(`Delete Daily Affirmation Text`, `Do you want to delete the Daily Affirmation Text ?`).then(res => {
    if(!res) return;
    this.httpService.markDailyAffirmationTextAsDeleted(item.dailyAffirmationTextId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
      if(!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Daily Affirmation Text");
      this.dialogService.success("Successfully deleted the Daily Affirmation Text");
      this.getDailyAffirmationTexts(this.searchModel);
       return;
    });
   });
  }

  /***
  public enableDailyAffirmationText(item: DailyAffirmationTextModel): void {
    this.dialogService.confirmWithTitle(`Enable DailyAffirmationText`, `Do you want to enable the DailyAffirmationText - ${item.dailyAffirmationTextName}?`).then(res => {
      if (!res) return;
      this.httpService.enableDailyAffirmationText(item.dailyAffirmationTextId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the DailyAffirmationText");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableDailyAffirmationText(item: DailyAffirmationTextModel): void {
    this.dialogService.confirmWithTitle(`Disable DailyAffirmationText`, `Do you want to disable the DailyAffirmationText - ${item.dailyAffirmationTextName}?`).then(res => {
      if (!res) return;
      this.httpService.disableDailyAffirmationText(item.dailyAffirmationTextId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the DailyAffirmationText");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportDailyAffirmationTextsModal(): void {
    const modalRef = this.modalService.open(DailyAffirmationTextCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationTextCsvFileImportModalComponent;
    componentInstance.title = 'Import DailyAffirmationTexts';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getDailyAffirmationTexts(this.searchModel);
    }
  }

  exportDailyAffirmationTextsModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportDailyAffirmationTextsOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getDailyAffirmationTexts(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if(f.invalid) return;	
          this.searchModel.categoryId =  this.selectedCategory ? this.selectedCategory.categoryId : null;
          this.searchModel.userId =  this.selectedUser ? this.selectedUser.userId : null;
    
     this.getDailyAffirmationTexts(this.searchModel);
  }

 public onReset(f: NgForm): void { 
    f.resetForm();
    this.searchModel.page = 1;
          this.searchModel.categoryId =  null;
          this.searchModel.userId =  null;
    this.getDailyAffirmationTexts(this.searchModel);
  }

   public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header,this.searchModel,{ currentSort : this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up': 'down';
    this.getDailyAffirmationTexts(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
      this.searchModel.page = e.page;
      this.searchModel.pageSize = e.pageSize;
      this.getDailyAffirmationTexts(this.searchModel);
  }
}
