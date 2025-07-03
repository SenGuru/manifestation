import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { LoggedInUserModel, WhatsAppNotificationServerCreateModel, WhatsAppNotificationServerModel, WhatsAppNotificationServerUpdateModel } from 'src/common/models/app-model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { CommonDialogService } from '../../services/common-dialog.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';

@Component({
    selector: 'app-whats-app-notification-server-crud-modal',
    templateUrl: './whats-app-notification-server-crud-modal.component.html',
    standalone: false
})
export class WhatsAppNotificationServerCrudModalComponent implements OnInit {

  @ViewChild('lvWhatsAppNotificationServerForm') public form!: NgForm;
  @Input() public whatsAppNotificationServerModel: WhatsAppNotificationServerModel = new WhatsAppNotificationServerModel();
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
  public onCloseCallback: ((res: WhatsAppNotificationServerModel | null) => void) | null = null;

  public whatsAppNotificationServerCode__size: number = WhatsAppNotificationServerModel.whatsAppNotificationServerCode__size;
  public whatsAppNotificationServerName__size: number = WhatsAppNotificationServerModel.whatsAppNotificationServerName__size;
  public apiKey__size: number = WhatsAppNotificationServerModel.apiKey__size;
  public apiSecret__size: number = WhatsAppNotificationServerModel.apiSecret__size;
  public senderPhoneNumber__size: number = WhatsAppNotificationServerModel.senderPhoneNumber__size;
  public businessAccountId__size: number = WhatsAppNotificationServerModel.businessAccountId__size;
  public phoneNumberId__size: number = WhatsAppNotificationServerModel.phoneNumberId__size;
  public accessToken__size: number = WhatsAppNotificationServerModel.accessToken__size;
  public versionNumber__size: number = WhatsAppNotificationServerModel.versionNumber__size;

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
    if (!this.isCreateMode) this.selectedPredefinedSmsAndVoiceServerType = this.predefinedSmsAndVoiceServerTypes.find(item => item.id === this.whatsAppNotificationServerModel.predefinedSmsAndVoiceServerTypeId);
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const whatsAppNotificationServerModel = this.generateModel(f.value);
    if (whatsAppNotificationServerModel) {
      // if (this.isCreateMode) {
      //   this.createWhatsAppNotificationServer(whatsAppNotificationServerModel, f);
      // } else {
      //   this.updateWhatsAppNotificationServer(whatsAppNotificationServerModel, f);
      // }
    }
  }

  public generateModel(obj: WhatsAppNotificationServerModel): WhatsAppNotificationServerModel | undefined {
    if (this.selectedPredefinedSmsAndVoiceServerType == null)
      return;
    Object.assign(this.whatsAppNotificationServerModel, obj);
    Object.keys(this.whatsAppNotificationServerModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.whatsAppNotificationServerModel[key] && typeof this.whatsAppNotificationServerModel[key] === 'object' && !(this.whatsAppNotificationServerModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.whatsAppNotificationServerModel[key] = null;
      }
    });

    this.whatsAppNotificationServerModel.predefinedSmsAndVoiceServerTypeId = this.selectedPredefinedSmsAndVoiceServerType?.id ?? 0;
    return this.whatsAppNotificationServerModel;
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
