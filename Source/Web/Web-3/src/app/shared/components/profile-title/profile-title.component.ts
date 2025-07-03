import { Component, OnInit } from '@angular/core';

import { ChangePasswordComponent } from '../../modals/change-password/change-password.component';
import { CommonDialogService } from '../../services/common-dialog.service';
import { LoggedInUserModel } from 'src/common/models/app-model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-profile-title',
    templateUrl: './profile-title.component.html',
    styleUrls: ['./profile-title.component.css'],
    standalone: false
})
export class ProfileTitleComponent implements OnInit {

  public currentUser: LoggedInUserModel | null = null;
  public roleName: string | undefined;
  public constructor(private sessionService: SessionService,
    private modalService: NgbModal,
    private commonService: CommonDialogService) { }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
  }

  public logout(): void {
    this.sessionService.logOut();
  }

  public onLogOutClick(): void {
    this.commonService
      .confirmWithSmall("Log Out", "Are you sure you want to logout ?")
      .then((response) => {
        if (response) {
          this.sessionService.logOut();
        }
      });
  }

  public showChangePasswordOptionModal(): void {
    const modalRef = this.modalService.open(ChangePasswordComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as ChangePasswordComponent;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Customer Token';
  }

}
