import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { CompanyModel, CompanySearchCriteria, JobTemplateModel, JobTemplateSearchCriteria, ScheduledJobModel, ScheduledJobSearchCriteria, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
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
import { Router } from '@angular/router';
import { ScheduledJobCrudModalComponent } from '../../../modals/scheduled-job-crud-modal/scheduled-job-crud-modal.component';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-scheduled-job-list-screen',
    templateUrl: './scheduled-job-list-screen.component.html',
    standalone: false
})
export class ScheduledJobListScreenComponent extends BaseListComponent<ScheduledJobModel, ScheduledJobSearchCriteria> implements OnInit {

  @Input() public scheduledJobAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: ScheduledJobSearchCriteria = new ScheduledJobSearchCriteria();

  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;
  public jobTemplateTypeahead$ = new Subject<string>();
  public jobTemplateTypeaheadLoading = false;
  public jobTemplates: JobTemplateModel[] = [];
  public selectedJobTemplate: JobTemplateModel | undefined;
  public predefinedJobOrReportTypes: Status[] = [];
  public selectedPredefinedJobOrReportTypes: Status[] = [];
  public submittedByUserTypeahead$ = new Subject<string>();
  public submittedByUserTypeaheadLoading = false;
  public runAsUserTypeahead$ = new Subject<string>();
  public runAsUserTypeaheadLoading = false;
  public runAsUsers: UserModel[] = [];
  public selectedRunAsUser: UserModel | undefined;
  public initialUrl: string | undefined;

  public override columns = [
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
      text: 'Company Name',
      align: 'left',
      sortable: true,
      value: 'companyName',
      width: '115px',
    },
    {
      text: 'Submitted By Name',
      align: 'left',
      sortable: true,
      value: 'submittedByUserName',
      width: '115px',
    },
    {
      text: 'Executable Folder',
      align: 'left',
      sortable: true,
      value: 'executableFolder',
      width: '115px',
    },
    {
      text: 'Executable Name',
      align: 'left',
      sortable: true,
      value: 'executableName',
      width: '115px',
    },
    {
      text: 'Executable Parameter',
      align: 'left',
      sortable: true,
      value: 'executableParameter',
      width: '115px',
    },
    {
      text: 'Serialized Job Parameter',
      align: 'left',
      sortable: true,
      value: 'serializedJobParameter',
      width: '115px',
    },
    {
      text: 'Report Template File',
      align: 'left',
      sortable: true,
      value: 'reportTemplateFile',
      width: '115px',
    },
    {
      text: 'Export Doc',
      align: 'left',
      sortable: true,
      value: 'exportDoc',
      width: '115px',
    },
    {
      text: 'Export Xls',
      align: 'left',
      sortable: true,
      value: 'exportXls',
      width: '115px',
    },
    {
      text: 'Export Ppt',
      align: 'left',
      sortable: true,
      value: 'exportPpt',
      width: '115px',
    },
    {
      text: 'Export Csv',
      align: 'left',
      sortable: true,
      value: 'exportCsv',
      width: '115px',
    },
    {
      text: 'Export Xml',
      align: 'left',
      sortable: true,
      value: 'exportXml',
      width: '115px',
    },
    {
      text: 'Export Pdf',
      align: 'left',
      sortable: true,
      value: 'exportPdf',
      width: '115px',
    },
    {
      text: 'Export Custom',
      align: 'left',
      sortable: true,
      value: 'exportCustom',
      width: '115px',
    },
    {
      text: 'Custom Export Name',
      align: 'left',
      sortable: true,
      value: 'customExportName',
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
      text: 'Job Name',
      align: 'left',
      sortable: true,
      value: 'jobName',
      width: '115px',
    },
    {
      text: 'Run As User Name',
      align: 'left',
      sortable: true,
      value: 'runAsUserName',
      width: '115px',
    },
    {
      text: 'Start Date',
      align: 'left',
      sortable: true,
      value: 'startDate',
      width: '115px',
    },
    {
      text: 'End Date',
      align: 'left',
      sortable: true,
      value: 'endDate',
      width: '115px',
    },
    {
      text: 'Start Time',
      align: 'left',
      sortable: true,
      value: 'startTime',
      width: '115px',
    },
    {
      text: 'Recurring',
      align: 'left',
      sortable: true,
      value: 'recurring',
      width: '115px',
    },
    {
      text: 'Run Monday',
      align: 'left',
      sortable: true,
      value: 'runMonday',
      width: '115px',
    },
    {
      text: 'Run Tuesday',
      align: 'left',
      sortable: true,
      value: 'runTuesday',
      width: '115px',
    },
    {
      text: 'Run Wednesday',
      align: 'left',
      sortable: true,
      value: 'runWednesday',
      width: '115px',
    },
    {
      text: 'Run Thursday',
      align: 'left',
      sortable: true,
      value: 'runThursday',
      width: '115px',
    },
    {
      text: 'Run Friday',
      align: 'left',
      sortable: true,
      value: 'runFriday',
      width: '115px',
    },
    {
      text: 'Run Saturday',
      align: 'left',
      sortable: true,
      value: 'runSaturday',
      width: '115px',
    },
    {
      text: 'Run Sunday',
      align: 'left',
      sortable: true,
      value: 'runSunday',
      width: '115px',
    },
    {
      text: 'Active',
      align: 'left',
      sortable: true,
      value: 'active',
      width: '115px',
    },
    {
      text: 'Is Monthly',
      align: 'left',
      sortable: true,
      value: 'isMonthly',
      width: '115px',
    },
    {
      text: 'Run Monthly Day',
      align: 'left',
      sortable: true,
      value: 'runMonthlyDay',
      width: '115px',
    },
    {
      text: 'Run Monthly Time',
      align: 'left',
      sortable: true,
      value: 'runMonthlyTime',
      width: '115px',
    },
    {
      text: 'Is Hourly',
      align: 'left',
      sortable: true,
      value: 'isHourly',
      width: '115px',
    },
    {
      text: 'Is Daily',
      align: 'left',
      sortable: true,
      value: 'isDaily',
      width: '115px',
    },
    {
      text: 'Run Daily Time',
      align: 'left',
      sortable: true,
      value: 'runDailyTime',
      width: '115px',
    },
    {
      text: 'Is Weekly',
      align: 'left',
      sortable: true,
      value: 'isWeekly',
      width: '115px',
    },
    {
      text: 'Run Hourly Minute',
      align: 'left',
      sortable: true,
      value: 'runHourlyMinute',
      width: '115px',
    },
    {
      text: 'Include Data Source File',
      align: 'left',
      sortable: true,
      value: 'includeDataSourceFile',
      width: '115px',
    },
    {
      text: 'Run On Week Number',
      align: 'left',
      sortable: true,
      value: 'runOnWeekNumber',
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
    private crudService: CrudService,
    private router: Router,
  ) {
    super();
  }

