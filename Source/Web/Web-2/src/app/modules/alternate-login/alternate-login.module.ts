import { AlternateLoginComponent } from './alternate-login/alternate-login.component';
import { AlternateLoginRoutingModule } from './alternate-login-routing.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [AlternateLoginComponent],
  imports: [
    CommonModule,
    AlternateLoginRoutingModule,
    FormsModule,
    NgbModalModule,
    SharedModule
  ]
})
export class AlternateLoginModule { }
