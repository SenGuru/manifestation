import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';

import { ConfigService } from '../../../common/infrastructure/config/config.service';
import { DateUtils } from '../../../common/utils/date.utils';
import { ObjectUtils } from '../../../common/utils/object.utils';
import { interval } from 'rxjs';

@Component({ selector: 'app-global-timeout', template: '' })
export class GlobalTimeoutComponent implements OnInit, OnDestroy {
  // eslint-disable-next-line
  public onTimeout: any;
  private lastUserAction: Date = new Date();
  // eslint-disable-next-line
  private subscription: any;

  public constructor(private configService: ConfigService) {
  }

  @HostListener('document:keyup', ['$event'])
public onKeyUp(ev: KeyboardEvent): void {
    this.updateLastUserAction();
  }

  @HostListener('document:keydown', ['$event'])
public onKeyDown(ev: KeyboardEvent): void {
    this.updateLastUserAction();
  }

  @HostListener('document:click', ['$event'])
public onClick(ev: MouseEvent): void {
    this.updateLastUserAction();
  }

public updateLastUserAction(): void {
    this.lastUserAction = this.getCurrentDate();
  }

public ngOnInit(): void {
    this.subscription = interval(5000).subscribe(res => {
      if (this.canTimeout()) {
        this.updateLastUserAction();
        if (ObjectUtils.isNullOrUndefined(this.onTimeout)) {
          return;
        }
        // eslint-disable-next-line @typescript-eslint/no-unsafe-call
        this.onTimeout();
      }
    });

  }

  public canTimeout(): boolean {
    return DateUtils.timeDiffInSec(this.lastUserAction, this.getCurrentDate())
      > this.getConfigTimeoutValueInSecs();
  }

  public getCurrentDate(): Date {
    return new Date();
  }

  public getConfigTimeoutValueInSecs(): number {
    return this.configService.frameworkConfiguration.appTimeoutInSecs;
  }

public ngOnDestroy(): void {
    // eslint-disable-next-line
    this.subscription.unsubscribe();
  }

}
