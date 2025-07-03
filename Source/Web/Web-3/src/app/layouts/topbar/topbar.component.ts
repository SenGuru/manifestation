import { Component, DOCUMENT, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { NavigationEnd, Params, Router } from '@angular/router'; 
import { Observable, debounceTime, distinctUntilChanged, map } from 'rxjs';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { UntypedFormControl } from '@angular/forms';
import { ReturnListModel } from 'src/common/framework/common/models/result/return-list.model';
import { GlobalSearchFeature, GlobalSearchRecordsCount, LightAndDarkModeTypes } from 'src/common/models/app-custom-model';
import { PredefinedCompanyTypes, PredefinedUserPermissionTypes, PredefinedUserTypes } from 'src/common/models/app-enums';
import { CompanyModel, CompanySearchCriteria, LoggedInUserModel, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { EventService } from '../service/event.service';
import { SidebarLayoutTypes } from 'src/common/models/app-custom-enums';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';
import { ChangePasswordComponent } from 'src/app/shared/modals/change-password/change-password.component';
import { UserCrudModalComponent } from 'src/app/shared/modals/user-crud-modal/user-crud-modal.component';

@UntilDestroy()
@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss'],
  standalone: false,
})

/**
 * Topbar component
 */
export class TopbarComponent implements OnInit {

  @Output() public mobileMenuButtonClicked = new EventEmitter();
  @Input() public enable: GlobalSearchFeature[] = [];
  @Input() public companyId: number = 0;
  @Input() public companyName: string | undefined;
  public lightAndDarkMode: string | undefined;
  public isDarkThemeMode: boolean = false;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public element: any;
  public initialUrl = '';
  public theme: string = '';
  public currentUser: LoggedInUserModel | null | undefined;
  public layoutTypeId: number | undefined;
  public isSuperAdmin = false;
  public UserPermissionTypes: typeof PredefinedUserPermissionTypes = PredefinedUserPermissionTypes;
  public searchFromControl = new UntypedFormControl('');
  public isCompanySearchLoading = true;
  public companySearchResult: ReturnListModel<CompanyModel> | null = null;
  public isUserSearchLoading = true;
  public userSearchResult: ReturnListModel<UserModel> | null = null;
  public globalSearchRecordsCount: GlobalSearchRecordsCount = new GlobalSearchRecordsCount();
  public userProfilePicture: string | undefined;
  public companyInfo: CompanyModel | null = null;
   
  public constructor(
    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types, @typescript-eslint/no-explicit-any
    @Inject(DOCUMENT) private document: any,
    private commonService: CommonDialogService,
    private eventService: EventService,
    private routerDataService: RouterDataService,
    private sessionService: SessionService,
    private companySessionService: CompanySessionService,
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private modalService: NgbModal, private router: Router) { }

  openMobileMenu: boolean | undefined;

  ngOnInit() {
    // this.initialAppState = initialState;
    // this.store.select('layout').subscribe((data) => {
    //   this.theme = data.DATA_LAYOUT;
    // })
    this.openMobileMenu = false;
    this.element = document.documentElement;
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.getUserPictureUrl(this.currentUser?.userId ?? 0, this.currentUser?.userProfileGuid ?? "", this.currentUser?.token ?? "");
    this.initialUrl = this.sessionService.getInitialUrl();
    this.element = document.documentElement;
    this.isDarkThemeMode = this.sessionService.isDarkThemeMode();
    if (this.isDarkThemeMode) {
      this.changeMode('dark');
    } else {
      this.changeMode('light');
    }
    // eslint-disable-next-line 
    this.router.events.pipe(untilDestroyed(this)).subscribe((event: any) => {
      if (document.documentElement.getAttribute('data-layout') != "twocolumn") {
        if (event instanceof NavigationEnd || event?.routerEvent instanceof NavigationEnd) {
          const routerParams: Params = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
          // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
          this.layoutTypeId = RouterUtils.getRouterDataNumber(routerParams['layoutTypeId']) ?? 0;
          if (this.currentUser && this.currentUser?.userTypeId && this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin) {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
            this.companyId = RouterUtils.getRouterDataNumber(routerParams['companyId']) ?? 0;
            this.isSuperAdmin = true;
            if (this.layoutTypeId !== SidebarLayoutTypes.CompanyList) {
              this.loadSessionData(this.companyId);
            }
          } else {
            this.companyId = this.currentUser?.companyId ?? 0;
            this.loadSessionData(this.companyId);
          }

          this.initializeSearch();
          this.enableSearchOptions();
        }
      }
    });
    this.initialUrl = this.sessionService.getInitialUrl();
    this.getUserPictureUrl(this.currentUser?.userId ?? 0, this.currentUser?.userProfileGuid ?? "", this.currentUser?.token ?? "");
  }

  public loadSessionData(companyId: number): void {
    if (ObjectUtils.isNullOrUndefined(companyId) || companyId <= 0) return;
    const sessionCompany = this.companySessionService.getCurrentCompany();
    if (sessionCompany && sessionCompany?.companyId === companyId) {
      this.companyName = sessionCompany?.companyName;
      this.companyInfo = sessionCompany;
    } else {
      this.getCompanyInfo(companyId);
    }
  }

