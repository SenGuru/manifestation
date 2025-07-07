import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DefaultGoalTrackerCreateModel, DefaultGoalTrackerModel, DefaultGoalTrackerUpdateModel, LoggedInUserModel } from 'src/common/models/app-model';
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
  selector: 'app-default-goal-tracker-crud-modal',
  templateUrl: './default-goal-tracker-crud-modal.component.html',
  standalone: false,
})
export class DefaultGoalTrackerCrudModalComponent implements OnInit {

  @ViewChild('lvDefaultGoalTrackerForm') public form!: NgForm;
  @Input() public defaultGoalTrackerModel: DefaultGoalTrackerModel = new DefaultGoalTrackerModel();  
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public defaultGoalTrackerId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: DefaultGoalTrackerModel | null) => void) | null = null;

            public defaultGoalTrackerText__size: number = DefaultGoalTrackerModel.defaultGoalTrackerText__size;



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
    this.initDefaultGoalTrackerInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {


        }
 
  public initDefaultGoalTrackerInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getDefaultGoalTrackerInfo(this.defaultGoalTrackerId);
      this.titleService.pageTitle = "Default Goal Tracker info";
    } else {
        this.initLoadData();
    }
  }
  
 public getDefaultGoalTrackerInfo(defaultGoalTrackerId: number): void {
    this.httpService.getDefaultGoalTracker(defaultGoalTrackerId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to get Default Goal Tracker', result);
          return;
        }
        
        if(result.model) {
        this.defaultGoalTrackerModel = result.model; 
         if(!this.isShowFullScreen) {
          this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Default Goal Tracker'),
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
     const defaultGoalTrackerModel = this.generateModel(f.value);

        if(defaultGoalTrackerModel) {
            if (this.isCreateMode) {
              this.createDefaultGoalTracker(defaultGoalTrackerModel, f);
            } else {
              this.updateDefaultGoalTracker(defaultGoalTrackerModel, f);
            }
       }  else {
         this.dialogService.error('Default Goal Tracker Information is not valid');
         return;
       }
  }

  public generateModel(obj: DefaultGoalTrackerModel) : DefaultGoalTrackerModel | null {
    Object.assign(this.defaultGoalTrackerModel, obj);
    Object.keys(this.defaultGoalTrackerModel).forEach(key => {
        //@ts-expect-error  --- following line expected to have an error
        if (this.defaultGoalTrackerModel[key] && typeof this.defaultGoalTrackerModel[key] === 'object' && !(this.defaultGoalTrackerModel[key] instanceof Date)) {
            //@ts-expect-error  --- following line expected to have an error
            this.defaultGoalTrackerModel[key] = null;        
      }
    });


    return this.defaultGoalTrackerModel;
 }
  
   public createDefaultGoalTracker(defaultGoalTrackerModel: DefaultGoalTrackerCreateModel, f: NgForm): void {
        this.httpService.createDefaultGoalTracker(defaultGoalTrackerModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to Create Default Goal Tracker', result);
          return;
        }
        this.dialogService.success('Successfully Created the Default Goal Tracker.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Default Goal Tracker'),
      complete: () => null,
    });
  }

 public updateDefaultGoalTracker(defaultGoalTrackerModel: DefaultGoalTrackerUpdateModel, f: NgForm): void {
    this.httpService.updateDefaultGoalTracker(defaultGoalTrackerModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
        this.dialogService.errorWithReturnModel('Not Able to update Default Goal Tracker', result);
          return;
        }
        this.dialogService.success('Successfully updated the Default Goal Tracker.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.defaultGoalTrackerId) this.getDefaultGoalTrackerInfo(result.model?.defaultGoalTrackerId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Default Goal Tracker'),
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
