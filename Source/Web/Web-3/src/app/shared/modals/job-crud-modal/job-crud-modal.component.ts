import { CompanyModel, CompanySearchCriteria, JobCreateModel, JobModel, JobTemplateModel, JobTemplateSearchCriteria, JobUpdateModel, JobWithDiffModel, LoggedInUserModel } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';

@UntilDestroy()
@Component({
    selector: 'app-job-crud-modal',
    templateUrl: './job-crud-modal.component.html',
    standalone: false
})
export class JobCrudModalComponent implements OnInit {

  @Input() public jobModel: JobWithDiffModel = new JobWithDiffModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;
  public aSubmittedDate: Date | null = null;
  public message__size: number = JobModel.message__size;
  public executableFolder__size: number = JobModel.executableFolder__size;
  public executableName__size: number = JobModel.executableName__size;
  public executableParameter__size: number = JobModel.executableParameter__size;
  public serializedJobParameter__size: number = JobModel.serializedJobParameter__size;
  public reportTemplateFile__size: number = JobModel.reportTemplateFile__size;
  public customExportName__size: number = JobModel.customExportName__size;
  public jobName__size: number = JobModel.jobName__size;
  public executionAppName__size: number = JobModel.executionAppName__size;
  public executionHostName__size: number = JobModel.executionHostName__size;

