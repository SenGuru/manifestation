import { RouterModule, Routes } from "@angular/router";

import { CompanyListScreenComponent } from "src/app/shared/components/screen/company-list-screen/company-list-screen.component";
import { NavGuard } from "src/common/guard/nav.guard";
import { NgModule } from "@angular/core";
import { ProfileComponent } from "src/app/shared/profile/profile.component";
import { SuperAdminLayoutVerticalComponent } from "../../super-admin/container/super-admin-layout-vertical/super-admin-layout-vertical.component";
import { UserListScreenComponent } from "src/app/shared/components/screen/user-list-screen/user-list-screen.component";

const routes: Routes = [
  {
    path: "",
    component: SuperAdminLayoutVerticalComponent,
    children: [
      { path: "home", component: CompanyListScreenComponent },
      { path: 'profile', component: ProfileComponent },
      { path: "users", component: UserListScreenComponent },
      { path: 'companies', component: CompanyListScreenComponent },
      { path: '**', redirectTo: 'home' },
    ],
    canDeactivate: [NavGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HostAdminRoutingModule { }
