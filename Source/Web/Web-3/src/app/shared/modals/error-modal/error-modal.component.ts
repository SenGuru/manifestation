import { Component, Input } from '@angular/core';

import { IReturnModel } from 'src/common/framework/common/models/result/i-return.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ReturnErrorItem } from 'src/common/framework/common/models/result/return-error-item.model';

@Component({
    selector: 'app-error-modal',
    templateUrl: './error-modal.component.html',
    styleUrls: ['./error-modal.component.scss'],
    standalone: false
})
export class ErrorModalComponent {

  @Input() public message: string | undefined;
  @Input() public errorItems: ReturnErrorItem[] = [];
  @Input() public title: string | undefined;
  @Input() public detailedModel: IReturnModel | null = null;
  @Input() public showList: boolean = false;
  public isShowMore = false;

  public constructor(private activeModal: NgbActiveModal) { }

public close(): void {
    this.errorItems = [];
    this.activeModal.dismiss();
  }

}
