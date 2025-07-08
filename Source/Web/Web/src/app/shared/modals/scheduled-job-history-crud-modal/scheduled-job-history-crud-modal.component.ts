import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { LoggedInUserModel, ScheduledJobHistoryCreateModel, ScheduledJobHistoryModel, ScheduledJobHistoryUpdateModel, ScheduledJobModel, ScheduledJobSearchCriteria } from 'src/common/models/app-model';
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
    selector: 'app-scheduled-job-history-crud-modal',
    templateUrl: './scheduled-job-history-crud-modal.component.html',
    standalone: false
})
export class ScheduledJobHistoryCrudModalComponent implements OnInit {

  @Input() public scheduledJobHistoryModel: ScheduledJobHistoryModel = new ScheduledJobHistoryModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;
  public aPickupDate: Date | null = null;
  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;

  public pickupMachineName__size: number = ScheduledJobHistoryModel.pickupMachineName__size;

  public scheduledJobTypeahead$ = new Subject<string>();
  public scheduledJobTypeaheadLoading = false;
  public scheduledJobs: ScheduledJobModel[] = [];
  public selectedScheduledJob: ScheduledJobModel | undefined; 
  public pickupProcessTypeahead$ = new Subject<string>();
  public pickupProcessTypeaheadLoading = false;
  // pickupProcesss: PickupProcessModel[] = [];
  // selectedPickupProcess: PickupProcessModel;  
  public pickupProcesss: unknown[] = [];
  public selectedPickupProcess: unknown;

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
    this.getPickupProcesss();
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
    // this.pickupProcessTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
    //   this.pickupProcessTypeaheadLoading = true;
    //   let pickupProcessSearchCriteria = new PickupProcessSearchCriteria();
    //   pickupProcessSearchCriteria.keyword = searchValue;
    //   return this.httpService.getPickupProcesss(pickupProcessSearchCriteria, getHeaderWithBlockUI());
    // }),untilDestroyed(this)).subscribe({
    //   next: (data) => {
    //     this.pickupProcessTypeaheadLoading = false;
    //     this.pickupProcesss = data.model;
    // },  
    // error: (err: unknown) => {
    // this.pickupProcessTypeaheadLoading = false;
    // console.error(err);
    // },
    // complete: () => null,});
  }

  public getScheduledJobs(): void {
    const searchModel: ScheduledJobSearchCriteria = new ScheduledJobSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getScheduledJobs(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.scheduledJobs = data.model;
          if (!this.isCreateMode) this.selectedScheduledJob = data.model?.find(item => item.scheduledJobId === this.scheduledJobHistoryModel.scheduledJobId);
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Scheduled Job');
      },
      complete: () => null,
    });
  }
 
  public getPickupProcesss(): void {
    // let searchModel: PickupProcessSearchCriteria = new PickupProcessSearchCriteria();
    // searchModel.sortAscending = false;
    // searchModel.sortBy = 'createdDate';
    // this.httpService.getPickupProcesss(searchModel).subscribe({
    // next: (data) => {
    //   if (!data.isSuccess)
    //     return;
    //   this.pickupProcesss = data.model;
    //   if (!this.isCreateMode) this.selectedPickupProcess = data.model.find(item => item.pickupProcessId === this.scheduledJobHistoryModel.pickupProcessId);
    // },
    // error: (error: unknown) => {
    // console.log('getPickupProcesss: error', error);
    // return this.dialogService.error('Not Able to get Pickup Process');
    // },
    // complete: () => null,
    // });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const scheduledJobHistoryModel = this.generateModel(f.value);
    if (scheduledJobHistoryModel) {
      if (this.isCreateMode) {
        this.createScheduledJobHistory(scheduledJobHistoryModel, f);
      } else {
        this.updateScheduledJobHistory(scheduledJobHistoryModel, f);
      }
    }
  }

  public generateModel(obj: ScheduledJobHistoryModel): ScheduledJobHistoryModel | undefined {
    if (this.selectedScheduledJob == null)
      return; 
    if (this.selectedPickupProcess == null)
      return;
    Object.assign(this.scheduledJobHistoryModel, obj);
    Object.keys(this.scheduledJobHistoryModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.scheduledJobHistoryModel[key] && typeof this.scheduledJobHistoryModel[key] === 'object' && !(this.scheduledJobHistoryModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.scheduledJobHistoryModel[key] = null;
      }
    });

    this.scheduledJobHistoryModel.scheduledJobId = this.selectedScheduledJob?.scheduledJobId ?? 0;
    // this.scheduledJobHistoryModel.pickupProcessId = this.selectedPickupProcess ? this.selectedPickupProcess.pickupProcessId : null;
    return this.scheduledJobHistoryModel;
  }

  public createScheduledJobHistory(scheduledJobHistoryModel: ScheduledJobHistoryCreateModel, f: NgForm): void {
    this.httpService.createScheduledJobHistory(scheduledJobHistoryModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create ScheduledJobHistory', result);
          return;
        }
        this.dialogService.success('Successfully Created the ScheduledJobHistory.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create ScheduledJobHistory'),
      complete: () => null,
    });
  }

  public updateScheduledJobHistory(scheduledJobHistoryModel: ScheduledJobHistoryUpdateModel, f: NgForm): void {
    this.httpService.updateScheduledJobHistory(scheduledJobHistoryModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update ScheduledJobHistory', result);
          return;
        }
        this.dialogService.success('Successfully updated the ScheduledJobHistory.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update ScheduledJobHistory'),
      complete: () => null,
    });
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
