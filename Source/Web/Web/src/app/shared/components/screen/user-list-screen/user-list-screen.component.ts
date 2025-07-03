import { CompanyModel, CompanySearchCriteria, LoggedInUserModel, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';
import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { SidebarLayoutTypes, UserStatusTypes } from 'src/common/models/app-custom-enums';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { CrudService } from '../../../services/crud.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { Router } from '@angular/router';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';
import { UserCrudModalComponent } from '../../../modals/user-crud-modal/user-crud-modal.component';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';

@UntilDestroy()
@Component({
    selector: 'app-user-list-screen',
    templateUrl: './user-list-screen.component.html',
    standalone: false
})
export class UserListScreenComponent extends BaseListComponent<UserModel, UserSearchCriteria> implements OnInit {

  @Input() public isMovieConfig = false;
  @Input() public movieId: number | undefined;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | undefined;
  @Input() public layoutTypeId: number | undefined;
  @Input() public isSuperAdmin = false;
  @Input() public isConfigurator = false;
  @Input() public isProductionExecutive = false;
  public override searchModel: UserSearchCriteria = new UserSearchCriteria();
  public currentUser: LoggedInUserModel | null = null;
  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;
  public predefinedUserTypes: Status[] = [];
  public selectedPredefinedUserTypes: Status[] = [];
  public userStatus: Status[] = [];
  public selectedUserStatus: Status | undefined;
  public sidebarLayoutType: typeof SidebarLayoutTypes = SidebarLayoutTypes;
  public predefinedUserType = PredefinedUserTypes;
  public override columns = [
    {
      text: 'User',
      align: 'left',
      sortable: true,
      value: 'userName',
      width: '115px',
    },
    {
      text: 'First Name',
      align: 'left',
      sortable: true,
      value: 'firstName',
      width: '115px',
    },
    {
      text: 'Last Name',
      align: 'left',
      sortable: true,
      value: 'lastName',
      width: '115px',
    },
    {
      text: 'Email',
      align: 'left',
      sortable: true,
      value: 'email',
      width: '115px',
    },
    {
      text: 'Type',
      align: 'left',
      sortable: true,
      value: 'predefinedUserTypeName',
      width: '115px',
    },
    {
      text: 'Company',
      align: 'left',
      sortable: true,
      value: 'companyName',
      width: '115px',
    },
    {
      text: 'Work Phone',
      align: 'left',
      sortable: true,
      value: 'workPhone',
      width: '115px',
    },
    {
      text: 'Active',
      align: 'left',
      sortable: true,
      value: 'active',
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
      text: 'Modified By',
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
    private customSelectService: CustomSelectService,
    private routerDataService: RouterDataService,
    private companySessionService: CompanySessionService,
    private router: Router,
  ) {
    super();
    this.titleService.pageTitle = "Users";
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.userStatus = this.customSelectService.getUserStatus();
    const routerParams = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    this.layoutTypeId = RouterUtils.getRouterDataNumber(routerParams?.['layoutTypeId']) ?? 0;
    if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.predefinedUserTypes = this.selectService.GetPredefinedUserTypes();
      this.isSuperAdmin = true;
      // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
      this.companyId = RouterUtils.getRouterDataNumber(routerParams?.['companyId']) ?? 0;
      if (!this.layoutTypeId || this.layoutTypeId == SidebarLayoutTypes.CompanyList) {
        this.getCompanies();
      } else {
        const sessionCompany = this.companySessionService.getCurrentCompany();
        if (sessionCompany && sessionCompany?.companyId === this.companyId) {
          this.companyName = sessionCompany?.companyName;
        } else {
          this.getCompanyInfo(this.companyId);
        }
      }
      this.initTypeAheads();
    } else if (this.currentUser && this.currentUser.userTypeId && (this.currentUser.userTypeId == PredefinedUserTypes.CompanyAdmin)) {
      this.predefinedUserTypes = this.selectService.GetPredefinedUserTypes().filter(item => item.id !== PredefinedUserTypes.SuperAdmin);
      this.companyId = this.currentUser?.companyId ?? 0;
      this.companyName = this.currentUser?.companyName;
      this.isConfigurator = true;
    }

    if (this.currentUser && this.currentUser.userTypeId) {
      this.isProductionExecutive = true;
    }

    if (!this.isSuperAdmin) {
      // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
      this.columns = this.columns.filter(item => item.value != 'companyName');
    }
    this.searchModel.active = true;
    if (this.companyId) this.searchModel.companyIds = [this.companyId];
    this.getUsers(this.searchModel);
  }

  public initTypeAheads(): void {
    this.companyTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.companyTypeaheadLoading = true;
      const companySearchCriteria = new CompanySearchCriteria();
      companySearchCriteria.keyword = searchValue;
      companySearchCriteria.companyId = this.companyId;
      return this.httpService.getCompanies(companySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) {
          this.companyTypeaheadLoading = false;
          if (data.model) this.companys = data.model;
        }
      },
      error: (err: unknown) => {
        this.companyTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

  public getCompanyInfo(companyId: number): void {
    this.httpService.getCompany(companyId).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Company", result);
          return;
        }
        if (result.model) {
          this.companyName = result.model?.companyName;
          this.companySessionService.setCurrentCompany(result.model);
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Company"),
      complete: () => null,
    });
  }

  public getCompanies(): void {
    const searchModel = new CompanySearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    searchModel.isActive = true;
    this.httpService.getCompanies(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.companys = data.model;
        this.selectedCompany = this.companys.find(item => item.companyId == this.companyId);
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Company');
      },
      complete: () => null,
    });
  }

  public getUsers(searchModel: UserSearchCriteria): void {
    if (!this.isSuperAdmin && this.companyId) searchModel.companyIds = [this.companyId];
    this.httpService.getUsers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Users", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Users"),
      complete: () => null,
    });
  }

  public showAddUserModal(): void {
    const modalRef = this.modalService.open(UserCrudModalComponent, { windowClass: 'my-class', size: 'xl', scrollable: true });
    const componentInstance = modalRef.componentInstance as UserCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create User';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.movieId = this.movieId;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.onCloseCallback = (): void => {
      this.getUsers(this.searchModel);
    }
  }

  public showViewUserModal(item: UserModel): void {
    const modalRef = this.modalService.open(UserCrudModalComponent, { windowClass: 'my-class', size: 'xl', scrollable: true });
    const componentInstance = modalRef.componentInstance as UserCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View User';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.movieId = this.movieId;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.userModel = JSON.parse(JSON.stringify(item)) as UserModel;
  }

  public showUpdateUserModal(item: UserModel): void {
    const modalRef = this.modalService.open(UserCrudModalComponent, { windowClass: 'my-class', size: 'xl', scrollable: true });
    const componentInstance = modalRef.componentInstance as UserCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit User';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.movieId = this.movieId;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.userModel = JSON.parse(JSON.stringify(item)) as UserModel;
    componentInstance.onCloseCallback = (): void => {
      this.getUsers(this.searchModel);
    }
  }

  public deleteUser(item: UserModel): void {
    this.dialogService.confirmWithTitle(`Delete User`, `Do you want to delete the User - ${item.userName}?`).then(res => {
      if (!res) return;
      this.httpService.markUserAsDeleted(item.userId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the User");
        this.getUsers(this.searchModel);
        this.dialogService.success("Successfully deleted the User");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    if (!this.layoutTypeId || this.layoutTypeId == SidebarLayoutTypes.CompanyList) {
      this.searchModel.companyIds = this.selectedCompany ? [this.selectedCompany.companyId] : null;
    }
    const predefinedUserTypesArray = this.selectedPredefinedUserTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedUserTypeIds = predefinedUserTypesArray;
    if (this.selectedUserStatus && this.selectedUserStatus.id == UserStatusTypes.Active) {
      this.searchModel.active = true;
    } else if (this.selectedUserStatus && this.selectedUserStatus.id == UserStatusTypes.InActive) {
      this.searchModel.active = false;
    } else {
      this.searchModel.active = null;
    }
    this.getUsers(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    if (!this.layoutTypeId || this.layoutTypeId == SidebarLayoutTypes.CompanyList) {
      this.searchModel.companyIds = null;
    }
    this.searchModel.predefinedUserTypeIds = null;
    this.searchModel.active = null;
    this.getUsers(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getUsers(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getUsers(this.searchModel);
  }

}
