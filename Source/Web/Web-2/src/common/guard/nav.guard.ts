import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { ComponentCommunicationService } from '../services/communication/component-communication.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NavGuard {

  public IsEditable: boolean = false;
  public statusMessage: string = '';
  public screenType: number | null = null;
  public constructor(private componentCommunicationService: ComponentCommunicationService,
    private dialogService: CommonDialogService,) { }
  public canDeactivate(
    component: unknown, // todo
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): unknown {

    this.componentCommunicationService.statusMessage.subscribe({
      next: value => this.statusMessage = value,
      // eslint-disable-next-line no-console
      error: (err) => console.error(err),
      complete: () => null,
    });

    this.componentCommunicationService.screenType.subscribe(
      {
        next: (value: number | null): void => {
          this.screenType = value
        },
        // eslint-disable-next-line no-console
        error: (err) => console.error(err),
        complete: () => null,
      });

    return this.showMessageBox();
  }

  public showMessageBox(): boolean | Promise<unknown> {
    if (this.IsEditable) {
      return this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'proceed');
    } else {
      return true;
    }
  }

  public resetStatus(): void {
    this.IsEditable = false;
    this.statusMessage = '';
    this.screenType = null;
    this.componentCommunicationService.changeStatus(false);
  }

}
