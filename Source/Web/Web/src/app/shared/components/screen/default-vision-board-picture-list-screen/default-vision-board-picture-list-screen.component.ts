
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DefaultVisionBoardPictureCrudModalComponent } from '../../../modals/default-vision-board-picture-crud-modal/default-vision-board-picture-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { AstroClassColumnModel, Status } from 'src/common/models/app-custom-model';
import {  DefaultVisionBoardPictureModel, DefaultVisionBoardPictureSearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
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
  selector: 'app-default-vision-board-picture-list-screen',
  templateUrl: './default-vision-board-picture-list-screen.component.html',
  standalone: false,
})
export class DefaultVisionBoardPictureListScreenComponent extends BaseListComponent<DefaultVisionBoardPictureModel, DefaultVisionBoardPictureSearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: DefaultVisionBoardPictureSearchCriteria = new DefaultVisionBoardPictureSearchCriteria();

  public currentUser: LoggedInUserModel | null = null;


  public override columns = [
    {
      text: 'Location',
      align: 'left',
      sortable: true,
      value: 'location',
      width: '115px',
    },
    {
      text: 'File Name',
      align: 'left',
      sortable: true,
      value: 'fileName',
      width: '115px',
    },
    {
      text: 'File Size',
      align: 'left',
      sortable: true,
      value: 'fileSize',
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
    this.titleService.pageTitle = "Default Vision Board Picture";
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
  this.getDefaultVisionBoardPictures(this.searchModel);
    this.initTypeAheads();
  }
 
 public initTypeAheads(): void {
  }

  
 public getDefaultVisionBoardPictures(searchModel: DefaultVisionBoardPictureSearchCriteria): void {
 
    this.httpService.getDefaultVisionBoardPictures(searchModel).subscribe({
      next: result => {
      if(!result.isSuccess) {
        this.dialogService.errorWithReturnModel("Unable to get Default Vision Board Pictures", result);
        return;
      }

      if(result.model) {
       this.listData = result.model;
       this.totalItems = result.totalRecords ??  0;
      }
     },
    error: (err) => this.dialogService.error("Unable to get Default Vision Board Pictures"),
    complete: () => null,
  });
  }

  public showAddDefaultVisionBoardPictureModal(): void {
    const modalRef = this.modalService.open(DefaultVisionBoardPictureCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultVisionBoardPictureCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Default Vision Board Picture';
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultVisionBoardPictures(this.searchModel);
    }
  }

  public showViewDefaultVisionBoardPictureModal(item: DefaultVisionBoardPictureModel): void {
    const modalRef = this.modalService.open(DefaultVisionBoardPictureCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultVisionBoardPictureCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Default Vision Board Picture';
    componentInstance.defaultVisionBoardPictureModel = JSON.parse(JSON.stringify(item)) as DefaultVisionBoardPictureModel;
  }

  public showUpdateDefaultVisionBoardPictureModal(item: DefaultVisionBoardPictureModel): void {
    const modalRef = this.modalService.open(DefaultVisionBoardPictureCrudModalComponent, { windowClass: 'my-class',size:'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultVisionBoardPictureCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Default Vision Board Picture';
    componentInstance.defaultVisionBoardPictureModel = JSON.parse(JSON.stringify(item)) as DefaultVisionBoardPictureModel;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultVisionBoardPictures(this.searchModel);
    }
  }

  public deleteDefaultVisionBoardPicture(item: DefaultVisionBoardPictureModel): void {
   this.dialogService.confirmWithTitle(`Delete Default Vision Board Picture`, `Do you want to delete the Default Vision Board Picture  ?`).then(res => {
    if(!res) return;
    this.httpService.markDefaultVisionBoardPictureAsDeleted(item.defaultVisionBoardPictureId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
      if(!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Default Vision Board Picture");
      this.dialogService.success("Successfully deleted the Default Vision Board Picture");
      this.getDefaultVisionBoardPictures(this.searchModel);
       return;
    });
   });
  }

  /***
  public enableDefaultVisionBoardPicture(item: DefaultVisionBoardPictureModel): void {
    this.dialogService.confirmWithTitle(`Enable DefaultVisionBoardPicture`, `Do you want to enable the DefaultVisionBoardPicture - ${item.defaultVisionBoardPictureName}?`).then(res => {
      if (!res) return;
      this.httpService.enableDefaultVisionBoardPicture(item.defaultVisionBoardPictureId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the DefaultVisionBoardPicture");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableDefaultVisionBoardPicture(item: DefaultVisionBoardPictureModel): void {
    this.dialogService.confirmWithTitle(`Disable DefaultVisionBoardPicture`, `Do you want to disable the DefaultVisionBoardPicture - ${item.defaultVisionBoardPictureName}?`).then(res => {
      if (!res) return;
      this.httpService.disableDefaultVisionBoardPicture(item.defaultVisionBoardPictureId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the DefaultVisionBoardPicture");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportDefaultVisionBoardPicturesModal(): void {
    const modalRef = this.modalService.open(DefaultVisionBoardPictureCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultVisionBoardPictureCsvFileImportModalComponent;
    componentInstance.title = 'Import DefaultVisionBoardPictures';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultVisionBoardPictures(this.searchModel);
    }
  }

  exportDefaultVisionBoardPicturesModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportDefaultVisionBoardPicturesOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getDefaultVisionBoardPictures(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if(f.invalid) return;	
    
     this.getDefaultVisionBoardPictures(this.searchModel);
  }

 public onReset(f: NgForm): void { 
    f.resetForm();
    this.searchModel.page = 1;
    this.getDefaultVisionBoardPictures(this.searchModel);
  }

   public changeSort(header: AstroClassColumnModel): void {
    this.crudService.baseChangeSort(header,this.searchModel,{ currentSort : this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up': 'down';
    this.getDefaultVisionBoardPictures(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
      this.searchModel.page = e.page;
      this.searchModel.pageSize = e.pageSize;
      this.getDefaultVisionBoardPictures(this.searchModel);
  }
}
