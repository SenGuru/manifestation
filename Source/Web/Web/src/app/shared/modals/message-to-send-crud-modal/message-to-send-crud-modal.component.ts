import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { LoggedInUserModel, MessageToSendCreateModel, MessageToSendModel, RealtimeMessageModel, RealtimeMessageSearchCriteria } from 'src/common/models/app-model';
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
    selector: 'app-message-to-send-crud-modal',
    templateUrl: './message-to-send-crud-modal.component.html',
    standalone: false
})
export class MessageToSendCrudModalComponent implements OnInit {

  @Input() public messageToSendModel: MessageToSendModel = new MessageToSendModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;

  public messageFrom__size: number = MessageToSendModel.messageFrom__size;
  public messageTo__size: number = MessageToSendModel.messageTo__size;
  public messageFromName__size: number = MessageToSendModel.messageFromName__size;
  public messageToName__size: number = MessageToSendModel.messageToName__size;
  public messageSubject__size: number = MessageToSendModel.messageSubject__size;
  public messageBody__size: number = MessageToSendModel.messageBody__size;
  public sentErrorMessage__size: number = MessageToSendModel.sentErrorMessage__size;

  public predefinedMessageToSendTypes: Status[] = [];
  public selectedPredefinedMessageToSendType: Status | undefined;
  public realtimeMessageTypeahead$ = new Subject<string>();
  public realtimeMessageTypeaheadLoading = false;
  public realtimeMessages: RealtimeMessageModel[] = [];
  public selectedRealtimeMessage: RealtimeMessageModel | undefined;
 

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedMessageToSendTypes = this.selectService.GetPredefinedMessageToSendTypes();
    if (!this.isCreateMode) this.selectedPredefinedMessageToSendType = this.predefinedMessageToSendTypes.find(item => item.id === this.messageToSendModel.predefinedMessageToSendTypeId);
    this.getRealtimeMessages();
   
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.realtimeMessageTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.realtimeMessageTypeaheadLoading = true;
      const realtimeMessageSearchCriteria = new RealtimeMessageSearchCriteria();
      realtimeMessageSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) realtimeMessageSearchCriteria.selectedId = this.selectedRealtimeMessage?.realtimeMessageId;
      return this.httpService.getRealtimeMessages(realtimeMessageSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) {
          this.realtimeMessageTypeaheadLoading = false;
          this.realtimeMessages = data.model;
        }
      },
      error: (err: unknown) => {
        this.realtimeMessageTypeaheadLoading = false;
      },
      complete: () => null,
    }); 
  }

  public getRealtimeMessages(): void {
    const searchModel: RealtimeMessageSearchCriteria = new RealtimeMessageSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.selectedRealtimeMessage?.realtimeMessageId;
    this.httpService.getRealtimeMessages(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.realtimeMessages = data.model;
          if (!this.isCreateMode) this.selectedRealtimeMessage = data.model.find(item => item.realtimeMessageId === this.messageToSendModel.realtimeMessageId);
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Realtime Message');
      },
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const messageToSendModel = this.generateModel(f.value);
    if (this.isCreateMode && messageToSendModel) {
      this.createMessageToSend(messageToSendModel, f);
    }
  }

  public generateModel(obj: MessageToSendModel): MessageToSendModel | undefined {
    if (this.selectedPredefinedMessageToSendType == null)
      return;
    if (this.selectedRealtimeMessage == null)
      return;
    Object.assign(this.messageToSendModel, obj);
    Object.keys(this.messageToSendModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.messageToSendModel[key] && typeof this.messageToSendModel[key] === 'object' && !(this.messageToSendModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.messageToSendModel[key] = null;
      }
    });

    this.messageToSendModel.predefinedMessageToSendTypeId = this.selectedPredefinedMessageToSendType?.id ?? 0;
    this.messageToSendModel.realtimeMessageId = this.selectedRealtimeMessage ? this.selectedRealtimeMessage.realtimeMessageId : null;
    return this.messageToSendModel;
  }

  public createMessageToSend(messageToSendModel: MessageToSendCreateModel, f: NgForm): void {
    this.httpService.createMessageToSend(messageToSendModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Message To Send', result);
          return;
        }
        this.dialogService.success('Successfully Created the MessageToSend.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create Message To Send'),
      complete: () => null,
    });
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
