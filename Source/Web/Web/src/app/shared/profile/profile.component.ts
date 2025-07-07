import { AuditOperationModel, AuditOperationSearchCriteria, ChangePasswordModel, LoggedInUserModel, UserModel, UserPictureWithFileFormModel, UserUpdateWithFileFormModel } from 'src/common/models/app-model';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { PredefinedAuditOperationTypes, PredefinedUserTypes } from 'src/common/models/app-enums';

import { ALLOWED_PROFILE_IMAGE_EXTENSIONS } from 'src/common/constants/app.constants';
import { CommonDialogService } from '../services/common-dialog.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { SidebarLayoutTypes } from 'src/common/models/app-custom-enums';
import { Status } from 'src/common/models/app-custom-model';
import { Subject } from 'rxjs';
import { TitleService } from '../services/title.service';
import { getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss'],
    standalone: false
})
export class ProfileComponent implements OnInit {

  @ViewChild('vcFileInput') public fileInput!: ElementRef;
  @Input() public isMovieConfig: boolean = false;
  @Input() public movieId: number | undefined;
  @Input() public layoutTypeId: number | undefined;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public isConfigurator: boolean = false;
  @Input() public isProductionExecutive: boolean = false;
  @Input() public userModel: UserModel = new UserModel();
  @Input() public title: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isDeletable: boolean = false;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | undefined;
  public sidebarLayoutType: typeof SidebarLayoutTypes = SidebarLayoutTypes;
  public currentUser: LoggedInUserModel | null = null;

  public changePasswordModel: ChangePasswordModel = new ChangePasswordModel();
  public userFileUpload: File | undefined | null;
  public allowedFileTypes: string = ALLOWED_PROFILE_IMAGE_EXTENSIONS;
  public userProfilePicture: string | undefined | null;
  public fileName: string | undefined;
  public initialPassword: string = '';
  public userName__size: number = UserModel.userName__size;
  public email__size: number = UserModel.email__size;
  public firstName__size: number = UserModel.firstName__size;
  public lastName__size: number = UserModel.lastName__size;
  public workPhone__size: number = UserModel.workPhone__size;
  public workPhoneExt__size: number = UserModel.workPhoneExt__size;
  public cellPhone__size: number = UserModel.cellPhone__size;
  public predefinedUserTypes: Status[] = [];
  public selectedPredefinedUserType: Status | undefined;
  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading: boolean = false; 
  public auditOperations: AuditOperationModel[] = [];

  public constructor(private sessionService: SessionService,
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    public customSelectService: CustomSelectService,
    public enumViewService: EnumViewService,
    private selectService: SelectService,
    private titleService: TitleService,
  ) {
    this.titleService.pageTitle = "Profile";
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    if (this.currentUser && this.currentUser.userTypeId && this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.predefinedUserTypes = this.selectService.GetPredefinedUserTypes();
      this.isSuperAdmin = true;
    } else {
      this.predefinedUserTypes = this.selectService.GetPredefinedUserTypes()?.filter(item => item.id !== PredefinedUserTypes.SuperAdmin);
    }

    this.selectedPredefinedUserType = this.predefinedUserTypes.find(item => item.id === this.currentUser?.userTypeId);
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.selectedPredefinedUserType = this.predefinedUserTypes.find(item => item.id === this.currentUser?.userTypeId);
    this.getUserPictureUrl(this.userModel?.userId, this.currentUser?.userProfileGuid ?? "", this.currentUser?.token ?? "");
    this.getUser(this.currentUser?.userId ?? 0);
    const auditOperationSearchCriteria = new AuditOperationSearchCriteria();
    auditOperationSearchCriteria.predefinedAuditOperationTypeIds = [PredefinedAuditOperationTypes.Login, PredefinedAuditOperationTypes.Logout, PredefinedAuditOperationTypes.LoginDisabled, PredefinedAuditOperationTypes.LoginEnabled, PredefinedAuditOperationTypes.LoginFailed,];
    this.getAuditOperations(auditOperationSearchCriteria);
  }

  public getAuditOperations(searchModel: AuditOperationSearchCriteria): void {
    this.httpService.getAuditOperations(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Audit Operations", result);
          return;
        }
        this.auditOperations = result.model ?? [];
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Audit Operations"),
      complete: () => null,
    });
  }

  public getUser(userId: number): void {
    this.httpService.getUser(userId).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.userModel = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get User');
      },
      complete: () => null,
    });
  }

  public getUserPictureUrl(userId: number, profileGuid: string, token: string): void {
    this.userProfilePicture = this.httpService.getUserPictureUrl(userId, profileGuid, token);
  }

  public uploadFile(event: Event): void {
    const inputElementFiles = (event.currentTarget as HTMLInputElement)?.files;
    if (inputElementFiles && inputElementFiles.length > 0) {
      this.userFileUpload = inputElementFiles[0] ?? null;
      this.fileName = inputElementFiles[0]?.name;
      const file: File | undefined = inputElementFiles[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e: ProgressEvent<FileReader>): void => {
          this.userProfilePicture = e.target?.result as string;;
        };
        reader.readAsDataURL(file);
      }
    }
  }

  public onClearFiles(): void {
    this.userFileUpload = null;
    this.fileName = "";
    if ((this.fileInput.nativeElement as HTMLInputElement)) (this.fileInput.nativeElement as HTMLInputElement).value = '';
    this.userProfilePicture = null;
  }

  public uploadUserPicture(userId: number): void {
    const userPictureWithFileFormModel = new UserPictureWithFileFormModel();
    userPictureWithFileFormModel.userId = userId;
    if (this.userFileUpload) userPictureWithFileFormModel.uploadedFile = this.userFileUpload;
    this.httpService.uploadUserPicture(userPictureWithFileFormModel).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Movie Picture', result);
          return;
        }

      },
      error: (err: unknown) => this.dialogService.error('Not Able to update Movie Picture'),
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const userModel = this.generateModel(f.value);
    const userUpdateWithFileFormModel = new UserUpdateWithFileFormModel();
    Object.assign(userUpdateWithFileFormModel, userModel);
    this.updateUser(userUpdateWithFileFormModel, f);
  }

  public generateModel(obj: UserModel): UserModel | undefined {
    if (this.selectedPredefinedUserType == null)
      return;
    Object.assign(this.userModel, obj);
    Object.keys(this.userModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.userModel[key] && typeof this.userModel[key] === 'object' && !(this.userModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.userModel[key] = null;
      }
    });

    this.userModel.predefinedUserTypeId = this.selectedPredefinedUserType?.id ?? 0;
    this.userModel.accountLockedUntil = new Date(); 
    if (this.isCreateMode) this.userModel.active = true;
    return this.userModel;
  }

  public updateUser(userModel: UserUpdateWithFileFormModel, f: NgForm): void {
    this.httpService.updateUserByAdmin(userModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update User', result);
          return;
        }
        this.dialogService.success('Successfully updated the User.');
        if (this.userFileUpload && result.model) {
          this.uploadUserPicture(result.model?.userId);
        }
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update User'),
      complete: () => null,
    });
  }

}
