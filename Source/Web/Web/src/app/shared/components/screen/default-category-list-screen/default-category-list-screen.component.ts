
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DefaultCategoryCrudModalComponent } from '../../../modals/default-category-crud-modal/default-category-crud-modal.component';
import { CrudService } from '../../../services/crud.service';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { DefaultCategoryModel, DefaultCategorySearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
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

@UntilDestroy()
@Component({
  selector: 'app-default-category-list-screen',
  templateUrl: './default-category-list-screen.component.html',
  standalone: false,
})
export class DefaultCategoryListScreenComponent extends BaseListComponent<DefaultCategoryModel, DefaultCategorySearchCriteria> implements OnInit {

  @Input() public companyId: number = 0;
  @Input() public companyTypeId: number = 0;
  @Input() public companyName: string = "";
  @Input() public layoutTypeId: number | null = null;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigScreen: boolean = false;

  public override searchModel: DefaultCategorySearchCriteria = new DefaultCategorySearchCriteria();

  public currentUser: LoggedInUserModel | null = null;

  public predefinedCategoryTypes: Status[] = [];
  public selectedPredefinedCategoryTypes: Status[] = [];

  public override columns = [
    {
      text: 'Default Category Name',
      align: 'left',
      sortable: true,
      value: 'defaultCategoryName',
      width: '115px',
    },
    {
      text: 'Predefined Category Type Name',
      align: 'left',
      sortable: true,
      value: 'predefinedCategoryTypeName',
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
    this.titleService.pageTitle = "Default Category";
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
    this.getDefaultCategorys(this.searchModel);
    this.predefinedCategoryTypes = this.selectService.GetPredefinedCategoryTypes();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
  }


  public getDefaultCategorys(searchModel: DefaultCategorySearchCriteria): void {
    this.httpService.getDefaultCategorys(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Default Categorys", result);
          return;
        }

        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err) => this.dialogService.error("Unable to get Default Categorys"),
      complete: () => null,
    });
  }

  public showAddDefaultCategoryModal(): void {
    const modalRef = this.modalService.open(DefaultCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultCategoryCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Create Default Category';
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultCategorys(this.searchModel);
    }
  }

  public showViewDefaultCategoryModal(item: DefaultCategoryModel): void {
    const modalRef = this.modalService.open(DefaultCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultCategoryCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'View Default Category';
    componentInstance.defaultCategoryModel = JSON.parse(JSON.stringify(item)) as DefaultCategoryModel;
  }

  public showUpdateDefaultCategoryModal(item: DefaultCategoryModel): void {
    const modalRef = this.modalService.open(DefaultCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultCategoryCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.title = 'Edit Default Category';
    componentInstance.defaultCategoryModel = JSON.parse(JSON.stringify(item)) as DefaultCategoryModel;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultCategorys(this.searchModel);
    }
  }

  public deleteDefaultCategory(item: DefaultCategoryModel): void {
    this.dialogService.confirmWithTitle(`Delete Default Category`, `Do you want to delete the Default Category - ${item.defaultCategoryName}?`).then(res => {
      if (!res) return;
      this.httpService.markDefaultCategoryAsDeleted(item.defaultCategoryId, getHeaderWithLoadingText("Deleting..")).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Default Category");
        this.dialogService.success("Successfully deleted the Default Category");
        this.getDefaultCategorys(this.searchModel);
        return;
      });
    });
  }

  /***
  public enableDefaultCategory(item: DefaultCategoryModel): void {
    this.dialogService.confirmWithTitle(`Enable DefaultCategory`, `Do you want to enable the DefaultCategory - ${item.defaultCategoryName}?`).then(res => {
      if (!res) return;
      this.httpService.enableDefaultCategory(item.defaultCategoryId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the DefaultCategory");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableDefaultCategory(item: DefaultCategoryModel): void {
    this.dialogService.confirmWithTitle(`Disable DefaultCategory`, `Do you want to disable the DefaultCategory - ${item.defaultCategoryName}?`).then(res => {
      if (!res) return;
      this.httpService.disableDefaultCategory(item.defaultCategoryId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the DefaultCategory");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }
  
  showImportDefaultCategorysModal(): void {
    const modalRef = this.modalService.open(DefaultCategoryCsvFileImportModalComponent, { size: 'lg', windowClass: 'my-class', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultCategoryCsvFileImportModalComponent;
    componentInstance.title = 'Import DefaultCategorys';
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (): void => {
      this.getDefaultCategorys(this.searchModel);
    }
  }

  exportDefaultCategorysModal(): void {
    const exportResourceSearchCriteria = new ExportResourceSearchCriteria();
    exportResourceSearchCriteria.companyId = this.companyId;
    const exportUrl = this.httpService.exportDefaultCategorysOfCompanyToCsvUrl(exportResourceSearchCriteria, this.currentUser.token);
    this.customSelectService.export(exportUrl);
    this.getDefaultCategorys(this.searchModel);
  }
   ***/

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    const predefinedCategoryTypesArray = this.selectedPredefinedCategoryTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedCategoryTypeIds = predefinedCategoryTypesArray;

    this.getDefaultCategorys(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.predefinedCategoryTypeIds = null;
    this.getDefaultCategorys(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getDefaultCategorys(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getDefaultCategorys(this.searchModel);
  }
}
