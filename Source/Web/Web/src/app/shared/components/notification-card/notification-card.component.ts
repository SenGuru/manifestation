import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { LoggedInUserModel, RealtimeMessageModel, RealtimeMessageSearchCriteria, } from 'src/common/models/app-model';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@Component({
    selector: 'app-notification-card',
    templateUrl: './notification-card.component.html',
    styleUrls: ['./notification-card.component.css'],
    standalone: false
})
export class NotificationCardComponent implements OnInit {

  @ViewChild('removenotification') public removenotification !: TemplateRef<unknown>;
  public toggleShow = false;
  public isNotificationLoading: boolean = false;
  public isMessageChecked: boolean = false;
  public messageState: boolean = false;
  public messages: unknown[] = [];
  public element: unknown;
  public allnotifications: RealtimeMessageModel[] = [];
  public mode: string | undefined;
  public initialUrl = '';
  public currentUser: LoggedInUserModel | null = null;
  public notifyId: unknown;
  public constructor(private modalService: NgbModal,
    private sessionService: SessionService,
    private httpService: HttpService,
    private dialogService: CommonDialogService,
  ) { }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.getNotShownRealtimeMessages();
  }

  public getNotShownRealtimeMessages(): void {
    this.isNotificationLoading = true;
    const searchModel: RealtimeMessageSearchCriteria = new RealtimeMessageSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'messageDateTime';
    searchModel.pageSize = 5;
    this.httpService.getRealtimeMessages(searchModel, getHeaderWithBlockUI()).subscribe({
      next: data => {
        this.isNotificationLoading = false;
        if (!data.isSuccess) return;

        this.allnotifications = data.model ?? [];
      },
      error: (err: unknown) => {
        this.dialogService.error('Unable to get realtime message');
      },
      complete: () => null
    });
  }

  /**
* Open modal
* @param content modal content
*/
  public publicopenModal(content: unknown): void {
    // this.submitted = false;
    this.modalService.open(content, { centered: true });
  }

  /**
  * Topbar Light-Dark Mode Change
  */
  public publicchangeMode(mode: string): void {
    this.mode = mode;
    switch (mode) {
      case 'light':
        document.documentElement.setAttribute('data-bs-theme', "light");
        break;
      case 'dark':
        document.documentElement.setAttribute('data-bs-theme', "dark");
        break;
      default:
        document.documentElement.setAttribute('data-bs-theme', "light");
        break;
    }
  }
}
