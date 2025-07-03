import { CommonModule, DecimalPipe } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from "@angular/common/http";
import { NgbActiveModal, NgbCollapseModule, NgbDropdownModule, NgbModalModule, NgbModule, NgbNavModule } from "@ng-bootstrap/ng-bootstrap";
import { TranslateLoader, TranslateModule } from "@ngx-translate/core";

import { AlternateLoginModule } from "./modules/alternate-login/alternate-login.module";
import { AppComponent } from "./app.component";
import { AppDialogService } from "./shared/services/app-dialog.service";
import { AppRoutingModule } from "./app-routing.module";
import { BlockUIModule } from "ng-block-ui";
import { BlockUiComponent } from "./shared/components/block-ui/block-ui.component";
import { BrowserModule } from "@angular/platform-browser";
import { CommonDialogService } from "./shared/services/common-dialog.service";
import { CompanySessionService } from "src/common/services/session/company-session-service";
import { ConfigService } from "src/common/framework/common/infrastructure/config/config.service";
import { CrudService } from "./shared/services/crud.service";
import { CustomSelectService } from "src/common/services/custom-select-service";
import { CustomToasterComponent } from "src/common/custom-toaster/custom-toaster.component";
import { EnumViewService } from "src/common/models/app-enum-view";
import { FlatpickrDefaults } from "angularx-flatpickr";
import { ForgotPasswordModule } from "./modules/forgot-password/forgot-password.module";
import { HomeModule } from "./modules/home/home.module";
import { HostAdminModule } from "./modules/host/host-admin/host-admin.module";
import { HttpConfigInterceptor } from "src/common/interceptors/httpconfig.interceptor";
import { LoginModule } from "./modules/login/login.module";
import { ModalModule } from "ngx-bootstrap/modal";
import { NavigationService } from "src/common/services/navigation/navigation.service";
import { NgIdleKeepaliveModule } from "@ng-idle/keepalive";
import { NgModule } from "@angular/core";
import { NgSelectModule } from "@ng-select/ng-select";
import { RealtimeClientService } from "./shared/services/realtime-client.service";
import { SessionService } from "src/common/framework/web/infrastructure/session/session.service";
import { SharedModule } from "./shared/shared.module";
import { SuperAdminModule } from "./modules/super-admin/super-admin.module";
import { TimeConversionService } from "src/common/services/time-conversion/time-conversion.service";
import { TitleService } from "./shared/services/title.service";
import { ToastrModule } from "ngx-toastr";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";

export function createTranslateLoader(http: HttpClient): unknown {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [ 
    HttpClientModule,
    AppRoutingModule,
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    FormsModule,
    NgbModule,
    NgbCollapseModule,
    ReactiveFormsModule,
    NgSelectModule,
    HomeModule,
    LoginModule,
    SuperAdminModule,
    HostAdminModule,
    AlternateLoginModule,
    ForgotPasswordModule,
    NgbModalModule,
    CommonModule,
    NgbDropdownModule,
    SharedModule, 
    BrowserModule,
    BlockUIModule.forRoot({
      template: BlockUiComponent,
      delayStart: 3000, // Wait 3s before starting to block
     }),
    ToastrModule.forRoot({
      toastComponent: CustomToasterComponent,
      timeOut: 3000
    }
    ),
    NgIdleKeepaliveModule.forRoot(), 
    ModalModule.forRoot(), 
    NgbNavModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpConfigInterceptor,
      multi: true,
    },
    ConfigService,
    SessionService,
    EnumViewService,
    CommonDialogService,
    CrudService,
    NgbActiveModal,
    DecimalPipe,
    TitleService,
    FlatpickrDefaults,
    CustomSelectService,
    TimeConversionService,
    CompanySessionService,
    NavigationService,
    RealtimeClientService,
    AppDialogService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
