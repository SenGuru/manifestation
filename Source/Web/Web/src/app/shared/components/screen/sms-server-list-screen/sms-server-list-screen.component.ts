import { Component, Input, OnInit } from '@angular/core';
import { LoggedInUserModel, SmsServerModel, SmsServerSearchCriteria } from 'src/common/models/app-model';
import { ManifestColumnModel } from 'src/common/models/app-custom-model';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { SmsServerCrudModalComponent } from 'src/app/shared/modals/sms-server-crud-modal/sms-server-crud-modal.component';
import { TitleService } from 'src/app/shared/services/title.service';

@Component({
    selector: 'app-sms-server-list-screen',
    templateUrl: './sms-server-list-screen.component.html',
    standalone: false
})
export class SmsServerListScreenComponent extends BaseListComponent<SmsServerModel, SmsServerSearchCriteria> implements OnInit {

  @Input() public isSuperAdmin = false;
  @Input() public isConfigurator = false;
  @Input() public companyId: number = 0;
  public currentUser: LoggedInUserModel | null = null;
  public initialUrl: string = '';
  public override searchModel: SmsServerSearchCriteria = new SmsServerSearchCriteria();

  public override columns = [
    {
      text: 'SMS Server Name',
      align: 'left',
      sortable: true,
      value: 'smsServerName',
      width: '115px',
    },
    {
      text: 'Host',
      align: 'left',
      sortable: true,
      value: 'host',
      width: '115px',
    },
    {
      text: 'Port',
      align: 'left',
      sortable: true,
      value: 'port',
      width: '115px',
    },
    {
      text: 'Sender Name',
      align: 'left',
      sortable: true,
      value: 'senderName',
      width: '115px',
    },
    {
      text: 'Sender Esms',
      align: 'left',
      sortable: true,
      value: 'senderEmail',
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
    private crudService: CrudService,
    private titleService: TitleService,
    private enumViewService: EnumViewService,
  ) {
    super();
    this.titleService.pageTitle = "SMS Server";
  }

public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initialUrl = this.enumViewService.getInititalUrl(this.currentUser?.userTypeId ?? 0);
    if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.isSuperAdmin = true;
    } 
    this.getSmsServers(this.searchModel);
  }

public getSmsServers(searchModel: SmsServerSearchCriteria): void {
    this.httpService.getSmsServers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get SMS Servers", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get SMS Servers"),
      complete: () => null,
    });
  }

public showAddSmsServerModal(): void {
    const modalRef = this.modalService.open(SmsServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as SmsServerCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create SMS Server';
    componentInstance.companyId = this.companyId;
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.onCloseCallback = (): void => {
      this.getSmsServers(this.searchModel);
    }
  }

public showViewSmsServerModal(item: SmsServerModel): void {
    const modalRef = this.modalService.open(SmsServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as SmsServerCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View SMS Server';
    componentInstance.companyId = this.companyId;
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.smsServerModel = JSON.parse(JSON.stringify(item)) as SmsServerModel;
  }

public showUpdateSmsServerModal(item: SmsServerModel): void {
    const modalRef = this.modalService.open(SmsServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as SmsServerCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit SMS Server';
    componentInstance.companyId = this.companyId;
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.smsServerModel = JSON.parse(JSON.stringify(item)) as SmsServerModel;
    componentInstance.onCloseCallback = (): void => {
      this.getSmsServers(this.searchModel);
    }
  }

public deleteSmsServer(item: SmsServerModel): void {
    this.dialogService.confirmWithTitle(`Delete SMS Server`, `Do you want to delete the SMS Server - ${item.userName}?`).then(res => {
      if (!res) return;
      this.httpService.markSmsServerAsDeleted(item.smsServerId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the SMS Server");
        this.getSmsServers(this.searchModel);
        this.dialogService.success("Successfully deleted the SMS Server");
        return;
      });
    });
  }

public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.getSmsServers(this.searchModel);
  }

public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    
    this.searchModel.selectedId = null;
    this.getSmsServers(this.searchModel);
  }

public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getSmsServers(this.searchModel);
  }

public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getSmsServers(this.searchModel);
  }

}
