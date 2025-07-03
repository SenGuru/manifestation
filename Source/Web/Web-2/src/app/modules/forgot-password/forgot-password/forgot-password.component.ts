import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { Component } from '@angular/core';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
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
  isMessageSendSuccessfully: boolean = false;
  
  public constructor(
    private httpService: HttpService,
    private titleService: TitleService,
    private commonDialogService: CommonDialogService) {
    this.titleService.pageTitle = "Forgot Password";
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid || !this.emailValue) return;
    // const recoverPassword = new RecoverPasswordRequestModel;
    // recoverPassword.userName = this.emailValue;
    // recoverPassword.clientRedirectURL = `${window.location.origin}/reset-password`;
    // this.httpService.userRecoverForgotPassword(recoverPassword).subscribe({
    //   next: result => {
    //     if (!result.isSuccess) {
    //       return this.commonDialogService.errorWithDefaultTitleForModel(result, 'Unable to reset password. Please contact support for help.');
    //     }
    //     this.isMessageSendSuccessfully = true;
    //     this.commonDialogService.successWithLargeToastr('Please check your email for new password.');
    //   },
    //   error: error => {
    //     this.commonDialogService.errorForHttpError(
    //       'Forgot Password',
    //       null,
    //       'Unable to reset password. Please contact support for help.'
    //     );
    //   },
    //   complete: () => null,
    // });
  }

  showMessageSuccessfully(): string {
    if (this.isMessageSendSuccessfully) {
      return 'If an account with that email exists, we have sent a password reset link. Please check your email and follow the instructions to reset your password.';
    } else {
      return 'Enter your email and instructions will be sent to you!.';
    }
  }
}
