import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { ScheduledJobModel, ScheduledJobNotificationEmailModel, ScheduledJobNotificationEmailSearchCriteria, ScheduledJobSearchCriteria } from 'src/common/models/app-model';
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
import { ScheduledJobNotificationEmailCrudModalComponent } from '../../../modals/scheduled-job-notification-email-crud-modal/scheduled-job-notification-email-crud-modal.component';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-scheduled-job-notification-email-list-screen',
    templateUrl: './scheduled-job-notification-email-list-screen.component.html',
    standalone: false
})
export class ScheduledJobNotificationEmailListScreenComponent extends BaseListComponent<ScheduledJobNotificationEmailModel, ScheduledJobNotificationEmailSearchCriteria> implements OnInit {

  @Input() public scheduledJobNotificationEmailAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: ScheduledJobNotificationEmailSearchCriteria = new ScheduledJobNotificationEmailSearchCriteria();

 
  public scheduledJobTypeahead$ = new Subject<string>();
  public scheduledJobTypeaheadLoading = false;
  public scheduledJobs: ScheduledJobModel[] = [];
  public selectedScheduledJob: ScheduledJobModel | undefined;

  public override columns = [
    {
      text: 'Scheduled Job Name',
      align: 'left',
      sortable: true,
      value: 'scheduledJobName',
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
      text: 'Send Status',
      align: 'left',
      sortable: true,
      value: 'sendStatus',
      width: '115px',
    },
    {
      text: 'Message',
      align: 'left',
      sortable: true,
      value: 'message',
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
    this.getScheduledJobNotificationEmails(this.searchModel);
   
    this.getScheduledJobs();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.scheduledJobTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.scheduledJobTypeaheadLoading = true;
      const scheduledJobSearchCriteria = new ScheduledJobSearchCriteria();
      scheduledJobSearchCriteria.keyword = searchValue;
      return this.httpService.getScheduledJobs(scheduledJobSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.scheduledJobTypeaheadLoading = false;
        if (data.model) this.scheduledJobs = data.model;
      },
      error: (err: unknown) => {
        this.scheduledJobTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

  

  public getScheduledJobs(): void {
    const searchModel = new ScheduledJobSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getScheduledJobs(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.scheduledJobs = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Scheduled Job');
      },
      complete: () => null,
    });
  }

  public getScheduledJobNotificationEmails(searchModel: ScheduledJobNotificationEmailSearchCriteria): void {
    this.httpService.getScheduledJobNotificationEmails(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Scheduled Job Notification Emails", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Scheduled Job Notification Emails"),
      complete: () => null,
    });
  }

  public showAddScheduledJobNotificationEmailModal(): void {
    const modalRef = this.modalService.open(ScheduledJobNotificationEmailCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as ScheduledJobNotificationEmailCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Scheduled Job Notification Email';
    componentInstance.onCloseCallback = (): void => {
      this.getScheduledJobNotificationEmails(this.searchModel);
    }
  }

  public showViewScheduledJobNotificationEmailModal(item: ScheduledJobNotificationEmailModel): void {
    const modalRef = this.modalService.open(ScheduledJobNotificationEmailCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as ScheduledJobNotificationEmailCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Scheduled Job Notification Email';
    componentInstance.scheduledJobNotificationEmailModel = JSON.parse(JSON.stringify(item)) as ScheduledJobNotificationEmailModel;
  }

  public showUpdateScheduledJobNotificationEmailModal(item: ScheduledJobNotificationEmailModel): void {
    const modalRef = this.modalService.open(ScheduledJobNotificationEmailCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as ScheduledJobNotificationEmailCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Scheduled Job Notification Email';
    componentInstance.scheduledJobNotificationEmailModel = JSON.parse(JSON.stringify(item)) as ScheduledJobNotificationEmailModel;
    componentInstance.onCloseCallback = (): void => {
      this.getScheduledJobNotificationEmails(this.searchModel);
    }
  }

  public deleteScheduledJobNotificationEmail(item: ScheduledJobNotificationEmailModel): void {
    this.dialogService.confirmWithTitle(`Delete Scheduled Job Notification Email`, `Do you want to delete the Scheduled Job Notification Email?`).then(res => {
      if (!res) return;
      this.httpService.markScheduledJobNotificationEmailAsDeleted(item.scheduledJobNotificationEmailId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Scheduled Job Notification Email");
        this.getScheduledJobNotificationEmails(this.searchModel);
        this.dialogService.success("Successfully deleted the Scheduled Job Notification Email");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    
    this.searchModel.scheduledJobId = this.selectedScheduledJob ? this.selectedScheduledJob.scheduledJobId : null;

    this.getScheduledJobNotificationEmails(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    
    this.searchModel.scheduledJobId = null;
    this.getScheduledJobNotificationEmails(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getScheduledJobNotificationEmails(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const scheduledJobNotificationEmailSearchCriteria = new ScheduledJobNotificationEmailSearchCriteria();
    scheduledJobNotificationEmailSearchCriteria.page = e.page;
    scheduledJobNotificationEmailSearchCriteria.pageSize = e.pageSize;
    this.getScheduledJobNotificationEmails(scheduledJobNotificationEmailSearchCriteria);
  }

}
