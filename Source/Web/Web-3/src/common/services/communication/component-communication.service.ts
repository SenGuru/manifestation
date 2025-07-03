import { BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ComponentCommunicationService {
  /**
   * we can use this service for trigger functions , events, or update any boolean status between non relational component
   */
  private statusSource = new BehaviorSubject(false); // set default status
  public statusMessage = new BehaviorSubject(''); // set default status
  public screenType = new BehaviorSubject<number | null>(null); // set default status
  currentStatus = this.statusSource.asObservable();

  constructor() { }

public changeStatus(status: boolean, message?: string, screentType: number | null = null): void {
    this.statusSource.next(status);
    this.statusMessage.next(message ?? '');
    this.screenType.next(screentType);
  }

}
