import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DailyAffirmationSessionCreateModel, DailyAffirmationSessionModel, DailyAffirmationSessionUpdateModel, LoggedInUserModel, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
import { FormGroup, NgForm } from '@angular/forms';
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
  selector: 'app-daily-affirmation-session-crud-modal',
  templateUrl: './daily-affirmation-session-crud-modal.component.html',
  standalone: false,
})
export class DailyAffirmationSessionCrudModalComponent implements OnInit {

  @ViewChild('lvDailyAffirmationSessionForm') public form!: NgForm;
  @Input() public dailyAffirmationSessionModel: DailyAffirmationSessionModel = new DailyAffirmationSessionModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public dailyAffirmationSessionId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: DailyAffirmationSessionModel | null) => void) | null = null;


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
    this.initDailyAffirmationSessionInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {

    this.getUsers(this.userSearchCriteria);

  }

  public initDailyAffirmationSessionInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getDailyAffirmationSessionInfo(this.dailyAffirmationSessionId);
      this.titleService.pageTitle = "Daily Affirmation Session info";
    } else {
      this.initLoadData();
    }
  }

  public getDailyAffirmationSessionInfo(dailyAffirmationSessionId: number): void {
    this.httpService.getDailyAffirmationSession(dailyAffirmationSessionId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to get Daily Affirmation Session', result);
          return;
        }

        if (result.model) {
          this.dailyAffirmationSessionModel = result.model;
          if (!this.isShowFullScreen) {
            this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Daily Affirmation Session'),
      complete: () => null,
    });
  }

  public initTypeAheads(): void {
    this.userTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.userTypeaheadLoading = true;
      const userSearchCriteria = new UserSearchCriteria();
      userSearchCriteria.keyword = searchValue;

      if (!this.isCreateMode) userSearchCriteria.selectedId = this.dailyAffirmationSessionModel.userId;
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
    if (!this.isCreateMode) searchModel.selectedId = this.dailyAffirmationSessionModel.userId;
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.users = data.model;
          if (!this.isCreateMode) this.selectedUser = data.model.find(item => item.userId === this.dailyAffirmationSessionModel.userId);
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
    const dailyAffirmationSessionModel = this.generateModel(f.value);

    if (dailyAffirmationSessionModel) {
      if (this.isCreateMode) {
        this.createDailyAffirmationSession(dailyAffirmationSessionModel, f);
      } else {
        this.updateDailyAffirmationSession(dailyAffirmationSessionModel, f);
      }
    } else {
      this.dialogService.error('Daily Affirmation Session Information is not valid');
      return;
    }
  }

  public generateModel(obj: DailyAffirmationSessionModel): DailyAffirmationSessionModel | null {
    if (this.selectedUser == null)
      return null;
    Object.assign(this.dailyAffirmationSessionModel, obj);
    Object.keys(this.dailyAffirmationSessionModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.dailyAffirmationSessionModel[key] && typeof this.dailyAffirmationSessionModel[key] === 'object' && !(this.dailyAffirmationSessionModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.dailyAffirmationSessionModel[key] = null;
      }
    });

    this.dailyAffirmationSessionModel.userId = this.selectedUser?.userId ?? null;

    return this.dailyAffirmationSessionModel;
  }

  public createDailyAffirmationSession(dailyAffirmationSessionModel: DailyAffirmationSessionCreateModel, f: NgForm): void {
    this.httpService.createDailyAffirmationSession(dailyAffirmationSessionModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Daily Affirmation Session', result);
          return;
        }
        this.dialogService.success('Successfully Created the Daily Affirmation Session.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Daily Affirmation Session'),
      complete: () => null,
    });
  }

  public updateDailyAffirmationSession(dailyAffirmationSessionModel: DailyAffirmationSessionUpdateModel, f: NgForm): void {
    this.httpService.updateDailyAffirmationSession(dailyAffirmationSessionModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Daily Affirmation Session', result);
          return;
        }
        this.dialogService.success('Successfully updated the Daily Affirmation Session.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.dailyAffirmationSessionId) this.getDailyAffirmationSessionInfo(result.model?.dailyAffirmationSessionId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Daily Affirmation Session'),
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
