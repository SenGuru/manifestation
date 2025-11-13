import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { LoggedInUserModel, VoiceNotificationServerCreateModel, VoiceNotificationServerModel, VoiceNotificationServerUpdateModel } from 'src/common/models/app-model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { CommonDialogService } from '../../services/common-dialog.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';
import { getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

@Component({
    selector: 'app-voice-notification-server-crud-modal',
    templateUrl: './voice-notification-server-crud-modal.component.html',
    standalone: false
})
export class VoiceNotificationServerCrudModalComponent implements OnInit {

  @ViewChild('lvVoiceNotificationServerForm') public form!: NgForm;
  @Input() public voiceNotificationServerModel: VoiceNotificationServerModel = new VoiceNotificationServerModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | undefined;
  @Input() public layoutTypeId: number | undefined;
  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: ((res: VoiceNotificationServerModel | null) => void) | null = null;

  public voiceNotificationServerCode__size: number = VoiceNotificationServerModel.voiceNotificationServerCode__size;
  public voiceNotificationServerName__size: number = VoiceNotificationServerModel.voiceNotificationServerName__size;
  public twilioAccountSid__size: number = VoiceNotificationServerModel.twilioAccountSid__size;
  public twilioAccountAuthToken__size: number = VoiceNotificationServerModel.twilioAccountAuthToken__size;
  public twilioAccountPhoneNumber__size: number = VoiceNotificationServerModel.twilioAccountPhoneNumber__size;

  public predefinedSmsAndVoiceServerTypes: Status[] = [];
  public selectedPredefinedSmsAndVoiceServerType: Status | undefined;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public customSelectService: CustomSelectService,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedSmsAndVoiceServerTypes = this.selectService.GetPredefinedSmsAndVoiceServerTypes();
    if (!this.isCreateMode) this.selectedPredefinedSmsAndVoiceServerType = this.predefinedSmsAndVoiceServerTypes.find(item => item.id === this.voiceNotificationServerModel.predefinedSmsAndVoiceServerTypeId);
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const voiceNotificationServerModel = this.generateModel(f.value);
    if (voiceNotificationServerModel) {
      if (this.isCreateMode) {
        this.createVoiceNotificationServer(voiceNotificationServerModel, f);
      } else {
        this.updateVoiceNotificationServer(voiceNotificationServerModel, f);
      }
    }
  }

  public generateModel(obj: VoiceNotificationServerModel): VoiceNotificationServerModel | undefined {
    if (this.selectedPredefinedSmsAndVoiceServerType == null)
      return;
    Object.assign(this.voiceNotificationServerModel, obj);
    Object.keys(this.voiceNotificationServerModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.voiceNotificationServerModel[key] && typeof this.voiceNotificationServerModel[key] === 'object' && !(this.voiceNotificationServerModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.voiceNotificationServerModel[key] = null;
      }
    });

    this.voiceNotificationServerModel.predefinedSmsAndVoiceServerTypeId = this.selectedPredefinedSmsAndVoiceServerType?.id ?? 0;
    return this.voiceNotificationServerModel;
  }

  public createVoiceNotificationServer(voiceNotificationServerModel: VoiceNotificationServerCreateModel, f: NgForm): void {
    this.httpService.createVoiceNotificationServer(voiceNotificationServerModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Voice Notification Server', result);
          return;
        }
        this.dialogService.success('Successfully Created the Voice Notification Server.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Voice Notification Server'),
      complete: () => null,
    });
  }

  public updateVoiceNotificationServer(voiceNotificationServerModel: VoiceNotificationServerUpdateModel, f: NgForm): void {
    this.httpService.updateVoiceNotificationServer(voiceNotificationServerModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Voice Notification Server', result);
          return;
        }
        this.dialogService.success('Successfully updated the Voice Notification Server.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(null);
      },
      error: (err) => this.dialogService.error('Not Able to update Voice Notification Server'),
      complete: () => null,
    });
  }

  public cancel(checkDirty: boolean): void {
    if (this.form.dirty && checkDirty) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'Discard').then(res => {
        if (res)
          return this.activeModal.close();
      });
    } else {
      this.activeModal.close();
    }
  }

}
