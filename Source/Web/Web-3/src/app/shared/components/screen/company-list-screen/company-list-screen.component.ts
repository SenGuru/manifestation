import { CompanyModel, CompanySearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
import { CompanyStatusTypes, SidebarLayoutTypes } from 'src/common/models/app-custom-enums';
import { Component, Input, OnInit } from '@angular/core';
import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { PredefinedCompanyTypes, PredefinedUserTypes } from 'src/common/models/app-enums';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CompanyCrudModalComponent } from '../../../modals/company-crud-modal/company-crud-modal.component';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { CrudService } from '../../../services/crud.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';

@Component({
    selector: 'app-company-list-screen',
    templateUrl: './company-list-screen.component.html',
    standalone: false
})
export class CompanyListScreenComponent extends BaseListComponent<CompanyModel, CompanySearchCriteria> implements OnInit {

  @Input() public companyAction: string | undefined;
  @Input() public isSuperAdmin = false;
  public override searchModel: CompanySearchCriteria = new CompanySearchCriteria();
  public companyStatus: Status[] = [];
  public selectedCompanyStatus: Status | undefined;
  public predefinedCompanyType = PredefinedCompanyTypes;
  public initialUrl: string | undefined;
  public currentUser: LoggedInUserModel | null = null;
  public override columns = [
    {
      text: 'Company Name',
      align: 'left',
      sortable: true,
      value: 'companyName',
      width: '115px',
    },
    {
      text: 'Contact Name',
      align: 'left',
      sortable: true,
      value: 'contactFirstName',
      width: '115px',
    },
    {
      text: 'Status',
      align: 'left',
      sortable: true,
      value: 'active',
      width: '115px',
    },
    {
      text: 'City',
      align: 'left',
      sortable: true,
      value: 'city',
      width: '115px',
    },
    {
      text: 'State',
      align: 'left',
      sortable: true,
      value: 'stateName',
      width: '115px',
    },
    {
      text: 'Country',
      align: 'left',
      sortable: true,
      value: 'countryName',
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
    private companySessionService: CompanySessionService,
    private httpService: HttpService,
    private modalService: NgbModal,
    private router: Router,
    private crudService: CrudService,
    private titleService: TitleService,
    private customSelectService: CustomSelectService,
    public enumViewService: EnumViewService,

  ) {
    super();
    this.titleService.pageTitle = "Company";
  }

  public ngOnInit(): void {
    this.initialUrl = this.sessionService.getInitialUrl();
    this.currentUser = this.sessionService.getCurrentUserInfo();
    if (this.currentUser && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.isSuperAdmin = true;
    }
    this.searchModel.isActive = null;
    this.getCompanies(this.searchModel);
    this.companyStatus = this.customSelectService.getCompaniestatus();
  }

  public getCompanies(searchModel: CompanySearchCriteria): void {
    this.httpService.getCompanies(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Companies", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Companies"),
      complete: () => null,
    });
  }

  public showAddCompanyModal(): void {
    const modalRef = this.modalService.open(CompanyCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as CompanyCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Company';
    componentInstance.onCloseCallback = (): void => {
      this.getCompanies(this.searchModel);
    }
  }

  public showViewCompanyDetails(item: CompanyModel): void {
    this.companySessionService.setCurrentCompany(item);
     
    this.router.navigateByUrl(`${this.initialUrl}/company/${SidebarLayoutTypes.CompanyDetail}/${item.companyId}/company-config`);
  }

  public showViewCompanyModal(item: CompanyModel): void {
    const modalRef = this.modalService.open(CompanyCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as CompanyCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Company';
    componentInstance.companyModel = JSON.parse(JSON.stringify(item)) as CompanyModel;
  }

  public showUpdateCompanyModal(item: CompanyModel): void {
    const modalRef = this.modalService.open(CompanyCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as CompanyCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Company';
    componentInstance.companyModel = JSON.parse(JSON.stringify(item)) as CompanyModel;
    componentInstance.onCloseCallback = (): void => {
      this.getCompanies(this.searchModel);
    }
  }

  public enableCompany(item: CompanyModel): void {
    this.dialogService.confirmWithTitle(`Enable Company`, `Do you want to enable the Company - ${item.companyName}?`).then(res => {
      if (!res) return;
      this.httpService.enableCompany(item.companyId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to enable the Company");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public disableCompany(item: CompanyModel): void {
    this.dialogService.confirmWithTitle(`Disable Company`, `Do you want to disable the Company - ${item.companyName}?`).then(res => {
      if (!res) return;
      this.httpService.disableCompany(item.companyId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to disable the Company");
        this.getCompanies(this.searchModel);
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
     
    if (this.selectedCompanyStatus && this.selectedCompanyStatus.id === CompanyStatusTypes.Active) {
      this.searchModel.isActive = true;
       
    } else if (this.selectedCompanyStatus && this.selectedCompanyStatus.id === CompanyStatusTypes.InActive) {
      this.searchModel.isActive = false;
    } else {
      this.searchModel.isActive = null;
    }
    this.getCompanies(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.predefinedCompanyTypeIds = null;
    this.searchModel.stateId = null;
    this.searchModel.isActive = null;
    this.searchModel.countryId = null;
    this.getCompanies(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getCompanies(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getCompanies(this.searchModel);
  }

}
