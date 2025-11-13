import { Injectable } from '@angular/core';
import { ObjectUtils } from '../../framework/common/utils/object.utils';
import { PredefinedUserTypes } from '../../models/app-enums';
import { SessionService } from '../../framework/web/infrastructure/session/session.service';

// import { Storage } from '@ionic/storage';
@Injectable({
  providedIn: 'root'
})
export class AppSessionService {

  public constructor(private sessionService: SessionService) {

  } 

  public get currentUserId(): number {
    return !ObjectUtils.isNullOrUndefined(this.sessionService.currentUser) ? this.sessionService.currentUser?.userId ?? 0 : 0;
  }

  public get userTypeId(): number {
    return !ObjectUtils.isNullOrUndefined(this.sessionService.currentUser) ? this.sessionService.currentUser?.userTypeId ?? 0 : 0;
  }
}