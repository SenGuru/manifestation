import { SavedJobModel, SavedJobNotificationEmailModel, SavedJobNotificationEmailSearchCriteria, SavedJobSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { AuditDuoColumnModel } from 'src/common/models/app-custom-model';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SavedJobNotificationEmailCrudModalComponent } from '../../../modals/saved-job-notification-email-crud-modal/saved-job-notification-email-crud-modal.component';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-saved-job-notification-email-list-screen',
    templateUrl: './saved-job-notification-email-list-screen.component.html',
    standalone: false
})
export class SavedJobNotificationEmailListScreenComponent extends BaseListComponent<SavedJobNotificationEmailModel, SavedJobNotificationEmailSearchCriteria> implements OnInit {

  @Input() public savedJobNotificationEmailAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: SavedJobNotificationEmailSearchCriteria = new SavedJobNotificationEmailSearchCriteria();

 
  public savedJobTypeahead$ = new Subject<string>();
  public savedJobTypeaheadLoading = false;
  public savedJobs: SavedJobModel[] = [];
  public selectedSavedJob: SavedJobModel | undefined;

  public override columns = [
    {
      text: 'Saved Job Name',
      align: 'left',
      sortable: true,
      value: 'savedJobName',
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
    this.getSavedJobNotificationEmails(this.searchModel);
   
    this.getSavedJobs();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    
    this.savedJobTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.savedJobTypeaheadLoading = true;
      const savedJobSearchCriteria = new SavedJobSearchCriteria();
      savedJobSearchCriteria.keyword = searchValue;
      return this.httpService.getSavedJobs(savedJobSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.savedJobTypeaheadLoading = false;
        if (data.model) this.savedJobs = data.model;
      },
      error: (err: unknown) => {
        this.savedJobTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

  
  public getSavedJobs(): void {
    const searchModel = new SavedJobSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getSavedJobs(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.savedJobs = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Saved Job');
      },
      complete: () => null,
    });
  }

  public getSavedJobNotificationEmails(searchModel: SavedJobNotificationEmailSearchCriteria): void {
    this.httpService.getSavedJobNotificationEmails(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Saved Job Notification Emails", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Saved Job Notification Emails"),
      complete: () => null,
    });
  }

  public showAddSavedJobNotificationEmailModal(): void {
    const modalRef = this.modalService.open(SavedJobNotificationEmailCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as SavedJobNotificationEmailCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Saved Job Notification Email';
    componentInstance.onCloseCallback = (): void => {
      this.getSavedJobNotificationEmails(this.searchModel);
    }
  }

  public showViewSavedJobNotificationEmailModal(item: SavedJobNotificationEmailModel): void {
    const modalRef = this.modalService.open(SavedJobNotificationEmailCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as SavedJobNotificationEmailCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Saved Job Notification Email';
    componentInstance.savedJobNotificationEmailModel = JSON.parse(JSON.stringify(item)) as SavedJobNotificationEmailModel;
  }

  public showUpdateSavedJobNotificationEmailModal(item: SavedJobNotificationEmailModel): void {
    const modalRef = this.modalService.open(SavedJobNotificationEmailCrudModalComponent, { windowClass: 'my-class', size: 'xl' });
    const componentInstance = modalRef.componentInstance as SavedJobNotificationEmailCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Saved Job Notification Email';
    componentInstance.savedJobNotificationEmailModel = JSON.parse(JSON.stringify(item)) as SavedJobNotificationEmailModel;
    componentInstance.onCloseCallback = (): void => {
      this.getSavedJobNotificationEmails(this.searchModel);
    }
  }

  public deleteSavedJobNotificationEmail(item: SavedJobNotificationEmailModel): void {
    this.dialogService.confirmWithTitle(`Delete Saved Job Notification Email`, `Do you want to delete the Saved Job Notification Email ?`).then(res => {
      if (!res) return;
      this.httpService.markSavedJobNotificationEmailAsDeleted(item.savedJobNotificationEmailId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Saved Job Notification Email");
        this.getSavedJobNotificationEmails(this.searchModel);
        this.dialogService.success("Successfully deleted the Save Job Notification Email");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    
    this.searchModel.savedJobId = this.selectedSavedJob ? this.selectedSavedJob.savedJobId : null;

    this.getSavedJobNotificationEmails(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    
    this.searchModel.savedJobId = null;
    this.getSavedJobNotificationEmails(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getSavedJobNotificationEmails(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const savedJobNotificationEmailSearchCriteria = new SavedJobNotificationEmailSearchCriteria();
    savedJobNotificationEmailSearchCriteria.page = e.page;
    savedJobNotificationEmailSearchCriteria.pageSize = e.pageSize;
    this.getSavedJobNotificationEmails(savedJobNotificationEmailSearchCriteria);
  }

}
