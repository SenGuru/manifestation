import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { SsoCallbackScreenComponent } from 'src/app/shared/components/screen/sso-callback-screen/sso-callback-screen.component';

const routes: Routes = [{
  path: '',
  component: HomeComponent
},
{ path: 'sso-callback', component: SsoCallbackScreenComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
