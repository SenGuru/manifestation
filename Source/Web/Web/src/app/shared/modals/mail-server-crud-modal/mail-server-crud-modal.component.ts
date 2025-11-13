import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { LoggedInUserModel, MailServerCreateModel, MailServerModel, MailServerUpdateModel } from 'src/common/models/app-model';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

@Component({
    selector: 'app-mail-server-crud-modal',
    templateUrl: './mail-server-crud-modal.component.html',
    standalone: false
})
export class MailServerCrudModalComponent implements OnInit {

  @ViewChild('lvMailServerForm') public form!: NgForm;
  @Input() public mailServerModel: MailServerModel = new MailServerModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isSuperAdmin = false;
  @Input() public companyId: number = 0;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: ((res: MailServerModel | null) => void) | null = null;

  public phoneFormGroup!: FormGroup;
  public host__size: number = MailServerModel.host__size;
  public senderName__size: number = MailServerModel.senderName__size;
  public senderEmail__size: number = MailServerModel.senderEmail__size;
  public userName__size: number = MailServerModel.userName__size;
  public password__size: number = MailServerModel.password__size;
  public mailServerName__size: number = MailServerModel.mailServerName__size;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const mailServerModel = this.generateModel(f.value);
    if (this.isCreateMode) {
      this.createMailServer(mailServerModel, f);
    } else {
      this.updateMailServer(mailServerModel, f);
    }
  }

  public generateModel(obj: MailServerModel): MailServerModel {
    Object.assign(this.mailServerModel, obj);
    Object.keys(this.mailServerModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.mailServerModel[key] && typeof this.mailServerModel[key] === 'object' && !(this.mailServerModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.mailServerModel[key] = null;
      }
    });

    return this.mailServerModel;
  }

  public createMailServer(mailServerModel: MailServerCreateModel, f: NgForm): void {
    this.httpService.createMailServer(mailServerModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create MailServer', result);
          return;
        }
        this.dialogService.success('Successfully Created the MailServer.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create MailServer'),
      complete: () => null,
    });
  }

  public updateMailServer(mailServerModel: MailServerUpdateModel, f: NgForm): void {
    this.httpService.updateMailServer(mailServerModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update MailServer', result);
          return;
        }
        this.dialogService.success('Successfully updated the MailServer.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(null);
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update MailServer'),
      complete: () => null,
    });
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
