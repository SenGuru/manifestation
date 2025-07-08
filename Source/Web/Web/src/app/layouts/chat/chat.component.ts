/* eslint-disable @typescript-eslint/no-unsafe-enum-comparison */

import { AfterViewInit, Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ChatMessageCreateModel, ChatMessageModel, ChatMessageSearchCriteria, ChatModel, ChatSearchCriteria, ChatUpdateModel, ChatUserModel, ChatUserSearchCriteria, LoggedInUserModel, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
import { IAlbum, Lightbox } from 'ngx-lightbox';
import { NgForm, UntypedFormBuilder } from '@angular/forms';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';

import { ChatUserGroupCustomModel } from 'src/common/models/app-custom-model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { IStringIndexedObjModel } from 'src/common/framework/common/models/i-string-indexed-object,model';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { RealtimeClientService } from 'src/app/shared/services/realtime-client.service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { StringUtils } from 'src/common/framework/common/utils/string.utils';
import { TitleService } from 'src/app/shared/services/title.service';
import { UserCrudModalComponent } from 'src/app/shared/modals/user-crud-modal/user-crud-modal.component';

// Light Box
@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  standalone: false
})
export class ChatComponent implements OnInit, AfterViewInit {
  @ViewChild('scrollRef') public scrollRef!: ElementRef;
  public isStatus = 'online';
  public isProfile = 'assets/images/users/multi-user.jpg';
  public images: IAlbum[] = [];
  // { src: string | undefined; thumb: string | undefined; caption: string }[] = [];
  public isreplyMessage = false;
  public userMessage = '';
  public isLoading: boolean = false;
  public isUpdateGroupName: boolean = false;
  public chatGroupName__size: number = ChatModel.chatName__size;

  // Emoji Picker
  public showEmojiPicker = false;
  public sets: string[] = [
    'native',
    'google',
    'twitter',
    'facebook',
    'emojione',
    'apple',
    'messenger'
  ];
  public isSuperAdmin = false;
  public contacts: ChatUserGroupCustomModel[] = [];
  public currentUser: LoggedInUserModel | null = null;
  public userSearchCriteria: UserSearchCriteria = new UserSearchCriteria();
  public chatSearchCriteria: ChatSearchCriteria = new ChatSearchCriteria();
  public chatMessageSearchCriteria: ChatMessageSearchCriteria = new ChatMessageSearchCriteria();
  public chatUserSearchCriteria: ChatUserSearchCriteria = new ChatUserSearchCriteria();
  public chatMessages: ChatMessageModel[] = [];
  public chats: ChatModel[] = [];
  public selectedChat: ChatModel = new ChatModel();
  public chatGroupUsers: ChatUserModel[] = [];
  public companyId: number = 0;
  public companyName: string | undefined;
  public userId: number | undefined;
  public isAddGroupContact: boolean = false;
  public isProfileUpdateGroupName: boolean = false;

  public constructor(
    public formBuilder: UntypedFormBuilder,
    public sessionService: SessionService,
    private lightbox: Lightbox,
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private offcanvasService: NgbOffcanvas,
    private realtimeClientService: RealtimeClientService,
    private titleService: TitleService,
    private modalService: NgbModal,
  ) {
    this.titleService.pageTitle = "Messages";

    for (let i = 1; i <= 24; i++) {
      const src = '../../../../assets/images/small/img-' + i + '.jpg';
      const caption = 'Image ' + i + ' caption here';
      const thumb = '../../../../assets/images/small/img-' + i + '-thumb.jpg';
      const item = {
        src: src,
        caption: caption,
        thumb: thumb
      };
      this.images.push(item);
    }
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.userId = this.currentUser?.userId;
    if (this.currentUser && this.currentUser?.userTypeId && this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.isSuperAdmin = true;
    }
    this.getContacts(this.userSearchCriteria);
    this.getChats(this.chatSearchCriteria);
    this.onListScroll();
    this.listenSignalrMessage();
  }

  public listenSignalrMessage(): void {
    this.realtimeClientService.receiveMessage().subscribe({
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

  public showUpdateContactModal(item: UserModel): void {
    const modalRef = this.modalService.open(UserCrudModalComponent, { windowClass: 'my-class', size: 'xl', scrollable: true });
    const componentInstance = modalRef.componentInstance as UserCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Contact';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.userModel = JSON.parse(JSON.stringify(item)) as UserModel;
    componentInstance.onCloseCallback = (): void => {
      this.getContacts(this.userSearchCriteria);
    }
  }

  public getContacts(searchModel: UserSearchCriteria): void {
    searchModel.active = true;
    this.httpService.getUsers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Contacts", result);
          return;
        }
        const contactData = result.model?.filter(item => item.userId !== this.userId);
        if (!ObjectUtils.isArrNullOrEmpty<UserModel>(contactData ?? [])) this.contacts = this.getGroupedContacts(contactData ?? []);
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Contacts"),
      complete: () => null,
    });
  }

