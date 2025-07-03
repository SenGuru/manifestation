import { Component, OnInit } from '@angular/core';

import { LAYOUT_THEME } from './layout';
import { LayoutService } from './service/layout.service';
import { RealtimeClientService } from '../shared/services/realtime-client.service';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss'],
    standalone: false
})

/**
 * Layout Component
 */
export class LayoutComponent implements OnInit {

  public constructor(
    private realtimeClientService: RealtimeClientService,
    private layoutService: LayoutService,
  ) { }

  public ngOnInit(): void {
    this.realtimeClientService.initializeSignalr();
    this.layoutService.setThemeLayout(LAYOUT_THEME.INTERACTIVE);
  }
}
