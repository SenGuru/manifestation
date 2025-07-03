import { ActiveToast, IndividualConfig, ToastrService } from 'ngx-toastr';

import { ConfirmDialogueComponent } from '../modals/confirm-dialogue/confirm-dialogue.component';
import { DeleteModalComponent } from '../modals/delete-modal/delete-modal.component';
import { ErrorModalComponent } from '../modals/error-modal/error-modal.component';
import { HttpErrorResponse } from '@angular/common/http';
import { IReturnModel } from 'src/common/framework/common/models/result/i-return.model';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { ReturnErrorItem } from 'src/common/framework/common/models/result/return-error-item.model';
import { ReturnModelHelper } from 'src/common/framework/common/models/result/return.model.helper';
import { SuccessModalComponent } from '../modals/success-modal/success-modal.component';

@Injectable({
  providedIn: 'root'
})
export class CommonDialogService {

  public constructor(private modalService: NgbModal,
    private toastr: ToastrService) { }

  public errorWithTitle(title: string, message: string, errorList?: ReturnErrorItem[], model?: IReturnModel): Promise<unknown> | null {
    if (ObjectUtils.isNullOrUndefined(message) === false || ObjectUtils.isNullOrUndefined(errorList) === false) {
      const modalRef = this.modalService.open(ErrorModalComponent, { centered: true, windowClass: 'modal-blur' });
      const componentInstance = modalRef.componentInstance as ErrorModalComponent;
      componentInstance.title = title;
      componentInstance.message = message;
      componentInstance.errorItems = errorList ?? [];
      componentInstance.showList = true;
      componentInstance.detailedModel = model ?? null;
      return modalRef.result;
    } else {
      return null;
    }
  }

  public error(message: string, errorList?: ReturnErrorItem[], model?: IReturnModel): Promise<unknown> | null {
    return this.errorWithTitle('Error', message, errorList, model);
  }

  public errorWithReturnModel(defaultMessage: string, model: IReturnModel): Promise<unknown> | null {
    const holder = ReturnModelHelper.getErrorHolder(model, defaultMessage);
    return this.error(ReturnModelHelper.getErrorMessage(model, defaultMessage), holder ? holder.errorItemList : [], model);
  }

  public errorWithDefaultTitleForModel(returnModel: IReturnModel, defaultMessage: string): Promise<unknown> | null {
    // tslint:disable-next-line: max-line-length
    return this.errorWithTitle('Error', ReturnModelHelper.getErrorMessage(returnModel, defaultMessage), ReturnModelHelper.getErrorHolderErrorItems(returnModel), returnModel);
  }

  public errorForModel(title: string, returnModel: IReturnModel, defaultMessage: string): Promise<unknown> | null {
    // tslint:disable-next-line: max-line-length
    return this.errorWithTitle(title, ReturnModelHelper.getErrorMessage(returnModel, defaultMessage), ReturnModelHelper.getErrorHolderErrorItems(returnModel), returnModel);
  }

  public errorForHttpError(title: string, httpError: HttpErrorResponse | null, defaultMessage: string): Promise<unknown> | null {
    if (!ObjectUtils.isNullOrUndefined(httpError) && httpError) {
      if (!ObjectUtils.isNullOrUndefined(httpError.error)) {
        // tslint:disable-next-line: max-line-length
        return this.errorWithTitle(title, httpError.message);
      }
    }
    return this.errorWithTitle(title, defaultMessage);
  }

  public successWithTitle(title: string, message: string, errorList?: string[] | null): Promise<unknown> {
    const modalRef = this.modalService.open(SuccessModalComponent, { centered: true, windowClass: 'modal-blur' });
    const componentInstance = modalRef.componentInstance as SuccessModalComponent;
    componentInstance.title = title;
    componentInstance.message = message;
    componentInstance.errorList = errorList ?? [];
    return modalRef.result;
  }

  public success(message: string): Promise<unknown> | null {
    return this.successWithTitle('Success', message, []);
  }

  public successInternal(message: string, isToastr?: boolean, customProperties?: Partial<IndividualConfig>): Promise<unknown> | null | ActiveToast<unknown> {
    if (!isToastr) {
      return this.successWithTitle('Success', message, []);
    }
    else {
      return this.toastr.success(message, '', customProperties);
    }
  }

  public successWithToastr(message: string, customProperties?: Partial<IndividualConfig>): Promise<unknown> | null | ActiveToast<unknown> {
    return this.successInternal(message, true, customProperties);
  }

  public successWithLargeToastr(message: string, positionClass = 'toastr-postion-large'): void {
    const customToastrClass = { positionClass: positionClass };
    this.successWithToastr(message, customToastrClass);
  }

  public errorWithToastr(message: string): ActiveToast<unknown> {
    return this.toastr.error(message);
  }

  public successWithReturnModel(defaultMessage: string, model: IReturnModel): Promise<unknown> {
    const holder = ReturnModelHelper.getErrorHolder(model, defaultMessage);
    return this.successWithTitle('Success', ReturnModelHelper.getErrorMessage(model, defaultMessage), holder ? holder.errorItemList?.map(item => `${item.key}: ${item.value}`) : null)
  }

  public delete(title: string, message: string, errorList?: string[]): Promise<unknown> {
    const modalRef = this.modalService.open(DeleteModalComponent, { centered: true, windowClass: 'modal-blur' });
    const componentInstance = modalRef.componentInstance as DeleteModalComponent;
    componentInstance.title = title;
    componentInstance.message = message;
    componentInstance.errorList = errorList ?? [];
    return modalRef.result;
  }

  public confirmWithTitle(title: string, message: string): Promise<unknown> {
    const modalRef = this.modalService.open(ConfirmDialogueComponent, { centered: true, windowClass: 'modal-blur', size: 'md' });
    const componentInstance = modalRef.componentInstance as ConfirmDialogueComponent;
    componentInstance.title = title;
    componentInstance.message = message;
    return modalRef.result;
  }

  public cancelConfirmWithTitle(title: string, message: string, type: string): Promise<unknown> {
    const modalRef = this.modalService.open(DeleteModalComponent, { centered: true });
    const componentInstance = modalRef.componentInstance as DeleteModalComponent;
    componentInstance.title = title;
    componentInstance.message = message;
    componentInstance.type = type;
    return modalRef.result;
  }

  public confirm(message: string): Promise<unknown> {
    return this.confirmWithTitle('Confirm', message);
  }

  public confirmWithSmall(title: string, message: string): Promise<unknown> {
    const modalRef = this.modalService.open(ConfirmDialogueComponent, { centered: true, windowClass: 'modal-blur', size: 'sm' });
    const componentInstance = modalRef.componentInstance as ConfirmDialogueComponent;
    componentInstance.title = title;
    componentInstance.message = message;
    return modalRef.result;
  }

}
