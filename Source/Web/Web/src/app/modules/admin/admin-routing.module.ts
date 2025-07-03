import { RouterModule, Routes } from "@angular/router";

import { CompanyConfigScreenComponent } from "src/app/shared/components/screen/company-config-screen/company-config-screen.component";
import { LayoutComponent } from "src/app/layouts/layout.component";
import { NgModule } from "@angular/core";
import { ProfileComponent } from "src/app/shared/profile/profile.component";
import { UserListScreenComponent } from "src/app/shared/components/screen/user-list-screen/user-list-screen.component";

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: "company-config", component: CompanyConfigScreenComponent, data: { companyConfig: true } },
      { path: "users", component: UserListScreenComponent },
      { path: "profile", component: ProfileComponent },
      { path: '**', redirectTo: 'company-config' },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SuperAdminRoutingModule { }
