import { ActivatedRoute, Router } from "@angular/router";
import { Component, ElementRef, Inject, OnInit, ViewChild, DOCUMENT } from "@angular/core";
import { GlobalConfigurationModel, LoggedInUserModel, LoginModel, LoginOptionModel } from "src/common/models/app-model";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";

import { CommonDialogService } from "src/app/shared/services/common-dialog.service";
import { CustomSelectService } from "src/common/services/custom-select-service";

import { EnumViewService } from "src/common/models/app-enum-view";
import { HttpService } from "src/common/services/app-http-client";
import { LAYOUT_THEME } from "src/app/layouts/layout";
import { LayoutService } from "src/app/layouts/service/layout.service";
import { NgForm } from "@angular/forms";
import { ObjectUtils } from "src/common/framework/common/utils/object.utils";
import { PredefinedSingleSignOnServerTypes } from "src/common/models/app-enums";
import { ReturnModel } from "src/common/framework/common/models/result/return.model";
import { SessionService } from "src/common/framework/web/infrastructure/session/session.service";
import { TitleService } from "src/app/shared/services/title.service";
import { getHeaderWithBlockUI } from "src/common/interceptors/httpconfig.interceptor";

@UntilDestroy()
@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.scss"],
    standalone: false
})
export class LoginComponent implements OnInit {
  @ViewChild('googleSignIn') public googleSignIn!: ElementRef;
  public fieldTextType!: boolean;
  public year: number = new Date().getFullYear();
  public credentials: LoginModel = new LoginModel();
  public access_token: string = '';
  // public loginType: PredefinedLoggedInUserTypes;
  // public loginTypeEnumValue: typeof PredefinedLoggedInUserTypes = PredefinedLoggedInUserTypes;
  public loginOption: LoginOptionModel = new LoginOptionModel();
  public siteDetails: GlobalConfigurationModel = new GlobalConfigurationModel();
  public ssoCallbackUrl: string = "";
  public currentUser: LoggedInUserModel | null = new LoggedInUserModel();
  public predefinedSingleSignOnServerType = PredefinedSingleSignOnServerTypes;
  public showNavigationArrows: boolean = false;

  public currentPage: number = 0;
  public isAuthCodeSubmitted: boolean = false;
  public disableResendButton: boolean = false;
  public isLoading: boolean = false;
  public timerValue: number = 0;
  // public externalUserLoginType: ExternalUserUserNameType;

  private accessToken = '';
  // eslint-disable-next-line @typescript-eslint/member-ordering

  public constructor(
    private sessionService: SessionService,
    private httpService: HttpService,
    @Inject(DOCUMENT) private document: Document,
    private dialogService: CommonDialogService,
    private route: ActivatedRoute,
    private router: Router,
    private enumViewService: EnumViewService,
    private titleService: TitleService,
    private layoutService: LayoutService,
    public customSelectService: CustomSelectService,
  ) {
    this.titleService.pageTitle = "Login";
    this.isLoading = true;
  }

  public ngOnInit(): void {
    this.ssoCallbackUrl = `${this.document.location.protocol}//${this.document.location.host}/sso-callback`;
    this.layoutService.setThemeLayout(LAYOUT_THEME.DEFAULT);
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.credentials = new LoginModel();
    if (!ObjectUtils.isNullOrUndefined(this.currentUser)) {
      this.httpService.authorizedPing(getHeaderWithBlockUI()).subscribe({
        next: result => {
          if (result.isSuccess) {
            this.enumViewService.navigateToModule(this.currentUser?.userTypeId ?? 0);
          }
        },
        error: err => {
          // eslint-disable-next-line no-console
          console.error(err);
        },
        complete: () => this.loadConfiguration(),
      });
    } else {
      this.loadConfiguration();
    }
    this.loadExternalLogin();

  }

  public loadConfiguration(): void {
    const site = this.document.location.host;
    this.httpService.getSiteConfiguration(site, getHeaderWithBlockUI()).subscribe({
      next: result => {
        this.isLoading = false;
        if (!result.isSuccess) return;
        if (result.model != null) {
          this.siteDetails = result.model;
          this.siteDetails.loginOptionModel.useBuiltInAuthentication = true; // todo: need to remove - only for testing puporse 
          this.loginOption = this.siteDetails.loginOptionModel;
        }
      },
      error: err => {
        this.dialogService.error("Unable to get site config deatils");
      },
      complete: () => this.isLoading = false,
    });
  }

