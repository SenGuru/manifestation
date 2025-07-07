
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DefaultGoalTrackerCrudModalComponent } from '../../../modals/default-goal-tracker-crud-modal/default-goal-tracker-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import {  DefaultGoalTrackerModel, DefaultGoalTrackerSearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
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
  selector: 'app-default-goal-tracker-list-screen',
  templateUrl: './default-goal-tracker-list-screen.component.html',
  standalone: false,
})
export class DefaultGoalTrackerListScreenComponent extends BaseListComponent<DefaultGoalTrackerModel, DefaultGoalTrackerSearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: DefaultGoalTrackerSearchCriteria = new DefaultGoalTrackerSearchCriteria();

  public currentUser: LoggedInUserModel | null = null;


  public override columns = [
    {
      text: 'Default Goal Tracker Text',
      align: 'left',
      sortable: true,
      value: 'defaultGoalTrackerText',
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
    this.titleService.pageTitle = "Default Goal Tracker";
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
  this.getDefaultGoalTrackers(this.searchModel);
    this.initTypeAheads();
  }
 
 public initTypeAheads(): void {
  }

  
 public getDefaultGoalTrackers(searchModel: DefaultGoalTrackerSearchCriteria): void {
 
    this.httpService.getDefaultGoalTrackers(searchModel).subscribe({
      next: result => {
      if(!result.isSuccess) {
        this.dialogService.errorWithReturnModel("Unable to get Default Goal Trackers", result);
        return;
      }

      if(result.model) {
       this.listData = result.model;
       this.totalItems = result.totalRecords ??  0;
      }
     },
    error: (err) => this.dialogService.error("Unable to get Default Goal Trackers"),
    complete: () => null,
  });
  }

  public showAddDefaultGoalTrackerModal(): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Default Goal Tracker';
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultGoalTrackers(this.searchModel);
    }
  }

  public showViewDefaultGoalTrackerModal(item: DefaultGoalTrackerModel): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Default Goal Tracker';
    componentInstance.defaultGoalTrackerModel = JSON.parse(JSON.stringify(item)) as DefaultGoalTrackerModel;
  }

  public showUpdateDefaultGoalTrackerModal(item: DefaultGoalTrackerModel): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Default Goal Tracker';
    componentInstance.defaultGoalTrackerModel = JSON.parse(JSON.stringify(item)) as DefaultGoalTrackerModel;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultGoalTrackers(this.searchModel);
    }
  }

  public deleteDefaultGoalTracker(item: DefaultGoalTrackerModel): void {
   this.dialogService.confirmWithTitle(`Delete Default Goal Tracker`, `Do you want to delete the Default Goal Tracker ?`).then(res => {
    if(!res) return;
    this.httpService.markDefaultGoalTrackerAsDeleted(item.defaultGoalTrackerId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
      if(!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Default Goal Tracker");
      this.dialogService.success("Successfully deleted the Default Goal Tracker");
      this.getDefaultGoalTrackers(this.searchModel);
       return;
    });
   });
  }

  /***
  public enableDefaultGoalTracker(item: DefaultGoalTrackerModel): void {
    this.dialogService.confirmWithTitle(`Enable DefaultGoalTracker`, `Do you want to enable the DefaultGoalTracker - ${item.defaultGoalTrackerName}?`).then(res => {
      if (!res) return;
      this.httpService.enableDefaultGoalTracker(item.defaultGoalTrackerId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the DefaultGoalTracker");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableDefaultGoalTracker(item: DefaultGoalTrackerModel): void {
    this.dialogService.confirmWithTitle(`Disable DefaultGoalTracker`, `Do you want to disable the DefaultGoalTracker - ${item.defaultGoalTrackerName}?`).then(res => {
      if (!res) return;
      this.httpService.disableDefaultGoalTracker(item.defaultGoalTrackerId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the DefaultGoalTracker");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportDefaultGoalTrackersModal(): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCsvFileImportModalComponent;
    componentInstance.title = 'Import DefaultGoalTrackers';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultGoalTrackers(this.searchModel);
    }
  }

  exportDefaultGoalTrackersModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportDefaultGoalTrackersOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getDefaultGoalTrackers(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if(f.invalid) return;	
    
     this.getDefaultGoalTrackers(this.searchModel);
  }

 public onReset(f: NgForm): void { 
    f.resetForm();
    this.searchModel.page = 1;
    this.getDefaultGoalTrackers(this.searchModel);
  }

   public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header,this.searchModel,{ currentSort : this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up': 'down';
    this.getDefaultGoalTrackers(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
      this.searchModel.page = e.page;
      this.searchModel.pageSize = e.pageSize;
      this.getDefaultGoalTrackers(this.searchModel);
  }
}
