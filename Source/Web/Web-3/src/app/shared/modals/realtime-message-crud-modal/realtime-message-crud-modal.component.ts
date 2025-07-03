import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { LoggedInUserModel, RealtimeMessageCreateModel, RealtimeMessageModel, RealtimeMessageUpdateModel, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
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
    selector: 'app-realtime-message-crud-modal',
    templateUrl: './realtime-message-crud-modal.component.html',
    standalone: false
})
export class RealtimeMessageCrudModalComponent implements OnInit {

  @Input() public realtimeMessageModel: RealtimeMessageModel = new RealtimeMessageModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;
  public aMessageDateTime: Date | null = null;
  public messageSubject__size: number = RealtimeMessageModel.messageSubject__size;
  public messageContent__size: number = RealtimeMessageModel.messageContent__size;
  public resourceName__size: number = RealtimeMessageModel.resourceName__size;
  public errorMessage__size: number = RealtimeMessageModel.errorMessage__size;

  public predefinedMessagePriorityTypes: Status[] = [];
  public selectedPredefinedMessagePriorityType: Status | undefined;
  public predefinedRealtimeMessageStatusTypes: Status[] = [];
  public selectedPredefinedRealtimeMessageStatusType: Status | undefined;
  public predefinedNotificationMessageTypes: Status[] = [];
  public selectedPredefinedNotificationMessageType: Status | undefined;
  public predefinedResourceTypes: Status[] = [];
  public selectedPredefinedResourceType: Status | undefined;
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedMessagePriorityTypes = this.selectService.GetPredefinedMessagePriorityTypes();
    if (!this.isCreateMode) this.selectedPredefinedMessagePriorityType = this.predefinedMessagePriorityTypes.find(item => item.id === this.realtimeMessageModel.predefinedMessagePriorityTypeId);
    this.predefinedRealtimeMessageStatusTypes = this.selectService.GetPredefinedRealtimeMessageStatusTypes();
    if (!this.isCreateMode) this.selectedPredefinedRealtimeMessageStatusType = this.predefinedRealtimeMessageStatusTypes.find(item => item.id === this.realtimeMessageModel.predefinedRealtimeMessageStatusTypeId);
    this.predefinedNotificationMessageTypes = this.selectService.GetPredefinedNotificationMessageTypes();
    if (!this.isCreateMode) this.selectedPredefinedNotificationMessageType = this.predefinedNotificationMessageTypes.find(item => item.id === this.realtimeMessageModel.predefinedNotificationMessageTypeId);
    this.predefinedResourceTypes = this.selectService.GetPredefinedResourceTypes();
    if (!this.isCreateMode) this.selectedPredefinedResourceType = this.predefinedResourceTypes.find(item => item.id === this.realtimeMessageModel.predefinedResourceTypeId);
    this.getUsers();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {

    this.userTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.userTypeaheadLoading = true;
      const userSearchCriteria = new UserSearchCriteria();
      userSearchCriteria.keyword = searchValue;
      return this.httpService.getUsers(userSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) {
          this.userTypeaheadLoading = false;
          this.users = data.model;
        }
      },
      error: (err: unknown) => {
        this.userTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

  public getUsers(): void {
    const searchModel: UserSearchCriteria = new UserSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.users = data.model;
          if (!this.isCreateMode) this.selectedUser = data.model.find(item => item.userId === this.realtimeMessageModel.userId);
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get User');
      },
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const realtimeMessageModel = this.generateModel(f.value);
    if (realtimeMessageModel) {
      if (this.isCreateMode) {
        this.createRealtimeMessage(realtimeMessageModel, f);
      } else {
        this.updateRealtimeMessage(realtimeMessageModel, f);
      }
    }
  }

  public generateModel(obj: RealtimeMessageModel): RealtimeMessageModel | undefined {
    if (this.selectedPredefinedMessagePriorityType == null)
      return;
    if (this.selectedPredefinedRealtimeMessageStatusType == null)
      return;
    if (this.selectedPredefinedNotificationMessageType == null)
      return;
    if (this.selectedPredefinedResourceType == null)
      return;
    if (this.selectedUser == null)
      return;
    Object.assign(this.realtimeMessageModel, obj);
    Object.keys(this.realtimeMessageModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.realtimeMessageModel[key] && typeof this.realtimeMessageModel[key] === 'object' && !(this.realtimeMessageModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.realtimeMessageModel[key] = null;
      }
    });

    this.realtimeMessageModel.predefinedMessagePriorityTypeId = this.selectedPredefinedMessagePriorityType?.id ?? 0;
    this.realtimeMessageModel.predefinedRealtimeMessageStatusTypeId = this.selectedPredefinedRealtimeMessageStatusType?.id ?? 0;
    this.realtimeMessageModel.predefinedNotificationMessageTypeId = this.selectedPredefinedNotificationMessageType?.id ?? 0;
    this.realtimeMessageModel.predefinedResourceTypeId = this.selectedPredefinedResourceType ? this.selectedPredefinedResourceType.id : null;
    this.realtimeMessageModel.userId = this.selectedUser?.userId ?? 0;
    return this.realtimeMessageModel;
  }

  public createRealtimeMessage(realtimeMessageModel: RealtimeMessageCreateModel, f: NgForm): void {
    this.httpService.createRealtimeMessage(realtimeMessageModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create RealtimeMessage', result);
          return;
        }
        this.dialogService.success('Successfully Created the RealtimeMessage.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create RealtimeMessage'),
      complete: () => null,
    });
  }

  public updateRealtimeMessage(realtimeMessageModel: RealtimeMessageUpdateModel, f: NgForm): void {
    this.httpService.updateRealtimeMessage(realtimeMessageModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update RealtimeMessage', result);
          return;
        }
        this.dialogService.success('Successfully updated the RealtimeMessage.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update RealtimeMessage'),
      complete: () => null,
    });
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
