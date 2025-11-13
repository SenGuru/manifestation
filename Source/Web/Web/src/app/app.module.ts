import { CommonModule, DecimalPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClient, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgbActiveModal, NgbCollapseModule, NgbDropdownModule, NgbModalModule, NgbModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';

import { AlternateLoginModule } from './modules/alternate-login/alternate-login.module';
import { AppComponent } from './app.component';
import { AppDialogService } from './shared/services/app-dialog.service';
import { AppRoutingModule } from './app-routing.module';
import { BlockUIModule } from 'ng-block-ui';
import { BlockUiComponent } from './shared/components/block-ui/block-ui.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CommonDialogService } from './shared/services/common-dialog.service';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { ConfigService } from 'src/common/framework/common/infrastructure/config/config.service';
import { CrudService } from './shared/services/crud.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { CustomToasterComponent } from 'src/common/custom-toaster/custom-toaster.component';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { FlatpickrDefaults } from 'angularx-flatpickr';
import { ForgotPasswordModule } from './modules/forgot-password/forgot-password.module';
import { HomeModule } from './modules/home/home.module';
import { HttpConfigInterceptor } from 'src/common/interceptors/httpconfig.interceptor';
import { LanguageService } from 'src/common/services/language/language.service';
import { LayoutsModule } from './layouts/layouts.module';
import { LoginModule } from './modules/login/login.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NavigationService } from 'src/common/services/navigation/navigation.service';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NgIdleKeepaliveModule } from "@ng-idle/keepalive";
import { NgModule } from '@angular/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { PickerComponent } from '@ctrl/ngx-emoji-mart';
import { RealtimeClientService } from './shared/services/realtime-client.service';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { SharedModule } from './shared/shared.module';
import { SuperAdminModule } from './modules/super-admin/super-admin.module';
import { TimeConversionService } from 'src/common/services/time-conversion/time-conversion.service';
import { TitleService } from './shared/services/title.service';
import { ToastrModule } from 'ngx-toastr';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

export function createTranslateLoader(http: HttpClient): unknown {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AppRoutingModule,
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    LayoutsModule,
    FormsModule,
    NgbModule,
    NgbCollapseModule,
    ReactiveFormsModule,
    NgSelectModule,
    HomeModule,
    LoginModule,
    SuperAdminModule,
    PickerComponent,
    AlternateLoginModule,
    ForgotPasswordModule,
    NgbModalModule,
    CommonModule,
    NgbDropdownModule,
    SharedModule,
    BrowserModule,
    NgApexchartsModule,
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
    BrowserAnimationsModule,
  ],
  providers: [
    ConfigService,
    SessionService,
    LanguageService,
    EnumViewService,
    CommonDialogService,
    CrudService,
    NgbActiveModal,
    DecimalPipe,
    TitleService,
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpConfigInterceptor,
      multi: true
    },
    FlatpickrDefaults,
    CustomSelectService,
    TimeConversionService,
    CompanySessionService,
    NavigationService,
    RealtimeClientService,
    RouterDataService,
    AppDialogService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
