import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DefaultCategoryModel, DefaultCategorySearchCriteria, DefaultDailyAffirmationTextCreateModel, DefaultDailyAffirmationTextModel, DefaultDailyAffirmationTextUpdateModel, LoggedInUserModel } from 'src/common/models/app-model';
import { FormGroup, NgForm } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { DefaultCategoryCrudModalComponent } from '../default-category-crud-modal/default-category-crud-modal.component';
import { HttpService } from 'src/common/services/app-http-client';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';
import { TitleService } from 'src/app/shared/services/title.service';

@UntilDestroy()
@Component({
  selector: 'app-default-daily-affirmation-text-crud-modal',
  templateUrl: './default-daily-affirmation-text-crud-modal.component.html',
  standalone: false,
})
export class DefaultDailyAffirmationTextCrudModalComponent implements OnInit {

  @ViewChild('lvDefaultDailyAffirmationTextForm') public form!: NgForm;
  @Input() public defaultDailyAffirmationTextModel: DefaultDailyAffirmationTextModel = new DefaultDailyAffirmationTextModel();  
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public defaultDailyAffirmationTextId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: DefaultDailyAffirmationTextModel | null) => void) | null = null;

            public defaultDailyAffirmationTextName__size: number = DefaultDailyAffirmationTextModel.defaultDailyAffirmationTextName__size;

  public defaultCategorySearchCriteria = new DefaultCategorySearchCriteria();
  public defaultCategoryTypeahead$ = new Subject<string>();
  public defaultCategoryTypeaheadLoading:boolean = false;    
  public defaultCategorys: DefaultCategoryModel[] = [];
  public selectedDefaultCategory: DefaultCategoryModel | undefined;


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
    this.initDefaultDailyAffirmationTextInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {

        this.getDefaultCategorys(this.defaultCategorySearchCriteria);

        }
 
  public initDefaultDailyAffirmationTextInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getDefaultDailyAffirmationTextInfo(this.defaultDailyAffirmationTextId);
      this.titleService.pageTitle = "Default Daily Affirmation Text info";
    } else {
        this.initLoadData();
    }
  }
  
 public getDefaultDailyAffirmationTextInfo(defaultDailyAffirmationTextId: number): void {
    this.httpService.getDefaultDailyAffirmationText(defaultDailyAffirmationTextId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to get Default Daily Affirmation Text', result);
          return;
        }
        
        if(result.model) {
        this.defaultDailyAffirmationTextModel = result.model; 
         if(!this.isShowFullScreen) {
          this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Default Daily Affirmation Text'),
      complete: () => null,
    });
  }

 public initTypeAheads(): void {
      this.defaultCategoryTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
        this.defaultCategoryTypeaheadLoading = true;
        const defaultCategorySearchCriteria = new DefaultCategorySearchCriteria();
        defaultCategorySearchCriteria.keyword = searchValue;
        if(!this.isCreateMode) defaultCategorySearchCriteria.selectedId = this.defaultDailyAffirmationTextModel.defaultCategoryId;
        return this.httpService.getDefaultCategorys(defaultCategorySearchCriteria, getHeaderWithBlockUI());
      }),untilDestroyed(this)).subscribe({
        next: (data) => {
          if(data.model) this.defaultCategorys = data.model;
      },  
      error: err => {
       //
      },
      complete: () => this.defaultCategoryTypeaheadLoading = false,});
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
        if(!this.isCreateMode) searchModel.selectedId = this.defaultDailyAffirmationTextModel.defaultCategoryId;
        this.httpService.getDefaultCategorys(searchModel).subscribe({
        next: (data) => {
          if (!data.isSuccess)
            return;
         if(data.model) {
          this.defaultCategorys = data.model;
          if (!this.isCreateMode) this.selectedDefaultCategory = data.model.find(item => item.defaultCategoryId === this.defaultDailyAffirmationTextModel.defaultCategoryId);
          if (!ObjectUtils.isNullOrUndefined(selecteddefaultCategoryId)) this.selectedDefaultCategory = data.model.find(item => item.defaultCategoryId === selecteddefaultCategoryId);
          }
        },
        error: (error) => this.dialogService.error('Not Able to get Default Category'),
        complete: () => null,
        });
      }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

     // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
     const defaultDailyAffirmationTextModel = this.generateModel(f.value);

        if(defaultDailyAffirmationTextModel) {
            if (this.isCreateMode) {
              this.createDefaultDailyAffirmationText(defaultDailyAffirmationTextModel, f);
            } else {
              this.updateDefaultDailyAffirmationText(defaultDailyAffirmationTextModel, f);
            }
       }  else {
         this.dialogService.error('Default Daily Affirmation Text Information is not valid');
         return;
       }
  }

  public generateModel(obj: DefaultDailyAffirmationTextModel) : DefaultDailyAffirmationTextModel | null {
        if (this.selectedDefaultCategory == null)
          return null;
    Object.assign(this.defaultDailyAffirmationTextModel, obj);
    Object.keys(this.defaultDailyAffirmationTextModel).forEach(key => {
        //@ts-expect-error  --- following line expected to have an error
        if (this.defaultDailyAffirmationTextModel[key] && typeof this.defaultDailyAffirmationTextModel[key] === 'object' && !(this.defaultDailyAffirmationTextModel[key] instanceof Date)) {
            //@ts-expect-error  --- following line expected to have an error
            this.defaultDailyAffirmationTextModel[key] = null;        
      }
    });

        this.defaultDailyAffirmationTextModel.defaultCategoryId = this.selectedDefaultCategory?.defaultCategoryId ?? null;

    return this.defaultDailyAffirmationTextModel;
 }
  
   public createDefaultDailyAffirmationText(defaultDailyAffirmationTextModel: DefaultDailyAffirmationTextCreateModel, f: NgForm): void {
        this.httpService.createDefaultDailyAffirmationText(defaultDailyAffirmationTextModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to Create Default Daily Affirmation Text', result);
          return;
        }
        this.dialogService.success('Successfully Created the Default Daily Affirmation Text.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Default Daily Affirmation Text'),
      complete: () => null,
    });
  }

 public updateDefaultDailyAffirmationText(defaultDailyAffirmationTextModel: DefaultDailyAffirmationTextUpdateModel, f: NgForm): void {
    this.httpService.updateDefaultDailyAffirmationText(defaultDailyAffirmationTextModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to update Default Daily Affirmation Text', result);
          return;
        }
        this.dialogService.success('Successfully updated the Default Daily Affirmation Text.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.defaultDailyAffirmationTextId) this.getDefaultDailyAffirmationTextInfo(result.model?.defaultDailyAffirmationTextId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Default Daily Affirmation Text'),
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
