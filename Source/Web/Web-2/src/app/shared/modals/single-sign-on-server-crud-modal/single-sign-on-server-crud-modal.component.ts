import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { LoggedInUserModel, SingleSignOnServerCreateModel, SingleSignOnServerModel, SingleSignOnServerUpdateModel } from 'src/common/models/app-model';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';
import { getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

@Component({
    selector: 'app-single-sign-on-server-crud-modal',
    templateUrl: './single-sign-on-server-crud-modal.component.html',
    standalone: false
})
export class SingleSignOnServerCrudModalComponent implements OnInit {

  @ViewChild('lvSingleSignOnServerForm') public form!: NgForm;
  @Input() public singleSignOnServerModel: SingleSignOnServerModel = new SingleSignOnServerModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isSuperAdmin = false;
  @Input() public companyId: number = 0;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: ((res: SingleSignOnServerModel | null) => void) | null = null;

  public singleSignOnServerName__size: number = SingleSignOnServerModel.singleSignOnServerName__size;
  public singleSignOnServerCode__size: number = SingleSignOnServerModel.singleSignOnServerCode__size;
  public host__size: number = SingleSignOnServerModel.host__size;
  public clientSecret__size: number = SingleSignOnServerModel.clientSecret__size;
  public predefinedSingleSignOnServerTypes: Status[] = [];
  public selectedPredefinedSingleSignOnServerType: Status | undefined;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedSingleSignOnServerTypes = this.selectService.GetPredefinedSingleSignOnServerTypes();
    if (!this.isCreateMode) this.selectedPredefinedSingleSignOnServerType = this.predefinedSingleSignOnServerTypes?.find(item => item.id === this.singleSignOnServerModel.predefinedSingleSignOnServerTypeId);
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const singleSignOnServerModel = this.generateModel(f.value);
    if (singleSignOnServerModel) {
      if (this.isCreateMode) {
        this.createSingleSignOnServer(singleSignOnServerModel, f);
      } else {
        this.updateSingleSignOnServer(singleSignOnServerModel, f);
      }
    }
  }

  public generateModel(obj: SingleSignOnServerModel): SingleSignOnServerModel | undefined {
    if (this.selectedPredefinedSingleSignOnServerType == null)
      return;
    Object.assign(this.singleSignOnServerModel, obj);
    Object.keys(this.singleSignOnServerModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.singleSignOnServerModel[key] && typeof this.singleSignOnServerModel[key] === 'object' && !(this.singleSignOnServerModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.singleSignOnServerModel[key] = null;
      }
    });

    this.singleSignOnServerModel.predefinedSingleSignOnServerTypeId = this.selectedPredefinedSingleSignOnServerType?.id ?? 0;
    return this.singleSignOnServerModel;
  }

  public createSingleSignOnServer(singleSignOnServerModel: SingleSignOnServerCreateModel, f: NgForm): void {
    this.httpService.createSingleSignOnServer(singleSignOnServerModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Single Sign-On Server', result);
          return;
        }
        this.dialogService.success('Successfully Created the Single Sign-On Server.');
        f.resetForm();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
        this.activeModal.close();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create Single Sign-On Server'),
      complete: () => null,
    });
  }

  public updateSingleSignOnServer(singleSignOnServerModel: SingleSignOnServerUpdateModel, f: NgForm): void {
    this.httpService.updateSingleSignOnServer(singleSignOnServerModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Single Sign-On Server', result);
          return;
        }
        this.dialogService.success('Successfully updated the Single Sign-On Server.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(null);
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update Single Sign-On Server'),
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
