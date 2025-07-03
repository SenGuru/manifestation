import { Component, OnInit } from '@angular/core';
import { RealtimeClientService } from 'src/app/shared/services/realtime-client.service';

@Component({
    selector: 'app-host-admin-layout',
    templateUrl: './host-admin-layout.component.html',
    styleUrls: ['./host-admin-layout.component.scss'],
    standalone: false
})
export class HostAdminLayoutComponent implements OnInit {

  constructor(private realtimeClientService: RealtimeClientService,) { }

  ngOnInit(): void {
    this.realtimeClientService.initializeSignalr();
  }

}
