import { Component, Input, OnInit } from '@angular/core';

import { MenuItem } from 'src/common/models/app-custom-model';
import { SUPER_ADMIN_CONFIGURATION_MENU } from 'src/app/shared/components/menu/super-admin-menu';

@Component({
    selector: 'app-super-admin-layout-vertical',
    templateUrl: './super-admin-layout-vertical.component.html',
    styleUrls: ['./super-admin-layout-vertical.component.scss'],
    standalone: false
})
export class SuperAdminLayoutVerticalComponent implements OnInit {
  @Input() public appMenuItems: MenuItem[] = [];

  public ngOnInit(): void {
    this.appMenuItems = SUPER_ADMIN_CONFIGURATION_MENU;
  }
}
