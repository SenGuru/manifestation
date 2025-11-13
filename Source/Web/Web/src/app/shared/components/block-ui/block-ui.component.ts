import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-block-ui',
    templateUrl: './block-ui.component.html',
    styleUrls: ['./block-ui.component.scss'],
    standalone: false
})
export class BlockUiComponent {

  @BlockUI() public blockUI: NgBlockUI | undefined;
  @Input() public message = 'PLEASE WAIT';

  public constructor() {
    // 
  }
}
