import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';

import { AlternateLoginModel } from 'src/common/models/app-model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { HttpErrorResponse } from '@angular/common/http';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { ReturnModel } from 'src/common/framework/common/models/result/return.model';
import { Router } from '@angular/router';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-alternate-login',
    templateUrl: './alternate-login.component.html',
    styleUrls: ['./alternate-login.component.scss'],
    standalone: false
})
export class AlternateLoginComponent implements OnInit, AfterViewInit {

  @ViewChild('loginInAsUsername') public _el!: ElementRef;
  public selected: number | undefined;
  public credentials: AlternateLoginModel;

  public constructor(
    private router: Router,
    private sessionService: SessionService,
    private httpService: HttpService,
    private commonDialogService: CommonDialogService
  ) {
    this.credentials = new AlternateLoginModel();
  }

  public ngAfterViewInit(): void {
    // eslint-disable-next-line
    this._el.nativeElement.focus();
  }

  public ngOnInit(): void {
    this.selected = 1;
  }

  public onClickSubmit(f: NgForm): void {
    // eslint-disable-next-line
    this.httpService.alternateLogin(f.value).subscribe({
      next: (result) => {
        if (ReturnModel.isSuccess(result) && result.model) {
          this.sessionService.setCurrentUserInfo(
            result.model.userName,
            result.model.token,
            result.model.firstName,
            result.model.lastName,
            result.model.userTypeId,
            result.model.userTypeName,
            result.model.userId, 
          );
          this.sessionService.setCurrentUser(result.model);
          this.selected = (result.model.userTypeId / 100);
          this.onSubmit();
          f.resetForm();
        } else {
          this.commonDialogService.errorWithReturnModel(
            'Unable to login. Please contact support for help.',
            result,
          );
        }
      },
      error: (err: unknown) => {
        this.commonDialogService.errorWithTitle(
          'Alternate login',
          'Unable to login. Please contact support for help.'
        );
      },
      complete: () => null,
    });
  }

  public onSubmit(): void {
    switch (this.selected) {
      case 2:
        this.router.navigate(['/ca/dashboard']);
        break;
      case 3:
        this.router.navigate(['/cm/dashboard']);
        break;
      case 6:
        this.router.navigate(['/cr/newupload']);
        break;
      case 5:
        this.router.navigate(['/vw/dashboard']);
        break;
      default:
        this.router.navigate(['/sa/dashboard']);
    }
  }

}
