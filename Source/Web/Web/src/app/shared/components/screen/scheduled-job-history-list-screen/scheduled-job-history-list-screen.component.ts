import { ScheduledJobHistoryModel, ScheduledJobHistorySearchCriteria, ScheduledJobModel, ScheduledJobSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { ManifestColumnModel } from 'src/common/models/app-custom-model';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ScheduledJobHistoryCrudModalComponent } from '../../../modals/scheduled-job-history-crud-modal/scheduled-job-history-crud-modal.component';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-scheduled-job-history-list-screen',
    templateUrl: './scheduled-job-history-list-screen.component.html',
    standalone: false
})
export class ScheduledJobHistoryListScreenComponent extends BaseListComponent<ScheduledJobHistoryModel, ScheduledJobHistorySearchCriteria> implements OnInit {

  @Input() public scheduledJobHistoryAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: ScheduledJobHistorySearchCriteria = new ScheduledJobHistorySearchCriteria();

 
  public scheduledJobTypeahead$ = new Subject<string>();
  public scheduledJobTypeaheadLoading = false;
  public scheduledJobs: ScheduledJobModel[] = [];
  public selectedScheduledJob: ScheduledJobModel | undefined;
  public pickupProcessTypeahead$ = new Subject<string>();
  public pickupProcessTypeaheadLoading = false;

  public override columns = [
    {
      text: 'Scheduled Job Name',
      align: 'left',
      sortable: true,
      value: 'scheduledJobName',
      width: '115px',
    },
    {
      text: 'Pickup Date',
      align: 'left',
      sortable: true,
      value: 'pickupDate',
      width: '115px',
    },
    {
      text: 'Pickup Machine Name',
      align: 'left',
      sortable: true,
      value: 'pickupMachineName',
      width: '115px',
    },
    {
      text: 'Pickup Process Name',
      align: 'left',
      sortable: true,
      value: 'pickupProcessName',
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
    this.getScheduledJobHistorys(this.searchModel);
   
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

  public getScheduledJobHistorys(searchModel: ScheduledJobHistorySearchCriteria): void {
    this.httpService.getScheduledJobHistorys(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Scheduled Job Historys", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Scheduled Job Historys"),
      complete: () => null,
    });
  }

  public showAddScheduledJobHistoryModal(): void {
    const modalRef = this.modalService.open(ScheduledJobHistoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as ScheduledJobHistoryCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Scheduled Job History';
    componentInstance.onCloseCallback = (): void => {
      this.getScheduledJobHistorys(this.searchModel);
    }
  }

  public showViewScheduledJobHistoryModal(item: ScheduledJobHistoryModel): void {
    const modalRef = this.modalService.open(ScheduledJobHistoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as ScheduledJobHistoryCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Scheduled Job History';
    componentInstance.scheduledJobHistoryModel = JSON.parse(JSON.stringify(item)) as ScheduledJobHistoryModel;
  }

  public showUpdateScheduledJobHistoryModal(item: ScheduledJobHistoryModel): void {
    const modalRef = this.modalService.open(ScheduledJobHistoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as ScheduledJobHistoryCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Scheduled Job History';
    componentInstance.scheduledJobHistoryModel = JSON.parse(JSON.stringify(item)) as ScheduledJobHistoryModel;
    componentInstance.onCloseCallback = (): void => {
      this.getScheduledJobHistorys(this.searchModel);
    }
  }

  public deleteScheduledJobHistory(item: ScheduledJobHistoryModel): void {
    this.dialogService.confirmWithTitle(`Delete Scheduled Job History`, `Do you want to delete the scheduledJobHistory ?`).then(res => {
      if (!res) return;
      this.httpService.markScheduledJobHistoryAsDeleted(item.scheduledJobHistoryId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Scheduled Job History");
        this.getScheduledJobHistorys(this.searchModel);
        this.dialogService.success("Successfully deleted the Scheduled Job History");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    
    this.searchModel.scheduledJobId = this.selectedScheduledJob ? this.selectedScheduledJob.scheduledJobId : null;

    this.getScheduledJobHistorys(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    
    this.searchModel.scheduledJobId = null;
    this.searchModel.pickupProcessId = null;
    this.getScheduledJobHistorys(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getScheduledJobHistorys(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const scheduledJobHistorySearchCriteria = new ScheduledJobHistorySearchCriteria();
    scheduledJobHistorySearchCriteria.page = e.page;
    scheduledJobHistorySearchCriteria.pageSize = e.pageSize;
    this.getScheduledJobHistorys(scheduledJobHistorySearchCriteria);
  }

}
