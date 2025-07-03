import { ChatMessageModel, LoggedInUserModel } from "src/common/models/app-model";
import { Component, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";

import { CommonDialogService } from "src/app/shared/services/common-dialog.service";
import { RealtimeClientService } from "src/app/shared/services/realtime-client.service";
import { SessionService } from "src/common/framework/web/infrastructure/session/session.service";

@UntilDestroy()
@Component({
    selector: 'app-admin-layout',
    templateUrl: './admin-layout.component.html',
    styleUrls: ['./admin-layout.component.scss'],
    standalone: false
})

export class AdminLayoutComponent implements OnInit {

  public chatMessages: ChatMessageModel[] = [];
  public currentUser: LoggedInUserModel | null = null;

  public userId: number = 0;
  
  public constructor(
    private realtimeClientService: RealtimeClientService, 
    public sessionService: SessionService,
    private dialogService: CommonDialogService,
  ) { }

  public ngOnInit(): void {
    this.currentUser = this.sessionService?.getCurrentUserInfo();
    this.userId = this.currentUser?.userId ?? 0;
    this.realtimeClientService.initializeSignalr();
    this.listenSignalrMessage();
  }

  public listenSignalrMessage(): void {
    this.realtimeClientService.receiveMessage().pipe(untilDestroyed(this)).subscribe({
      next: (message: ChatMessageModel) => {
        this.chatMessages.push(message);
        if (message.userId !== this.userId) this.dialogService.successWithLargeToastr(message?.chatMessageText ?? '');
      },
      error: (err: unknown) => {
        this.dialogService.error("Unable to get realtime messages");
      },
      complete: () => null,
    });
  }

}
