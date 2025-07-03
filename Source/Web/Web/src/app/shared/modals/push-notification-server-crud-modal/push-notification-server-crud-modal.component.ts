import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { LoggedInUserModel, PushNotificationServerCreateModel, PushNotificationServerModel, PushNotificationServerUpdateModel } from 'src/common/models/app-model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';
import { getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

@Component({
    selector: 'app-push-notification-server-crud-modal',
    templateUrl: './push-notification-server-crud-modal.component.html',
    standalone: false
})
export class PushNotificationServerCrudModalComponent implements OnInit {

  @ViewChild('lvPushNotificationServerForm') public form!: NgForm;
  @Input() public pushNotificationServerModel: PushNotificationServerModel = new PushNotificationServerModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isSuperAdmin = false;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | undefined;
  @Input() public layoutTypeId: number | undefined;
  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: ((res: PushNotificationServerModel | null) => void) | null = null;

  public pushNotificationServerCode__size: number = PushNotificationServerModel.pushNotificationServerCode__size;
  public pushNotificationServerName__size: number = PushNotificationServerModel.pushNotificationServerName__size;
  public apiKey__size: number = PushNotificationServerModel.apiKey__size;
  public apiSecret__size: number = PushNotificationServerModel.apiSecret__size;
  public appleAuthKey__size: number = PushNotificationServerModel.appleAuthKey__size;
  public appleKeyId__size: number = PushNotificationServerModel.appleKeyId__size;
  public appleTeamId__size: number = PushNotificationServerModel.appleTeamId__size;
  public projectId__size: number = PushNotificationServerModel.projectId__size;
  public predefinedPushNotificationServerTypes: Status[] = [];
  public selectedPredefinedPushNotificationServerType: Status | undefined;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedPushNotificationServerTypes = this.selectService.GetPredefinedPushNotificationServerTypes();
    if (!this.isCreateMode) this.selectedPredefinedPushNotificationServerType = this.predefinedPushNotificationServerTypes.find(item => item.id === this.pushNotificationServerModel.predefinedPushNotificationServerTypeId);
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const pushNotificationServerModel = this.generateModel(f.value);
    if (pushNotificationServerModel) {
      if (this.isCreateMode) {
        this.createPushNotificationServer(pushNotificationServerModel, f);
      } else {
        this.updatePushNotificationServer(pushNotificationServerModel, f);
      }
    }
  }

  public generateModel(obj: PushNotificationServerModel): PushNotificationServerModel | undefined {
    if (this.selectedPredefinedPushNotificationServerType == null)
      return;
    Object.assign(this.pushNotificationServerModel, obj);
    Object.keys(this.pushNotificationServerModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.pushNotificationServerModel[key] && typeof this.pushNotificationServerModel[key] === 'object' && !(this.pushNotificationServerModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.pushNotificationServerModel[key] = null;
      }
    });

    this.pushNotificationServerModel.predefinedPushNotificationServerTypeId = this.selectedPredefinedPushNotificationServerType?.id ?? 0;
    return this.pushNotificationServerModel;
  }

  public createPushNotificationServer(pushNotificationServerModel: PushNotificationServerCreateModel, f: NgForm): void {
    this.httpService.createPushNotificationServer(pushNotificationServerModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Push Notification Server', result);
          return;
        }
        this.dialogService.success('Successfully Created the Push Notification Server.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create Push Notification Server'),
      complete: () => null,
    });
  }

  public updatePushNotificationServer(pushNotificationServerModel: PushNotificationServerUpdateModel, f: NgForm): void {
    this.httpService.updatePushNotificationServer(pushNotificationServerModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Push Notification Server', result);
          return;
        }
        this.dialogService.success('Successfully updated the Push Notification Server.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(null);
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update Push Notification Server'),
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
