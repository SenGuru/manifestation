import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { CompanyModel, CompanySearchCriteria, RealtimeMessageModel, RealtimeMessageSearchCriteria, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RealtimeMessageCrudModalComponent } from '../../../modals/realtime-message-crud-modal/realtime-message-crud-modal.component';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-realtime-message-list-screen',
    templateUrl: './realtime-message-list-screen.component.html',
    standalone: false
})
export class RealtimeMessageListScreenComponent extends BaseListComponent<RealtimeMessageModel, RealtimeMessageSearchCriteria> implements OnInit {

  @Input() public realtimeMessageAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: RealtimeMessageSearchCriteria = new RealtimeMessageSearchCriteria();

  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;
  public predefinedMessagePriorityTypes: Status[] = [];
  public selectedPredefinedMessagePriorityTypes: Status[] = [];
  public predefinedRealtimeMessageStatusTypes: Status[] = [];
  public selectedPredefinedRealtimeMessageStatusTypes: Status[] = [];
  public predefinedNotificationMessageTypes: Status[] = [];
  public selectedPredefinedNotificationMessageTypes: Status[] = [];
  public predefinedResourceTypes: Status[] = [];
  public selectedPredefinedResourceTypes: Status[] = [];
  public resourceTypeahead$ = new Subject<string>();
  public resourceTypeaheadLoading = false;
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public override columns = [
    {
      text: 'Priority',
      align: 'left',
      sortable: true,
      value: 'predefinedMessagePriorityTypeName',
      width: '115px',
    },
    {
      text: 'Date Time',
      align: 'left',
      sortable: true,
      value: 'messageDateTime',
      width: '115px',
    },
    {
      text: 'Subject',
      align: 'left',
      sortable: true,
      value: 'messageSubject',
      width: '115px',
    },
    {
      text: 'Message Content',
      align: 'left',
      sortable: true,
      value: 'messageContent',
      width: '115px',
    },
    {
      text: 'Sent DateTime',
      align: 'left',
      sortable: true,
      value: 'messageSentDateTime',
      width: '115px',
    },
    {
      text: 'Viewed DateTime',
      align: 'left',
      sortable: true,
      value: 'messageViewedDateTime',
      width: '115px',
    },
    {
      text: 'Status Type',
      align: 'left',
      sortable: true,
      value: 'predefinedRealtimeMessageStatusTypeName',
      width: '115px',
    },
    {
      text: 'Message Type',
      align: 'left',
      sortable: true,
      value: 'predefinedNotificationMessageTypeName',
      width: '115px',
    },
    {
      text: 'Resource Name',
      align: 'left',
      sortable: true,
      value: 'resourceName',
      width: '115px',
    },
    {
      text: 'Type',
      align: 'left',
      sortable: true,
      value: 'predefinedResourceTypeName',
      width: '115px',
    },
    {
      text: 'Resource Name',
      align: 'left',
      sortable: true,
      value: 'resourceName',
      width: '115px',
    },
    {
      text: 'Error Message',
      align: 'left',
      sortable: true,
      value: 'errorMessage',
      width: '115px',
    },
    {
      text: 'Error Occured On',
      align: 'left',
      sortable: true,
      value: 'errorOccuredOn',
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
      text: 'Is Deleted',
      align: 'left',
      sortable: true,
      value: 'isDeleted',
      width: '115px',
    },
    {
      text: 'Created Date',
      align: 'left',
      sortable: true,
      value: 'createdDate',
      width: '115px',
    },
    {
      text: 'Created By',
      align: 'left',
      sortable: true,
      value: 'createdByUserFirstName',
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
    private crudService: CrudService

  ) {
    super();
  }

  public ngOnInit(): void {
    this.getRealtimeMessages(this.searchModel);
    this.getCompanies();
    this.predefinedMessagePriorityTypes = this.selectService.GetPredefinedMessagePriorityTypes();
    this.predefinedRealtimeMessageStatusTypes = this.selectService.GetPredefinedRealtimeMessageStatusTypes();
    this.predefinedNotificationMessageTypes = this.selectService.GetPredefinedNotificationMessageTypes();
    this.predefinedResourceTypes = this.selectService.GetPredefinedResourceTypes();
    this.getUsers();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.companyTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.companyTypeaheadLoading = true;
      const companySearchCriteria = new CompanySearchCriteria();
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

    this.userTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.userTypeaheadLoading = true;
      const userSearchCriteria = new UserSearchCriteria();
      userSearchCriteria.keyword = searchValue;
      return this.httpService.getUsers(userSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) {
          this.userTypeaheadLoading = false;
          this.users = data.model;
        }
      },
      error: (err: unknown) => {
        this.userTypeaheadLoading = false;
      },
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
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Company');
      },
      complete: () => null,
    });
  }

  public getUsers(): void {
    const searchModel = new UserSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.users = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get User');
      },
      complete: () => null,
    });
  }

  public getRealtimeMessages(searchModel: RealtimeMessageSearchCriteria): void {
    this.httpService.getRealtimeMessages(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Realtime Messages", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Realtime Messages"),
      complete: () => null,
    });
  }

  public showAddRealtimeMessageModal(): void {
    const modalRef = this.modalService.open(RealtimeMessageCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as RealtimeMessageCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Realtime Message';
    componentInstance.onCloseCallback = (): void => {
      this.getRealtimeMessages(this.searchModel);
    }
  }

  public showViewRealtimeMessageModal(item: RealtimeMessageModel): void {
    const modalRef = this.modalService.open(RealtimeMessageCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as RealtimeMessageCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Realtime Message';
    componentInstance.realtimeMessageModel = JSON.parse(JSON.stringify(item)) as RealtimeMessageModel;
  }

  public showUpdateRealtimeMessageModal(item: RealtimeMessageModel): void {
    const modalRef = this.modalService.open(RealtimeMessageCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as RealtimeMessageCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Realtime Message';
    componentInstance.realtimeMessageModel = JSON.parse(JSON.stringify(item)) as RealtimeMessageModel;
    componentInstance.onCloseCallback = (): void => {
      this.getRealtimeMessages(this.searchModel);
    }
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.searchModel.companyId = this.selectedCompany ? this.selectedCompany.companyId : null;
    const predefinedMessagePriorityTypesArray = this.selectedPredefinedMessagePriorityTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedMessagePriorityTypeIds = predefinedMessagePriorityTypesArray;
    const predefinedRealtimeMessageStatusTypesArray = this.selectedPredefinedRealtimeMessageStatusTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedRealtimeMessageStatusTypeIds = predefinedRealtimeMessageStatusTypesArray;
    const predefinedNotificationMessageTypesArray = this.selectedPredefinedNotificationMessageTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedNotificationMessageTypeIds = predefinedNotificationMessageTypesArray;
    const predefinedResourceTypesArray = this.selectedPredefinedResourceTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedResourceTypeIds = predefinedResourceTypesArray;
    this.searchModel.userId = this.selectedUser ? this.selectedUser.userId : null;
    this.getRealtimeMessages(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.companyId = null;
    this.searchModel.predefinedMessagePriorityTypeIds = null;
    this.searchModel.predefinedRealtimeMessageStatusTypeIds = null;
    this.searchModel.predefinedNotificationMessageTypeIds = null;
    this.searchModel.predefinedResourceTypeIds = null;
    this.searchModel.resourceId = null;
    this.searchModel.userId = null;
    this.getRealtimeMessages(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getRealtimeMessages(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const realtimeMessageSearchCriteria = new RealtimeMessageSearchCriteria();
    realtimeMessageSearchCriteria.page = e.page;
    realtimeMessageSearchCriteria.pageSize = e.pageSize;
    this.getRealtimeMessages(realtimeMessageSearchCriteria);
  }

}
