import { RouterModule, Routes } from '@angular/router';
import { AlternateLoginComponent } from './alternate-login/alternate-login.component';
import { NgModule } from '@angular/core';

const routes: Routes = [{
  path:  '', component: AlternateLoginComponent,
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AlternateLoginRoutingModule { }
