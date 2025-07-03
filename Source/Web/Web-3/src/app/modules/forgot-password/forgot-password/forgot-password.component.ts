import { LAYOUT_MODE_TYPES, SIDEBAR_TYPE, TOPBAR_MODE_TYPES } from 'src/app/layouts/layout';
import { LoggedInUserModel, RecoverPasswordRequestModel } from 'src/common/models/app-model';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { Component } from '@angular/core';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { LayoutService } from 'src/app/layouts/service/layout.service';
import { NgForm } from '@angular/forms';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.scss'],
    standalone: false
})

export class ForgotPasswordComponent {

  // set the current year
  public year: number = new Date().getFullYear();
  public emailValue: string | undefined;
  public showNavigationArrows: boolean = false;
  public isMessageSendSuccessfully: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;

  public constructor(
    private sessionService: SessionService,
    private titleService: TitleService,
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private layoutService: LayoutService,
    private enumViewService: EnumViewService) {
    this.titleService.pageTitle = "Forgot Password";
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.layoutService.setThemeLayout(LAYOUT_MODE_TYPES.LIGHTMODE, SIDEBAR_TYPE.DARK, TOPBAR_MODE_TYPES.DARK);
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid || !this.emailValue) return;
    const recoverPassword = new RecoverPasswordRequestModel();
    recoverPassword.userName = this.emailValue;
    recoverPassword.clientRedirectURL = `${window.location.origin}/reset-password`;
    this.httpService.userRecoverForgotPassword(recoverPassword).subscribe({
      next: result => {
        if (!result.isSuccess) {
          return this.dialogService.errorWithDefaultTitleForModel(result, 'Unable to reset password. Please contact support for help.');
        }
        this.isMessageSendSuccessfully = true;
        this.dialogService.successWithLargeToastr('Please check your email for new password.');
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

  public navigateBackToView(): void {
    if (!ObjectUtils.isNullOrUndefined(this.currentUser)) {
      this.enumViewService.navigateToModule(this.currentUser?.userTypeId ?? 0);
    } else {
      this.enumViewService.navigateToModule(0);
    }
  }

  public showMessageSuccessfully(): string {
    if (this.isMessageSendSuccessfully) {
      return 'If an account with that email exists, we have sent a password reset link. Please check your email and follow the instructions to reset your password.';
    } else {
      return 'Enter your email and instructions will be sent to you!.';
    }
  }
}
