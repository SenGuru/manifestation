import { ALLOWED_PROFILE_IMAGE_EXTENSIONS, COUNTRY_ID_INDIA, MAXIMUM_PASSWORD_CHARACTER_LENGTH, MINIMUM_PASSWORD_CHARACTER_LENGTH } from 'src/common/constants/app.constants';
import { ChangeDetectorRef, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { CompanyCreateWithFileFormModel, CompanyModel, CompanyPictureWithFileFormModel, CompanyUpdateWithFileFormModel, CountryModel, LoggedInUserModel, MailServerModel, MailServerSearchCriteria, PushNotificationServerModel, PushNotificationServerSearchCriteria, SingleSignOnServerModel, SingleSignOnServerSearchCriteria, SmsServerModel, SmsServerSearchCriteria, StateModel, StateSearchCriteria, VoiceNotificationServerModel, VoiceNotificationServerSearchCriteria, WhatsAppNotificationServerModel, WhatsAppNotificationServerSearchCriteria } from 'src/common/models/app-model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { Router } from '@angular/router';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';
import { TitleService } from '../../services/title.service';

@UntilDestroy()
@Component({
  selector: 'app-company-crud-modal',
  templateUrl: './company-crud-modal.component.html',
  standalone: false,
})
export class CompanyCrudModalComponent implements OnInit {

  @ViewChild('lvCompanyForm') public form!: NgForm;
  @Input() public companyModel: CompanyModel = new CompanyModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public isDeletable: boolean = false;
  @Input() public isShowFullScreen: boolean = false;
  @Input() public isSuperAdmin: boolean = false;
  @Input() public companyId: number = 0;
  @Input() public companyName: string | undefined;
  @Input() public layoutTypeId: number | undefined;
  @ViewChild('vcFileInput') public fileInput!: ElementRef;

  public stateTypeahead$ = new Subject<string>();
  public stateTypeaheadLoading: boolean = false;
  public mailServerTypeahead$ = new Subject<string>();
  public mailServerTypeaheadLoading: boolean = false;
  public singleSignOnServerTypeahead$ = new Subject<string>();
  public singleSignOnServerTypeaheadLoading: boolean = false;
  public countryTypeahead$ = new Subject<string>();
  public countryTypeaheadLoading: boolean = false;
  public smsServerSearchCriteria = new SmsServerSearchCriteria();
  public smsServerTypeahead$ = new Subject<string>();
  public smsServerTypeaheadLoading: boolean = false;
  public smsServers: SmsServerModel[] = [];
  public selectedSmsServer: SmsServerModel | undefined | null;
  public whatsAppNotificationServerSearchCriteria = new WhatsAppNotificationServerSearchCriteria();
  public whatsAppNotificationServerTypeahead$ = new Subject<string>();
  public whatsAppNotificationServerTypeaheadLoading: boolean = false;
  public whatsAppNotificationServers: WhatsAppNotificationServerModel[] = [];
  public selectedWhatsAppNotificationServer: WhatsAppNotificationServerModel | undefined | null;
  public voiceNotificationServerSearchCriteria = new VoiceNotificationServerSearchCriteria();
  public voiceNotificationServerTypeahead$ = new Subject<string>();
  public voiceNotificationServerTypeaheadLoading: boolean = false;
  public voiceNotificationServers: VoiceNotificationServerModel[] = [];
  public selectedVoiceNotificationServer: VoiceNotificationServerModel | undefined | null;
  public pushNotificationServerSearchCriteria = new PushNotificationServerSearchCriteria();
  public pushNotificationServerTypeahead$ = new Subject<string>();
  public pushNotificationServerTypeaheadLoading: boolean = false;
  public pushNotificationServers: PushNotificationServerModel[] = [];
  public selectedPushNotificationServer: PushNotificationServerModel | undefined | null;
  public digitalSignatureServerTypeahead$ = new Subject<string>();
  public digitalSignatureServerTypeaheadLoading: boolean = false;
  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: ((res: CompanyModel | null) => void) | null = null;
  public licenseStartDate: Date | null = null;
  public licenseEndDate: Date | null = null;
  public companyFileUpload: File | undefined | null;
  public allowedFileTypes: string = ALLOWED_PROFILE_IMAGE_EXTENSIONS;
  public companyProfilePicture: string | undefined | null;
  public fileName: string | undefined;
  public companyName__size: number = CompanyModel.companyName__size;
  public email__size: number = CompanyModel.email__size;
  public contactFirstName__size: number = CompanyModel.contactFirstName__size;
  public contactLastName__size: number = CompanyModel.contactLastName__size;
  public workPhone__size: number = CompanyModel.workPhone__size;
  public workPhoneExt__size: number = CompanyModel.workPhoneExt__size;
  public cellPhone__size: number = CompanyModel.cellPhone__size;
  public address1__size: number = CompanyModel.address1__size;
  public address2__size: number = CompanyModel.address2__size;
  public city__size: number = CompanyModel.city__size;
  public website__size: number = CompanyModel.website__size;
  public subdomain__size: number = CompanyModel.subdomain__size;
  public stateSearchCriteria: StateSearchCriteria = new StateSearchCriteria();
  public mailServerSearchCriteria: MailServerSearchCriteria = new MailServerSearchCriteria();
  public singleSignOnSearchCriteria: SingleSignOnServerSearchCriteria = new SingleSignOnServerSearchCriteria();

  public states: StateModel[] = [];
  public selectedState: StateModel | undefined | null;
  public countrys: CountryModel[] = [];
  public selectedCountry: CountryModel | undefined | null;
  public mailServers: MailServerModel[] = [];
  public selectedMailServer: MailServerModel | undefined | null;
  public singleSignOnServers: SingleSignOnServerModel[] = [];
  public selectedSingleSignOnServer: SingleSignOnServerModel | undefined | null;

  public predefinedCurrencyTypes: Status[] = [];
  public selectedPredefinedCurrencyType: Status | undefined | null;
  public initialPasswordMin__size: number = MINIMUM_PASSWORD_CHARACTER_LENGTH;
  public initialPasswordMax__size: number = MAXIMUM_PASSWORD_CHARACTER_LENGTH;
  public initialPassword: string | undefined | null;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    public customSelectService: CustomSelectService,
    private routerDataService: RouterDataService,
    private router: Router,
    private selectService: SelectService,
    private cdRef: ChangeDetectorRef,
    private modalService: NgbModal,
    private titleService: TitleService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedCurrencyTypes = this.selectService.GetPredefinedCurrencyTypes();
    if (this.isCreateMode) {
      this.companyModel.useMailNotification = true;
    } else {
      this.initSelectData();
    }
    const routerParams = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    if (this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.isSuperAdmin = true;
      // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
      this.companyId = RouterUtils.getRouterDataNumber(routerParams?.['companyId']) ?? 0;
      this.mailServerSearchCriteria.companyId = this.companyId;
      this.getMailServers(this.mailServerSearchCriteria);
      this.getSmsServers(this.smsServerSearchCriteria);
      this.getPushNotificationServers(this.pushNotificationServerSearchCriteria);
      this.getVoiceNotificationServers(this.voiceNotificationServerSearchCriteria);
      this.getWhatsAppNotificationServers(this.whatsAppNotificationServerSearchCriteria);
      this.getSingleSignOnServers(this.singleSignOnSearchCriteria);
    } else {
      this.companyId = this.currentUser?.companyId ?? 0;
    }
    this.initCompanyInfoAsFullScreen();
    this.initTypeAheads();
  }

  public initSelectData(): void {
    this.selectedPredefinedCurrencyType = this.predefinedCurrencyTypes.find(item => item.id == this.companyModel.predefinedCurrencyTypeId);
  }

  public initCompanyInfoAsFullScreen(): void {
    if (this.isShowFullScreen) {
      this.buttonText = "Save";
      this.isEditMode = true;
      this.getCompanyInfo(this.companyId);
      this.getCompanyPictureUrl(this.companyId, this.currentUser?.userProfileGuid ?? "", this.currentUser?.token ?? "");

      this.titleService.pageTitle = "Company info";
    } else {
      this.getCountrys('');
      this.getCompanyPictureUrl(this.companyId, this.currentUser?.userProfileGuid ?? "", this.currentUser?.token ?? "");
    }
  }

  public initTypeAheads(): void {
    this.stateTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.stateTypeaheadLoading = true;
      const stateSearchCriteria = new StateSearchCriteria();
      Object.assign(stateSearchCriteria, this.stateSearchCriteria);
      stateSearchCriteria.keyword = searchValue;
      if (this.selectedCountry) stateSearchCriteria.countryId = this.selectedCountry.countryId;
      return this.httpService.getStates(stateSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.stateTypeaheadLoading = false;
        if (data.model) this.states = data.model;
      },
      error: err => {
        this.stateTypeaheadLoading = false;
      },
      complete: () => null,
    });

    this.countryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.countryTypeaheadLoading = true;
      return this.httpService.getSupportedCountries(getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.countryTypeaheadLoading = false;
        if (data.model) this.countrys = data.model;
      },
      error: err => {
        this.countryTypeaheadLoading = false;
      },
      complete: () => null,
    });

    this.mailServerTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.mailServerTypeaheadLoading = true;
      const mailServerSearchCriteria = new MailServerSearchCriteria();
      mailServerSearchCriteria.keyword = searchValue;
      return this.httpService.getMailServers(mailServerSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.mailServerTypeaheadLoading = false;
        if (data.model) this.mailServers = data.model;
      },
      error: err => {
        this.mailServerTypeaheadLoading = false;
      },
      complete: () => null,
    });

    this.singleSignOnServerTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.singleSignOnServerTypeaheadLoading = true;
      const singleSignOnServerSearchCriteria = new SingleSignOnServerSearchCriteria();
      singleSignOnServerSearchCriteria.keyword = searchValue;
      return this.httpService.getSingleSignOnServers(singleSignOnServerSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.singleSignOnServerTypeaheadLoading = false;
        if (data.model) this.singleSignOnServers = data.model;
      },
      error: err => {
        this.singleSignOnServerTypeaheadLoading = false;
      },
      complete: () => null,
    });
    this.smsServerTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.smsServerTypeaheadLoading = true;
      const smsServerSearchCriteria = new SmsServerSearchCriteria();
      smsServerSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) smsServerSearchCriteria.selectedId = this.companyModel.smsServerId;
      return this.httpService.getSmsServers(smsServerSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.smsServerTypeaheadLoading = false;
        if (data.model) this.smsServers = data.model;
      },
      error: err => {
        this.smsServerTypeaheadLoading = false;
      },
      complete: () => null,
    });
    this.whatsAppNotificationServerTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.whatsAppNotificationServerTypeaheadLoading = true;
      const whatsAppNotificationServerSearchCriteria = new WhatsAppNotificationServerSearchCriteria();
      whatsAppNotificationServerSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) whatsAppNotificationServerSearchCriteria.selectedId = this.companyModel.whatsAppNotificationServerId;
      return this.httpService.getWhatsAppNotificationServers(whatsAppNotificationServerSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.whatsAppNotificationServerTypeaheadLoading = false;
        if (data.model) this.whatsAppNotificationServers = data.model;
      },
      error: err => {
        this.whatsAppNotificationServerTypeaheadLoading = false;
      },
      complete: () => null,
    });
    this.voiceNotificationServerTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.voiceNotificationServerTypeaheadLoading = true;
      const voiceNotificationServerSearchCriteria = new VoiceNotificationServerSearchCriteria();
      voiceNotificationServerSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) voiceNotificationServerSearchCriteria.selectedId = this.companyModel.voiceNotificationServerId;
      return this.httpService.getVoiceNotificationServers(voiceNotificationServerSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.voiceNotificationServerTypeaheadLoading = false;
        if (data.model) this.voiceNotificationServers = data.model;
      },
      error: err => {
        this.voiceNotificationServerTypeaheadLoading = false;
      },
      complete: () => null,
    });
    this.pushNotificationServerTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.pushNotificationServerTypeaheadLoading = true;
      const pushNotificationServerSearchCriteria = new PushNotificationServerSearchCriteria();
      pushNotificationServerSearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) pushNotificationServerSearchCriteria.selectedId = this.companyModel.pushNotificationServerId;
      return this.httpService.getPushNotificationServers(pushNotificationServerSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.pushNotificationServerTypeaheadLoading = false;
        if (data.model) this.pushNotificationServers = data.model;
      },
      error: err => {
        this.pushNotificationServerTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

  public getCompanyPictureUrl(companyId: number, profileGuid: string, token: string): void {
    this.companyProfilePicture = this.httpService.getCompanyPictureUrl(companyId, profileGuid, token);
    this.cdRef.detectChanges();
  }

  public uploadFile(event: Event): void {
    const inputElementFiles = (event.currentTarget as HTMLInputElement)?.files;
    if (inputElementFiles && inputElementFiles.length > 0) {
      this.companyFileUpload = inputElementFiles[0] ?? null;
      this.fileName = inputElementFiles[0]?.name;
      const file: File | undefined = inputElementFiles[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e: ProgressEvent<FileReader>): void => {
          this.companyProfilePicture = e.target?.result as string;;
        };
        reader.readAsDataURL(file);
      }
    }
  }

  public onClearFiles(): void {
    this.companyFileUpload = null;
    this.fileName = "";
    (this.fileInput.nativeElement as HTMLInputElement).value = '';
    this.getCompanyPictureUrl(this.companyId, this.currentUser?.userProfileGuid ?? "", this.currentUser?.token ?? "");
  }

  public uploadCompanyPicture(companyId: number): void {
    const companyPictureWithFileFormModel = new CompanyPictureWithFileFormModel();
    companyPictureWithFileFormModel.companyId = companyId;
    if (this.companyFileUpload) companyPictureWithFileFormModel.uploadedFile = this.companyFileUpload;
    this.httpService.uploadCompanyPicture(companyPictureWithFileFormModel).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Picture', result);
          return;
        }
        this.companyFileUpload = null;
        if (!this.isCreateMode) {
          this.getCompanyPictureUrl(this.companyId, this.currentUser?.userProfileGuid ?? "", this.currentUser?.token ?? "");
        }
      },
      error: (err) => this.dialogService.error('Not Able to update Picture'),
      complete: () => null,
    });
  }

  public getCompanyInfo(companyId: number): void {
    this.httpService.getCompany(companyId).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to get Company', result);
          return;
        }
        if (result.model) this.companyModel = result.model;
        this.initSelectData();
        this.getMailServers(this.mailServerSearchCriteria);
        this.getSingleSignOnServers(this.singleSignOnSearchCriteria);
        this.getCountrys('');
      },
      error: (err) => this.dialogService.error('Not Able to get Company'),
      complete: () => null,
    });
  }

  public scrollToBottom(): void {
    const modalScrollDiv = document.getElementById("companyCrudModal");
    if (modalScrollDiv) modalScrollDiv.scrollBy({ behavior: 'smooth', top: modalScrollDiv.scrollHeight })
  }

  public getMailServers(searchModel: MailServerSearchCriteria, selectedMailServerId?: number): void {
    this.httpService.getMailServers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Mail Servers", result);
          return;
        }
        this.mailServers = result.model ?? [];
        if (!this.isCreateMode) this.selectedMailServer = this.mailServers.find(item => item.mailServerId == this.companyModel.mailServerId);
        if (selectedMailServerId) {
          this.selectedMailServer = this.mailServers.find(item => item.mailServerId == selectedMailServerId);
        }
      },
      error: (err) => this.dialogService.error("Unable to get Mail Servers"),
      complete: () => null,
    });
  }

  public getSingleSignOnServers(searchModel: SingleSignOnServerSearchCriteria, selectedSingleSignOnServerId?: number): void {
    this.httpService.getSingleSignOnServers(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Single Sign-On Servers", result);
          return;
        }
        this.singleSignOnServers = result.model ?? [];
        if (!this.isCreateMode) this.selectedSingleSignOnServer = this.singleSignOnServers.find(item => item.singleSignOnServerId == this.companyModel.singleSignOnServerId);
        if (selectedSingleSignOnServerId) {
          this.selectedSingleSignOnServer = this.singleSignOnServers.find(item => item.singleSignOnServerId == selectedSingleSignOnServerId);
        }
      },
      error: (err) => this.dialogService.error("Unable to get Single Sign-On Servers"),
      complete: () => null,
    });
  }

  public getSmsServers(searchModel: SmsServerSearchCriteria, smsServerId?: number): void {
    searchModel.sortAscending = false;
    searchModel.companyId = this.companyId ?? 0;
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.companyModel.smsServerId;
    this.httpService.getSmsServers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.smsServers = data.model;
          if (!this.isCreateMode) this.selectedSmsServer = data.model.find(item => item.smsServerId === this.companyModel.smsServerId);
          if (smsServerId) this.selectedSmsServer = data.model.find(item => item.smsServerId === smsServerId);
        }
      },
      error: (error) => {
        this.dialogService.error('Not Able to get Sms Server');
      },
      complete: () => null,
    });
  }

  public getWhatsAppNotificationServers(searchModel: WhatsAppNotificationServerSearchCriteria, selectedWhatsAppNotificationServerId?: number): void {
    searchModel.sortAscending = false;
    searchModel.companyId = this.companyId ?? 0;
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.companyModel.whatsAppNotificationServerId;
    this.httpService.getWhatsAppNotificationServers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.whatsAppNotificationServers = data.model;
          if (!this.isCreateMode) this.selectedWhatsAppNotificationServer = data.model.find(item => item.whatsAppNotificationServerId === this.companyModel.whatsAppNotificationServerId);
          if (selectedWhatsAppNotificationServerId) this.selectedWhatsAppNotificationServer = data.model.find(item => item.whatsAppNotificationServerId === selectedWhatsAppNotificationServerId);
        }
      },
      error: (error) => {
        this.dialogService.error('Not Able to get Whats App Notification Server');
      },
      complete: () => null,
    });
  }

  public getVoiceNotificationServers(searchModel: VoiceNotificationServerSearchCriteria, selectedVoiceNotificationServerId?: number): void {
    searchModel.sortAscending = false;
    searchModel.companyId = this.companyId ?? 0;
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.companyModel.voiceNotificationServerId;
    this.httpService.getVoiceNotificationServers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.voiceNotificationServers = data.model;
          if (!this.isCreateMode) this.selectedVoiceNotificationServer = data.model.find(item => item.voiceNotificationServerId === this.companyModel.voiceNotificationServerId);
          if (selectedVoiceNotificationServerId) this.selectedVoiceNotificationServer = data.model.find(item => item.voiceNotificationServerId === selectedVoiceNotificationServerId);
        }
      },
      error: (error) => {
        this.dialogService.error('Not Able to get Voice Notification Server');
      },
      complete: () => null,
    });
  }

  public changeSmsCheckbox(value: boolean): void {
    if (!value) {
      this.selectedSmsServer = null;
      this.companyModel.numberOfSmsNotificationsAllowed = null;
      this.companyModel.numberOfSmsNotificationsUsed = null;
    }
  }

  public changeVoiceNotificationCheckbox(value: boolean): void {
    if (!value) {
      this.selectedVoiceNotificationServer = null;
      this.companyModel.numberOfVoiceNotificationsAllowed = null;
      this.companyModel.numberOfVoiceNotificationsUsed = null;
    }
  }

  public changePushNotificationtheckbox(value: boolean): void {
    if (!value) {
      this.selectedPushNotificationServer = null;
      this.companyModel.numberOfPushNotificationsAllowed = null;
      this.companyModel.numberOfPushNotificationsUsed = null;
    }
  }

  public changeWhatsappNotificationtheckbox(value: boolean): void {
    if (!value) {
      this.selectedWhatsAppNotificationServer = null;
      this.companyModel.numberOfWhatsAppNotificationsAllowed = null;
      this.companyModel.numberOfWhatsAppNotificationsUsed = null;
    }
  }

  public getPushNotificationServers(searchModel: PushNotificationServerSearchCriteria, selectedPushNotificationServerId?: number): void {
    searchModel.sortAscending = false;
    searchModel.companyId = this.companyId ?? 0;
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.companyModel.pushNotificationServerId;
    this.httpService.getPushNotificationServers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.pushNotificationServers = data.model;
          if (!this.isCreateMode) this.selectedPushNotificationServer = data.model.find(item => item.pushNotificationServerId === this.companyModel.pushNotificationServerId);
          if (selectedPushNotificationServerId) this.selectedPushNotificationServer = data.model.find(item => item.pushNotificationServerId === selectedPushNotificationServerId);
        }
      },
      error: (error) => {
        this.dialogService.error('Not Able to get Push Notification Server');
      },
      complete: () => null,
    });
  }

  public changeSingleSignOnServer(): void {
    if (this.isCreateMode) this.initialPassword = '';
  }

  public getStates(stateSearchCriteria: StateSearchCriteria): void {
    this.httpService.getStates(stateSearchCriteria).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to get states', result);
          return;
        }
        this.states = result.model ?? [];
        if (!this.isCreateMode) {
          this.selectedState = this.states.find(item => item.stateId == this.companyModel.stateId);
        }
      },
      error: (err) => this.dialogService.error('Not Able to get states'),
      complete: () => null,
    });
  }

  public getCountrys(keyword: string): void {
    this.httpService.getSupportedCountries().subscribe(result => {
      if (!result.isSuccess) return;
      this.countrys = result.model ?? [];
      if (this.countrys.length === 0) return;
      if (!ObjectUtils.isNullOrUndefined(this.companyModel.countryId) && this.companyModel.countryId !== 0) {
        this.selectedCountry = this.countrys.find(item => item.countryId == this.companyModel.countryId);
        this.stateSearchCriteria.countryId = this.selectedCountry ? this.selectedCountry.countryId : null;
        this.getStates(this.stateSearchCriteria);
      } else {
        this.selectedCountry = this.countrys.find(item => item.countryId == COUNTRY_ID_INDIA);
        this.stateSearchCriteria.countryId = COUNTRY_ID_INDIA;
        this.getStates(this.stateSearchCriteria);
      }
      return;
    });
  }

  public onChangeCountry(selectedCountry: CountryModel | undefined | null): void {
    if (selectedCountry) {
      this.selectedState = null;
      const stateSearchCriteria = new StateSearchCriteria();
      stateSearchCriteria.countryId = selectedCountry.countryId;
      this.getStates(stateSearchCriteria);
    }
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const companyModel = this.generateModel(f.value);
    if (this.isCreateMode) {
      const companyCreateWithFileFormModel = new CompanyCreateWithFileFormModel();
      Object.assign(companyCreateWithFileFormModel, companyModel);
      companyCreateWithFileFormModel.singleSignOnServerId = this.selectedSingleSignOnServer ? this.selectedSingleSignOnServer.singleSignOnServerId : null;
      if (this.selectedSingleSignOnServer && this.isCreateMode) companyCreateWithFileFormModel.initialPassword = '';
      this.createCompany(companyCreateWithFileFormModel, f);
    } else {
      const companyUpdateWithFileFormModel = new CompanyUpdateWithFileFormModel();
      Object.assign(companyUpdateWithFileFormModel, companyModel);
      this.updateCompany(companyUpdateWithFileFormModel, f);
    }
  }

  public generateModel(obj: CompanyModel): CompanyModel | undefined {
    if (this.selectedCountry == null)
      return;
    Object.assign(this.companyModel, obj);
    Object.keys(this.companyModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.companyModel[key] && typeof this.companyModel[key] === 'object' && !(this.companyModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.companyModel[key] = null;
      }
    });

    this.companyModel.countryId = this.selectedCountry ? this.selectedCountry.countryId : null;
    this.companyModel.stateId = this.selectedState ? this.selectedState.stateId : null;
    this.companyModel.mailServerId = this.selectedMailServer ? this.selectedMailServer.mailServerId : null;
    this.companyModel.predefinedCurrencyTypeId = this.selectedPredefinedCurrencyType?.id ?? 0;
    this.companyModel.singleSignOnServerId = this.selectedSingleSignOnServer ? this.selectedSingleSignOnServer.singleSignOnServerId : null;
    this.companyModel.smsServerId = this.selectedSmsServer ? this.selectedSmsServer.smsServerId : null;
    this.companyModel.pushNotificationServerId = this.selectedPushNotificationServer ? this.selectedPushNotificationServer.pushNotificationServerId : null;
    this.companyModel.voiceNotificationServerId = this.selectedVoiceNotificationServer ? this.selectedVoiceNotificationServer.voiceNotificationServerId : null;
    this.companyModel.whatsAppNotificationServerId = this.selectedWhatsAppNotificationServer ? this.selectedWhatsAppNotificationServer.whatsAppNotificationServerId : null;
    this.companyModel.active = true;
    return this.companyModel;
  }

  public createCompany(companyModel: CompanyCreateWithFileFormModel, f: NgForm): void {
    this.httpService.createCompany(companyModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create Company', result);
          return;
        }
        if (result.model) {
          this.dialogService.success('Successfully Created the Company.');
          if (this.companyFileUpload) {
            this.uploadCompanyPicture(result.model.companyId);
          }
          this.activeModal.close();
          if (this.onCloseCallback) this.onCloseCallback(result.model);
        }
      },
      error: (err) => this.dialogService.error('Not Able to create Company'),
      complete: () => null,
    });
  }

  public updateCompany(companyModel: CompanyUpdateWithFileFormModel, f: NgForm): void {
    this.httpService.updateCompany(companyModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update Company', result);
          return;
        }
        if (result.model) {
          this.dialogService.success('Successfully updated the Company.');
          if (this.companyFileUpload) {
            this.uploadCompanyPicture(result.model.companyId);
          }
          this.activeModal.close();
          if (this.onCloseCallback) this.onCloseCallback(null);
        }
      },
      error: (err) => this.dialogService.error('Not Able to update Company'),
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
