import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DefaultCategoryModel, DefaultCategorySearchCriteria, DefaultGoalTrackerCategoryCreateModel, DefaultGoalTrackerCategoryModel, DefaultGoalTrackerCategoryUpdateModel, DefaultGoalTrackerModel, DefaultGoalTrackerSearchCriteria, LoggedInUserModel } from 'src/common/models/app-model';
import { FormGroup, NgForm } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { DefaultCategoryCrudModalComponent } from '../default-category-crud-modal/default-category-crud-modal.component';
import { DefaultGoalTrackerCrudModalComponent } from '../default-goal-tracker-crud-modal/default-goal-tracker-crud-modal.component';
import { HttpService } from 'src/common/services/app-http-client';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';
import { TitleService } from 'src/app/shared/services/title.service';

@UntilDestroy()
@Component({
  selector: 'app-default-goal-tracker-category-crud-modal',
  templateUrl: './default-goal-tracker-category-crud-modal.component.html',
  standalone: false,
})
export class DefaultGoalTrackerCategoryCrudModalComponent implements OnInit {

  @ViewChild('lvDefaultGoalTrackerCategoryForm') public form!: NgForm;
  @Input() public defaultGoalTrackerCategoryModel: DefaultGoalTrackerCategoryModel = new DefaultGoalTrackerCategoryModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public defaultGoalTrackerCategoryId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: DefaultGoalTrackerCategoryModel | null) => void) | null = null;


  public defaultCategorySearchCriteria = new DefaultCategorySearchCriteria();
  public defaultCategoryTypeahead$ = new Subject<string>();
  public defaultCategoryTypeaheadLoading: boolean = false;
  public defaultCategorys: DefaultCategoryModel[] = [];
  public selectedDefaultCategory: DefaultCategoryModel | undefined;
  public defaultGoalTrackerSearchCriteria = new DefaultGoalTrackerSearchCriteria();
  public defaultGoalTrackerTypeahead$ = new Subject<string>();
  public defaultGoalTrackerTypeaheadLoading: boolean = false;
  public defaultGoalTrackers: DefaultGoalTrackerModel[] = [];
  public selectedDefaultGoalTracker: DefaultGoalTrackerModel | undefined;


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
    this.initDefaultGoalTrackerCategoryInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {

    this.getDefaultCategorys(this.defaultCategorySearchCriteria);
    this.getDefaultGoalTrackers(this.defaultGoalTrackerSearchCriteria);

  }

  public initDefaultGoalTrackerCategoryInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getDefaultGoalTrackerCategoryInfo(this.defaultGoalTrackerCategoryId);
      this.titleService.pageTitle = "Default Goal Tracker Category info";
    } else {
      this.initLoadData();
    }
  }

  public getDefaultGoalTrackerCategoryInfo(defaultGoalTrackerCategoryId: number): void {
    this.httpService.getDefaultGoalTrackerCategory(defaultGoalTrackerCategoryId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to get Default Goal Tracker Category', result);
          return;
        }

        if (result.model) {
          this.defaultGoalTrackerCategoryModel = result.model;
          if (!this.isShowFullScreen) {
            this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Default Goal Tracker Category'),
      complete: () => null,
    });
  }

  public initTypeAheads(): void {
    this.defaultCategoryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.defaultCategoryTypeaheadLoading = true;
      const defaultCategorySearchCriteria = new DefaultCategorySearchCriteria();
      defaultCategorySearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) defaultCategorySearchCriteria.selectedId = this.defaultGoalTrackerCategoryModel.defaultCategoryId;
      return this.httpService.getDefaultCategorys(defaultCategorySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.defaultCategorys = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.defaultCategoryTypeaheadLoading = false,
    });
    this.defaultGoalTrackerTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.defaultGoalTrackerTypeaheadLoading = true;
      const defaultGoalTrackerSearchCriteria = new DefaultGoalTrackerSearchCriteria();
      defaultGoalTrackerSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) defaultGoalTrackerSearchCriteria.selectedId = this.defaultGoalTrackerCategoryModel.defaultGoalTrackerId;
      return this.httpService.getDefaultGoalTrackers(defaultGoalTrackerSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) this.defaultGoalTrackers = data.model;
      },
      error: err => {
        //
      },
      complete: () => this.defaultGoalTrackerTypeaheadLoading = false,
    });
  }

  public showAddDefaultCategoryModal(): void {
    const modalRef = this.modalService.open(DefaultCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultCategoryCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Default Category';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (res: DefaultCategoryModel | null): void => {
      if (ObjectUtils.isObjNullOrUndefined(res)) return;
      this.getDefaultCategorys(this.defaultCategorySearchCriteria, res?.defaultCategoryId);
    }
  }



  public getDefaultCategorys(searchModel: DefaultCategorySearchCriteria, selecteddefaultCategoryId?: number): void {
    searchModel.sortAscending = false;

    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.defaultGoalTrackerCategoryModel.defaultCategoryId;
    this.httpService.getDefaultCategorys(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.defaultCategorys = data.model;
          if (!this.isCreateMode) this.selectedDefaultCategory = data.model.find(item => item.defaultCategoryId === this.defaultGoalTrackerCategoryModel.defaultCategoryId);
          if (!ObjectUtils.isNullOrUndefined(selecteddefaultCategoryId)) this.selectedDefaultCategory = data.model.find(item => item.defaultCategoryId === selecteddefaultCategoryId);
        }
      },
      error: (error) => this.dialogService.error('Not Able to get Default Category'),
      complete: () => null,
    });
  }
  public showAddDefaultGoalTrackerModal(): void {
    const modalRef = this.modalService.open(DefaultGoalTrackerCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as DefaultGoalTrackerCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Default Goal Tracker';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.onCloseCallback = (res: DefaultGoalTrackerModel | null): void => {
      if (ObjectUtils.isObjNullOrUndefined(res)) return;
      this.getDefaultGoalTrackers(this.defaultGoalTrackerSearchCriteria, res?.defaultGoalTrackerId);
    }
  }



  public getDefaultGoalTrackers(searchModel: DefaultGoalTrackerSearchCriteria, selecteddefaultGoalTrackerId?: number): void {
    searchModel.sortAscending = false;

    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.defaultGoalTrackerCategoryModel.defaultGoalTrackerId;
    this.httpService.getDefaultGoalTrackers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.defaultGoalTrackers = data.model;
          if (!this.isCreateMode) this.selectedDefaultGoalTracker = data.model.find(item => item.defaultGoalTrackerId === this.defaultGoalTrackerCategoryModel.defaultGoalTrackerId);
          if (!ObjectUtils.isNullOrUndefined(selecteddefaultGoalTrackerId)) this.selectedDefaultGoalTracker = data.model.find(item => item.defaultGoalTrackerId === selecteddefaultGoalTrackerId);
        }
      },
      error: (error) => this.dialogService.error('Not Able to get Default Goal Tracker'),
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const defaultGoalTrackerCategoryModel = this.generateModel(f.value);

    if (defaultGoalTrackerCategoryModel) {
      if (this.isCreateMode) {
        this.createDefaultGoalTrackerCategory(defaultGoalTrackerCategoryModel, f);
      } else {
        this.updateDefaultGoalTrackerCategory(defaultGoalTrackerCategoryModel, f);
      }
    } else {
      this.dialogService.error('Default Goal Tracker Category Information is not valid');
      return;
    }
  }

  public generateModel(obj: DefaultGoalTrackerCategoryModel): DefaultGoalTrackerCategoryModel | null {
    if (this.selectedDefaultCategory == null)
      return null;
    if (this.selectedDefaultGoalTracker == null)
      return null;
    Object.assign(this.defaultGoalTrackerCategoryModel, obj);
    Object.keys(this.defaultGoalTrackerCategoryModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.defaultGoalTrackerCategoryModel[key] && typeof this.defaultGoalTrackerCategoryModel[key] === 'object' && !(this.defaultGoalTrackerCategoryModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.defaultGoalTrackerCategoryModel[key] = null;
      }
    });

    this.defaultGoalTrackerCategoryModel.defaultCategoryId = this.selectedDefaultCategory?.defaultCategoryId ?? null;
    this.defaultGoalTrackerCategoryModel.defaultGoalTrackerId = this.selectedDefaultGoalTracker?.defaultGoalTrackerId ?? null;

    return this.defaultGoalTrackerCategoryModel;
  }

  public createDefaultGoalTrackerCategory(defaultGoalTrackerCategoryModel: DefaultGoalTrackerCategoryCreateModel, f: NgForm): void {
    this.httpService.createDefaultGoalTrackerCategory(defaultGoalTrackerCategoryModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Default Goal Tracker Category', result);
          return;
        }
        this.dialogService.success('Successfully Created the Default Goal Tracker Category.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Default Goal Tracker Category'),
      complete: () => null,
    });
  }

  public updateDefaultGoalTrackerCategory(defaultGoalTrackerCategoryModel: DefaultGoalTrackerCategoryUpdateModel, f: NgForm): void {
    this.httpService.updateDefaultGoalTrackerCategory(defaultGoalTrackerCategoryModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Default Goal Tracker Category', result);
          return;
        }
        this.dialogService.success('Successfully updated the Default Goal Tracker Category.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.defaultGoalTrackerCategoryId) this.getDefaultGoalTrackerCategoryInfo(result.model?.defaultGoalTrackerCategoryId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Default Goal Tracker Category'),
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
