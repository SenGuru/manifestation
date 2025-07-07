import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { Component, Input, OnInit } from '@angular/core';
import { JobModel, JobSearchCriteria, JobTemplateModel, JobTemplateSearchCriteria, JobWithDiffModel } from 'src/common/models/app-model';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { HttpService } from 'src/common/services/app-http-client';
import { JobCrudModalComponent } from '../../../modals/job-crud-modal/job-crud-modal.component';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { RunJobModalComponent } from 'src/app/shared/modals/run-job-modal/run-job-modal.component';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-job-list-screen',
    templateUrl: './job-list-screen.component.html',
    standalone: false
})
export class JobListScreenComponent extends BaseListComponent<JobModel, JobSearchCriteria> implements OnInit {

  @Input() public jobAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: JobSearchCriteria = new JobSearchCriteria();

  public predefinedJobTypes: Status[] = [];
  public selectedPredefinedJobTypes: Status[] = [];
  public jobTemplateTypeahead$ = new Subject<string>();
  public jobTemplateTypeaheadLoading = false;
  public jobTemplates: JobTemplateModel[] = [];
  public selectedJobTemplate: JobTemplateModel | undefined;
  public predefinedJobStatusTypes: Status[] = [];
  public selectedPredefinedJobStatusTypes: Status[] = [];
  public initialUrl: string | undefined;
  public override columns = [
    {
      text: 'Job Type',
      align: 'left',
      sortable: true,
      value: 'predefinedJobTypeName',
      width: '115px',
    },
    {
      text: 'Job Template Name',
      align: 'left',
      sortable: true,
      value: 'jobTemplateName',
      width: '115px',
    },
    {
      text: 'Job Or Report Type',
      align: 'left',
      sortable: true,
      value: 'predefinedJobOrReportTypeName',
      width: '115px',
    },
    {
      text: 'Completed Date',
      align: 'left',
      sortable: true,
      value: 'completedDate',
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
      text: 'Job Status',
      align: 'left',
      sortable: true,
      value: 'predefinedJobStatusTypeName',
      width: '115px',
    },
    {
      text: 'Job Name',
      align: 'left',
      sortable: true,
      value: 'jobName',
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
    private router: Router,
    private crudService: CrudService,
    private titleService: TitleService,
  ) {
    super();
    this.titleService.pageTitle = "Jobs";
  }

  public ngOnInit(): void {
    this.initialUrl = this.sessionService.getInitialUrl();
    this.getJobs(this.searchModel);
    this.predefinedJobTypes = this.selectService.GetPredefinedJobTypes();
    this.getJobTemplates();
    this.predefinedJobStatusTypes = this.selectService.GetPredefinedJobStatusTypes();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.jobTemplateTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.jobTemplateTypeaheadLoading = true;
      const jobTemplateSearchCriteria = new JobTemplateSearchCriteria();
      jobTemplateSearchCriteria.keyword = searchValue;
      return this.httpService.getJobTemplates(jobTemplateSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.jobTemplateTypeaheadLoading = false;
        if (data.model) this.jobTemplates = data.model;
      },
      error: (err: unknown) => {
        this.jobTemplateTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

  public getJobTemplates(): void {
    const searchModel = new JobTemplateSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getJobTemplates(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.jobTemplates = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Job Template');
      },
      complete: () => null,
    });
  }

  public getJobs(searchModel: JobSearchCriteria): void {
    this.httpService.getJobs(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Jobs", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Jobs"),
      complete: () => null,
    });
  }

  public showScheduledJobScreen(): void {
    this.router.navigateByUrl(`${this.initialUrl}/jobs/scheduled-jobs`);
  }

  public showRunJobModal(): void {
    const modalRef = this.modalService.open(RunJobModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as RunJobModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Run';
    componentInstance.title = 'Run Job';
    componentInstance.onCloseCallback = (): void => {
      this.getJobs(this.searchModel);
    }
  }

  public showViewJobModal(item: JobModel): void {
    const modalRef = this.modalService.open(JobCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as JobCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Job';
    componentInstance.jobModel = JSON.parse(JSON.stringify(item)) as JobWithDiffModel;
  }

  public showUpdateJobModal(item: JobModel): void {
    const modalRef = this.modalService.open(JobCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as JobCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Job';
    componentInstance.jobModel = JSON.parse(JSON.stringify(item)) as JobWithDiffModel;
    componentInstance.onCloseCallback = (): void => {
      this.getJobs(this.searchModel);
    }
  }

  public deleteJob(item: JobModel): void {
    this.dialogService.confirmWithTitle(`Delete Job`, `Do you want to delete the job - ${item.jobName}?`).then(res => {
      if (!res) return;
      this.httpService.markJobAsDeleted(item.jobId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Job");
        this.getJobs(this.searchModel);
        this.dialogService.success("Successfully deleted the Job");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    const predefinedJobTypesArray = this.selectedPredefinedJobTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedJobTypeIds = predefinedJobTypesArray;
    this.searchModel.jobTemplateId = this.selectedJobTemplate ? this.selectedJobTemplate.jobTemplateId : null;
    const predefinedJobStatusTypesArray = this.selectedPredefinedJobStatusTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedJobStatusTypeIds = predefinedJobStatusTypesArray;

    this.getJobs(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    
    this.searchModel.predefinedJobTypeIds = null;
    this.searchModel.jobTemplateId = null;
    this.searchModel.predefinedJobOrReportTypeIds = null;
    this.searchModel.predefinedJobStatusTypeIds = null;
    this.searchModel.submittedByUserId = null;
    this.searchModel.executionProcessId = null;
    this.searchModel.executionThreadId = null;
    this.getJobs(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getJobs(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const jobSearchCriteria = new JobSearchCriteria();
    jobSearchCriteria.page = e.page;
    jobSearchCriteria.pageSize = e.pageSize;
    this.getJobs(jobSearchCriteria);
  }

}
