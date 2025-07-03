import { AfterViewInit, Component, OnDestroy, Renderer2, TemplateRef, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { DEFAULT_INTERRUPTSOURCES, Idle } from '@ng-idle/core';

import { CommonDialogService } from './shared/services/common-dialog.service';
import { Keepalive } from '@ng-idle/keepalive';
import { RealtimeClientService } from './shared/services/realtime-client.service';
import { Router } from '@angular/router';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent implements AfterViewInit, OnDestroy {
  
  @ViewChild('childModal', { static: false }) public childModal!: TemplateRef<ModalDirective>;
  public title = 'Astrolyzer';
  public idleState = 'Not started.';
  public timedOut = false;
  public lastPing: Date | undefined;

  public modalRef: BsModalRef | undefined;

  public constructor(private idle: Idle, private keepalive: Keepalive,
    private realtimeClientService: RealtimeClientService,
    private router: Router, private modalService: BsModalService, private sessionService: SessionService, private renderer: Renderer2,
    private dialogService: CommonDialogService) {
    // sets an idle timeout of 5 seconds, for testing purposes.
    idle.setIdle(30 * 60);
    // sets a timeout period of 5 seconds. after 10 seconds of inactivity, the user will be considered timed out. 
    idle.setTimeout(15);
    // sets the default interrupts, in this case, things like clicks, scrolls, touches to the document
    idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);

    idle.onIdleEnd.subscribe((): void => {
      this.idleState = 'No longer idle.'
      this.reset();
    });

    idle.onTimeout.subscribe((): void => {
      this.idleState = 'Timed out!';
      this.timedOut = true;
      this.modalRef?.hide();
      this.dialogService.errorWithTitle("Session Timeout", "Your session has expired due to inactivity. Please log back to continue.");
      this.sessionService.logOut();
    });

    idle.onIdleStart.subscribe((): void => {
      this.idleState = 'Your session is in Idle mode!'
      this.modalRef = this.modalService.show(this.childModal, {
        animated: true,
        keyboard: false,
        backdrop: true,
        ignoreBackdropClick: true
      })
    });

    idle.onTimeoutWarning.subscribe((countdown) => {
      this.idleState = 'Your session will expire automatically in ' + countdown + ' seconds!. Select "Continue Session" to extend your session.';
    });

    idle.onInterrupt.subscribe((): void => {
      if (this.modalRef)
        this.modalRef.hide();
    })

    // sets the ping interval to 15 seconds
    keepalive.interval(5);
    keepalive.onPing.subscribe(() => this.lastPing = new Date());
    this.sessionService.getUserLoggedIn().subscribe({
      next: userLoggedIn => {
        if (userLoggedIn || this.sessionService.isUserLoggedIn()) {
          idle.watch()
          this.timedOut = false;
        } else {
          idle.stop();
        }
      },
      error: (err: unknown) => {
        // eslint-disable-next-line no-console
        console.error(err);
      },
      complete: () => null,
    })
  }

  public topFunction(): void {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
  }

  public ngAfterViewInit(): void {
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    const loader = this.renderer.selectRootElement('#preloader');
    this.renderer.setStyle(loader, 'display', 'none');
    if (!this.sessionService.getCurrentUserInfo()) {
      this.router.navigateByUrl('/login');
    }
  }

  public ngOnDestroy(): void {
    this.realtimeClientService.stopConnection();
  }

  public reset(): void {
    this.idle.watch();
    this.timedOut = false;
  }

  public hideChildModal(): void {
    this.modalRef?.hide();
  }

  public stay(): void {
    this.modalRef?.hide();
    this.reset();
  }

  public logout(): void {
    this.modalRef?.hide();
    this.sessionService.logOut();
  }

}
