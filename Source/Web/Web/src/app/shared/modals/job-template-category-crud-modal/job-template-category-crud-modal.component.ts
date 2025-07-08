import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { JobTemplateCategoryCreateModel, JobTemplateCategoryModel, JobTemplateCategoryUpdateModel, LoggedInUserModel } from 'src/common/models/app-model';
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
    selector: 'app-job-template-category-crud-modal',
    templateUrl: './job-template-category-crud-modal.component.html',
    standalone: false
})
export class JobTemplateCategoryCrudModalComponent implements OnInit {

  @ViewChild('lvJobTemplateCategoryForm') public form!: NgForm;
  @Input() public jobTemplateCategoryModel: JobTemplateCategoryModel = new JobTemplateCategoryModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: ((res: JobTemplateCategoryModel | null) => void) | null = null;

  public name__size: number = JobTemplateCategoryModel.name__size;

 

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
    const jobTemplateCategoryModel = this.generateModel(f.value);
    if (jobTemplateCategoryModel) {
      if (this.isCreateMode) {
        this.createJobTemplateCategory(jobTemplateCategoryModel, f);
      } else {
        this.updateJobTemplateCategory(jobTemplateCategoryModel, f);
      }
    }
  }

  public generateModel(obj: JobTemplateCategoryModel): JobTemplateCategoryModel | undefined {
    Object.assign(this.jobTemplateCategoryModel, obj);
    Object.keys(this.jobTemplateCategoryModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.jobTemplateCategoryModel[key] && typeof this.jobTemplateCategoryModel[key] === 'object' && !(this.jobTemplateCategoryModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.jobTemplateCategoryModel[key] = null;
      }
    });

    return this.jobTemplateCategoryModel;
  }

  public createJobTemplateCategory(jobTemplateCategoryModel: JobTemplateCategoryCreateModel, f: NgForm): void {
    this.httpService.createJobTemplateCategory(jobTemplateCategoryModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Job Template Category', result);
          return;
        }
        this.dialogService.success('Successfully Created the Job Template Category.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create Job Template Category'),
      complete: () => null,
    });
  }

  public updateJobTemplateCategory(jobTemplateCategoryModel: JobTemplateCategoryUpdateModel, f: NgForm): void {
    this.httpService.updateJobTemplateCategory(jobTemplateCategoryModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Job Template Category', result);
          return;
        }
        this.dialogService.success('Successfully updated the Job Template Category.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update Job Template Category'),
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
