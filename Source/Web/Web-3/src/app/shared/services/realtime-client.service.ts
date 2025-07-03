import * as signalR from '@microsoft/signalr';

import { ChatMessageCreateModel, ChatMessageModel, ChatUserCreateModel, RealtimeMessageModel, UserModel } from 'src/common/models/app-model';

import { CommonDialogService } from './common-dialog.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RealtimeClientService {
  private hubConnection: signalR.HubConnection;

  public constructor(
    private dialogService: CommonDialogService,
    private sessionService: SessionService
  ) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(`${environment.baseUrl}realtime-messages`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => this.sessionService.getCurrentToken(),
        withCredentials: false // Return access token
      }).withAutomaticReconnect([0, 2000, 10000, 30000]).build();

  }

  public startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then((): void => {
          // eslint-disable-next-line no-console
          console.log('Connection established with SignalR hub');
          // observer.next();
          // observer.complete();
        })
        .catch((error: unknown) => {
          // eslint-disable-next-line no-console
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });

      this.hubConnection.onreconnected((connectionId: string | undefined) => {
        // eslint-disable-next-line no-console
        console.log('Connection Re-established with SignalR hub');
      });

      this.hubConnection.onreconnecting((error?: Error) => {
        // eslint-disable-next-line no-console
        console.error('Connection Failed due to ', error);
      });

      this.hubConnection.onclose((error?: Error) => {
        // eslint-disable-next-line no-console
        console.error('Connection Closed due to ', error);
        setTimeout(() => {
          this.startConnection()
        }, 2000);
      });

    });
  }

  public recieveNotificationMessage(): Observable<RealtimeMessageModel> {
    return new Observable<RealtimeMessageModel>((observer) => {
      this.hubConnection.on('RecieveNotificationMessage', (message: RealtimeMessageModel) => {
        observer.next(message);
      });
    });
  }

  public receiveMessage(): Observable<ChatMessageModel> {
    return new Observable<ChatMessageModel>((observer) => {
      this.hubConnection.on('RecieveMessageAsync', (message: ChatMessageModel) => {
        observer.next(message);
      });
    });
  }

  public userTyping(): Observable<UserModel> {
    return new Observable<UserModel>((observer) => {
      this.hubConnection.on('UserTypingAsync', (UserModel: UserModel) => {
        observer.next(UserModel);
      });
    });
  }

  public sendTypingIndicator(chatId: number): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection.invoke('SendTypingIndicatorAsync', chatId)
        .then((): void => {
          observer.next();
          observer.complete();
        })
        .catch((error: unknown) => {
          observer.error(error);
        });
    });
  }

  public sendMessage(message: ChatMessageCreateModel): void {
    this.hubConnection.invoke('SendMessageAsync', message);
  }

  public stopConnection(): void {
    this.hubConnection.stop();
  }

  public joinChat(model: ChatUserCreateModel): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection.invoke('JoinChatAsync', model)
        .then((): void => {
          observer.next();
          observer.complete();
        })
        .catch((error: unknown) => {
          observer.error(error);
        });
    });
  }

  public initializeSignalr(): void {
    this.startConnection().subscribe((): void => {
      this.recieveNotificationMessage().subscribe({
        next: (message: RealtimeMessageModel) => {
          // eslint-disable-next-line no-console
          console.log("Connection established with signalR Hub");
          this.dialogService.successWithLargeToastr(message?.messageContent ?? '');
        },
        error: (err: unknown) => {
          // eslint-disable-next-line no-console
          console.error("error", err);
        },
        complete: () => null,
      });
    });
  }

}
