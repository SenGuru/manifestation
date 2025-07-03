import { CompanyModel, CompanySearchCriteria, LoggedInUserModel, MessageTemplateCreateModel, MessageTemplateModel, MessageTemplateUpdateModel } from 'src/common/models/app-model';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MessageTemplateTabs, Status } from 'src/common/models/app-custom-model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { CommonDialogService } from '../../services/common-dialog.service';
import { CompanyCrudModalComponent } from '../company-crud-modal/company-crud-modal.component';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { ReturnModel } from 'src/common/framework/common/models/result/return.model';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Subject } from 'rxjs';

@Component({
    selector: 'app-message-template-crud-modal',
    templateUrl: './message-template-crud-modal.component.html',
    standalone: false
})
export class MessageTemplateCrudModalComponent implements OnInit {

  @ViewChild('lvMessageTemplateForm') public form!: NgForm;
  @Input() public messageTemplateModel: MessageTemplateModel = new MessageTemplateModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  public onCloseCallback: (() => void) | null = null;
  public predefinedMessageTemplateTypes: Status[] = [];
  public selectedPredefinedMessageTemplateType: Status | undefined;
  public messageTemplateTypeId: number = 0;
  public currentUser: LoggedInUserModel | null = null;
  public showEmail: boolean = true;
  public showSMS: boolean = false;
  public showNotification: boolean = false;

  public emailSubject__size: number = MessageTemplateModel.emailSubject__size;
  public emailBody__size: number = MessageTemplateModel.emailBody__size;
  public smsBody__size: number = MessageTemplateModel.smsBody__size;
  public appNotificationSubject__size: number = MessageTemplateModel.appNotificationSubject__size;
  public appNotificationBody__size: number = MessageTemplateModel.appNotificationBody__size;
  public webNotificationSubject__size: number = MessageTemplateModel.webNotificationSubject__size;
  public webNotificationBody__size: number = MessageTemplateModel.webNotificationBody__size;
  // additionalRecipientEmail__size : number = MessageTemplateModel.additionalRecipientEmail__size;
  // additionalRecipientPhone__size : number = MessageTemplateModel.additionalRecipientPhone__size;
  public additionalRecipientEmail__size: number = 224;
  public additionalRecipientPhone__size: number = 224;
  public additionalRecipientPhone: number | undefined;
  public additionalRecipientEmail: number | undefined;
  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading: boolean = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;
  public companySearchModel: CompanySearchCriteria = new CompanySearchCriteria();

