import { CompanyModel, CompanySearchCriteria, PushNotificationServerModel, PushNotificationServerSearchCriteria } from 'src/common/models/app-model';
import { Component, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';

import { AuditDuoColumnModel } from 'src/common/models/app-custom-model';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PushNotificationServerCrudModalComponent } from '../../../modals/push-notification-server-crud-modal/push-notification-server-crud-modal.component';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';
import { untilDestroyed } from '@ngneat/until-destroy';

@Component({
    selector: 'app-push-notification-server-list-screen',
    templateUrl: './push-notification-server-list-screen.component.html',
    standalone: false
})
export class PushNotificationServerListScreenComponent extends BaseListComponent<PushNotificationServerModel, PushNotificationServerSearchCriteria> implements OnInit {

   public override searchModel: PushNotificationServerSearchCriteria = new PushNotificationServerSearchCriteria();

  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;

   public override columns = [
    {
      text: 'Server Name',
      align: 'left',
      sortable: true,
      value: 'pushNotificationServerName',
      width: '115px',
    },
    {
      text: 'Server Type',
      align: 'left',
      sortable: true,
      value: 'predefinedPushNotificationServerTypeName',
      width: '115px',
    },
    {
      text: 'Project Name',
      align: 'left',
      sortable: true,
      value: 'projectName',
      width: '115px',
    },
    {
      text: 'Is Enabled',
      align: 'left',
      sortable: true,
      value: 'isEnabled',
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
  ) {
    super();
    this.titleService.pageTitle = "Push Notification Server";
  }

public ngOnInit(): void {
    this.getPushNotificationServers(this.searchModel);
    this.getCompanies();
    this.initTypeAheads();
  }

public initTypeAheads(): void {
    this.companyTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.companyTypeaheadLoading = true;
      const companySearchCriteria = new CompanySearchCriteria();
      companySearchCriteria.isActive = true;
      companySearchCriteria.keyword = searchValue;
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

public getCompanies(): void {
    const searchModel = new CompanySearchCriteria();
    searchModel.sortAscending = false;
    searchModel.isActive = true;
    searchModel.sortBy = 'createdDate';
    this.httpService.getCompanies(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.companys = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Company');
      },
      complete: () => null,
    });
  }

public getPushNotificationServers(searchModel: PushNotificationServerSearchCriteria): void {
    this.httpService.getPushNotificationServers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Push Notification Servers", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Push Notification Servers"),
      complete: () => null,
    });
  }

public showAddPushNotificationServerModal(): void {
    const modalRef = this.modalService.open(PushNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as PushNotificationServerCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Push Notification Server';
    componentInstance.onCloseCallback = (): void => {
      this.getPushNotificationServers(this.searchModel);
    }
  }

public showViewPushNotificationServerModal(item: PushNotificationServerModel): void {
    const modalRef = this.modalService.open(PushNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as PushNotificationServerCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Push Notification Server';
    componentInstance.pushNotificationServerModel = JSON.parse(JSON.stringify(item)) as PushNotificationServerModel ;
  }

public showUpdatePushNotificationServerModal(item: PushNotificationServerModel): void {
    const modalRef = this.modalService.open(PushNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as PushNotificationServerCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Push Notification Server';
    componentInstance.pushNotificationServerModel = JSON.parse(JSON.stringify(item)) as PushNotificationServerModel;
    componentInstance.onCloseCallback = (): void => {
      this.getPushNotificationServers(this.searchModel);
    }
  }

public deletePushNotificationServer(item: PushNotificationServerModel): void {
    this.dialogService.confirmWithTitle(`Delete Push Notification Server`, `Do you want to delete the Push Notification Server - ${item.pushNotificationServerName}?`).then(res => {
      if (!res) return;
      this.httpService.markPushNotificationServerAsDeleted(item.pushNotificationServerId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Push Notification Server");
        this.dialogService.success("Successfully deleted the Push Notification Server");
        this.getPushNotificationServers(this.searchModel);
        return;
      });
    });
  }

public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.searchModel.companyId = this.selectedCompany ? this.selectedCompany.companyId : null;

    this.getPushNotificationServers(this.searchModel);
  }

public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.companyId = null;
    this.getPushNotificationServers(this.searchModel);
  }

public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getPushNotificationServers(this.searchModel);
  }

public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getPushNotificationServers(this.searchModel);
  }
}
