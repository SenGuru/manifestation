import { CategoryModel, CategorySearchCriteria, DailyAffirmationSessionCategoryCreateModel, DailyAffirmationSessionCategoryModel, DailyAffirmationSessionCategoryUpdateModel, DailyAffirmationSessionModel, DailyAffirmationSessionSearchCriteria, LoggedInUserModel, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { DailyAffirmationSessionCrudModalComponent } from '../daily-affirmation-session-crud-modal/daily-affirmation-session-crud-modal.component';
import { HttpService } from 'src/common/services/app-http-client';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';
import { TitleService } from 'src/app/shared/services/title.service';
import { UserCrudModalComponent } from '../user-crud-modal/user-crud-modal.component';

@UntilDestroy()
@Component({
  selector: 'app-daily-affirmation-session-category-crud-modal',
  templateUrl: './daily-affirmation-session-category-crud-modal.component.html',
  standalone: false,
})
export class DailyAffirmationSessionCategoryCrudModalComponent implements OnInit {

  @ViewChild('lvDailyAffirmationSessionCategoryForm') public form!: NgForm;
  @Input() public dailyAffirmationSessionCategoryModel: DailyAffirmationSessionCategoryModel = new DailyAffirmationSessionCategoryModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public dailyAffirmationSessionCategoryId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: DailyAffirmationSessionCategoryModel | null) => void) | null = null;


  public categorySearchCriteria = new CategorySearchCriteria();
  public categoryTypeahead$ = new Subject<string>();
  public categoryTypeaheadLoading: boolean = false;
  public categorys: CategoryModel[] = [];
  public selectedCategory: CategoryModel | undefined;
  public dailyAffirmationSessionSearchCriteria = new DailyAffirmationSessionSearchCriteria();
  public dailyAffirmationSessionTypeahead$ = new Subject<string>();
  public dailyAffirmationSessionTypeaheadLoading: boolean = false;
  public dailyAffirmationSessions: DailyAffirmationSessionModel[] = [];
  public selectedDailyAffirmationSession: DailyAffirmationSessionModel | undefined;
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
    this.initDailyAffirmationSessionCategoryInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {

    this.getCategorys(this.categorySearchCriteria);
    this.getDailyAffirmationSessions(this.dailyAffirmationSessionSearchCriteria);
    this.getUsers(this.userSearchCriteria);

  }

  public initDailyAffirmationSessionCategoryInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getDailyAffirmationSessionCategoryInfo(this.dailyAffirmationSessionCategoryId);
      this.titleService.pageTitle = "Daily Affirmation Session Category info";
    } else {
      this.initLoadData();
    }
  }

  public getDailyAffirmationSessionCategoryInfo(dailyAffirmationSessionCategoryId: number): void {
    this.httpService.getDailyAffirmationSessionCategory(dailyAffirmationSessionCategoryId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to get Daily Affirmation Session Category', result);
          return;
        }

        if (result.model) {
          this.dailyAffirmationSessionCategoryModel = result.model;
          if (!this.isShowFullScreen) {
            this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Daily Affirmation Session Category'),
      complete: () => null,
    });
  }

  public initTypeAheads(): void {
    this.categoryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.categoryTypeaheadLoading = true;
      const categorySearchCriteria = new CategorySearchCriteria();
      categorySearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) categorySearchCriteria.selectedId = this.dailyAffirmationSessionCategoryModel.categoryId;
      return this.httpService.getCategorys(categorySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.categorys = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.categoryTypeaheadLoading = false,
    });
    this.dailyAffirmationSessionTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.dailyAffirmationSessionTypeaheadLoading = true;
      const dailyAffirmationSessionSearchCriteria = new DailyAffirmationSessionSearchCriteria();
      dailyAffirmationSessionSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) dailyAffirmationSessionSearchCriteria.selectedId = this.dailyAffirmationSessionCategoryModel.dailyAffirmationSessionId;
      return this.httpService.getDailyAffirmationSessions(dailyAffirmationSessionSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.dailyAffirmationSessions = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.dailyAffirmationSessionTypeaheadLoading = false,
    });
    this.userTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.userTypeaheadLoading = true;
      const userSearchCriteria = new UserSearchCriteria();
      userSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) userSearchCriteria.selectedId = this.dailyAffirmationSessionCategoryModel.userId;
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

  public showAddCategoryModal(): void {
    // const modalRef = this.modalService.open(CategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    // const componentInstance = modalRef.componentInstance as CategoryCrudModalComponent;
    // componentInstance.isCreateMode = true;
    // componentInstance.buttonText = 'Create';
    // componentInstance.title = 'Create Category';
    // componentInstance.isSuperAdmin = this.isSuperAdmin;
    // componentInstance.companyId = this.companyId;
    // componentInstance.companyName = this.companyName;
    // componentInstance.layoutTypeId = this.layoutTypeId;
    // componentInstance.onCloseCallback = (res: CategoryModel | null): void => {
    //   if (ObjectUtils.isObjNullOrUndefined(res)) return;
    //   this.getCategorys(this.categorySearchCriteria, res?.categoryId);
    // }
  }



  public getCategorys(searchModel: CategorySearchCriteria, selectedcategoryId?: number): void {
    searchModel.sortAscending = false;
 
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.dailyAffirmationSessionCategoryModel.categoryId;
    this.httpService.getCategorys(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.categorys = data.model;
          if (!this.isCreateMode) this.selectedCategory = data.model.find(item => item.categoryId === this.dailyAffirmationSessionCategoryModel.categoryId);
          if (!ObjectUtils.isNullOrUndefined(selectedcategoryId)) this.selectedCategory = data.model.find(item => item.categoryId === selectedcategoryId);
        }
      },
      error: (error) => this.dialogService.error('Not Able to get Category'),
      complete: () => null,
    });
  }
  public showAddDailyAffirmationSessionModal(): void {
    const modalRef = this.modalService.open(DailyAffirmationSessionCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DailyAffirmationSessionCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Daily Affirmation Session';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = this.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.onCloseCallback = (res: DailyAffirmationSessionModel | null): void => {
      if (ObjectUtils.isObjNullOrUndefined(res)) return;
      this.getDailyAffirmationSessions(this.dailyAffirmationSessionSearchCriteria, res?.dailyAffirmationSessionId);
    }
  }



  public getDailyAffirmationSessions(searchModel: DailyAffirmationSessionSearchCriteria, selecteddailyAffirmationSessionId?: number): void {
    searchModel.sortAscending = false;
 
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.dailyAffirmationSessionCategoryModel.dailyAffirmationSessionId;
    this.httpService.getDailyAffirmationSessions(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.dailyAffirmationSessions = data.model;
          if (!this.isCreateMode) this.selectedDailyAffirmationSession = data.model.find(item => item.dailyAffirmationSessionId === this.dailyAffirmationSessionCategoryModel.dailyAffirmationSessionId);
          if (!ObjectUtils.isNullOrUndefined(selecteddailyAffirmationSessionId)) this.selectedDailyAffirmationSession = data.model.find(item => item.dailyAffirmationSessionId === selecteddailyAffirmationSessionId);
        }
      },
      error: (error) => this.dialogService.error('Not Able to get Daily Affirmation Session'),
      complete: () => null,
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
    if (!this.isCreateMode) searchModel.selectedId = this.dailyAffirmationSessionCategoryModel.userId;
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.users = data.model;
          if (!this.isCreateMode) this.selectedUser = data.model.find(item => item.userId === this.dailyAffirmationSessionCategoryModel.userId);
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
    const dailyAffirmationSessionCategoryModel = this.generateModel(f.value);

    if (dailyAffirmationSessionCategoryModel) {
      if (this.isCreateMode) {
        this.createDailyAffirmationSessionCategory(dailyAffirmationSessionCategoryModel, f);
      } else {
        this.updateDailyAffirmationSessionCategory(dailyAffirmationSessionCategoryModel, f);
      }
    } else {
      this.dialogService.error('Daily Affirmation Session Category Information is not valid');
      return;
    }
  }

  public generateModel(obj: DailyAffirmationSessionCategoryModel): DailyAffirmationSessionCategoryModel | null {
    if (this.selectedCategory == null)
      return null;
    if (this.selectedDailyAffirmationSession == null)
      return null;
    if (this.selectedUser == null)
      return null;
    Object.assign(this.dailyAffirmationSessionCategoryModel, obj);
    Object.keys(this.dailyAffirmationSessionCategoryModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.dailyAffirmationSessionCategoryModel[key] && typeof this.dailyAffirmationSessionCategoryModel[key] === 'object' && !(this.dailyAffirmationSessionCategoryModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.dailyAffirmationSessionCategoryModel[key] = null;
      }
    });

    this.dailyAffirmationSessionCategoryModel.categoryId = this.selectedCategory?.categoryId ?? null;
    this.dailyAffirmationSessionCategoryModel.dailyAffirmationSessionId = this.selectedDailyAffirmationSession?.dailyAffirmationSessionId ?? null;
    this.dailyAffirmationSessionCategoryModel.userId = this.selectedUser?.userId ?? null;

    return this.dailyAffirmationSessionCategoryModel;
  }

  public createDailyAffirmationSessionCategory(dailyAffirmationSessionCategoryModel: DailyAffirmationSessionCategoryCreateModel, f: NgForm): void {
    this.httpService.createDailyAffirmationSessionCategory(dailyAffirmationSessionCategoryModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Daily Affirmation Session Category', result);
          return;
        }
        this.dialogService.success('Successfully Created the Daily Affirmation Session Category.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Daily Affirmation Session Category'),
      complete: () => null,
    });
  }

  public updateDailyAffirmationSessionCategory(dailyAffirmationSessionCategoryModel: DailyAffirmationSessionCategoryUpdateModel, f: NgForm): void {
    this.httpService.updateDailyAffirmationSessionCategory(dailyAffirmationSessionCategoryModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Daily Affirmation Session Category', result);
          return;
        }
        this.dialogService.success('Successfully updated the Daily Affirmation Session Category.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.dailyAffirmationSessionCategoryId) this.getDailyAffirmationSessionCategoryInfo(result.model?.dailyAffirmationSessionCategoryId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Daily Affirmation Session Category'),
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
