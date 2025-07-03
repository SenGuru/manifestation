import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { CompanyModel, CompanySearchCriteria, VoiceNotificationServerModel, VoiceNotificationServerSearchCriteria } from 'src/common/models/app-model';
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
import { VoiceNotificationServerCrudModalComponent } from '../../../modals/voice-notification-server-crud-modal/voice-notification-server-crud-modal.component';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-voice-notification-server-list-screen',
    templateUrl: './voice-notification-server-list-screen.component.html',
    standalone: false
})
export class VoiceNotificationServerListScreenComponent extends BaseListComponent<VoiceNotificationServerModel, VoiceNotificationServerSearchCriteria> implements OnInit {

  public override searchModel: VoiceNotificationServerSearchCriteria = new VoiceNotificationServerSearchCriteria();

  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;
  public predefinedSmsAndVoiceServerTypes: Status[] = [];
  public selectedPredefinedSmsAndVoiceServerTypes: Status[] = [];

  public override columns = [
    {
      text: 'Server Name',
      align: 'left',
      sortable: true,
      value: 'voiceNotificationServerName',
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
    this.titleService.pageTitle = "Voice Notification Server";
  }

public ngOnInit(): void {
    this.getVoiceNotificationServers(this.searchModel);
    this.getCompanys();
    this.predefinedSmsAndVoiceServerTypes = this.selectService.GetPredefinedSmsAndVoiceServerTypes();
    this.initTypeAheads();
  }

public initTypeAheads(): void {
    this.companyTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.companyTypeaheadLoading = true;
      const companySearchCriteria = new CompanySearchCriteria();
      companySearchCriteria.keyword = searchValue;
      companySearchCriteria.isActive = true;
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

public getCompanys(): void {
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

public getVoiceNotificationServers(searchModel: VoiceNotificationServerSearchCriteria): void {
    // this.httpService.getVoiceNotificationServers(searchModel).subscribe({
    //   next: result => {
    //     if (!result.isSuccess) {
    //       this.dialogService.errorWithReturnModel("Unable to get Voice Notification Servers", result);
    //       return;
    //     }
    //     if (result.model) {
    //       this.listData = result.model;
    //       this.totalItems = result.totalRecords ?? 0;
    //     }
    //   },
    //   error: (err: unknown) => this.dialogService.error("Unable to get Voice Notification Servers"),
    //   complete: () => null,
    // });
  }

public showAddVoiceNotificationServerModal(): void {
    const modalRef = this.modalService.open(VoiceNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as VoiceNotificationServerCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Voice Notification Server';
    componentInstance.onCloseCallback = (): void => {
      this.getVoiceNotificationServers(this.searchModel);
    }
  }

public showViewVoiceNotificationServerModal(item: VoiceNotificationServerModel): void {
    const modalRef = this.modalService.open(VoiceNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as VoiceNotificationServerCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Voice Notification Server';
    componentInstance.voiceNotificationServerModel = JSON.parse(JSON.stringify(item)) as VoiceNotificationServerModel;
  }

public showUpdateVoiceNotificationServerModal(item: VoiceNotificationServerModel): void {
    const modalRef = this.modalService.open(VoiceNotificationServerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as VoiceNotificationServerCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Voice Notification Server';
    componentInstance.voiceNotificationServerModel = JSON.parse(JSON.stringify(item)) as VoiceNotificationServerModel;
    componentInstance.onCloseCallback = (): void => {
      this.getVoiceNotificationServers(this.searchModel);
    }
  }

public deleteVoiceNotificationServer(item: VoiceNotificationServerModel): void {
    this.dialogService.confirmWithTitle(`Delete Voice Notification Server`, `Do you want to delete the Voice Notification Server - ${item.voiceNotificationServerName}?`).then(res => {
      if (!res) return;
      // this.httpService.markVoiceNotificationServerAsDeleted(item.voiceNotificationServerId).subscribe(result => {
      //   if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Voice Notification Server");
      //   this.dialogService.success("Successfully deleted the Voice Notification Server");
      //   this.getVoiceNotificationServers(this.searchModel);
      //   return;
      // });
    });
  }

public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.searchModel.companyId = this.selectedCompany ? this.selectedCompany.companyId : null;
    const predefinedSmsAndVoiceServerTypesArray = this.selectedPredefinedSmsAndVoiceServerTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedSmsAndVoiceServerTypeIds = predefinedSmsAndVoiceServerTypesArray;

    this.getVoiceNotificationServers(this.searchModel);
  }

public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.companyId = null;
    this.searchModel.predefinedSmsAndVoiceServerTypeIds = null;
    this.getVoiceNotificationServers(this.searchModel);
  }

public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getVoiceNotificationServers(this.searchModel);
  }

public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getVoiceNotificationServers(this.searchModel);
  }
}
