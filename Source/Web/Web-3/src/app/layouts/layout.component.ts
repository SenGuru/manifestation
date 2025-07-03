import { AfterViewInit, Component, OnInit } from '@angular/core';
import { LAYOUT_MODE_TYPES, SIDEBAR_TYPE, TOPBAR_MODE_TYPES } from './layout';

import { LayoutService } from './service/layout.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  standalone: false,
})

export class LayoutComponent implements OnInit {

  constructor(private layoutService: LayoutService,) {
  }

  ngOnInit() { 
    this.layoutService.setThemeLayout(LAYOUT_MODE_TYPES.LIGHTMODE, SIDEBAR_TYPE.DARK, TOPBAR_MODE_TYPES.DARK);
  } 

}
