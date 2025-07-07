
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DefaultGoalTrackerCategoryCrudModalComponent } from '../../../modals/default-goal-tracker-category-crud-modal/default-goal-tracker-category-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { DefaultCategoryModel , DefaultCategorySearchCriteria ,DefaultGoalTrackerModel , DefaultGoalTrackerSearchCriteria , DefaultGoalTrackerCategoryModel, DefaultGoalTrackerCategorySearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
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
  selector: 'app-default-goal-tracker-category-list-screen',
  templateUrl: './default-goal-tracker-category-list-screen.component.html',
  standalone: false,
})
export class DefaultGoalTrackerCategoryListScreenComponent extends BaseListComponent<DefaultGoalTrackerCategoryModel, DefaultGoalTrackerCategorySearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: DefaultGoalTrackerCategorySearchCriteria = new DefaultGoalTrackerCategorySearchCriteria();

  public currentUser: LoggedInUserModel | null = null;

  public defaultCategoryTypeahead$ = new Subject<string>();
  public defaultCategoryTypeaheadLoading:boolean = false;    
  public defaultCategorys: DefaultCategoryModel[] = [];
  public selectedDefaultCategory: DefaultCategoryModel | undefined;
  public defaultGoalTrackerTypeahead$ = new Subject<string>();
  public defaultGoalTrackerTypeaheadLoading:boolean = false;    
  public defaultGoalTrackers: DefaultGoalTrackerModel[] = [];
  public selectedDefaultGoalTracker: DefaultGoalTrackerModel | undefined;

  public override columns = [
    {
      text: 'Default Category Name',
      align: 'left',
      sortable: true,
      value: 'defaultCategoryName',
      width: '115px',
    },
    {
      text: 'Default Goal Tracker Name',
      align: 'left',
      sortable: true,
      value: 'defaultGoalTrackerName',
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
    this.titleService.pageTitle = "Default Goal Tracker Category";
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
  this.getDefaultGoalTrackerCategorys(this.searchModel);
	    this.getDefaultCategorys();
	    this.getDefaultGoalTrackers();
    this.initTypeAheads();
  }
 
 public initTypeAheads(): void {
      this.defaultCategoryTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
        this.defaultCategoryTypeaheadLoading = true;
        const defaultCategorySearchCriteria = new DefaultCategorySearchCriteria();
        defaultCategorySearchCriteria.keyword = searchValue;
        return this.httpService.getDefaultCategorys(defaultCategorySearchCriteria, getHeaderWithBlockUI());
      }),untilDestroyed(this)).subscribe({
        next: (data) => {
          if(data.model) this.defaultCategorys = data.model;
      },  
      error: err => {
       //
      },
      complete: () =>  this.defaultCategoryTypeaheadLoading = false,});
      this.defaultGoalTrackerTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
        this.defaultGoalTrackerTypeaheadLoading = true;
        const defaultGoalTrackerSearchCriteria = new DefaultGoalTrackerSearchCriteria();
        defaultGoalTrackerSearchCriteria.keyword = searchValue;
        return this.httpService.getDefaultGoalTrackers(defaultGoalTrackerSearchCriteria, getHeaderWithBlockUI());
      }),untilDestroyed(this)).subscribe({
        next: (data) => {
          if(data.model) this.defaultGoalTrackers = data.model;
      },  
      error: err => {
       //
      },
      complete: () =>  this.defaultGoalTrackerTypeaheadLoading = false,});
  }

      public getDefaultCategorys(): void {
        const searchModel = new DefaultCategorySearchCriteria();
        searchModel.sortAscending = false;
        searchModel.sortBy = 'createdDate';
        this.httpService.getDefaultCategorys(searchModel).subscribe({
        next: (data) => {
          if (!data.isSuccess)
            return;
          if(data.model) this.defaultCategorys = data.model;
        },
        error: (error) => this.dialogService.error('Not Able to get Default Category'),
        complete: () => null,
        });
      }
      public getDefaultGoalTrackers(): void {
        const searchModel = new DefaultGoalTrackerSearchCriteria();
        searchModel.sortAscending = false;
        searchModel.sortBy = 'createdDate';
        this.httpService.getDefaultGoalTrackers(searchModel).subscribe({
        next: (data) => {
          if (!data.isSuccess)
            return;
          if(data.model) this.defaultGoalTrackers = data.model;
        },
        error: (error) => this.dialogService.error('Not Able to get Default Goal Tracker'),
        complete: () => null,
        });
      }
  
 public getDefaultGoalTrackerCategorys(searchModel: DefaultGoalTrackerCategorySearchCriteria): void {
 
    this.httpService.getDefaultGoalTrackerCategorys(searchModel).subscribe({
      next: result => {
      if(!result.isSuccess) {
        this.dialogService.errorWithReturnModel("Unable to get Default Goal Tracker Categorys", result);
        return;
      }

      if(result.model) {
       this.listData = result.model;
       this.totalItems = result.totalRecords ??  0;
      }
     },
    error: (err) => this.dialogService.error("Unable to get Default Goal Tracker Categorys"),
    complete: () => null,
  });
  }

  public showAddDefaultGoalTrackerCategoryModal(): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCategoryCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCategoryCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Default Goal Tracker Category';
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultGoalTrackerCategorys(this.searchModel);
    }
  }

  public showViewDefaultGoalTrackerCategoryModal(item: DefaultGoalTrackerCategoryModel): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCategoryCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCategoryCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Default Goal Tracker Category';
    componentInstance.defaultGoalTrackerCategoryModel = JSON.parse(JSON.stringify(item)) as DefaultGoalTrackerCategoryModel;
  }

  public showUpdateDefaultGoalTrackerCategoryModal(item: DefaultGoalTrackerCategoryModel): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCategoryCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCategoryCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Default Goal Tracker Category';
    componentInstance.defaultGoalTrackerCategoryModel = JSON.parse(JSON.stringify(item)) as DefaultGoalTrackerCategoryModel;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultGoalTrackerCategorys(this.searchModel);
    }
  }

  public deleteDefaultGoalTrackerCategory(item: DefaultGoalTrackerCategoryModel): void {
   this.dialogService.confirmWithTitle(`Delete Default Goal Tracker Category`, `Do you want to delete the Default Goal Tracker Category ?`).then(res => {
    if(!res) return;
    this.httpService.markDefaultGoalTrackerCategoryAsDeleted(item.defaultGoalTrackerCategoryId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
      if(!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Default Goal Tracker Category");
      this.dialogService.success("Successfully deleted the Default Goal Tracker Category");
      this.getDefaultGoalTrackerCategorys(this.searchModel);
       return;
    });
   });
  }

  /***
  public enableDefaultGoalTrackerCategory(item: DefaultGoalTrackerCategoryModel): void {
    this.dialogService.confirmWithTitle(`Enable DefaultGoalTrackerCategory`, `Do you want to enable the DefaultGoalTrackerCategory - ${item.defaultGoalTrackerCategoryName}?`).then(res => {
      if (!res) return;
      this.httpService.enableDefaultGoalTrackerCategory(item.defaultGoalTrackerCategoryId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the DefaultGoalTrackerCategory");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableDefaultGoalTrackerCategory(item: DefaultGoalTrackerCategoryModel): void {
    this.dialogService.confirmWithTitle(`Disable DefaultGoalTrackerCategory`, `Do you want to disable the DefaultGoalTrackerCategory - ${item.defaultGoalTrackerCategoryName}?`).then(res => {
      if (!res) return;
      this.httpService.disableDefaultGoalTrackerCategory(item.defaultGoalTrackerCategoryId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the DefaultGoalTrackerCategory");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportDefaultGoalTrackerCategorysModal(): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCategoryCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCategoryCsvFileImportModalComponent;
    componentInstance.title = 'Import DefaultGoalTrackerCategorys';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultGoalTrackerCategorys(this.searchModel);
    }
  }

  exportDefaultGoalTrackerCategorysModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportDefaultGoalTrackerCategorysOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getDefaultGoalTrackerCategorys(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if(f.invalid) return;	
          this.searchModel.defaultCategoryId =  this.selectedDefaultCategory ? this.selectedDefaultCategory.defaultCategoryId : null;
          this.searchModel.defaultGoalTrackerId =  this.selectedDefaultGoalTracker ? this.selectedDefaultGoalTracker.defaultGoalTrackerId : null;
    
     this.getDefaultGoalTrackerCategorys(this.searchModel);
  }

 public onReset(f: NgForm): void { 
    f.resetForm();
    this.searchModel.page = 1;
          this.searchModel.defaultCategoryId =  null;
          this.searchModel.defaultGoalTrackerId =  null;
    this.getDefaultGoalTrackerCategorys(this.searchModel);
  }

   public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header,this.searchModel,{ currentSort : this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up': 'down';
    this.getDefaultGoalTrackerCategorys(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
      this.searchModel.page = e.page;
      this.searchModel.pageSize = e.pageSize;
      this.getDefaultGoalTrackerCategorys(this.searchModel);
  }
}
