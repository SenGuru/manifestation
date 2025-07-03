import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class TitleService {

  public constructor(private titleService: Title) { }

  public set pageTitle(value: string) {
    this.titleService.setTitle(`AuditDuo - ${value}`);
  }
}
