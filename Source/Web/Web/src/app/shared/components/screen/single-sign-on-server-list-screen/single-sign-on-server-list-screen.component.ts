import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { Component, OnInit } from '@angular/core';
import { SingleSignOnServerModel, SingleSignOnServerSearchCriteria } from 'src/common/models/app-model';

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
import { SingleSignOnServerCrudModalComponent } from '../../../modals/single-sign-on-server-crud-modal/single-sign-on-server-crud-modal.component';
import { TitleService } from 'src/app/shared/services/title.service';

@Component({
    selector: 'app-single-sign-on-server-list-screen',
    templateUrl: './single-sign-on-server-list-screen.component.html',
    standalone: false
})
export class SingleSignOnServerListScreenComponent extends BaseListComponent<SingleSignOnServerModel, SingleSignOnServerSearchCriteria> implements OnInit {

  public override searchModel: SingleSignOnServerSearchCriteria = new SingleSignOnServerSearchCriteria();

  public predefinedSingleSignOnServerTypes: Status[] = [];
  public selectedPredefinedSingleSignOnServerTypes: Status[] = [];

  public override columns = [
    {
      text: 'Name',
      align: 'left',
      sortable: true,
      value: 'singleSignOnServerName',
      width: '115px',
    },
    {
      text: 'Server Type',
      align: 'left',
      sortable: true,
      value: 'predefinedSingleSignOnServerTypeName',
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
      text: 'For All Company',
      align: 'left',
      sortable: true,
      value: 'forAllCompany',
      width: '115px',
    },
    {
      text: 'Is Default',
      align: 'left',
      sortable: true,
      value: 'isDefault',
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
    this.titleService.pageTitle = "Single Sign-On Server";
  }

public ngOnInit(): void {
    this.getSingleSignOnServers(this.searchModel);
    this.predefinedSingleSignOnServerTypes = this.selectService.GetPredefinedSingleSignOnServerTypes();
  }

public getSingleSignOnServers(searchModel: SingleSignOnServerSearchCriteria): void {
    this.httpService.getSingleSignOnServers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Single Sign-On Servers", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Single Sign-On Servers"),
      complete: () => null,
    });
  }

public showAddSingleSignOnServerModal(): void {
    const modalRef = this.modalService.open(SingleSignOnServerCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as SingleSignOnServerCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Single Sign-On Server';
    componentInstance.onCloseCallback = (): void => {
      this.getSingleSignOnServers(this.searchModel);
    }
  }

public showViewSingleSignOnServerModal(item: SingleSignOnServerModel): void {
    const modalRef = this.modalService.open(SingleSignOnServerCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as SingleSignOnServerCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Single Sign-On Server';
    componentInstance.singleSignOnServerModel = JSON.parse(JSON.stringify(item)) as SingleSignOnServerModel;
  }

public showUpdateSingleSignOnServerModal(item: SingleSignOnServerModel): void {
    const modalRef = this.modalService.open(SingleSignOnServerCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as SingleSignOnServerCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Single Sign-On Server';
    componentInstance.singleSignOnServerModel = JSON.parse(JSON.stringify(item)) as SingleSignOnServerModel;
    componentInstance.onCloseCallback = (): void => {
      this.getSingleSignOnServers(this.searchModel);
    }
  }

public deleteSingleSignOnServer(item: SingleSignOnServerModel): void {
    this.dialogService.confirmWithTitle(`Delete Single Sign-On Server`, `Do you want to delete the Single Sign-On Server - ${item.singleSignOnServerName}?`).then(res => {
      if (!res) return;
      this.httpService.markSingleSignOnServerAsDeleted(item.singleSignOnServerId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Single Sign-On Server");
        this.getSingleSignOnServers(this.searchModel);
        this.dialogService.success("Successfully deleted the Single Sign-On Server");
        return;
      });
    });
  }

public onSearch(f: NgForm): void {
    if (f.invalid) return;
    const predefinedSingleSignOnServerTypesArray = this.selectedPredefinedSingleSignOnServerTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedSingleSignOnServerTypeIds = predefinedSingleSignOnServerTypesArray;

    this.getSingleSignOnServers(this.searchModel);
  }

public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.predefinedSingleSignOnServerTypeIds = null;
    this.getSingleSignOnServers(this.searchModel);
  }

public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getSingleSignOnServers(this.searchModel);
  }

public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getSingleSignOnServers(this.searchModel);
  }

public completeActionSuccess(message: string): void {
    this.dialogService.success(message);
  }

public completeActionFailure(message: string): void {
    this.dialogService.error(message);
  }
}
