import { Component, Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-delete-modal',
    templateUrl: './delete-modal.component.html',
    styleUrls: ['./delete-modal.component.scss'],
    standalone: false
})
export class DeleteModalComponent {

  @Input() public message: string| undefined;
  @Input() public errorList: string[] = [];
  @Input() public title: string| undefined;
  @Input() public type: string| undefined;

 public constructor(private activeModal: NgbActiveModal) { }

public close(): void {
    this.errorList = [];
    this.activeModal.close(true);
  }

public dismiss(): void {
    this.errorList = [];
    this.activeModal.dismiss();
  }
}
