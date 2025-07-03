import { Injectable } from '@angular/core';
import { LoggedInUserModel } from '../../models/app-model';

@Injectable({
  providedIn: 'root'
})

export class SessionStorageService {
  public remove(key: string): Error {
    throw new Error('Method not implemented.');
  }
  
  public get(key: string): LoggedInUserModel {
    throw new Error('Method not implemented.');
  }

}