  public getChatMessages(searchModel: ChatMessageSearchCriteria): void {
    searchModel.sortBy = 'createdDate';
    this.httpService.getChatMessages(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Contacts", result);
          return;
        }
        this.chatMessages = result.model ?? [];
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Contacts"),
      complete: () => null,
    });
  }

  public getChatUsers(searchModel: ChatUserSearchCriteria): void {
    if (!this.selectedChat) return;
    searchModel.chatId = this.selectedChat.chatId;
    searchModel.sortBy = 'createdDate';
    this.httpService.getChatUsers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get chat users", result);
          return;
        }
        this.chatGroupUsers = result.model?.filter(item => item.userId !== this.userId) ?? [];
      },
      error: (err: unknown) => this.dialogService.error("Unable to get chat users"),
      complete: () => null,
    });
  }

  public getChats(searchModel: ChatSearchCriteria): void {
    this.isLoading = true;
    this.httpService.getChats(searchModel).subscribe({
      next: result => {
        this.isLoading = false;
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Contacts", result);
          return;
        }
        this.chats = result.model ?? [];
        if (!ObjectUtils.isArrNullOrEmpty(this.chats)) {
          this.selectedChat = this.chats[0] ?? new ChatModel();
          this.chatMessageSearchCriteria.chatId = this.selectedChat.chatId;
          this.getChatMessages(this.chatMessageSearchCriteria);
          this.getChatUsers(this.chatUserSearchCriteria);
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Contacts"),
      complete: () => this.isLoading = false,
    });
  }

  public changeChat(chat: ChatModel): void {
    if (ObjectUtils.isNullOrUndefined(chat)) return;
    this.selectedChat = chat;
    this.chatMessages = [];
    this.chatMessageSearchCriteria.chatId = chat.chatId;
    this.getChatMessages(this.chatMessageSearchCriteria);
    this.getChatUsers(this.chatUserSearchCriteria);
  }

  public messageSearch(): void {
    // 
  }

  public contactGroupSearch(): void {
    // 
  }

  public enableUpdateGroupName(data: ChatModel): void {
    this.selectedChat = data;
    this.isUpdateGroupName = true;
  }

  public enableProfileUpdateGroupName(): void {
    this.isProfileUpdateGroupName = true;
  }

  public updateGroupName(): void {
    const chatUpdateModel: ChatUpdateModel = new ChatUpdateModel();
    chatUpdateModel.chatName = this.selectedChat.chatName;
    chatUpdateModel.chatId = this.selectedChat.chatId;
    chatUpdateModel.existingChatUsers = this.chatGroupUsers;
    this.httpService.updateChat(chatUpdateModel).subscribe((result) => {
      if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to update the Chat Name");
      this.dialogService.successWithToastr("Successfully updated the Chat Name");
      this.getChats(this.chatSearchCriteria);
      this.isUpdateGroupName = false;
      this.isProfileUpdateGroupName = false;
      return;
    });
  }

  public deleteContact(item: ChatUserModel): void {
    this.dialogService.confirmWithTitle(`Delete Contact`, `Do you want to delete the Contact - ${item?.userFirstName} ${item?.userLastName}?`).then(res => {
      if (!res) return;
      this.httpService.markChatUserAsDeleted(item.chatUserId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Contact");
        this.dialogService.successWithToastr("Successfully deleted the Contact");
        this.getContacts(this.userSearchCriteria);
        this.getChatUsers(this.chatUserSearchCriteria);
        return;
      });
    });
  }

  public deleteChat(item: ChatModel | undefined): void {
    this.dialogService.confirmWithTitle(`Delete Chat`, `Do you want to delete the Chat - ${item?.chatName}?`).then(res => {
      if (!res) return;
      this.httpService.markChatAsDeleted(item?.chatId ?? 0).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Chat");
        this.dialogService.successWithToastr("Successfully deleted the Chat");
        this.getChats(this.chatSearchCriteria);
        this.closeSidebar();
        return;
      });
    });
  }

  public ngAfterViewInit(): void {
    (this.scrollRef.nativeElement as HTMLInputElement).scrollTop = 300;
    this.onListScroll();
  }

  public getGroupedContacts(users: UserModel[]): ChatUserGroupCustomModel[] {
    const data = users.reduce((result, obj) => {
      const firstLetter = obj.firstName[0]?.toUpperCase() ?? '';
      // @ts-expect-error -- the following line expected to have an error
      if (!result[firstLetter]) {
        const userObj = new ChatUserGroupCustomModel();
        userObj.group = firstLetter;
        userObj.users = [obj];
        // @ts-expect-error -- the following line expected to have an error
        result[firstLetter] = userObj;
      } else {
        // @ts-expect-error -- the following line expected to have an error
        // eslint-disable-next-line
        result[firstLetter].users?.push(obj);
      }
      return result;
    }, {});

    // Convert the object to an array of values
    return Object.values(data) as ChatUserGroupCustomModel[];

  }

  public onListScroll(): void {
    if (this.scrollRef !== undefined) {
      setTimeout((): void => {
        (this.scrollRef.nativeElement as HTMLInputElement).scrollTop = (this.scrollRef.nativeElement as HTMLInputElement).scrollHeight;
      }, 500);
    }
  }

  /**
   * Save the message in chat
  */
  public messageSave(f: NgForm): void {
    if (!f.valid) return;
    if (ObjectUtils.isNullOrUndefined(this.userMessage) || this.userMessage?.trim() == "") return;
    const newMessage = new ChatMessageCreateModel();
    newMessage.chatId = this.selectedChat.chatId;
    newMessage.chatMessageText = this.userMessage;
    newMessage.userId = this.userId ?? 0;
    this.realtimeClientService.sendMessage(newMessage);
    f.resetForm();
  }

  public getuserPictureUrl(userId: number, profileGuid: string, token: string): string {
    return this.httpService.getUserPictureUrl(userId, profileGuid, token);
  }

  public open(index: number): void {
    // open lightbox
    this.lightbox.open(this.images, index, {});
  }

  public close(): void {
    // close lightbox programmatically
    this.lightbox.close();
  }

  // Copy Message
  public copyMessage(event: Event): void {
    navigator.clipboard.writeText((event.currentTarget as HTMLInputElement)?.closest('.chat-list')?.querySelector('.ctext-content')?.innerHTML ?? '');
    (document.getElementById("copyClipBoard") as HTMLElement).style.display = "block";
    setTimeout((): void => {
      (document.getElementById("copyClipBoard") as HTMLElement).style.display = "none";
    }, 1000);
  }

  // Delete Message
  public deleteUserMessage(item: ChatMessageModel): void {
    this.dialogService.confirmWithTitle(`Delete Message`, `Do you want to delete this message - ${StringUtils.getWrappedText(item.chatMessageText)}?`).then(res => {
      if (!res) return;
      this.httpService.markChatMessageAsDeleted(item.chatMessageId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Message");
        this.getChatMessages(this.chatMessageSearchCriteria);
        return;
      });
    });
  }

  // Replay Message
  public replyMessage(event: Event): void {
    this.isreplyMessage = true;
    document.querySelector('.replyCard')?.classList.add('show');
    const copyText = (event.currentTarget as HTMLInputElement)?.closest('.chat-list')?.querySelector('.ctext-content')?.innerHTML;
    (document.querySelector(".replyCard .replymessage-block .flex-grow-1 .mb-0") as HTMLAreaElement).innerHTML = copyText ?? '';
    const msgOwnerName: string = (event.currentTarget as HTMLInputElement)?.closest(".chat-list")?.classList.contains("right") == true ? 'You' : document.querySelector('.username')?.innerHTML ?? '';
    (document.querySelector(".replyCard .replymessage-block .flex-grow-1 .conversation-name") as HTMLAreaElement).innerHTML = msgOwnerName;
  }

  public closeReplay(): void {
    document.querySelector('.replyCard')?.classList.remove('show');
  }

  // Delete All Message
  public deleteAllMessage(): void {
    this.dialogService.confirmWithTitle(`Clear Message`, `Do you want to clear all message from - ${StringUtils.getWrappedText(this.selectedChat.chatName)}?`).then(res => {
      if (!res) return;
    });
  }

  //  Filter Offcanvas Set
  public openEnd(content: TemplateRef<unknown>): void {
    this.offcanvasService.open(content, { position: 'end' });
  }

  public closeSidebar(): void {
    this.offcanvasService.dismiss('Cross click');
  }

  public toggleEmojiPicker(): void {
    this.showEmojiPicker = !this.showEmojiPicker;
  }

  public addEmoji(event: Event): void {
    const { userMessage } = this;
    const text = `${userMessage}`; //${event.emoji.native}
    this.userMessage = text;
    this.showEmojiPicker = false;
  }

  public onFocus(): void {
    this.showEmojiPicker = false;
  }

  public onBlur(): void {
    // 
  }

  public hideChat(): void {
    const userChatShow = document.querySelector('.user-chat');
    if (userChatShow != null) {
      userChatShow.classList.remove('user-chat-show');
    }
  }

}
