import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BusyHttpInterceptor } from './interceptors/http/busy.http.interceptor';
import { CommonModule } from '@angular/common';
import { FormFieldErrorDisplayComponent } from './components/ui/errors/form-field-error-display/form-field-error-display.component';
import { GlobalTimeoutComponent } from './components/base/global-timeout.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoadingSpinnerModule } from './components/ui/spinner/loading-spinner.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgModule } from '@angular/core';
import { NoContentComponent } from './components/no-content/no-content.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@NgModule({
    declarations: [
      GlobalTimeoutComponent,
      NoContentComponent,
      FormFieldErrorDisplayComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        PaginationModule,
        LoadingSpinnerModule.forRoot(),
        ModalModule.forRoot(),
        ReactiveFormsModule],
    exports: [
        FormsModule,
        PaginationModule,
        LoadingSpinnerModule,
        ModalModule,
        ReactiveFormsModule],
    providers: [
      {
        provide: HTTP_INTERCEPTORS,
        useClass: BusyHttpInterceptor,
        multi: true,
    },
    ]
})
export class FrameworkWebModule {}
