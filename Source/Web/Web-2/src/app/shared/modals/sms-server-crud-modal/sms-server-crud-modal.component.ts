import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { LoggedInUserModel, SmsServerCreateModel, SmsServerModel, SmsServerUpdateModel } from 'src/common/models/app-model';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';

@Component({
    selector: 'app-sms-server-crud-modal',
    templateUrl: './sms-server-crud-modal.component.html',
    standalone: false
})
export class SmsServerCrudModalComponent implements OnInit {

  @ViewChild('lvSmsServerForm') public form!: NgForm;
  @Input() public smsServerModel: SmsServerModel = new SmsServerModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public companyId: number = 0;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: ((res: SmsServerModel | null) => void) | null = null;
  public predefinedSmsAndVoiceServerTypes: Status[] = [];
  public selectedPredefinedSmsAndVoiceServerType: Status | undefined;
  public host__size: number = SmsServerModel.host__size;
  public senderName__size: number = SmsServerModel.senderName__size;
  public senderEmail__size: number = SmsServerModel.senderEmail__size;
  public userName__size: number = SmsServerModel.userName__size;
  public password__size: number = SmsServerModel.password__size;
  public smsServerName__size: number = SmsServerModel.smsServerName__size;
  public twilioAccountPhoneNumber__size: number = SmsServerModel.twilioAccountPhoneNumber__size;
  public twilioAccountSid__size: number = SmsServerModel.twilioAccountSid__size;
  public twilioAccountAuthToken__size: number = SmsServerModel.twilioAccountAuthToken__size;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedSmsAndVoiceServerTypes = this.selectService.GetPredefinedSmsAndVoiceServerTypes();
    if (!this.isCreateMode) this.selectedPredefinedSmsAndVoiceServerType = this.predefinedSmsAndVoiceServerTypes.find(item => item.id == this.smsServerModel.predefinedSmsAndVoiceServerTypeId);
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const smsServerModel = this.generateModel(f.value);
    // if (this.isCreateMode) {
    //   this.createSmsServer(smsServerModel, f);
    // } else {
    //   this.updateSmsServer(smsServerModel, f);
    // }
  }

  public generateModel(obj: SmsServerModel): SmsServerModel {
    Object.assign(this.smsServerModel, obj);
    Object.keys(this.smsServerModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.smsServerModel[key] && typeof this.smsServerModel[key] === 'object' && !(this.smsServerModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.smsServerModel[key] = null;
      }
    });
    this.smsServerModel.predefinedSmsAndVoiceServerTypeId = this.selectedPredefinedSmsAndVoiceServerType?.id ?? 0;
    return this.smsServerModel;
  }
 
  public cancel(checkDirty: boolean): void {
    if (this.form.dirty && checkDirty) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'Discard').then(res => {
        if (res)
          return this.activeModal.close();
      });
    }
    else {
      this.activeModal.close();
    }
  }
}
