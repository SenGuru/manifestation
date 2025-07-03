import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { ResetPasswordRequestModel } from 'src/common/models/app-model';
import { TitleService } from 'src/app/shared/services/title.service';

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrl: './reset-password.component.scss',
    standalone: false
})
export class ResetPasswordComponent implements OnInit {
  public year: number = new Date().getFullYear();
  public showNavigationArrows: boolean | undefined;
  public newPassword: string = '';
  public confirmPassword: string = '';
  public token: string = '';
  public isResetPasswordSuccessfully: boolean = false;

  public constructor(
    private httpService: HttpService,
    private titleService: TitleService,
    private route: ActivatedRoute,
    private dialogService: CommonDialogService) {
    this.titleService.pageTitle = "Reset Password";
  }

  public ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
      this.token = params['token'] || '';
    });
    if (ObjectUtils.isNullOrUndefined(this.token) || this.token === '') {
      this.dialogService.error("You'r not authorized to access this page.");
    }
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid || !this.newPassword || !this.confirmPassword) return;

    if (this.newPassword !== this.confirmPassword) {
      this.dialogService.error("New password and confirm password do not match.");
      f.reset();
      return;
    }
    const resetPassword = new ResetPasswordRequestModel();
    resetPassword.newPassword = this.newPassword;
    resetPassword.confirmPassword = this.confirmPassword;
    resetPassword.token = this.token;
    this.httpService.userSetNewPassword(resetPassword).subscribe({
      next: result => {
        if (!result.isSuccess) {
          return this.dialogService.errorWithDefaultTitleForModel(result, 'Unable to reset password. Please contact support for help.');
        }
        this.isResetPasswordSuccessfully = true;
        this.dialogService.successWithLargeToastr('New password changed successfully.');
        return;
      },
      error: error => {
        this.dialogService.errorForHttpError(
          'Forgot Password',
          null,
          'Unable to reset password. Please contact support for help.'
        );
      },
      complete: () => null,
    });
  }

}
