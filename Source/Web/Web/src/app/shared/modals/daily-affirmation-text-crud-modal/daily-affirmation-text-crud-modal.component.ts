import { CategoryModel, CategorySearchCriteria, DailyAffirmationTextCreateModel, DailyAffirmationTextModel, DailyAffirmationTextUpdateModel, LoggedInUserModel, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
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
  selector: 'app-daily-affirmation-text-crud-modal',
  templateUrl: './daily-affirmation-text-crud-modal.component.html',
  standalone: false,
})
export class DailyAffirmationTextCrudModalComponent implements OnInit {

  @ViewChild('lvDailyAffirmationTextForm') public form!: NgForm;
  @Input() public dailyAffirmationTextModel: DailyAffirmationTextModel = new DailyAffirmationTextModel();  
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public dailyAffirmationTextId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: DailyAffirmationTextModel | null) => void) | null = null;

            public dailyAffirmationTextValue__size: number = DailyAffirmationTextModel.dailyAffirmationTextValue__size;

  public categorySearchCriteria = new CategorySearchCriteria();
  public categoryTypeahead$ = new Subject<string>();
  public categoryTypeaheadLoading:boolean = false;    
  public categorys: CategoryModel[] = [];
  public selectedCategory: CategoryModel | undefined;
  public userSearchCriteria = new UserSearchCriteria();
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading:boolean = false;    
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
    this.initDailyAffirmationTextInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {

        this.getCategorys(this.categorySearchCriteria);
        this.getUsers(this.userSearchCriteria);

        }
 
  public initDailyAffirmationTextInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getDailyAffirmationTextInfo(this.dailyAffirmationTextId);
      this.titleService.pageTitle = "Daily Affirmation Text info";
    } else {
        this.initLoadData();
    }
  }
  
 public getDailyAffirmationTextInfo(dailyAffirmationTextId: number): void {
    this.httpService.getDailyAffirmationText(dailyAffirmationTextId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to get Daily Affirmation Text', result);
          return;
        }
        
        if(result.model) {
        this.dailyAffirmationTextModel = result.model; 
         if(!this.isShowFullScreen) {
          this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Daily Affirmation Text'),
      complete: () => null,
    });
  }

 public initTypeAheads(): void {
      this.categoryTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
        this.categoryTypeaheadLoading = true;
        const categorySearchCriteria = new CategorySearchCriteria();
        categorySearchCriteria.keyword = searchValue;
       
        if(!this.isCreateMode) categorySearchCriteria.selectedId = this.dailyAffirmationTextModel.categoryId;
        return this.httpService.getCategorys(categorySearchCriteria, getHeaderWithBlockUI());
      }),untilDestroyed(this)).subscribe({
        next: (data) => {
          if(data.model) this.categorys = data.model;
      },  
      error: err => {
       //
      },
      complete: () => this.categoryTypeaheadLoading = false,});
      this.userTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
        this.userTypeaheadLoading = true;
        const userSearchCriteria = new UserSearchCriteria();
        userSearchCriteria.keyword = searchValue;
        
        if(!this.isCreateMode) userSearchCriteria.selectedId = this.dailyAffirmationTextModel.userId;
        return this.httpService.getUsers(userSearchCriteria, getHeaderWithBlockUI());
      }),untilDestroyed(this)).subscribe({
        next: (data) => {
          if(data.model) this.users = data.model;
      },  
      error: err => {
       //
      },
      complete: () => this.userTypeaheadLoading = false,});
  }

      public showAddCategoryModal(): void {
        //  const modalRef = this.modalService.open(CategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
        //  const componentInstance = modalRef.componentInstance as CategoryCrudModalComponent;
        //  componentInstance.isCreateMode = true;
        //  componentInstance.buttonText = 'Create';
        //  componentInstance.title = 'Create Category';
        //  componentInstance.isSuperAdmin = this.isSuperAdmin;
        //  componentInstance.companyId = this.companyId;
        //  componentInstance.onCloseCallback = (res: CategoryModel | null): void => {
        //    if (ObjectUtils.isObjNullOrUndefined(res)) return;
        //    this.getCategorys(this.categorySearchCriteria, res?.categoryId);
        //  }
       }
      
      
      
      public getCategorys(searchModel: CategorySearchCriteria, selectedcategoryId?: number): void {
        searchModel.sortAscending = false;
     
        searchModel.sortBy = 'createdDate';
        if(!this.isCreateMode) searchModel.selectedId = this.dailyAffirmationTextModel.categoryId;
        this.httpService.getCategorys(searchModel).subscribe({
        next: (data) => {
          if (!data.isSuccess)
            return;
         if(data.model) {
          this.categorys = data.model;
          if (!this.isCreateMode) this.selectedCategory = data.model.find(item => item.categoryId === this.dailyAffirmationTextModel.categoryId);
          if (!ObjectUtils.isNullOrUndefined(selectedcategoryId)) this.selectedCategory = data.model.find(item => item.categoryId === selectedcategoryId);
          }
        },
        error: (error) => this.dialogService.error('Not Able to get Category'),
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
        if(!this.isCreateMode) searchModel.selectedId = this.dailyAffirmationTextModel.userId;
        this.httpService.getUsers(searchModel).subscribe({
        next: (data) => {
          if (!data.isSuccess)
            return;
         if(data.model) {
          this.users = data.model;
          if (!this.isCreateMode) this.selectedUser = data.model.find(item => item.userId === this.dailyAffirmationTextModel.userId);
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
     const dailyAffirmationTextModel = this.generateModel(f.value);

        if(dailyAffirmationTextModel) {
            if (this.isCreateMode) {
              this.createDailyAffirmationText(dailyAffirmationTextModel, f);
            } else {
              this.updateDailyAffirmationText(dailyAffirmationTextModel, f);
            }
       }  else {
         this.dialogService.error('Daily Affirmation Text Information is not valid');
         return;
       }
  }

  public generateModel(obj: DailyAffirmationTextModel) : DailyAffirmationTextModel | null {
        if (this.selectedCategory == null)
          return null;
        if (this.selectedUser == null)
          return null;
    Object.assign(this.dailyAffirmationTextModel, obj);
    Object.keys(this.dailyAffirmationTextModel).forEach(key => {
        //@ts-expect-error  --- following line expected to have an error
        if (this.dailyAffirmationTextModel[key] && typeof this.dailyAffirmationTextModel[key] === 'object' && !(this.dailyAffirmationTextModel[key] instanceof Date)) {
            //@ts-expect-error  --- following line expected to have an error
            this.dailyAffirmationTextModel[key] = null;        
      }
    });

        this.dailyAffirmationTextModel.categoryId = this.selectedCategory?.categoryId ?? null;
        this.dailyAffirmationTextModel.userId = this.selectedUser?.userId ?? null;

   
    return this.dailyAffirmationTextModel;
 }
  
   public createDailyAffirmationText(dailyAffirmationTextModel: DailyAffirmationTextCreateModel, f: NgForm): void {
        this.httpService.createDailyAffirmationText(dailyAffirmationTextModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to Create Daily Affirmation Text', result);
          return;
        }
        this.dialogService.success('Successfully Created the Daily Affirmation Text.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Daily Affirmation Text'),
      complete: () => null,
    });
  }

 public updateDailyAffirmationText(dailyAffirmationTextModel: DailyAffirmationTextUpdateModel, f: NgForm): void {
    this.httpService.updateDailyAffirmationText(dailyAffirmationTextModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to update Daily Affirmation Text', result);
          return;
        }
        this.dialogService.success('Successfully updated the Daily Affirmation Text.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.dailyAffirmationTextId) this.getDailyAffirmationTextInfo(result.model?.dailyAffirmationTextId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Daily Affirmation Text'),
      complete: () => null,
    });
  }

  public cancel(checkDirty: boolean): void {
    if(this.form.dirty && checkDirty){
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'cancel').then(res => {
        if (res) {
        if(this.onCloseCallback && !this.isViewMode) this.onCloseCallback(null);
        return this.activeModal.close();
        }
        });
    } else{
        if(this.onCloseCallback && !this.isViewMode) this.onCloseCallback(null);
        this.activeModal.close();
    }
  }

}
