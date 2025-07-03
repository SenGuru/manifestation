import { Component, Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-confirm-dialogue',
    templateUrl: './confirm-dialogue.component.html',
    styleUrls: ['./confirm-dialogue.component.scss'],
    standalone: false
})
export class ConfirmDialogueComponent {

  @Input() public message: string | undefined;
  @Input() public title: string | undefined;

  public constructor(private activeModal: NgbActiveModal) { }

  public close(): void {
    this.activeModal.close(true);
  }

  public dismiss(): void {
    this.activeModal.dismiss();
  }

}
