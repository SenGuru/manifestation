import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { LoggedInUserModel, UserModel, UserSearchCriteria, VisionBoardSessionCreateModel, VisionBoardSessionModel, VisionBoardSessionUpdateModel } from 'src/common/models/app-model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';
import { TitleService } from 'src/app/shared/services/title.service';
import { UserCrudModalComponent } from '../user-crud-modal/user-crud-modal.component';

@UntilDestroy()
@Component({
  selector: 'app-vision-board-session-crud-modal',
  templateUrl: './vision-board-session-crud-modal.component.html',
  standalone: false,
})
export class VisionBoardSessionCrudModalComponent implements OnInit {

  @ViewChild('lvVisionBoardSessionForm') public form!: NgForm;
  @Input() public visionBoardSessionModel: VisionBoardSessionModel = new VisionBoardSessionModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public visionBoardSessionId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: VisionBoardSessionModel | null) => void) | null = null;


  public userSearchCriteria = new UserSearchCriteria();
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading: boolean = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;


  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private titleService: TitleService,
    private modalService: NgbModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initVisionBoardSessionInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {

    this.getUsers(this.userSearchCriteria);

  }

  public initVisionBoardSessionInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getVisionBoardSessionInfo(this.visionBoardSessionId);
      this.titleService.pageTitle = "Vision Board Session info";
    } else {
      this.initLoadData();
    }
  }

  public getVisionBoardSessionInfo(visionBoardSessionId: number): void {
    this.httpService.getVisionBoardSession(visionBoardSessionId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to get Vision Board Session', result);
          return;
        }

        if (result.model) {
          this.visionBoardSessionModel = result.model;
          if (!this.isShowFullScreen) {
            this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Vision Board Session'),
      complete: () => null,
    });
  }

  public initTypeAheads(): void {
    this.userTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.userTypeaheadLoading = true;
      const userSearchCriteria = new UserSearchCriteria();
      userSearchCriteria.keyword = searchValue;

      if (!this.isCreateMode) userSearchCriteria.selectedId = this.visionBoardSessionModel.userId;
      return this.httpService.getUsers(userSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.users = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.userTypeaheadLoading = false,
    });
  }

  public showAddUserModal(): void {
    const modalRef = this.modalService.open(UserCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as UserCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create User';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (res: UserModel | null): void => {
      if (ObjectUtils.isObjNullOrUndefined(res)) return;
      this.getUsers(this.userSearchCriteria, res?.userId);
    }
  }



  public getUsers(searchModel: UserSearchCriteria, selecteduserId?: number): void {
    searchModel.sortAscending = false;

    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.visionBoardSessionModel.userId;
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.users = data.model;
          if (!this.isCreateMode) this.selectedUser = data.model.find(item => item.userId === this.visionBoardSessionModel.userId);
          if (!ObjectUtils.isNullOrUndefined(selecteduserId)) this.selectedUser = data.model.find(item => item.userId === selecteduserId);
        }
      },
      error: (error) => this.dialogService.error('Not Able to get User'),
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const visionBoardSessionModel = this.generateModel(f.value);

    if (visionBoardSessionModel) {
      if (this.isCreateMode) {
        this.createVisionBoardSession(visionBoardSessionModel, f);
      } else {
        this.updateVisionBoardSession(visionBoardSessionModel, f);
      }
    } else {
      this.dialogService.error('Vision Board Session Information is not valid');
      return;
    }
  }

  public generateModel(obj: VisionBoardSessionModel): VisionBoardSessionModel | null {
    if (this.selectedUser == null)
      return null;
    Object.assign(this.visionBoardSessionModel, obj);
    Object.keys(this.visionBoardSessionModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.visionBoardSessionModel[key] && typeof this.visionBoardSessionModel[key] === 'object' && !(this.visionBoardSessionModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.visionBoardSessionModel[key] = null;
      }
    });

    this.visionBoardSessionModel.userId = this.selectedUser?.userId ?? null;
    return this.visionBoardSessionModel;
  }

  public createVisionBoardSession(visionBoardSessionModel: VisionBoardSessionCreateModel, f: NgForm): void {
    this.httpService.createVisionBoardSession(visionBoardSessionModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Vision Board Session', result);
          return;
        }
        this.dialogService.success('Successfully Created the Vision Board Session.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Vision Board Session'),
      complete: () => null,
    });
  }

  public updateVisionBoardSession(visionBoardSessionModel: VisionBoardSessionUpdateModel, f: NgForm): void {
    this.httpService.updateVisionBoardSession(visionBoardSessionModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Vision Board Session', result);
          return;
        }
        this.dialogService.success('Successfully updated the Vision Board Session.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.visionBoardSessionId) this.getVisionBoardSessionInfo(result.model?.visionBoardSessionId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Vision Board Session'),
      complete: () => null,
    });
  }

  public cancel(checkDirty: boolean): void {
    if (this.form.dirty && checkDirty) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'cancel').then(res => {
        if (res) {
          if (this.onCloseCallback && !this.isViewMode) this.onCloseCallback(null);
          return this.activeModal.close();
        }
      });
    } else {
      if (this.onCloseCallback && !this.isViewMode) this.onCloseCallback(null);
      this.activeModal.close();
    }
  }

}
