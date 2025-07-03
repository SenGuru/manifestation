import { Component, OnInit } from '@angular/core';

import { MenuItem } from 'src/common/models/app-custom-model';
import { SUPER_ADMIN_CONFIGURATION_MENU } from 'src/app/shared/components/menu/super-admin-menu';

@Component({
    selector: 'app-super-admin-configuration-container',
    templateUrl: './super-admin-configuration-container.component.html',
    styleUrl: './super-admin-configuration-container.component.scss',
    standalone: false
})
export class SuperAdminConfigurationContainerComponent implements OnInit {
  public appMenuItems: MenuItem[] = [];

  public constructor() {
    // 
  }

  public ngOnInit(): void {
    this.appMenuItems = JSON.parse(JSON.stringify(SUPER_ADMIN_CONFIGURATION_MENU)) as MenuItem[];
  }

}
