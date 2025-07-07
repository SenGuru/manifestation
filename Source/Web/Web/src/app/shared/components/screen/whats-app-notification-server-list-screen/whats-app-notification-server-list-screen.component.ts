import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { WhatsAppNotificationServerModel, WhatsAppNotificationServerSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';
import { WhatsAppNotificationServerCrudModalComponent } from '../../../modals/whats-app-notification-server-crud-modal/whats-app-notification-server-crud-modal.component';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-whats-app-notification-server-list-screen',
    templateUrl: './whats-app-notification-server-list-screen.component.html',
    standalone: false
})
export class WhatsAppNotificationServerListScreenComponent extends BaseListComponent<WhatsAppNotificationServerModel, WhatsAppNotificationServerSearchCriteria> implements OnInit {

  public override searchModel: WhatsAppNotificationServerSearchCriteria = new WhatsAppNotificationServerSearchCriteria();

 
  public predefinedSmsAndVoiceServerTypes: Status[] = [];
  public selectedPredefinedSmsAndVoiceServerTypes: Status[] = [];

  public override columns = [
    {
      text: 'Server Name',
      align: 'left',
      sortable: true,
      value: 'whatsAppNotificationServerName',
      width: '115px',
    },
    {
      text: 'Server Type',
      align: 'left',
      sortable: true,
      value: 'predefinedSmsAndVoiceServerTypeName',
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
    this.titleService.pageTitle = "Whats App Notification Server";
  }

public ngOnInit(): void {
    this.getWhatsAppNotificationServers(this.searchModel);
    this.predefinedSmsAndVoiceServerTypes = this.selectService.GetPredefinedSmsAndVoiceServerTypes();
  }
 


public getWhatsAppNotificationServers(searchModel: WhatsAppNotificationServerSearchCriteria): void {
    this.httpService.getWhatsAppNotificationServers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Whats App Notification Servers", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Whats App Notification Servers"),
      complete: () => null,
    });
  }

public showAddWhatsAppNotificationServerModal(): void {
    const modalRef = this.modalService.open(WhatsAppNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as WhatsAppNotificationServerCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Whats App Notification Server';
    componentInstance.onCloseCallback = (): void => {
      this.getWhatsAppNotificationServers(this.searchModel);
    }
  }

public showViewWhatsAppNotificationServerModal(item: WhatsAppNotificationServerModel): void {
    const modalRef = this.modalService.open(WhatsAppNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as WhatsAppNotificationServerCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Whats App Notification Server';
    componentInstance.whatsAppNotificationServerModel = JSON.parse(JSON.stringify(item)) as WhatsAppNotificationServerModel;
  }

public showUpdateWhatsAppNotificationServerModal(item: WhatsAppNotificationServerModel): void {
    const modalRef = this.modalService.open(WhatsAppNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as WhatsAppNotificationServerCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Whats App Notification Server';
    componentInstance.whatsAppNotificationServerModel = JSON.parse(JSON.stringify(item)) as WhatsAppNotificationServerModel;
    componentInstance.onCloseCallback = (): void => {
      this.getWhatsAppNotificationServers(this.searchModel);
    }
  }

public deleteWhatsAppNotificationServer(item: WhatsAppNotificationServerModel): void {
    this.dialogService.confirmWithTitle(`Delete Whats App Notification Server`, `Do you want to delete the Whats App Notification Server - ${item.whatsAppNotificationServerName}?`).then(res => {
      if (!res) return;
      this.httpService.markWhatsAppNotificationServerAsDeleted(item.whatsAppNotificationServerId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Whats App Notification Server");
        this.dialogService.success("Successfully deleted the Whats App Notification Server");
        this.getWhatsAppNotificationServers(this.searchModel);
        return;
      });
    });
  }

public onSearch(f: NgForm): void {
    if (f.invalid) return;
    
    const predefinedSmsAndVoiceServerTypesArray = this.selectedPredefinedSmsAndVoiceServerTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedSmsAndVoiceServerTypeIds = predefinedSmsAndVoiceServerTypesArray;

    this.getWhatsAppNotificationServers(this.searchModel);
  }

public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    
    this.searchModel.predefinedSmsAndVoiceServerTypeIds = null;
    this.getWhatsAppNotificationServers(this.searchModel);
  }

public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getWhatsAppNotificationServers(this.searchModel);
  }

public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getWhatsAppNotificationServers(this.searchModel);
  }
}
