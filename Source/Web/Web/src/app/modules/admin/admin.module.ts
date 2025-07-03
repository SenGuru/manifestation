import { AdminLayoutComponent } from './container/admin-layout/admin-layout.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LayoutsModule } from 'src/app/layouts/layouts.module';
import { NgModule } from '@angular/core';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from 'src/app/shared/shared.module';
import { SuperAdminRoutingModule } from './admin-routing.module';

@NgModule({
  declarations: [
    AdminLayoutComponent,
  ],
  imports: [
    CommonModule,
    SuperAdminRoutingModule,
    FormsModule,
    NgbModalModule,
    LayoutsModule,
    SharedModule
  ],
  exports:[
    AdminLayoutComponent,
  ]
})
export class AdminModule { }
