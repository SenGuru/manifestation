import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgbCarouselModule, NgbModalModule } from "@ng-bootstrap/ng-bootstrap";

import { CommonModule } from "@angular/common";
import { LoginComponent } from "./login/login.component";
import { LoginRoutingModule } from "./login-routing.module";
import { NgModule } from "@angular/core";
import { SharedModule } from "src/app/shared/shared.module";

@NgModule({
  declarations: [LoginComponent],
  imports: [
    CommonModule,
    LoginRoutingModule,
    FormsModule,
    NgbModalModule,
    NgbCarouselModule,
    ReactiveFormsModule,
    SharedModule,
  ]
})
export class LoginModule {}
