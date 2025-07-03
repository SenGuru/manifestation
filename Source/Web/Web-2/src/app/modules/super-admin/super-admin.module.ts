import { NgbDropdownModule, NgbModalModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SuperAdminCompanyContanierComponent } from './container/super-admin-company-contanier/super-admin-company-contanier.component';
import { SuperAdminConfigurationContainerComponent } from './container/super-admin-configuration-container/super-admin-configuration-container.component';
import { SuperAdminLayoutVerticalComponent } from './container/super-admin-layout-vertical/super-admin-layout-vertical.component';
import { SuperAdminRoutingModule } from './super-admin-routing.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    SuperAdminLayoutVerticalComponent,
    SuperAdminCompanyContanierComponent,
    SuperAdminConfigurationContainerComponent,
  ],
  imports: [
    CommonModule,
    SuperAdminRoutingModule,
    FormsModule,
    NgbModalModule,
    NgbNavModule,
    NgbDropdownModule,
    SharedModule,
    TranslateModule,
  ]
})
export class SuperAdminModule { }