  public constructor(
    private httpService: HttpService,
    private selectService: SelectService,
    private dialogService: CommonDialogService,
    private modalService: NgbModal,
    public customSelectService: CustomSelectService,
    private activeModal: NgbActiveModal,
    private sessionService: SessionService
  ) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedMessageTemplateTypes = this.selectService.GetPredefinedMessageTemplateTypes();
    this.selectedPredefinedMessageTemplateType = this.predefinedMessageTemplateTypes.find(i => i.id === this.messageTemplateModel.predefinedMessageTemplateTypeId);
    if (this.isCreateMode) {
      this.messageTemplateModel.sendEmailForExternalUser = true;
      this.messageTemplateModel.sendEmailForInternalUser = true;
    }
    this.getCompanies(this.companySearchModel);
  }

  public showAddCompanyModal(): void {
    const modalRef = this.modalService.open(CompanyCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as CompanyCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Company';
    componentInstance.onCloseCallback = (res: CompanyModel | null): void => {
      if (ObjectUtils.isObjNullOrUndefined(res)) return;
      this.getCompanies(this.companySearchModel, res?.companyId);
    }
  }

  public getCompanies(searchModel: CompanySearchCriteria, selectedCompanyId?: number): void {
    searchModel.sortAscending = false;
    searchModel.isActive = true;
    searchModel.sortBy = 'createdDate';
    searchModel.isActive = true;
    this.httpService.getCompanies(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.companys = data.model;
          if (!this.isCreateMode) this.selectedCompany = data.model.find(item => item.companyId === this.messageTemplateModel.companyId);
          if (selectedCompanyId) {
            this.selectedCompany = this.companys.find(item => item.companyId == selectedCompanyId);
          }
        }
      },
      error: (error) => {
        this.dialogService.error('Not Able to get Company');
      },
      complete: () => null,
    });
  }

  public changeTab(tab: MessageTemplateTabs): void {
    this.showEmail = false;
    this.showSMS = false;
    this.showNotification = false;
    if (tab === 'email') this.showEmail = true;
    else if (tab === 'sms') this.showSMS = true;
    else if (tab === 'notification') this.showNotification = true;
  }

  public completeActionSuccess(message: string): void {
    this.dialogService.success(message);
  }

  public completeActionFailure(message: string): void {
    this.dialogService.error(message);
  }

  // need to use for delete from modal
  public deleteMessageTemplate(messageTemplateId: number): void {
    if (messageTemplateId == null || messageTemplateId == undefined) {
      return;
    }
    this.dialogService.confirmWithTitle('Delete the message template?', 'If you proceed, you cannot undo later.').then(res => {
      if (!res) {
        return;
      }
      this.httpService.markMessageTemplateAsDeleted(messageTemplateId)
        .subscribe({
          next: result => {
            if (!result.isSuccess) {
              this.dialogService.errorWithReturnModel('Unable to delete message template', result);
              return;
            }
            this.completeActionSuccess('Successfully deleted the message template');
          },
          error: err => {
            this.completeActionFailure('Unable to delete the message template');
          },
          complete: () => null
        });
    });
  }

  public cancel(confirmationDialog: boolean): void {
    if (confirmationDialog) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'Discard').then(res => {
        if (!res)
          return;
        this.goBack();
      });
    } else {
      this.goBack();
    }
  }

  public goBack(): void {
    this.activeModal.close();
  }

  public onSubmit(f: NgForm): Promise<unknown> | undefined | null {
    if (!f.form.valid) {
      return;
    }
    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    this.generateModel(f.value);
    if (this.messageTemplateModel.sendSmsForInternalUser || this.messageTemplateModel.sendSmsForExternalUser) {
      this.changeTab("sms");
      if (ObjectUtils.isNullOrUndefined(this.messageTemplateModel.smsBody) || this.messageTemplateModel.smsBody.trim() == "") return this.dialogService.error("Sms Body is required");
    }
    if (this.messageTemplateModel.sendAppNotificationForExternalUser || this.messageTemplateModel.sendAppNotificationForInternalUser) {
      this.changeTab("notification");
      if (ObjectUtils.isNullOrUndefined(this.messageTemplateModel.appNotificationBody) || this.messageTemplateModel.appNotificationBody.trim() == "") return this.dialogService.error("App Notification Body is required");
      if (ObjectUtils.isNullOrUndefined(this.messageTemplateModel.appNotificationSubject) || this.messageTemplateModel.appNotificationSubject.trim() == "") return this.dialogService.error("App Notification Subject is required");
      if (ObjectUtils.isNullOrUndefined(this.messageTemplateModel.webNotificationBody) || this.messageTemplateModel.webNotificationBody.trim() == "") return this.dialogService.error("Web Notification Body is required");
      if (ObjectUtils.isNullOrUndefined(this.messageTemplateModel.webNotificationSubject) || this.messageTemplateModel.webNotificationSubject.trim() == "") return this.dialogService.error("Web Notification Subject is required");
    }

    if (this.messageTemplateModel.sendEmailForExternalUser || this.messageTemplateModel.sendEmailForInternalUser) {
      this.changeTab("email");
      if (ObjectUtils.isNullOrUndefined(this.messageTemplateModel.emailSubject) || this.messageTemplateModel.emailSubject.trim() == "") return this.dialogService.error("Email Subject is required");
      if (ObjectUtils.isNullOrUndefined(this.messageTemplateModel.emailBody) || this.messageTemplateModel.emailBody.trim() == "") return this.dialogService.error("Email Body is required");
    }
    if (this.isCreateMode) {
      this.createMessageTemplate(this.messageTemplateModel);
    }
    else if (this.isEditMode) {
      this.updateMessageTemplate(this.messageTemplateModel);
    }
    return;
  }

  public generateModel(obj: MessageTemplateCreateModel): void {

    Object.assign(this.messageTemplateModel, obj);
    Object.keys(this.messageTemplateModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.messageTemplateModel[key] && typeof this.messageTemplateModel[key] === 'object' && !(this.messageTemplateModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.messageTemplateModel[key] = null;
      }
    });
    this.messageTemplateModel.predefinedMessageTemplateTypeId = this.selectedPredefinedMessageTemplateType?.id ?? 0;
    this.messageTemplateModel.companyId = this.selectedCompany ? this.selectedCompany.companyId : null;
  }

  public createMessageTemplate(messageTemplateModel: MessageTemplateModel): void {
    this.httpService.createMessageTemplate(messageTemplateModel as unknown as MessageTemplateCreateModel).subscribe({
      next: result => {
        if (ReturnModel.isSuccess(result)) {
          this.dialogService.success('Successfully created the message template');
          if (this.onCloseCallback) this.onCloseCallback();
          this.goBack();
        } else {
          this.dialogService.errorWithReturnModel('Unable to edit message template. Please contact support for help.', result);
        }
      },
      error: err => {
        this.dialogService.error(
          'Unable to edit message template. Please contact support for help.'
        );
      },
      complete: () => null
    });
  }

  public updateMessageTemplate(messageTemplateModel: MessageTemplateModel): void {

    this.httpService.updateMessageTemplate(messageTemplateModel as unknown as MessageTemplateUpdateModel).subscribe({
      next: result => {
        if (ReturnModel.isSuccess(result)) {
          this.dialogService.success('Successfully updated the message template');
          if (this.onCloseCallback) this.onCloseCallback();
          this.goBack();
        } else {
          this.dialogService.errorWithReturnModel('Unable to edit message template. Please contact support for help.', result);
        }
      },
      error: err => {
        this.dialogService.error(
          'Unable to edit message template. Please contact support for help.'
        );
      },
      complete: () => null
    });
  }

  public close(checkDirty: boolean): void {
    if (this.form.dirty && checkDirty) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'Discard').then(res => {
        if (!res)
          return;
        this.activeModal.close();
      });
    }
    else {
      this.activeModal.close();
    }
  }

}