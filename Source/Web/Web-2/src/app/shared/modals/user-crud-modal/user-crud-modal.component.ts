import { ALLOWED_PROFILE_IMAGE_EXTENSIONS, MAXIMUM_PASSWORD_CHARACTER_LENGTH, MINIMUM_PASSWORD_CHARACTER_LENGTH } from 'src/common/constants/app.constants';
import { CompanyModel, CompanySearchCriteria, LoggedInUserModel, UserCreateWithFileFormModel, UserModel, UserPictureWithFileFormModel, UserUpdateWithFileFormModel } from 'src/common/models/app-model';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PredefinedUserPermissionTypes, PredefinedUserTypes } from 'src/common/models/app-enums';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { AppScreenLayoutTypes } from 'src/common/models/app-custom-enums';
import { CommonDialogService } from '../../services/common-dialog.service';
import { CompanyCrudModalComponent } from '../company-crud-modal/company-crud-modal.component';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';

@UntilDestroy()
@Component({
    selector: 'app-user-crud-modal',
    templateUrl: './user-crud-modal.component.html',
    styleUrls: ['./user-crud-modal.component.scss'],
    standalone: false
})
export class UserCrudModalComponent implements OnInit {

  @ViewChild('lvUserForm') public form!: NgForm;
  @Input() public userModel: UserModel = new UserModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isDeletable: boolean = false;
  @Input() public movieId: number | undefined;
  @Input() public companyId: number = 0;
  @Input() public layoutTypeId: number | undefined;
  @Input() public companyName: string | undefined;
  @Input() public isSuperAdmin: boolean = false;
  @ViewChild('vcFileInput') public fileInput!: ElementRef;

