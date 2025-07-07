
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VisionBoardSessionCrudModalComponent } from '../../../modals/vision-board-session-crud-modal/vision-board-session-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { UserModel , UserSearchCriteria , VisionBoardSessionModel, VisionBoardSessionSearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
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
  selector: 'app-vision-board-session-list-screen',
  templateUrl: './vision-board-session-list-screen.component.html',
  standalone: false,
})
export class VisionBoardSessionListScreenComponent extends BaseListComponent<VisionBoardSessionModel, VisionBoardSessionSearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: VisionBoardSessionSearchCriteria = new VisionBoardSessionSearchCriteria();

  public currentUser: LoggedInUserModel | null = null;

  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading:boolean = false;    
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public override columns = [
    {
      text: 'Vision Board Session Date',
      align: 'left',
      sortable: true,
      value: 'visionBoardSessionDate',
      width: '115px',
    },
    {
      text: 'Vision Board Session Time Spent In Seconds',
      align: 'left',
      sortable: true,
      value: 'visionBoardSessionTimeSpentInSeconds',
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
    this.titleService.pageTitle = "Vision Board Session";
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
  this.getVisionBoardSessions(this.searchModel);
	    this.getUsers();
    this.initTypeAheads();
  }
 
 public initTypeAheads(): void {
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
  
 public getVisionBoardSessions(searchModel: VisionBoardSessionSearchCriteria): void {
 
    this.httpService.getVisionBoardSessions(searchModel).subscribe({
      next: result => {
      if(!result.isSuccess) {
        this.dialogService.errorWithReturnModel("Unable to get Vision Board Sessions", result);
        return;
      }

      if(result.model) {
       this.listData = result.model;
       this.totalItems = result.totalRecords ??  0;
      }
     },
    error: (err) => this.dialogService.error("Unable to get Vision Board Sessions"),
    complete: () => null,
  });
  }

  public showAddVisionBoardSessionModal(): void {
    const modalRef = this.modalService.open(VisionBoardSessionCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as VisionBoardSessionCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Vision Board Session';
    componentInstance.onCloseCallback = (): void => {
      this.getVisionBoardSessions(this.searchModel);
    }
  }

  public showViewVisionBoardSessionModal(item: VisionBoardSessionModel): void {
    const modalRef = this.modalService.open(VisionBoardSessionCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as VisionBoardSessionCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Vision Board Session';
    componentInstance.visionBoardSessionModel = JSON.parse(JSON.stringify(item)) as VisionBoardSessionModel;
  }

  public showUpdateVisionBoardSessionModal(item: VisionBoardSessionModel): void {
    const modalRef = this.modalService.open(VisionBoardSessionCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as VisionBoardSessionCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Vision Board Session';
    componentInstance.visionBoardSessionModel = JSON.parse(JSON.stringify(item)) as VisionBoardSessionModel;
    componentInstance.onCloseCallback = (): void => {
      this.getVisionBoardSessions(this.searchModel);
    }
  }

  public deleteVisionBoardSession(item: VisionBoardSessionModel): void {
   this.dialogService.confirmWithTitle(`Delete Vision Board Session`, `Do you want to delete the Vision Board Session - ${item.visionBoardSessionName}?`).then(res => {
    if(!res) return;
    this.httpService.markVisionBoardSessionAsDeleted(item.visionBoardSessionId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
      if(!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Vision Board Session");
      this.dialogService.success("Successfully deleted the Vision Board Session");
      this.getVisionBoardSessions(this.searchModel);
       return;
    });
   });
  }

  /***
  public enableVisionBoardSession(item: VisionBoardSessionModel): void {
    this.dialogService.confirmWithTitle(`Enable VisionBoardSession`, `Do you want to enable the VisionBoardSession - ${item.visionBoardSessionName}?`).then(res => {
      if (!res) return;
      this.httpService.enableVisionBoardSession(item.visionBoardSessionId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the VisionBoardSession");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableVisionBoardSession(item: VisionBoardSessionModel): void {
    this.dialogService.confirmWithTitle(`Disable VisionBoardSession`, `Do you want to disable the VisionBoardSession - ${item.visionBoardSessionName}?`).then(res => {
      if (!res) return;
      this.httpService.disableVisionBoardSession(item.visionBoardSessionId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the VisionBoardSession");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportVisionBoardSessionsModal(): void {
    const modalRef = this.modalService.open(VisionBoardSessionCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as VisionBoardSessionCsvFileImportModalComponent;
    componentInstance.title = 'Import VisionBoardSessions';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getVisionBoardSessions(this.searchModel);
    }
  }

  exportVisionBoardSessionsModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportVisionBoardSessionsOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getVisionBoardSessions(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if(f.invalid) return;	
          this.searchModel.userId =  this.selectedUser ? this.selectedUser.userId : null;
    
     this.getVisionBoardSessions(this.searchModel);
  }

 public onReset(f: NgForm): void { 
    f.resetForm();
    this.searchModel.page = 1;
          this.searchModel.userId =  null;
    this.getVisionBoardSessions(this.searchModel);
  }

   public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header,this.searchModel,{ currentSort : this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up': 'down';
    this.getVisionBoardSessions(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
      this.searchModel.page = e.page;
      this.searchModel.pageSize = e.pageSize;
      this.getVisionBoardSessions(this.searchModel);
  }
}