  public getCompanyInfo(companyId: number): void {
    this.httpService.getCompany(companyId).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Company", result);
          return;
        }
        if (result.model) {
          this.companyInfo = result.model;
          this.companySessionService.setCurrentCompany(result.model);
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Company"),
      complete: () => null,
    });
  }

  public initializeSearch(): void {
    this.searchFromControl.valueChanges.pipe(debounceTime(800), distinctUntilChanged(), untilDestroyed(this)).subscribe({
      next: (keyword: string) => {
        this.globalSearchRecordsCount = new GlobalSearchRecordsCount();
        this.isCompanySearchLoading = true;
        this.isUserSearchLoading = true;
        this.companySearchResult = null;
        this.userSearchResult = null;

        const companySearchModel: CompanySearchCriteria = new CompanySearchCriteria();
        companySearchModel.page = 1;
        companySearchModel.pageSize = 5;
        companySearchModel.keyword = keyword;
        companySearchModel.isActive = null;

        const userSearchModel: UserSearchCriteria = new UserSearchCriteria();
        userSearchModel.page = 1;
        userSearchModel.pageSize = 5;
        if (!ObjectUtils.isNullOrUndefined(this.companyId) && !Number.isNaN(this.companyId)) userSearchModel.companyIds = [this.companyId];
        userSearchModel.keyword = keyword;
        userSearchModel.active = true;

        if (this.enable.includes('company') && this.isSuperAdmin) {
          this.httpService.getCompanies(companySearchModel, getHeaderWithBlockUI()).subscribe({
            next: data => {
              this.isCompanySearchLoading = false;
              this.companySearchResult = data;
              this.globalSearchRecordsCount.companyCount = data.totalRecords ?? 0;
            }, error: (err: unknown) => {
              this.dialogService.error("Unable to get companies");
              this.isCompanySearchLoading = false;
            },
            complete: () => this.isCompanySearchLoading = false
          });
        }

        if (this.enable.includes('user') && this.isSuperAdmin) {
          this.httpService.getUsers(userSearchModel, getHeaderWithBlockUI()).subscribe({
            next: data => {
              this.isUserSearchLoading = false;
              this.userSearchResult = data;
              this.globalSearchRecordsCount.userCount = data.totalRecords ?? 0;
            }, error: (err: unknown) => {
              this.dialogService.error("Unable to get users");
              this.isUserSearchLoading = false;
            },
            complete: () => this.isUserSearchLoading = false
          });
        }

      }, error: (err: unknown) => {
        this.dialogService.error("Unable to get Data");
      },
      complete: () => null,
    });
  }

  public enableSearchOptions(): void {
    if (this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin) {
      if (this.layoutTypeId == SidebarLayoutTypes.CompanyList || ObjectUtils.isNullOrUndefined(this.layoutTypeId)) {
        this.enable = [
          'company',
          'user',
        ];
      } else {
        this.enable = [
          'company',
          'appointment',
          'customer',
          'rule',
          'user',
        ];
      }
    } else {
      this.enable = [
        'appointment',
        'customer',
        'rule',
      ];
    }
  }

  public getUserPictureUrl(userId: number, profileGuid: string, token: string): void {
    this.userProfilePicture = this.httpService.getUserPictureUrl(userId, profileGuid, token);
  }

  /**
   * Toggle the menu bar when having mobile screen
   */
  toggleMobileMenu(event: any) {
    event.preventDefault();
    this.mobileMenuButtonClicked.emit();
  }

  /**
   * Logout the user
   */
  logout() {
    this.commonService
      .confirmWithSmall("Log Out", "Are you sure you want to logout ?")
      .then((response) => {
        if (response) {
          this.sessionService.logOut();
        }
      }).catch((err: unknown) => null);
  }

  public changeMode(lightAndDarkModeType: LightAndDarkModeTypes): void {
    this.lightAndDarkMode = lightAndDarkModeType;
    this.eventService.broadcast('changeMode', lightAndDarkModeType);

    if (lightAndDarkModeType === 'dark') {
      document.documentElement.setAttribute('data-bs-theme', "dark");
      this.isDarkThemeMode = true;
      this.sessionService.setDarkThemeMode(this.isDarkThemeMode);
    } else {
      document.documentElement.setAttribute('data-bs-theme', "light");
      this.isDarkThemeMode = false;
      this.sessionService.setDarkThemeMode(this.isDarkThemeMode);
    }
  }

  /**
   * Fullscreen method
   */
  fullscreen() {
    document.body.classList.toggle('fullscreen-enable');
    if (
      !document.fullscreenElement && !this.element.mozFullScreenElement &&
      !this.element.webkitFullscreenElement) {
      if (this.element.requestFullscreen) {
        this.element.requestFullscreen();
      } else if (this.element.mozRequestFullScreen) {
        /* Firefox */
        this.element.mozRequestFullScreen();
      } else if (this.element.webkitRequestFullscreen) {
        /* Chrome, Safari and Opera */
        this.element.webkitRequestFullscreen();
      } else if (this.element.msRequestFullscreen) {
        /* IE/Edge */
        this.element.msRequestFullscreen();
      }
    } else {
      if (this.document.exitFullscreen) {
        this.document.exitFullscreen();
      } else if (this.document.mozCancelFullScreen) {
        /* Firefox */
        this.document.mozCancelFullScreen();
      } else if (this.document.webkitExitFullscreen) {
        /* Chrome, Safari and Opera */
        this.document.webkitExitFullscreen();
      } else if (this.document.msExitFullscreen) {
        /* IE/Edge */
        this.document.msExitFullscreen();
      }
    }
  }

  public selectCompany(item: CompanyModel): void {
    this.resetSearch();
    if (item.predefinedCompanyTypeId === PredefinedCompanyTypes.System) return;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      if (this.isSuperAdmin) {
        this.router.navigateByUrl(`${this.initialUrl}/company/${SidebarLayoutTypes.CompanyDetail}/${item.companyId}/company-config`);
      }
    });
  }

  public selectUser(item: UserModel): void {
    this.resetSearch();
    const modalRef = this.modalService.open(UserCrudModalComponent, { windowClass: 'my-class', size: 'xl', scrollable: true });
    const componentInstance = modalRef.componentInstance as UserCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit User';
    componentInstance.isSuperAdmin = this.isSuperAdmin;
    componentInstance.companyId = this.companyId;
    componentInstance.companyName = item.companyName;
    componentInstance.layoutTypeId = this.layoutTypeId;
    componentInstance.userModel = JSON.parse(JSON.stringify(item)) as UserModel;
  }

  public resetSearch(): void {
    this.searchFromControl.setValue('');
    this.closeBtn();
  }

  public goToDetailsPage(type: GlobalSearchFeature): void {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      if (type === 'user') {
        if (this.isSuperAdmin) {
          this.router.navigateByUrl(`${this.initialUrl}/users`);
        }
      } else if (type === 'company') {
        if (this.isSuperAdmin) {
          this.router.navigateByUrl(`${this.initialUrl}/companies`);
        }
      } else if (type === 'customer' && !ObjectUtils.isNullOrUndefined(this.companyId)) {
        if (this.isSuperAdmin) {
          this.router.navigateByUrl(`${this.initialUrl}/company/${SidebarLayoutTypes.CompanyDetail}/${this.companyId}/customers`);
        } else {
          this.router.navigateByUrl(`${this.initialUrl}/customers`);
        }
      } else if (type === 'appointment' && !ObjectUtils.isNullOrUndefined(this.companyId)) {
        if (this.isSuperAdmin) {
          this.router.navigateByUrl(`${this.initialUrl}/company/${SidebarLayoutTypes.CompanyDetail}/${this.companyId}/appointments`);
        } else {
          this.router.navigateByUrl(`${this.initialUrl}/appointments`);
        }
      } else if (type === 'rule' && !ObjectUtils.isNullOrUndefined(this.companyId)) {
        if (this.isSuperAdmin) {
          this.router.navigateByUrl(`${this.initialUrl}/company/${SidebarLayoutTypes.CompanyDetail}/${this.companyId}/analysis-rules`);
        } else {
          this.router.navigateByUrl(`${this.initialUrl}/analysis-rules`);
        }
      }
    });
    this.searchFromControl.setValue('');
  }

  public windowScroll(): void {
    if (document.body.scrollTop > 80 || document.documentElement.scrollTop > 80) {
      (document.getElementById("back-to-top") as HTMLElement).style.display = "block";
      document.getElementById('page-topbar')?.classList.add('topbar-shadow');
    } else {
      (document.getElementById("back-to-top") as HTMLElement).style.display = "none";
      document.getElementById('page-topbar')?.classList.remove('topbar-shadow');
    }
  }

  // Search Topbar
  public Search(): void {
    const searchOptions = document.getElementById("search-close-options") as HTMLAreaElement;
    const dropdown = document.getElementById("search-dropdown") as HTMLAreaElement;
    const input = document.getElementById("search-options") as HTMLInputElement;
     
    const filter = input.value.toUpperCase();
     
    const inputLength = filter.length;

    if (inputLength > 0) {
      dropdown.classList.add("show");
      searchOptions.classList.remove("d-none");
    } else {
      dropdown.classList.remove("show");
      searchOptions.classList.add("d-none");
    }
  }

  /**
   * Search Close Btn
   */
  public closeBtn(): void {
    const searchOptions = document.getElementById("search-close-options") as HTMLAreaElement;
    const dropdown = document.getElementById("search-dropdown") as HTMLAreaElement;
    const searchInputReponsive = document.getElementById("search-options") as HTMLInputElement;
    dropdown.classList.remove("show");
    searchOptions.classList.add("d-none");
    searchInputReponsive.value = "";
  }

  public showChangePasswordOptionModal(): void {
    const modalRef = this.modalService.open(ChangePasswordComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as ChangePasswordComponent;
    componentInstance.buttonText = 'Reset';
    componentInstance.title = 'Reset Account Password';
  }
}