  public onTypeUsername(): void {
    this.httpService.getLoginOption(this.credentials.username, this.ssoCallbackUrl, getHeaderWithBlockUI()).subscribe({
      next: result => {
        if (!result.isSuccess) return;
        if (result.model != null) {
          this.loginOption = result.model;
          this.siteDetails.loginOptionModel.useBuiltInAuthentication = result.model.useBuiltInAuthentication;
        }
      },
      error: err => {
        this.dialogService.error("Unable to get site config deatils");
      },
      complete: () => null,
    });
  }

  public loginServerWithGlAndMs(serverType: PredefinedSingleSignOnServerTypes): void {
    // this.httpService.getSingleSignOnAuthenticationUrlByPredefinedSingleSignOnServerTypeId(serverType, this.ssoCallbackUrl, getHeaderWithBlockUI()).subscribe({
    //   next: result => {
    //     if (!result.isSuccess) {
    //       return this.dialogService.errorWithReturnModel(`Unable to Sign in with ${serverType === this.predefinedSingleSignOnServerType.Google ? 'Google' : 'MicroSoft'}`, result);
    //     }
    //     if (ObjectUtils.isNullOrUndefined(result.model) || ObjectUtils.isNullOrUndefined(result.model?.signInUrl)) {
    //       return this.dialogService.error(`Unable to Sign in with ${serverType === this.predefinedSingleSignOnServerType.Google ? 'Google' : 'MicroSoft'}.`);
    //     }
    //     window.open(result.model?.signInUrl, "_self");
    //   },
    //   error: err => {
    //     this.dialogService.error(`Unable to Sign in with ${serverType === this.predefinedSingleSignOnServerType.Google ? 'Google' : 'MicroSoft'}.`);
    //   },
    //   complete: () => null,
    // });
  }

  public onSubmitUserLogin(f: NgForm): void {
    if (f.invalid) {
      if (this.currentPage == 1) {
        this.isAuthCodeSubmitted = true;
      } else {
        return;
      }
    };
  }

  public navigateToCreateAccountPage(): void {
    this.router.navigateByUrl('/create-new-account');
  }

  public onSubmitAdminLogin(f: NgForm): void {
    if (this.loginOption && !this.loginOption?.useBuiltInAuthentication) {
      if (ObjectUtils.isNullOrUndefined(this.loginOption.signInUrl) || this.loginOption.signInUrl == "") {
        this.dialogService.error("Authentication failed with SSO. Please contact support help.");
        return;
      }
      window.open(this.loginOption.signInUrl, "_self");
    }
    else {
      // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
      this.httpService.login(f.value, getHeaderWithBlockUI()).subscribe({
        next: (result) => {
          if (ReturnModel.isSuccess(result)) {
            this.sessionService.setCurrentUserInfo(
              result.model?.userName ?? '',
              result.model?.token ?? '',
              result.model?.firstName ?? '',
              result.model?.lastName ?? '',
              result.model?.userTypeId ?? 0,
              result.model?.userTypeName ?? '',
              result.model?.userId ?? 0,
              result.model?.companyId ?? 0,
              result.model?.companyName ?? '',
              result.model?.permissions ?? [],
            );
            if (result.model != null) {
              this.sessionService.setUserLoggedIn(true);
              this.enumViewService.navigateToModule(result.model.userTypeId);
            }
            f.resetForm();
          } else {
            this.dialogService.errorWithTitle(
              "Error",
              ReturnModel.getErrorMessage(result, "Unable to validate")
            );
          }
        },
        error: (error) => {
          this.dialogService.error("Unable to login. Please contact support for help."
          );
        },
        complete: () => null,
      });
    }
  }

  public externalLogin(providerName: string, e: Event): void {
    e.preventDefault();
    localStorage.setItem('provider', providerName);
  }

  public toggleFieldTextType(): void {
    this.fieldTextType = !this.fieldTextType;
  }

  private loadExternalLogin(): void {
    const providerType = localStorage.getItem('provider');
  }

}
