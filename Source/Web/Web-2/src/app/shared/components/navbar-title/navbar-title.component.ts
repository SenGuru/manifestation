import { Component, OnInit } from '@angular/core';

import { CommonDialogService } from '../../services/common-dialog.service';
import { LoggedInUserModel } from 'src/common/models/app-model';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-navbar-title',
    templateUrl: './navbar-title.component.html',
    styleUrls: ['./navbar-title.component.css'],
    standalone: false
})
export class NavbarTitleComponent implements OnInit {

  public currentUser: LoggedInUserModel | undefined | null;
  public fullName?: string;
  public firstName?: string;
  public lastName?: string;
  public roleName?: string;
  public url?: string;
  public currentToken?: string;
  public profileUrl?: string;
  public entityProfileUserToken?: string;
  public selectRolesList?: [];
  public activeRole?: number;
  public initialUrl?: string;

  public constructor(private sessionService: SessionService,
    private dialogService: CommonDialogService) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.firstName = this.currentUser?.firstName;
    this.lastName = this.currentUser?.lastName;
    // this.roleName = this.currentUser.userTypeName;
  }

  public logout(): void {
    this.dialogService.cancelConfirmWithTitle('Log out', 'Are you sure, do you want to log out', 'log out').then((response: unknown) => {
      if (response) {
        this.sessionService.logOut();
      }
    })

  }
}
