import { ActivatedRoute, Router } from '@angular/router';
import { Component, Inject, OnInit, DOCUMENT } from '@angular/core';
import { ExternalLoginCallBackModel, LoggedInUserModel } from 'src/common/models/app-model';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { ConvertUtils } from 'src/common/framework/common/utils/convert.utils';

import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';

@UntilDestroy()
@Component({
    selector: 'app-sso-callback-screen',
    templateUrl: './sso-callback-screen.component.html',
    styleUrls: ['./sso-callback-screen.component.css'],
    standalone: false
})
export class SsoCallbackScreenComponent implements OnInit {

  public provider = "";
  public callBackUri = "";
  public singleSignOnIdentifier = "";
  public code = "";
  public id_token = "";
  public access_token = "";
  public error = "";
  public state = "";
  public currentPageUrl = "";
  public loggedInUserModel: LoggedInUserModel | undefined;
  public constructor(@Inject(DOCUMENT) private document: Document,
    private route: ActivatedRoute,
    private sessionService: SessionService,
    private router: Router,
    private enumViewService: EnumViewService,
    private dialogService: CommonDialogService,
    private titleService: TitleService,
    private httpService: HttpService) {

    this.route.queryParams.pipe(untilDestroyed(this)).subscribe({
      next: (params) => {
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        this.provider = params['provider'];
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        this.callBackUri = params['callBackUri'];
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        this.singleSignOnIdentifier = params['tenantSingleSignOnIdentifier'];
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        this.code = params['code'];
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        this.id_token = params['id_token'];
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        this.access_token = params['access_token'];
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        this.state = params['state'];
      },
      error: (err: unknown) => {
        this.dialogService.error("Unable to login");
      },
      complete: () => null
    });
    this.titleService.pageTitle = 'Single Sign-On';
  }

  public ngOnInit(): void {
    this.currentPageUrl = `${this.document.location.protocol}//${this.document.location.host}/sso-callback`;
    this.externalLoginCallBack();
  }

  public externalLoginCallBack(): void {
    const externalLoginCallBackModel = new ExternalLoginCallBackModel();
    externalLoginCallBackModel.id_token = this.id_token;
    externalLoginCallBackModel.access_token = this.access_token;
    externalLoginCallBackModel.callBackUri = this.currentPageUrl;
    externalLoginCallBackModel.code = this.code;
    externalLoginCallBackModel.error = this.error;
    externalLoginCallBackModel.state = this.state;
    externalLoginCallBackModel.provider = this.provider;
    externalLoginCallBackModel.singleSignOnIdentifier = this.singleSignOnIdentifier;

    this.httpService.externalLoginCallBack(externalLoginCallBackModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.router.navigate(['/login']);
          return this.dialogService.errorWithReturnModel("unable to get login info", result);
        }
        if (result.model) {
          this.loggedInUserModel = result.model;
          if (result.isSuccess) {
            const previousSelectedRole = ConvertUtils.toNumber(localStorage.getItem("selectedRole"));
            if (previousSelectedRole) {
              result.model.userTypeId = previousSelectedRole;
              if (result.model && ObjectUtils.isNullOrUndefined(result.model?.userTypeId)) return this.dialogService.error('User is not configured to view any apps.');
              this.sessionService.setCurrentUserInfo(
                result.model.userName,
                result.model.token,
                result.model.firstName,
                result.model.lastName,
                result.model.userTypeId,
                result.model.userTypeName,
                result.model.userId,
                result.model.companyId,
                result.model.companyName,
                result.model.permissions ?? []);
              this.sessionService.setUserLoggedIn(true);
              this.enumViewService.navigateToModule(result.model.userTypeId);
            }
          } else {
            this.dialogService.errorWithReturnModel('Unable to validate', result);
          }
        }
        return;
      },
      error: (err: unknown) => {
        this.dialogService.error('Unable to External Login callback information');
      },
      complete: () => null
    });
  }

}
