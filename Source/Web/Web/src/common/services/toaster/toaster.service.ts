import * as Rx from 'rxjs';

import { Injectable } from '@angular/core';
import { MessageOption } from '../../models/app-custom-model';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})

export class ToasterService {

  public showLoader = new Rx.Subject();

  public constructor(private toastr: ToastrService) { }

public success(option: MessageOption): void {
    this.toastr.success(option.title, option.message);
  }

public warning(option: MessageOption): void {
    this.toastr.success(option.title, option.message);
  }

public info(option: MessageOption): void {
    this.toastr.success(option.title, option.message);
  }

public error(option: MessageOption): void {
    this.toastr.error(option.title, option.message);
  }

}
