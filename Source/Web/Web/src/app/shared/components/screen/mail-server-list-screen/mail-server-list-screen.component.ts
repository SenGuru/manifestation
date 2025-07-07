import { Component, Input, OnInit } from '@angular/core';
import { LoggedInUserModel, MailServerModel, MailServerSearchCriteria } from 'src/common/models/app-model';
import { AuditDuoColumnModel } from 'src/common/models/app-custom-model';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { MailServerCrudModalComponent } from '../../../modals/mail-server-crud-modal/mail-server-crud-modal.component';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';

@Component({
    selector: 'app-mail-server-list-screen',
    templateUrl: './mail-server-list-screen.component.html',
    standalone: false
})
export class MailServerListScreenComponent extends BaseListComponent<MailServerModel, MailServerSearchCriteria> implements OnInit {

  @Input() public isSuperAdmin = false;
  @Input() public isConfigurator = false;
  @Input() public companyId: number = 0;
  public currentUser: LoggedInUserModel | null = null;
  public initialUrl: string = '';
  public override searchModel: MailServerSearchCriteria = new MailServerSearchCriteria();

  public override columns = [
    {
      text: 'Mail Server Name',
      align: 'left',
      sortable: true,
      value: 'mailServerName',
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
      text: 'Sender Email',
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
    this.titleService.pageTitle = "Mail Server";
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initialUrl = this.enumViewService.getInititalUrl(this.currentUser?.userTypeId ?? 0);
    if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.isSuperAdmin = true;
    }  
    this.getMailServers(this.searchModel);
  }

  public getMailServers(searchModel: MailServerSearchCriteria): void {
    this.httpService.getMailServers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Mail Servers", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Mail Servers"),
      complete: () => null,
    });
  }

  public showAddMailServerModal(): void {
    const modalRef = this.modalService.open(MailServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as MailServerCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Mail Server';
    componentInstance.companyId = this.companyId;
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.onCloseCallback = (): void => {
      this.getMailServers(this.searchModel);
    }
  }

  public showViewMailServerModal(item: MailServerModel): void {
    const modalRef = this.modalService.open(MailServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as MailServerCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Mail Server';
    componentInstance.companyId = this.companyId;
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.mailServerModel = JSON.parse(JSON.stringify(item)) as MailServerModel;
  }

  public showUpdateMailServerModal(item: MailServerModel): void {
    const modalRef = this.modalService.open(MailServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as MailServerCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Mail Server';
    componentInstance.companyId = this.companyId;
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.mailServerModel = JSON.parse(JSON.stringify(item)) as MailServerModel;
    componentInstance.onCloseCallback = (): void => {
      this.getMailServers(this.searchModel);
    }
  }

  public deleteMailServer(item: MailServerModel): void {
    this.dialogService.confirmWithTitle(`Delete Mail Server`, `Do you want to delete the Mail Server - ${item.userName}?`).then(res => {
      if (!res) return;
      this.httpService.markMailServerAsDeleted(item.mailServerId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Mail Server");
        this.getMailServers(this.searchModel);
        this.dialogService.success("Successfully deleted the Mail Server");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.getMailServers(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    
    this.searchModel.selectedId = null;
    this.getMailServers(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getMailServers(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getMailServers(this.searchModel);
  }

}