  public ngOnInit(): void {
    this.initialUrl = this.sessionService.getInitialUrl();
    this.getScheduledJobs(this.searchModel);
    this.getCompanies();
    this.getJobTemplates();
    this.predefinedJobOrReportTypes = this.selectService.GetPredefinedJobOrReportTypes();
    this.getRunAsUsers();
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
          this.companys = data.model;
        }
      },
      error: (err: unknown) => {
        this.companyTypeaheadLoading = false;
      },
      complete: () => null,
    });
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

    this.runAsUserTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.runAsUserTypeaheadLoading = true;
      const runAsUserSearchCriteria = new UserSearchCriteria();
      runAsUserSearchCriteria.keyword = searchValue;
      return this.httpService.getUsers(runAsUserSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.runAsUserTypeaheadLoading = false;
        if (data.model) this.runAsUsers = data.model;
      },
      error: (err: unknown) => {
        this.runAsUserTypeaheadLoading = false;
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

  public getRunAsUsers(): void {
    const searchModel = new UserSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.runAsUsers = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Run As User');
      },
      complete: () => null,
    });
  }

  public getScheduledJobs(searchModel: ScheduledJobSearchCriteria): void {
    this.httpService.getScheduledJobs(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Scheduled Jobs", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Scheduled Jobs"),
      complete: () => null,
    });
  }

  public showJobSelectionScreen(): void {
    this.router.navigateByUrl(`${this.initialUrl}/jobs`);
  }

  public showAddScheduledJobModal(): void {
    const modalRef = this.modalService.open(ScheduledJobCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as ScheduledJobCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Scheduled Job';
    componentInstance.onCloseCallback = (): void => {
      this.getScheduledJobs(this.searchModel);
    }
  }

  public showViewScheduledJobModal(item: ScheduledJobModel): void {
    const modalRef = this.modalService.open(ScheduledJobCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as ScheduledJobCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Scheduled Job';
    componentInstance.scheduledJobModel = JSON.parse(JSON.stringify(item)) as ScheduledJobModel;
  }

  public showUpdateScheduledJobModal(item: ScheduledJobModel): void {
    const modalRef = this.modalService.open(ScheduledJobCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as ScheduledJobCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Scheduled Job';
    componentInstance.scheduledJobModel = JSON.parse(JSON.stringify(item)) as ScheduledJobModel;
    componentInstance.onCloseCallback = (): void => {
      this.getScheduledJobs(this.searchModel);
    }
  }

  public deleteScheduledJob(item: ScheduledJobModel): void {
    this.dialogService.confirmWithTitle(`Delete Scheduled Job`, `Do you want to delete the scheduledJob - ${item.jobName}?`).then(res => {
      if (!res) return;
      this.httpService.markScheduledJobAsDeleted(item.scheduledJobId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Scheduled Job");
        this.getScheduledJobs(this.searchModel);
        this.dialogService.success("Successfully deleted the Scheduled Job");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.searchModel.companyId = this.selectedCompany ? this.selectedCompany.companyId : null;
    this.searchModel.jobTemplateId = this.selectedJobTemplate ? this.selectedJobTemplate.jobTemplateId : null;
    const predefinedJobOrReportTypesArray = this.selectedPredefinedJobOrReportTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedJobOrReportTypeIds = predefinedJobOrReportTypesArray;

    this.getScheduledJobs(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.companyId = null;
    this.searchModel.jobTemplateId = null;
    this.searchModel.predefinedJobOrReportTypeIds = null;
    this.searchModel.submittedByUserId = null;
    this.searchModel.runAsUserId = null;
    this.getScheduledJobs(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getScheduledJobs(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const scheduledJobSearchCriteria = new ScheduledJobSearchCriteria();
    scheduledJobSearchCriteria.page = e.page;
    scheduledJobSearchCriteria.pageSize = e.pageSize;
    this.getScheduledJobs(scheduledJobSearchCriteria);
  }

}
