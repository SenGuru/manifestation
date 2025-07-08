import { Component, Input, OnInit } from '@angular/core';
import { JobTemplateModel, JobTemplateSearchCriteria, LoggedInUserModel, ScheduledJobCreateModel, ScheduledJobModel, ScheduledJobUpdateModel } from 'src/common/models/app-model';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';

@UntilDestroy()
@Component({
    selector: 'app-scheduled-job-crud-modal',
    templateUrl: './scheduled-job-crud-modal.component.html',
    standalone: false
})
export class ScheduledJobCrudModalComponent implements OnInit {

  @Input() public scheduledJobModel: ScheduledJobModel = new ScheduledJobModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;

  public executableFolder__size: number = ScheduledJobModel.executableFolder__size;
  public executableName__size: number = ScheduledJobModel.executableName__size;
  public executableParameter__size: number = ScheduledJobModel.executableParameter__size;
  public serializedJobParameter__size: number = ScheduledJobModel.serializedJobParameter__size;
  public reportTemplateFile__size: number = ScheduledJobModel.reportTemplateFile__size;
  public customExportName__size: number = ScheduledJobModel.customExportName__size;
  public jobName__size: number = ScheduledJobModel.jobName__size;

  public jobTemplateTypeahead$ = new Subject<string>();
  public jobTemplateTypeaheadLoading = false;
  public jobTemplates: JobTemplateModel[] = [];
  public selectedJobTemplate: JobTemplateModel | undefined;
  public predefinedJobOrReportTypes: Status[] = [];
  public selectedPredefinedJobOrReportType: Status | undefined; 

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.getJobTemplates();
    this.predefinedJobOrReportTypes = this.selectService.GetPredefinedJobOrReportTypes();
    if (!this.isCreateMode) this.selectedPredefinedJobOrReportType = this.predefinedJobOrReportTypes.find(item => item.id === this.scheduledJobModel.predefinedJobOrReportTypeId);
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
    const searchModel: JobTemplateSearchCriteria = new JobTemplateSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getJobTemplates(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.jobTemplates = data.model;
          if (!this.isCreateMode) this.selectedJobTemplate = data.model?.find(item => item.jobTemplateId === this.scheduledJobModel.jobTemplateId);
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Job Template');
      },
      complete: () => null,
    });
  } 
  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const scheduledJobModel = this.generateModel(f.value);
    if (scheduledJobModel) {
      if (this.isCreateMode) {
        this.createScheduledJob(scheduledJobModel, f);
      } else {
        this.updateScheduledJob(scheduledJobModel, f);
      }
    }
  }

  public generateModel(obj: ScheduledJobModel): ScheduledJobModel | undefined {
    if (this.selectedJobTemplate == null)
      return;
    if (this.selectedPredefinedJobOrReportType == null)
      return; 
    Object.assign(this.scheduledJobModel, obj);
    Object.keys(this.scheduledJobModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.scheduledJobModel[key] && typeof this.scheduledJobModel[key] === 'object' && !(this.scheduledJobModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.scheduledJobModel[key] = null;
      }
    });

    this.scheduledJobModel.jobTemplateId = this.selectedJobTemplate?.jobTemplateId ?? 0;
    this.scheduledJobModel.predefinedJobOrReportTypeId = this.selectedPredefinedJobOrReportType ? this.selectedPredefinedJobOrReportType.id : null;
    return this.scheduledJobModel;
  }

  public createScheduledJob(scheduledJobModel: ScheduledJobCreateModel, f: NgForm): void {
    this.httpService.createScheduledJob(scheduledJobModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create ScheduledJob', result);
          return;
        }
        this.dialogService.success('Successfully Created the ScheduledJob.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create ScheduledJob'),
      complete: () => null,
    });
  }

  public updateScheduledJob(scheduledJobModel: ScheduledJobUpdateModel, f: NgForm): void {
    this.httpService.updateScheduledJob(scheduledJobModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update ScheduledJob', result);
          return;
        }
        this.dialogService.success('Successfully updated the ScheduledJob.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update ScheduledJob'),
      complete: () => null,
    });
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
