/**
 * Code adapted from https://github.com/amitmahida92/ng4-loading-spinner
 */

import { Component, Input, OnDestroy, ViewEncapsulation } from '@angular/core';

import { LoadingSpinnerService } from './loading-spinner.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls: ['./loading-spinner.component.scss'],
  encapsulation: ViewEncapsulation.None // Use the native Shadow DOM to encapsulate our CSS
})
export class LoadingSpinnerComponent implements OnDestroy {

  public subscription: Subscription | undefined;
  public showSpinner = false;

  private _template = `
  <div style="color: #64d6e2" class="la-ball-clip-rotate-multiple la-3x">
    <div>hello world</div>
    <div></div>
    <div></div>
  </div>`;

  private _loadingText = '';

  private _threshold = 500;

  private _zIndex = 9999;

  public constructor(
    private spinnerService: LoadingSpinnerService
  ) {
    this.createServiceSubscription();
  }

  public get loadingText(): string {
    return this._loadingText;
  }

  public get threshold(): number {
    return this._threshold;
  }

  public get zIndex(): number {
    return this._zIndex;
  }

  public get template(): string {
    return this._template;
  }
   
  @Input()
  public set loadingText(value: string) {
    this._loadingText = value;
  }
   
  @Input()
  public set template(value: string) {
    this._template = value;
  }
   
  @Input()
  public set zIndex(value: number) {
    this._zIndex = value;
  }
   
  @Input()
  public set threshold(value: number) {
    this._threshold = value;
  }

public ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

public createServiceSubscription(): void {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    let timer: any;

    this.subscription =
      this.spinnerService.getMessage().subscribe(show => {
        if (show > 0) {

          if (timer) {
            return;
          }
           
          timer = setTimeout(() => {
            timer = null;
             
            this.showSpinner = show > 0;
          }, this.threshold);

        } else {
          // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
          clearTimeout(timer);
          this.showSpinner = false;
        }
      });
  }

}
