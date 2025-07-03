import { Component, Input, OnInit, ViewChild } from '@angular/core';

import { ChangePasswordModel } from 'src/common/models/app-model';
import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { KeyValueString } from 'src/common/models/app-custom-model';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.scss'],
    standalone: false
})
export class ChangePasswordComponent implements OnInit {

  @ViewChild('changePasswordForm') public form!: NgForm;
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  public changePasswordModel: ChangePasswordModel = new ChangePasswordModel();
  public errors: KeyValueString[] = [];

  public constructor(private activeModal: NgbActiveModal,
    private httpService: HttpService,
    private sessionService: SessionService,
    private dialogService: CommonDialogService) {

  }

  public ngOnInit(): void {
    this.changePasswordModel = new ChangePasswordModel();
    this.changePasswordModel.currentPassword = '';
    this.errors = [];
  }

  public onClickSubmit(f: NgForm): void {
    if (f.invalid) return;
    if (this.changePasswordModel) {
      this.httpService.changeCurrentPassword(this.changePasswordModel).subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.dialogService.errorWithReturnModel('Not Able to update password', result);
            return;
          }
          this.dialogService.success('Successfully password the updated.')?.then(res => {
            this.sessionService.logOut();
          }).catch(() => this.sessionService.logOut());
          f.resetForm();

        },
        error: (err: unknown) => this.dialogService.error('Not Able to update password'),
        complete: () => null,
      });
    }
  }

  public cancel(checkDirty: boolean): void {
    if (this.form.dirty && checkDirty) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'cancel').then(res => {
        if (res) {
          return this.activeModal.close();
        }
      });
    } else {
      this.activeModal.close();
    }
  }
}
