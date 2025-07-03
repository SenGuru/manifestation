import { RouterModule, Routes } from '@angular/router';

import { LogoutComponent } from './shared/components/logout/logout.component';
import { NgModule } from '@angular/core';
import { ResetPasswordComponent } from './shared/components/screen/reset-password/reset-password.component';

const routes: Routes = [
  { path: 'logout/:logoutType', component: LogoutComponent },
  { path: 'logout/:logoutType/:loginType', component: LogoutComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: '', loadChildren: () => import('./modules/home/home.module').then(m => m.HomeModule) },
  { path: 'login', loadChildren: () => import('./modules/login/login.module').then(m => m.LoginModule) },
  { path: 'forgot-password', loadChildren: () => import('./modules/forgot-password/forgot-password.module').then(m => m.ForgotPasswordModule) },
  { path: 'alternatelogin', loadChildren: () => import('./modules/alternate-login/alternate-login.module').then(m => m.AlternateLoginModule) },
  { path: 'sa', loadChildren: () => import('./modules/super-admin/super-admin.module').then(m => m.SuperAdminModule) },
  { path: 'host-admin', loadChildren: () => import('./modules/host/host-admin/host-admin.module').then(m => m.HostAdminModule) },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
