import { AuditOperationDetailModel, AuditOperationModel, LoggedInUserModel } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-audit-operation-crud-modal',
    templateUrl: './audit-operation-crud-modal.component.html',
    standalone: false
})
export class AuditOperationCrudModalComponent implements OnInit {

  @Input() public auditOperationModel: AuditOperationModel = new AuditOperationModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isSuperAdmin = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;

  public ipAddress__size: number = AuditOperationModel.ipAddress__size;
  public description__size: number = AuditOperationModel.description__size;
  public auditOperationDetailModel: AuditOperationDetailModel = new AuditOperationDetailModel();

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.getAuditOperationDetail(this.auditOperationModel.auditOperationId);
  }

  public getAuditOperationDetail(auditOperationId: number): void {
    this.httpService.getAuditOperationDetailByAuditOperationId(auditOperationId).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.auditOperationDetailModel = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Audit Operation Detail');
      },
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

  }

  public generateModel(obj: AuditOperationModel): AuditOperationModel {
    Object.assign(this.auditOperationModel, obj);
    Object.keys(this.auditOperationModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.auditOperationModel[key] && typeof this.auditOperationModel[key] === 'object' && !(this.auditOperationModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.auditOperationModel[key] = null;
      }
    });
    return this.auditOperationModel;
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
