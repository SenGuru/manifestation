
import { Component, OnInit, ViewChild, ElementRef, Inject, DOCUMENT } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { CommonDialogService } from "src/app/shared/services/common-dialog.service";
import { TitleService } from "src/app/shared/services/title.service";
import { ReturnModel } from "src/common/framework/common/models/result/return.model";
import { ObjectUtils } from "src/common/framework/common/utils/object.utils";
import { SessionService } from "src/common/framework/web/infrastructure/session/session.service";
import { getHeaderWithBlockUI } from "src/common/interceptors/httpconfig.interceptor";
import { EnumViewService } from "src/common/models/app-enum-view";
import { PredefinedSingleSignOnServerTypes } from "src/common/models/app-enums";
import { LoginModel, LoginOptionModel, GlobalConfigurationModel, LoggedInUserModel } from "src/common/models/app-model";
import { HttpService } from "src/common/services/app-http-client";
import { CustomSelectService } from "src/common/services/custom-select-service";

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
  public currentUser: LoggedInUserModel = new LoggedInUserModel();
  public predefinedSingleSignOnServerType = PredefinedSingleSignOnServerTypes;
  public showNavigationArrows: boolean | undefined;

  public currentPage: number = 0;
  // public anonymousLogin: AnonymousLoginModel;
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
    private enumViewService: EnumViewService,
    private titleService: TitleService,
    public customSelectService: CustomSelectService,
  ) {
    this.titleService.pageTitle = "Login";
    this.isLoading = true;
  }

  public ngOnInit(): void {
    this.ssoCallbackUrl = `${this.document.location.protocol}//${this.document.location.host}/sso-callback`;
    // this.layoutService.setThemeLayout(LAYOUT_THEME.INTERACTIVE);
    // this.currentUser = this.sessionService.getCurrentUserInfo();
    this.credentials = new LoginModel();
    if (!ObjectUtils.isNullOrUndefined(this.currentUser)) {
      this.httpService.authorizedPing(getHeaderWithBlockUI()).subscribe({
        next: result => {
          if (result.isSuccess) {
            this.enumViewService.navigateToModule(this.currentUser.userTypeId);
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
    // this.route.data.pipe(untilDestroyed(this)).subscribe({
    //   next: (data) => {
    //     if (!ObjectUtils.isObjNullOrUndefined(data)) {
    //       // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    //       this.loginType = data.predefinedLoggedInUserTypeId;
    //       this.titleService.pageTitle = `${this.loginType == PredefinedLoggedInUserTypes.NormalUser ? "Admin" : "User"} Login`;
    //       if (this.loginType === PredefinedLoggedInUserTypes.AnonymousUser) {
    //         this.anonymousLogin = new AnonymousLoginModel();
    //         this.anonymousLogin.isEmail = true;
    //         this.anonymousLogin.authCode = null;
    //         this.disableResendButton = true;
    //         this.changeTab('email');
    //       }
    //     }
    //   }, error: () => {
    //     this.dialogService.error("Unable to get path information");
    //   },
    //   complete: () => null,
    // });

  }

  public loginServerWithGlAndMs(serverType: PredefinedSingleSignOnServerTypes): void {
    //   this.httpService.getSingleSignOnAuthenticationUrlByPredefinedSingleSignOnServerTypeId(serverType, this.ssoCallbackUrl, getHeaderWithBlockUI()).subscribe({
    //     next: result => {
    //       if (!result.isSuccess) {
    //         return this.dialogService.errorWithReturnModel(`Unable to Sign in with ${serverType === this.predefinedSingleSignOnServerType.Google ? 'Google' : 'MicroSoft'}`, result);
    //       }
    //       if (ObjectUtils.isNullOrUndefined(result.model) || ObjectUtils.isNullOrUndefined(result.model?.signInUrl)) {
    //         return this.dialogService.error(`Unable to Sign in with ${serverType === this.predefinedSingleSignOnServerType.Google ? 'Google' : 'MicroSoft'}.`);
    //       }
    //       window.open(result.model?.signInUrl, "_self");
    //     },
    //     error: err => {
    //       this.dialogService.error(`Unable to Sign in with ${serverType === this.predefinedSingleSignOnServerType.Google ? 'Google' : 'MicroSoft'}.`);
    //     },
    //     complete: () => null,
    //   });
  }

  public loadConfiguration(): void {
    const site = this.document.location.host;
    this.httpService.getSiteConfiguration(site, getHeaderWithBlockUI()).subscribe({
      next: result => {
        this.isLoading = false;
        if (!result.isSuccess || !result.model) return;
        this.siteDetails = result.model;
        this.siteDetails.loginOptionModel.useBuiltInAuthentication = true; // todo: need to remove - only for testing puporse 
        this.loginOption = this.siteDetails.loginOptionModel;
      },
      error: err => {
        this.dialogService.error("Unable to get site config deatils");
      },
      complete: () => this.isLoading = false,
    });
  }

  // onTypeUsername(): void {
  //   this.httpService.getLoginOption(this.credentials.username, this.ssoCallbackUrl, getHeaderWithBlockUI()).subscribe({
  //     next: result => {
  //       if (!result.isSuccess) return;
  //       if (!ObjectUtils.isObjNullOrUndefined(result.model)) {
  //         this.loginOption = result.model;
  //         this.siteDetails.loginOptionModel.useBuiltInAuthentication = result.model.useBuiltInAuthentication;
  //       }
  //     },
  //     error: err => {
  //       this.dialogService.error("Unable to get site config deatils");
  //     },
  //     complete: () => null,
  //   });
  // }

  // resendAnonymousUserAuth(): void {
  //   this.anonymousLogin.authCode = null;
  //   this.httpService.generateOtpForAnonymousUser(this.anonymousLogin, getHeaderWithBlockUI()).subscribe({
  //     next: result => {
  //       if (!result.isSuccess) {
  //         return this.dialogService.errorWithReturnModel('Unable to Generate Password', result);
  //       }

  //       if (ObjectUtils.isNullOrUndefined(result.model)) {
  //         return this.dialogService.error('Issue with login credentials. Please contact admin.');
  //       }
  //       this.currentPage = 1;
  //       this.dialogService.successWithLargeToastr("Successfully resent the passcode.");
  //     },
  //     error: err => {
  //       this.dialogService.error("Unable to get current user info");
  //     },
  //     complete: () => null,
  //   });
  // }

  // handleCountdownEvent(event: CountdownEvent): void {
  //   if (event.action == "done") {
  //     this.disableResendButton = false;
  //   }
  // }

  // public onSubmitUserLogin(f: NgForm): Promise<unknown> {
  //   if (f.invalid) {
  //     if (this.currentPage == 1) {
  //       this.isAuthCodeSubmitted = true;
  //     } else {
  //       return n;
  //     }
  //   };

  //   if (this.currentPage === 0) {
  //     this.anonymousLogin.authCode = null;
  //     this.isAuthCodeSubmitted = false;
  //     this.httpService.generateOtpForAnonymousUser(this.anonymousLogin, getHeaderWithBlockUI()).subscribe({
  //       next: result => {
  //         if (!result.isSuccess) {
  //           return this.dialogService.errorWithReturnModel('Unable to Generate Password', result);
  //         }

  //         if (ObjectUtils.isNullOrUndefined(result.model)) {
  //           return this.dialogService.error('Issue with login credentials. Please contact admin.');
  //         }
  //         this.currentPage = 1;
  //         this.dialogService.successWithLargeToastr("Successfully sent the passcode.");
  //       },
  //       error: err => {
  //         this.dialogService.error("Unable to get current user info");
  //       },
  //       complete: () => null,
  //     });
  //   } else if (this.currentPage == 1) {
  //     this.isAuthCodeSubmitted = true;
  //     this.httpService.anonymousLogin(this.anonymousLogin, getHeaderWithBlockUI()).subscribe({
  //       next: result => {
  //         if (!result.isSuccess) {
  //           return this.dialogService.errorWithReturnModel('Unable to login', result);
  //         }

  //         if (ObjectUtils.isNullOrUndefined(result.model)) {
  //           return this.dialogService.error('Issue with login credentials. Please contact admin.');
  //         }
  //         this.sessionService.setCurrentUserInfo(
  //           result.model.userName,
  //           result.model.token,
  //           result.model.firstName,
  //           result.model.lastName,
  //           result.model.userTypeId,
  //           result.model.userTypeName,
  //           result.model.userId,
  //           result.model.companyId,
  //           result.model.companyName,
  //           result.model.permissions,
  //           result.model.predefinedLoggedInUserTypeId,
  //           result.model.predefinedCurrencyTypeId,
  //           result.model.crewDetailsWhichHasMatchingPhoneOrEmailItems,
  //         );
  //         this.sessionService.setCurrentUser(result.model);
  //         this.sessionService.setUserLoggedIn(true);
  //         this.enumViewService.navigateToAnonymousUserModule(result.model.predefinedLoggedInUserTypeId);
  //       },
  //       error: err => {
  //         this.dialogService.error("Unable to get login");
  //       },
  //       complete: () => null,
  //     });
  //   }

  // }

  // public changeTab(tab: ExternalUserUserNameType): void {
  //   this.externalUserLoginType = tab;
  //   this.anonymousLogin.authValue = null;
  //   if (tab === 'email') {
  //     this.anonymousLogin.isEmail = true;
  //   } else if (tab === 'phone') {
  //     this.anonymousLogin.isEmail = false;
  //   }
  // }

  // public cancelAnonymousLogin(): void {
  //   this.currentPage = 0;
  //   this.anonymousLogin.authCode = null;
  // }

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
          if (ReturnModel.isSuccess(result) && result.model) {
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
              // result.model?.predefinedLoggedInUserTypeId ?? 0,
              // result.model?.predefinedCurrencyTypeId ?? 0,
              // result.model?.crewDetailsWhichHasMatchingPhoneOrEmailItems ?? '',
            );
            this.sessionService.setCurrentUser(result.model);
            this.sessionService.setUserLoggedIn(true);
            this.enumViewService.navigateToModule(result.model.userTypeId);
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
