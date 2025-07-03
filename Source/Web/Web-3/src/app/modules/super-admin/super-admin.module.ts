import { NgbCollapseModule, NgbDropdownModule, NgbModalModule, NgbModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LayoutsModule } from 'src/app/layouts/layouts.module';
import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SuperAdminLayoutComponent } from './container/super-admin-layout/super-admin-layout.component';
import { SuperAdminRoutingModule } from './super-admin-routing.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    SuperAdminLayoutComponent,
  ],
  imports: [
    CommonModule,
    SuperAdminRoutingModule,
    FormsModule,
    NgbModalModule,
    NgbModule,
    NgbCollapseModule,
    NgbNavModule,
    NgbDropdownModule,
    SharedModule,
    TranslateModule,
    LayoutsModule,
  ]
})
export class SuperAdminModule { }