  public predefinedJobTypes: Status[] = [];
  public selectedPredefinedJobType: Status | undefined;
  public jobTemplateTypeahead$ = new Subject<string>();
  public jobTemplateTypeaheadLoading = false;
  public jobTemplates: JobTemplateModel[] = [];
  public selectedJobTemplate: JobTemplateModel | undefined;
  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;
  public predefinedJobOrReportTypes: Status[] = [];
  public selectedPredefinedJobOrReportType: Status | undefined;
  public predefinedJobStatusTypes: Status[] = [];
  public selectedPredefinedJobStatusType: Status | undefined;
  public submittedByUserTypeahead$ = new Subject<string>();
  public submittedByUserTypeaheadLoading = false;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public submittedByUsers: any[] = [];
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public selectedSubmittedByUser: any;
  // submittedByUsers: SubmittedByUserModel[] = [];
  // selectedSubmittedByUser: SubmittedByUserModel;
  public executionAppTypeahead$ = new Subject<string>();
  public executionAppTypeaheadLoading = false;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public executionApps: any[] = [];
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public selectedExecutionApp: any;
  // executionApps: ExecutionAppModel[] = [];
  // selectedExecutionApp: ExecutionAppModel;  
  public executionProcessTypeahead$ = new Subject<string>();
  public executionProcessTypeaheadLoading = false;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public executionProcesss: any[] = [];
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public selectedExecutionProcess: any;
  // executionProcesss: ExecutionProcessModel[] = [];
  // selectedExecutionProcess: ExecutionProcessModel;
  public executionThreadTypeahead$ = new Subject<string>();
  public executionThreadTypeaheadLoading = false;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public executionThreads: any[] = [];
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public selectedExecutionThread: any;
  // executionThreads: ExecutionThreadModel[] = [];
  // selectedExecutionThread: ExecutionThreadModel;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedJobTypes = this.selectService.GetPredefinedJobTypes();
    if (!this.isCreateMode) this.selectedPredefinedJobType = this.predefinedJobTypes.find(item => item.id === this.jobModel.predefinedJobTypeId);
    this.getJobTemplates();
    this.getCompanies();
    this.predefinedJobOrReportTypes = this.selectService.GetPredefinedJobOrReportTypes();
    if (!this.isCreateMode) this.selectedPredefinedJobOrReportType = this.predefinedJobOrReportTypes.find(item => item.id === this.jobModel.predefinedJobOrReportTypeId);
    this.predefinedJobStatusTypes = this.selectService.GetPredefinedJobStatusTypes();
    if (!this.isCreateMode) this.selectedPredefinedJobStatusType = this.predefinedJobStatusTypes.find(item => item.id === this.jobModel.predefinedJobStatusTypeId);
    this.getSubmittedByUsers();
    this.getExecutionApps();
    this.getExecutionProcesss();
    this.getExecutionThreads();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.jobTemplateTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.jobTemplateTypeaheadLoading = true;
      const jobTemplateSearchCriteria = new JobTemplateSearchCriteria();
      jobTemplateSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) jobTemplateSearchCriteria.selectedId = this.jobModel.jobTemplateId;
      return this.httpService.getJobTemplates(jobTemplateSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) {
          this.jobTemplateTypeaheadLoading = false;
          this.jobTemplates = data.model;
        }
      },
      error: (err: unknown) => {
        this.jobTemplateTypeaheadLoading = false;
      },
      complete: () => null,
    });
    this.companyTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.companyTypeaheadLoading = true;
      const companySearchCriteria = new CompanySearchCriteria();
      companySearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) companySearchCriteria.selectedId = this.jobModel.companyId;
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
    // this.submittedByUserTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
    //   this.submittedByUserTypeaheadLoading = true;
    //   let submittedByUserSearchCriteria = new SubmittedByUserSearchCriteria();
    //   submittedByUserSearchCriteria.keyword = searchValue;
    //   return this.httpService.getSubmittedByUsers(submittedByUserSearchCriteria, getHeaderWithBlockUI());
    // }),untilDestroyed(this)).subscribe({
    //   next: (data) => {
    //     this.submittedByUserTypeaheadLoading = false;
    //     this.submittedByUsers = data.model;
    // },  
    // error: (err: unknown) => {
    // this.submittedByUserTypeaheadLoading = false;
    // console.error(err);
    // },
    // complete: () => null,});
    // this.executionAppTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
    //   this.executionAppTypeaheadLoading = true;
    //   let executionAppSearchCriteria = new ExecutionAppSearchCriteria();
    //   executionAppSearchCriteria.keyword = searchValue;
    //   return this.httpService.getExecutionApps(executionAppSearchCriteria, getHeaderWithBlockUI());
    // }),untilDestroyed(this)).subscribe({
    //   next: (data) => {
    //     this.executionAppTypeaheadLoading = false;
    //     this.executionApps = data.model;
    // },  
    // error: (err: unknown) => {
    // this.executionAppTypeaheadLoading = false;
    // console.error(err);
    // },
    // complete: () => null,});
    // this.executionProcessTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
    //   this.executionProcessTypeaheadLoading = true;
    //   let executionProcessSearchCriteria = new ExecutionProcessSearchCriteria();
    //   executionProcessSearchCriteria.keyword = searchValue;
    //   return this.httpService.getExecutionProcesss(executionProcessSearchCriteria, getHeaderWithBlockUI());
    // }),untilDestroyed(this)).subscribe({
    //   next: (data) => {
    //     this.executionProcessTypeaheadLoading = false;
    //     this.executionProcesss = data.model;
    // },  
    // error: (err: unknown) => {
    // this.executionProcessTypeaheadLoading = false;
    // console.error(err);
    // },
    // complete: () => null,});
    // this.executionThreadTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
    //   this.executionThreadTypeaheadLoading = true;
    //   let executionThreadSearchCriteria = new ExecutionThreadSearchCriteria();
    //   executionThreadSearchCriteria.keyword = searchValue;
    //   return this.httpService.getExecutionThreads(executionThreadSearchCriteria, getHeaderWithBlockUI());
    // }),untilDestroyed(this)).subscribe({
    //   next: (data) => {
    //     this.executionThreadTypeaheadLoading = false;
    //     this.executionThreads = data.model;
    // },  
    // error: (err: unknown) => {
    // this.executionThreadTypeaheadLoading = false;
    // console.error(err);
    // },
    // complete: () => null,});
  }

  public getJobTemplates(): void {
    const searchModel: JobTemplateSearchCriteria = new JobTemplateSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.jobModel.jobTemplateId;
    this.httpService.getJobTemplates(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.jobTemplates = data.model;
          if (!this.isCreateMode) this.selectedJobTemplate = data.model.find(item => item.jobTemplateId === this.jobModel.jobTemplateId);
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Job Template');
      },
      complete: () => null,
    });
  }
  public getCompanies(): void {
    const searchModel: CompanySearchCriteria = new CompanySearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.jobModel.companyId;
    this.httpService.getCompanies(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.companys = data.model;
          if (!this.isCreateMode) this.selectedCompany = data.model.find(item => item.companyId === this.jobModel.companyId);
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Company');
      },
      complete: () => null,
    });
  }
  public getSubmittedByUsers(): void {
    // let searchModel: SubmittedByUserSearchCriteria = new SubmittedByUserSearchCriteria();
    // searchModel.sortAscending = false;
    // searchModel.sortBy = 'createdDate';
    // this.httpService.getSubmittedByUsers(searchModel).subscribe({
    // next: (data) => {
    //   if (!data.isSuccess)
    //     return;
    //   this.submittedByUsers = data.model;
    //   if (!this.isCreateMode) this.selectedSubmittedByUser = data.model.find(item => item.submittedByUserId === this.jobModel.submittedByUserId);
    // },
    // error: (error: unknown) => {
    // console.log('getSubmittedByUsers: error', error);
    // return this.dialogService.error('Not Able to get Submitted By');
    // },
    // complete: () => null,
    // });
  }
  public getExecutionApps(): void {
    // let searchModel: ExecutionAppSearchCriteria = new ExecutionAppSearchCriteria();
    // searchModel.sortAscending = false;
    // searchModel.sortBy = 'createdDate';
    // this.httpService.getExecutionApps(searchModel).subscribe({
    // next: (data) => {
    //   if (!data.isSuccess)
    //     return;
    //   this.executionApps = data.model;
    //   if (!this.isCreateMode) this.selectedExecutionApp = data.model.find(item => item.executionAppId === this.jobModel.executionAppId);
    // },
    // error: (error: unknown) => {
    // console.log('getExecutionApps: error', error);
    // return this.dialogService.error('Not Able to get Execution App');
    // },
    // complete: () => null,
    // });
  }
  public getExecutionProcesss(): void {
    // let searchModel: ExecutionProcessSearchCriteria = new ExecutionProcessSearchCriteria();
    // searchModel.sortAscending = false;
    // searchModel.sortBy = 'createdDate';
    // this.httpService.getExecutionProcesss(searchModel).subscribe({
    // next: (data) => {
    //   if (!data.isSuccess)
    //     return;
    //   this.executionProcesss = data.model;
    //   if (!this.isCreateMode) this.selectedExecutionProcess = data.model.find(item => item.executionProcessId === this.jobModel.executionProcessId);
    // },
    // error: (error: unknown) => {
    // console.log('getExecutionProcesss: error', error);
    // return this.dialogService.error('Not Able to get Execution Process');
    // },
    // complete: () => null,
    // });
  }
  public getExecutionThreads(): void {
    // let searchModel: ExecutionThreadSearchCriteria = new ExecutionThreadSearchCriteria();
    // searchModel.sortAscending = false;
    // searchModel.sortBy = 'createdDate';
    // this.httpService.getExecutionThreads(searchModel).subscribe({
    // next: (data) => {
    //   if (!data.isSuccess)
    //     return;
    //   this.executionThreads = data.model;
    //   if (!this.isCreateMode) this.selectedExecutionThread = data.model.find(item => item.executionThreadId === this.jobModel.executionThreadId);
    // },
    // error: (error: unknown) => {
    // console.log('getExecutionThreads: error', error);
    // return this.dialogService.error('Not Able to get Execution Thread');
    // },
    // complete: () => null,
    // });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const jobModel = this.generateModel(f.value);
    if (jobModel) {
      if (this.isCreateMode) {
        this.createJob(jobModel, f);
      } else {
        this.updateJob(jobModel, f);
      }
    }
  }

  public generateModel(obj: JobModel): JobWithDiffModel | undefined {
    if (this.selectedPredefinedJobType == null)
      return;
    if (this.selectedJobTemplate == null)
      return;
    if (this.selectedCompany == null)
      return;
    if (this.selectedPredefinedJobOrReportType == null)
      return;
    if (this.selectedPredefinedJobStatusType == null)
      return;
    if (this.selectedSubmittedByUser == null)
      return;
    if (this.selectedExecutionApp == null)
      return;
    if (this.selectedExecutionProcess == null)
      return;
    if (this.selectedExecutionThread == null)
      return;
    Object.assign(this.jobModel, obj);
    Object.keys(this.jobModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.jobModel[key] && typeof this.jobModel[key] === 'object' && !(this.jobModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.jobModel[key] = null;
      }
    });

    this.jobModel.predefinedJobTypeId = this.selectedPredefinedJobType ? this.selectedPredefinedJobType.id : null;
    this.jobModel.jobTemplateId = this.selectedJobTemplate?.jobTemplateId ?? 0;
    this.jobModel.companyId = this.selectedCompany ? this.selectedCompany.companyId : null;
    this.jobModel.predefinedJobOrReportTypeId = this.selectedPredefinedJobOrReportType ? this.selectedPredefinedJobOrReportType.id : null;
    this.jobModel.predefinedJobStatusTypeId = this.selectedPredefinedJobStatusType?.id ?? 0;
    // this.jobModel.submittedByUserId = this.selectedSubmittedByUser ? this.selectedSubmittedByUser.submittedByUserId : null;
    // this.jobModel.executionAppId = this.selectedExecutionApp ? this.selectedExecutionApp.executionAppId : null;
    // this.jobModel.executionProcessId = this.selectedExecutionProcess ? this.selectedExecutionProcess.executionProcessId : null;
    // this.jobModel.executionThreadId = this.selectedExecutionThread ? this.selectedExecutionThread.executionThreadId : null;
    return this.jobModel;
  }

  public createJob(jobModel: JobCreateModel, f: NgForm): void {
    this.httpService.createJob(jobModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Job', result);
          return;
        }
        this.dialogService.success('Successfully Created the Job.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create Job'),
      complete: () => null,
    });
  }

  public updateJob(jobModel: JobUpdateModel, f: NgForm): void {
    this.httpService.updateJob(jobModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Job', result);
          return;
        }
        this.dialogService.success('Successfully updated the Job.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update Job'),
      complete: () => null,
    });
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
