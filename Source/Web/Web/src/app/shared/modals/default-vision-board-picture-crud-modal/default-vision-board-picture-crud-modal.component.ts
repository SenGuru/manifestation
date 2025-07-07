import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DefaultVisionBoardPictureCreateModel, DefaultVisionBoardPictureModel, DefaultVisionBoardPictureUpdateModel, LoggedInUserModel } from 'src/common/models/app-model';
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
  selector: 'app-default-vision-board-picture-crud-modal',
  templateUrl: './default-vision-board-picture-crud-modal.component.html',
  standalone: false,
})
export class DefaultVisionBoardPictureCrudModalComponent implements OnInit {

  @ViewChild('lvDefaultVisionBoardPictureForm') public form!: NgForm;
  @Input() public defaultVisionBoardPictureModel: DefaultVisionBoardPictureModel = new DefaultVisionBoardPictureModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public defaultVisionBoardPictureId: number = 0;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | null = null;
  @Input() public layoutTypeId: number | null = null;
  @Input() public isShowFullScreen: boolean = false;
  public currentUser: LoggedInUserModel | undefined | null;
  public onCloseCallback: ((res: DefaultVisionBoardPictureModel | null) => void) | null = null;

  public location__size: number = DefaultVisionBoardPictureModel.location__size;
  public fileName__size: number = DefaultVisionBoardPictureModel.fileName__size;



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
    this.initDefaultVisionBoardPictureInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initLoadData(): void {


  }

  public initDefaultVisionBoardPictureInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getDefaultVisionBoardPictureInfo(this.defaultVisionBoardPictureId);
      this.titleService.pageTitle = "Default Vision Board Picture info";
    } else {
      this.initLoadData();
    }
  }

  public getDefaultVisionBoardPictureInfo(defaultVisionBoardPictureId: number): void {
    this.httpService.getDefaultVisionBoardPicture(defaultVisionBoardPictureId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to get Default Vision Board Picture', result);
          return;
        }

        if (result.model) {
          this.defaultVisionBoardPictureModel = result.model;
          if (!this.isShowFullScreen) {
            this.initLoadData();
          }
        }
      },
      error: (err) => this.dialogService.error('Not Able to get Default Vision Board Picture'),
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
    const defaultVisionBoardPictureModel = this.generateModel(f.value);

    if (defaultVisionBoardPictureModel) {
      if (this.isCreateMode) {
        this.createDefaultVisionBoardPicture(defaultVisionBoardPictureModel, f);
      } else {
        this.updateDefaultVisionBoardPicture(defaultVisionBoardPictureModel, f);
      }
    } else {
      this.dialogService.error('Default Vision Board Picture Information is not valid');
      return;
    }
  }

  public generateModel(obj: DefaultVisionBoardPictureModel): DefaultVisionBoardPictureModel | null {
    Object.assign(this.defaultVisionBoardPictureModel, obj);
    Object.keys(this.defaultVisionBoardPictureModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.defaultVisionBoardPictureModel[key] && typeof this.defaultVisionBoardPictureModel[key] === 'object' && !(this.defaultVisionBoardPictureModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.defaultVisionBoardPictureModel[key] = null;
      }
    });


    return this.defaultVisionBoardPictureModel;
  }

  public createDefaultVisionBoardPicture(defaultVisionBoardPictureModel: DefaultVisionBoardPictureCreateModel, f: NgForm): void {
    this.httpService.createDefaultVisionBoardPicture(defaultVisionBoardPictureModel, getHeaderWithLoadingText("Saving..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Default Vision Board Picture', result);
          return;
        }
        this.dialogService.success('Successfully Created the Default Vision Board Picture.');
        f.resetForm();
        this.activeModal.close();
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to create Default Vision Board Picture'),
      complete: () => null,
    });
  }

  public updateDefaultVisionBoardPicture(defaultVisionBoardPictureModel: DefaultVisionBoardPictureUpdateModel, f: NgForm): void {
    this.httpService.updateDefaultVisionBoardPicture(defaultVisionBoardPictureModel, getHeaderWithLoadingText("Updating..")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Default Vision Board Picture', result);
          return;
        }
        this.dialogService.success('Successfully updated the Default Vision Board Picture.');
        f.resetForm();
        this.activeModal.close();
        if (this.isShowFullScreen && result.model?.defaultVisionBoardPictureId) this.getDefaultVisionBoardPictureInfo(result.model?.defaultVisionBoardPictureId);
        if (this.onCloseCallback) this.onCloseCallback(result.model);
      },
      error: (err) => this.dialogService.error('Not Able to update Default Vision Board Picture'),
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
