import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HostAdminLayoutComponent } from './container/host-admin-layout/host-admin-layout.component';
import { HostAdminRoutingModule } from './host-admin-routing.module';
import { NgModule } from '@angular/core';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    HostAdminLayoutComponent,
  ],
  imports: [
    CommonModule,
    HostAdminRoutingModule,
    FormsModule,
    NgbModalModule,
    SharedModule
  ],
  exports:[
    HostAdminLayoutComponent,
  ]
})
export class HostAdminModule { }
