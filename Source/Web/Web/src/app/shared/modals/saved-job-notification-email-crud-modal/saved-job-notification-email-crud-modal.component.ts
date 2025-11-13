import { Component, Input, OnInit } from '@angular/core';
import { LoggedInUserModel, SavedJobModel, SavedJobNotificationEmailCreateModel, SavedJobNotificationEmailModel, SavedJobNotificationEmailUpdateModel, SavedJobSearchCriteria } from 'src/common/models/app-model';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@UntilDestroy()
@Component({
    selector: 'app-saved-job-notification-email-crud-modal',
    templateUrl: './saved-job-notification-email-crud-modal.component.html',
    standalone: false
})
export class SavedJobNotificationEmailCrudModalComponent implements OnInit {

  @Input() public savedJobNotificationEmailModel: SavedJobNotificationEmailModel = new SavedJobNotificationEmailModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;

  public email__size: number = SavedJobNotificationEmailModel.email__size;
  public message__size: number = SavedJobNotificationEmailModel.message__size;

  public savedJobTypeahead$ = new Subject<string>();
  public savedJobTypeaheadLoading = false;
  public savedJobs: SavedJobModel[] = [];
  public selectedSavedJob: SavedJobModel | undefined;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
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
    const searchModel: SavedJobSearchCriteria = new SavedJobSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getSavedJobs(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.savedJobs = data.model;
          if (!this.isCreateMode) this.selectedSavedJob = data.model?.find(item => item.savedJobId === this.savedJobNotificationEmailModel.savedJobId);
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Saved Job');
      },
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const savedJobNotificationEmailModel = this.generateModel(f.value);
    if (savedJobNotificationEmailModel) {
      if (this.isCreateMode) {
        this.createSavedJobNotificationEmail(savedJobNotificationEmailModel, f);
      } else {
        this.updateSavedJobNotificationEmail(savedJobNotificationEmailModel, f);
      }
    }
  }

  public generateModel(obj: SavedJobNotificationEmailModel): SavedJobNotificationEmailModel | undefined {
    if (this.selectedSavedJob == null)
      return;
    Object.assign(this.savedJobNotificationEmailModel, obj);
    Object.keys(this.savedJobNotificationEmailModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.savedJobNotificationEmailModel[key] && typeof this.savedJobNotificationEmailModel[key] === 'object' && !(this.savedJobNotificationEmailModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.savedJobNotificationEmailModel[key] = null;
      }
    });

    this.savedJobNotificationEmailModel.savedJobId = this.selectedSavedJob?.savedJobId ?? 0;
    return this.savedJobNotificationEmailModel;
  }

  public createSavedJobNotificationEmail(savedJobNotificationEmailModel: SavedJobNotificationEmailCreateModel, f: NgForm): void {
    this.httpService.createSavedJobNotificationEmail(savedJobNotificationEmailModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create SavedJobNotificationEmail', result);
          return;
        }
        this.dialogService.success('Successfully Created the SavedJobNotificationEmail.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create SavedJobNotificationEmail'),
      complete: () => null,
    });
  }

  public updateSavedJobNotificationEmail(savedJobNotificationEmailModel: SavedJobNotificationEmailUpdateModel, f: NgForm): void {
    this.httpService.updateSavedJobNotificationEmail(savedJobNotificationEmailModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update SavedJobNotificationEmail', result);
          return;
        }
        this.dialogService.success('Successfully updated the SavedJobNotificationEmail.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update SavedJobNotificationEmail'),
      complete: () => null,
    });
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
