import { Component, Input, OnInit } from '@angular/core';

import { MenuItem } from 'src/common/models/app-custom-model';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';

@Component({
    selector: 'app-app-sidebar',
    templateUrl: './app-sidebar.component.html',
    styleUrl: './app-sidebar.component.scss',
    standalone: false
})
export class AppSidebarComponent implements OnInit {

  @Input() public appMenuItems: MenuItem[] = [];

  public ngOnInit(): void {
    if (ObjectUtils.isArrNullOrEmpty(this.appMenuItems)) {
      this.appMenuItems = [];
    }
  }
}
