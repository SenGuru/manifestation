import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { LoggedInUserModel, ScheduledJobModel, ScheduledJobNotificationEmailCreateModel, ScheduledJobNotificationEmailModel, ScheduledJobNotificationEmailUpdateModel, ScheduledJobSearchCriteria } from 'src/common/models/app-model';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@UntilDestroy()
@Component({
    selector: 'app-scheduled-job-notification-email-crud-modal',
    templateUrl: './scheduled-job-notification-email-crud-modal.component.html',
    standalone: false
})
export class ScheduledJobNotificationEmailCrudModalComponent implements OnInit {

  @Input() public scheduledJobNotificationEmailModel: ScheduledJobNotificationEmailModel = new ScheduledJobNotificationEmailModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;

  public email__size: number = ScheduledJobNotificationEmailModel.email__size;
  public message__size: number = ScheduledJobNotificationEmailModel.message__size;

  public scheduledJobTypeahead$ = new Subject<string>();
  public scheduledJobTypeaheadLoading = false;
  public scheduledJobs: ScheduledJobModel[] = [];
  public selectedScheduledJob: ScheduledJobModel | undefined;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
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
    const searchModel: ScheduledJobSearchCriteria = new ScheduledJobSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getScheduledJobs(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.scheduledJobs = data.model;
        if (!this.isCreateMode) this.selectedScheduledJob = data.model?.find(item => item.scheduledJobId === this.scheduledJobNotificationEmailModel.scheduledJobId);
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Scheduled Job');
      },
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const scheduledJobNotificationEmailModel = this.generateModel(f.value);
    if (scheduledJobNotificationEmailModel) {
      if (this.isCreateMode) {
        this.createScheduledJobNotificationEmail(scheduledJobNotificationEmailModel, f);
      } else {
        this.updateScheduledJobNotificationEmail(scheduledJobNotificationEmailModel, f);
      }
    }
  }

  public generateModel(obj: ScheduledJobNotificationEmailModel): ScheduledJobNotificationEmailModel | undefined {
    if (this.selectedScheduledJob == null)
      return;
    Object.assign(this.scheduledJobNotificationEmailModel, obj);
    Object.keys(this.scheduledJobNotificationEmailModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.scheduledJobNotificationEmailModel[key] && typeof this.scheduledJobNotificationEmailModel[key] === 'object' && !(this.scheduledJobNotificationEmailModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.scheduledJobNotificationEmailModel[key] = null;
      }
    });

    this.scheduledJobNotificationEmailModel.scheduledJobId = this.selectedScheduledJob?.scheduledJobId ?? 0;
    return this.scheduledJobNotificationEmailModel;
  }

  public createScheduledJobNotificationEmail(scheduledJobNotificationEmailModel: ScheduledJobNotificationEmailCreateModel, f: NgForm): void {
    this.httpService.createScheduledJobNotificationEmail(scheduledJobNotificationEmailModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create ScheduledJobNotificationEmail', result);
          return;
        }
        this.dialogService.success('Successfully Created the ScheduledJobNotificationEmail.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create ScheduledJobNotificationEmail'),
      complete: () => null,
    });
  }

  public updateScheduledJobNotificationEmail(scheduledJobNotificationEmailModel: ScheduledJobNotificationEmailUpdateModel, f: NgForm): void {
    this.httpService.updateScheduledJobNotificationEmail(scheduledJobNotificationEmailModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update ScheduledJobNotificationEmail', result);
          return;
        }
        this.dialogService.success('Successfully updated the ScheduledJobNotificationEmail.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update ScheduledJobNotificationEmail'),
      complete: () => null,
    });
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
