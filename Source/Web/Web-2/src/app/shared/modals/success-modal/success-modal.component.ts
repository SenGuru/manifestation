import { Component, Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-success-modal',
    templateUrl: './success-modal.component.html',
    styleUrls: ['./success-modal.component.scss'],
    standalone: false
})
export class SuccessModalComponent {
  @Input() public message: string | undefined;
  @Input() public errorList: string[] = [];
  @Input() public title: string | undefined;
  public constructor(private activeModal: NgbActiveModal) { }

  public close(): void {
    this.errorList = [];
    this.activeModal.dismiss();
  }

}
