import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DefaultCategoryCreateModel, DefaultCategoryModel, DefaultCategoryUpdateModel, LoggedInUserModel } from 'src/common/models/app-model';
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

@UntilDestroy()
@Component({
  selector: 'app-default-category-crud-modal',
  templateUrl: './default-category-crud-modal.component.html',
  standalone: false,
})
export class DefaultCategoryCrudModalComponent implements OnInit {

  @ViewChild('lvDefaultCategoryForm') public form!: NgForm;
  @Input() public defaultCategoryModel: DefaultCategoryModel = new DefaultCategoryModel();  
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public defaultCategoryId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: DefaultCategoryModel | null) => void) | null = null;

            public defaultCategoryName__size: number = DefaultCategoryModel.defaultCategoryName__size;

  public predefinedCategoryTypes: Status[] = [];  
  public selectedPredefinedCategoryType: Status | undefined;


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
    this.initDefaultCategoryInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {

        this.predefinedCategoryTypes = this.selectService.GetPredefinedCategoryTypes();
        if (!this.isCreateMode) this.selectedPredefinedCategoryType = this.predefinedCategoryTypes.find(item => item.id === this.defaultCategoryModel.predefinedCategoryTypeId);

        }
 
  public initDefaultCategoryInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getDefaultCategoryInfo(this.defaultCategoryId);
      this.titleService.pageTitle = "Default Category info";
    } else {
        this.initLoadData();
    }
  }
  
 public getDefaultCategoryInfo(defaultCategoryId: number): void {
    this.httpService.getDefaultCategory(defaultCategoryId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to get Default Category', result);
          return;
        }
        
        if(result.model) {
        this.defaultCategoryModel = result.model; 
         if(!this.isShowFullScreen) {
          this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Default Category'),
      complete: () => null,
    });
  }

 public initTypeAheads(): void {
  }


  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

     // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
     const defaultCategoryModel = this.generateModel(f.value);

        if(defaultCategoryModel) {
            if (this.isCreateMode) {
              this.createDefaultCategory(defaultCategoryModel, f);
            } else {
              this.updateDefaultCategory(defaultCategoryModel, f);
            }
       }  else {
         this.dialogService.error('Default Category Information is not valid');
         return;
       }
  }

  public generateModel(obj: DefaultCategoryModel) : DefaultCategoryModel | null {
        if (this.selectedPredefinedCategoryType == null)
          return null;
    Object.assign(this.defaultCategoryModel, obj);
    Object.keys(this.defaultCategoryModel).forEach(key => {
        //@ts-expect-error  --- following line expected to have an error
        if (this.defaultCategoryModel[key] && typeof this.defaultCategoryModel[key] === 'object' && !(this.defaultCategoryModel[key] instanceof Date)) {
            //@ts-expect-error  --- following line expected to have an error
            this.defaultCategoryModel[key] = null;        
      }
    });

        this.defaultCategoryModel.predefinedCategoryTypeId =  this.selectedPredefinedCategoryType?.id ?? null;

    return this.defaultCategoryModel;
 }
  
   public createDefaultCategory(defaultCategoryModel: DefaultCategoryCreateModel, f: NgForm): void {
        this.httpService.createDefaultCategory(defaultCategoryModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to Create Default Category', result);
          return;
        }
        this.dialogService.success('Successfully Created the Default Category.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Default Category'),
      complete: () => null,
    });
  }

 public updateDefaultCategory(defaultCategoryModel: DefaultCategoryUpdateModel, f: NgForm): void {
    this.httpService.updateDefaultCategory(defaultCategoryModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to update Default Category', result);
          return;
        }
        this.dialogService.success('Successfully updated the Default Category.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.defaultCategoryId) this.getDefaultCategoryInfo(result.model?.defaultCategoryId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Default Category'),
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