  public userFileUpload: File | undefined | null;
  public allowedFileTypes: string = ALLOWED_PROFILE_IMAGE_EXTENSIONS;
  public userProfilePicture: string | undefined | null;
  public fileName: string | undefined;
  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: ((res: UserModel | null) => void) | null = null;
  public initialPassword: string = '';
  public userName__size: number = UserModel.userName__size;
  public email__size: number = UserModel.email__size;
  public firstName__size: number = UserModel.firstName__size;
  public lastName__size: number = UserModel.lastName__size;
  public workPhone__size: number = UserModel.workPhone__size;
  public workPhoneExt__size: number = UserModel.workPhoneExt__size;
  public cellPhone__size: number = UserModel.cellPhone__size;
  public initialPasswordMin__size: number = MINIMUM_PASSWORD_CHARACTER_LENGTH;
  public initialPasswordMax__size: number = MAXIMUM_PASSWORD_CHARACTER_LENGTH;
  public sidebarLayoutType = AppScreenLayoutTypes
  public predefinedUserTypes: Status[] = [];
  public selectedPredefinedUserType: Status | undefined;
  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading: boolean = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;
  public companySearchModel: CompanySearchCriteria = new CompanySearchCriteria();
  public userType: typeof PredefinedUserTypes = PredefinedUserTypes;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public customSelectService: CustomSelectService,
    private modalService: NgbModal,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    if (this.isSuperAdmin) {
      this.predefinedUserTypes = this.selectService.GetPredefinedUserTypes();
      if ((this.layoutTypeId == AppScreenLayoutTypes.SuperAdminWithCompanyList || !this.layoutTypeId)) {
        this.getCompanies(this.companySearchModel);
        this.initTypeAheads();
      }
    } else {
      this.predefinedUserTypes = this.selectService.GetPredefinedUserTypes().filter(item => item.id !== PredefinedUserTypes.SuperAdmin);
    }
    if (!this.isCreateMode) {
      this.selectedPredefinedUserType = this.predefinedUserTypes.find(item => item.id === this.userModel.predefinedUserTypeId);
    }
    this.getuserPictureUrl(this.userModel.userId);
  }

  public initTypeAheads(): void {
    this.companyTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.companyTypeaheadLoading = true;
      const companySearchCriteria = new CompanySearchCriteria();
      Object.assign(companySearchCriteria, this.companySearchModel);
      companySearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) companySearchCriteria.selectedId = this.userModel.companyId;
      return this.httpService.getCompanies(companySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) {
          this.companyTypeaheadLoading = false;
          if (data.model) this.companys = data.model;
        }
      },
      error: err => {
        this.companyTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

  public getuserPictureUrl(userId: number): void {
    this.userProfilePicture = this.httpService.getUserPictureUrl(userId, this.currentUser?.token);
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
      error: (err) => this.dialogService.error('Not Able to update Movie Picture'),
      complete: () => null,
    });
  }

  public showAddCompanyModal(): void {
    const modalRef = this.modalService.open(CompanyCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as CompanyCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Company';
    componentInstance.onCloseCallback = (res: CompanyModel | null): void => {
      if (ObjectUtils.isObjNullOrUndefined(res)) return;
      this.getCompanies(this.companySearchModel, res?.companyId);
    }
  }

  public getCompanies(searchModel: CompanySearchCriteria, selectedCompanyId?: number): void {
    searchModel.sortAscending = false;
    searchModel.isActive = true;
    searchModel.sortBy = 'createdDate';
    searchModel.isActive = true;
    if (!this.isCreateMode) searchModel.selectedId = this.userModel.companyId;
    this.httpService.getCompanies(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.companys = data.model;
        if (!this.isCreateMode) this.selectedCompany = data.model?.find(item => item.companyId === this.userModel?.companyId);
        else this.selectedCompany = this.companys.find(item => item.companyId == this.companyId);

        if (selectedCompanyId) {
          this.selectedCompany = this.companys.find(item => item.companyId == selectedCompanyId);
        }
      },
      error: (error) => {
        this.dialogService.error('Not Able to get Company');
      },
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const userModel = this.generateModel(f.value);
    if (this.isCreateMode) {
      const userCreateWithFileFormModel = new UserCreateWithFileFormModel();
      Object.assign(userCreateWithFileFormModel, userModel);
      userCreateWithFileFormModel.newPermissionTypeIds = [PredefinedUserPermissionTypes.ConfigureCompany, PredefinedUserPermissionTypes.ConfigureSystem];
      this.createUser(userCreateWithFileFormModel, f);
    } else {
      const userUpdateWithFileFormModel = new UserUpdateWithFileFormModel();
      Object.assign(userUpdateWithFileFormModel, userModel);
      this.updateUser(userUpdateWithFileFormModel, f);
    }
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
    this.userModel.userGuid = '';
    if (this.isSuperAdmin && (this.layoutTypeId == AppScreenLayoutTypes.SuperAdminWithCompanyList || !this.layoutTypeId)) {
      this.userModel.companyId = this.selectedCompany?.companyId ?? 0;
    }
    else {
      this.userModel.companyId = this.companyId;
    }
    if (this.isCreateMode) this.userModel.active = true;
    return this.userModel;
  }

  public createUser(userModel: UserCreateWithFileFormModel, f: NgForm): void {
    this.httpService.createUser(userModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create User', result);
          return;
        }
        this.dialogService.success('Successfully Created the User.');
        if (result.model) {
          if (this.userFileUpload) {
            this.uploadUserPicture(result.model.userId);
          }
          this.activeModal.close();
          if (this.onCloseCallback) this.onCloseCallback(result.model);
        }
      },
      error: (err) => this.dialogService.error('Not Able to create User'),
      complete: () => null,
    });
  }

  public updateUser(userModel: UserUpdateWithFileFormModel, f: NgForm): void {
    this.httpService.updateUserByAdmin(userModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update User', result);
          return;
        }
        this.dialogService.success('Successfully updated the User.');
        if (result.model) {
          if (this.userFileUpload) {
            this.uploadUserPicture(result.model.userId);
          }
          this.activeModal.close();
          if (this.onCloseCallback) this.onCloseCallback(null);
        }
      },
      error: (err) => this.dialogService.error('Not Able to update User'),
      complete: () => null,
    });
  }

  public cancel(checkDirty: boolean): void {
    if (this.form.dirty && checkDirty) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'Discard').then(res => {
        if (res)
          return this.activeModal.close();
      });
    }
    else {
      this.activeModal.close();
    }
  }
}
