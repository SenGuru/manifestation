import { ADMIN_COMPANY_MENU, SUPER_ADMIN_MENU } from '../menus/super-admin-menu';
import { AfterViewInit, Component, ElementRef, Input, OnChanges, OnInit, ViewChild } from '@angular/core';
import { CompanyMenuTypes, SidebarLayoutTypes } from 'src/common/models/app-custom-enums';
import { CompanyModel, LoggedInUserModel } from 'src/common/models/app-model';
import { NavigationEnd, Params, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { ADMIN_MENU } from '../menus/admin-menu';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { EventService } from '../service/event.service';
import { HttpClient } from '@angular/common/http';
import { HttpService } from 'src/common/services/app-http-client';
import { MENU } from './menu';
import { MenuItem } from 'src/common/models/app-custom-model';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@UntilDestroy()
@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  standalone: false,
})

/**
 * Sidebar component
 */
export class SidebarComponent implements OnInit, AfterViewInit, OnChanges {

  @ViewChild('componentRef') public scrollRef!: any;
  @Input() isCondensed = false;
  menu: any;
  data: any;
  public isLoadingSidebarMenu: boolean = false;
  public currentUser: LoggedInUserModel | null = null;
  menuItems: MenuItem[] = [];
  public companyInfo: CompanyModel | undefined;
  public initialUrl = '';

  @ViewChild('sideMenu') public sideMenu!: ElementRef;

  constructor(private eventService: EventService,
    private router: Router,
    private companySessionService: CompanySessionService,
    private httpService: HttpService,
    private sessionService: SessionService,
    private enumViewService: EnumViewService,
    public translate: TranslateService,
    private routerDataService: RouterDataService,
    private dialogService: CommonDialogService,
    private http: HttpClient) {
  }

  public ngOnInit(): void {
    // Menu Items
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initialUrl = this.enumViewService.getInititalUrl(this.currentUser?.userTypeId ?? 0);
    this.initialize();
    // eslint-disable-next-line 
    this.router.events.pipe(untilDestroyed(this)).subscribe((event: any) => {
      if (document.documentElement.getAttribute('data-layout') != "twocolumn") {
        if (event instanceof NavigationEnd || event?.routerEvent instanceof NavigationEnd) {
          this.initialize();
          this._activateMenuDropdown();
          this._scrollElement();
        }
      }
    });
  }

  ngAfterViewInit() {
    this._activateMenuDropdown();
  }

  ngOnChanges() {
    if (!this.isCondensed && this.sideMenu || this.isCondensed) {
    //  
    } else if (this.menu) {
      this.menu.dispose();
    }
  }
  _scrollElement() {
    setTimeout(() => {
      if (document.getElementsByClassName("mm-active").length > 0) {

        //@ts-expect-error -- this line expected to have an error
        const currentPosition = document.getElementsByClassName("mm-active")[0]['offsetTop'];
        if (currentPosition > 500)
          if (this.scrollRef.SimpleBar !== null)
            this.scrollRef.SimpleBar.getScrollElement().scrollTop =
              currentPosition + 300;
      }
    }, 300);
  }

  /**
   * remove active and mm-active class
   */

  _removeAllClass(className: string) {
    const els = document.getElementsByClassName(className);
    while (els[0]) {
      els[0].classList.remove(className);
    }
  }

  /**
   * Activate the parent dropdown
   */
  _activateMenuDropdown() {
    this._removeAllClass('mm-active');
    this._removeAllClass('mm-show');
    const links = document.getElementsByClassName('side-nav-link-ref');
    let menuItemEl = null;
    // tslint:disable-next-line: prefer-for-of
    const paths = [];
    for (let i = 0; i < links.length; i++) {
      const linkObj = links[i];
      if (linkObj) {

        //@ts-expect-error -- this line expected to have an error
        paths.push(linkObj['pathname']);
      }
    }
    const itemIndex = paths.indexOf(window.location.pathname);
    if (itemIndex === -1) {
      const strIndex = window.location.pathname.lastIndexOf('/');
      const item = window.location.pathname.substr(0, strIndex).toString();
      menuItemEl = links[paths.indexOf(item)];
    } else {
      menuItemEl = links[itemIndex];
    }
    if (menuItemEl) {
      menuItemEl.classList.add('active');
      const parentEl = menuItemEl.parentElement;
      if (parentEl) {
        parentEl.classList.add('mm-active');
        if (parentEl.parentElement) {
          const parent2El = parentEl.parentElement.closest('ul');
          if (parent2El && parent2El.id !== 'side-menu') {
            parent2El.classList.add('mm-show');
            const parent3El = parent2El.parentElement;
            if (parent3El && parent3El.id !== 'side-menu') {
              parent3El.classList.add('mm-active');
              const childAnchor = parent3El.querySelector('.has-arrow');
              const childDropdown = parent3El.querySelector('.has-dropdown');
              if (childAnchor) { childAnchor.classList.add('mm-active'); }
              if (childDropdown) { childDropdown.classList.add('mm-active'); }
              const parent4El = parent3El.parentElement;
              if (parent4El && parent4El.id !== 'side-menu') {
                parent4El.classList.add('mm-show');
                const parent5El = parent4El.parentElement;
                if (parent5El && parent5El.id !== 'side-menu') {
                  parent5El.classList.add('mm-active');
                  const childanchor = parent5El.querySelector('.is-parent');
                  if (childanchor && parent5El.id !== 'side-menu') { childanchor.classList.add('mm-active'); }
                }
              }
            }
          }
        }
      }
    }

  }

  /**
   * Initialize
   */
  initialize(): void {

    this.isLoadingSidebarMenu = true;
    const routerParams: Params = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    const routerData = this.routerDataService.getRouterData(this.router.routerState.snapshot.root);

    // eslint-disable-next-line 
    let companyId = RouterUtils.getRouterDataNumber(routerParams?.['companyId']) ?? 0;

    if (this.currentUser?.userTypeId !== PredefinedUserTypes.SuperAdmin) {
      companyId = this.currentUser?.companyId ?? 0;
    }

    // eslint-disable-next-line 
    const layoutTypeId = RouterUtils.getRouterDataNumber(routerParams?.['layoutTypeId']) ?? 0;

    // eslint-disable-next-line 
    const customerId = RouterUtils.getRouterDataNumber(routerParams?.['customerId']) ?? 0;

    // eslint-disable-next-line 
    const analysisRuleId = RouterUtils.getRouterDataNumber(routerParams?.['analysisRuleId']) ?? 0;

    this.menuItems = [];
     
    if (this.currentUser && this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.menuItems = JSON.parse(JSON.stringify(SUPER_ADMIN_MENU)) as MenuItem[];
      // company data
      if (routerParams && !ObjectUtils.isObjNullOrUndefined(routerParams)) {
        if (!ObjectUtils.isNullOrUndefined(companyId) && companyId > 0 && !ObjectUtils.isNullOrUndefined(layoutTypeId) && layoutTypeId > 0) {

          if (layoutTypeId == SidebarLayoutTypes.CompanyList) {
            // use json parse for avoiding object reference issue
            this.menuItems = JSON.parse(JSON.stringify(SUPER_ADMIN_MENU)) as MenuItem[];
          }
          if (layoutTypeId == SidebarLayoutTypes.CompanyDetail) {
            this.menuItems = JSON.parse(JSON.stringify(ADMIN_COMPANY_MENU)) as MenuItem[];
          }

        }
      } else {
        this.defaultSidebarMovieInformation();
      }
    }
     
    else if (this.currentUser && this.currentUser?.userTypeId == PredefinedUserTypes.CompanyAdmin) {
      this.menuItems = JSON.parse(JSON.stringify(ADMIN_MENU)) as MenuItem[];
      if (routerParams && !ObjectUtils.isObjNullOrUndefined(routerParams)) {
        this.menuItems = JSON.parse(JSON.stringify(ADMIN_MENU)) as MenuItem[];
      } else {
        this.defaultSidebarMovieInformation();
      }
    }

    // update url path 

    // company id
    if (!ObjectUtils.isNullOrUndefined(companyId) && companyId > 0) {
      this.menuItems = this.updateUrlWithId(this.menuItems, 'companyId', companyId?.toString() ?? '');

      const sessionCompany = this.companySessionService.getCurrentCompany();
      if (sessionCompany && sessionCompany?.companyId === companyId) {
        this.companyInfo = sessionCompany;
        this.applyPermission(layoutTypeId, this.companyInfo);
      } else {
        this.isLoadingSidebarMenu = true;
        this.getCompanyInfo(layoutTypeId, companyId);
      }

    } else {
      this.isLoadingSidebarMenu = false;
    }

    // layout Type Id
    if (!ObjectUtils.isNullOrUndefined(layoutTypeId)) {
      this.menuItems = this.updateUrlWithId(this.menuItems, 'layoutTypeId', layoutTypeId?.toString());
    }

    // layout Type Id
    if (!ObjectUtils.isNullOrUndefined(customerId)) {
      this.menuItems = this.updateUrlWithId(this.menuItems, 'customerId', customerId?.toString());
    }
    // layout Type Id
    if (!ObjectUtils.isNullOrUndefined(analysisRuleId)) {
      this.menuItems = this.updateUrlWithId(this.menuItems, 'analysisRuleId', analysisRuleId?.toString());
    }
  }

  public defaultSidebarMovieInformation(): void {
    if (this.currentUser && this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.menuItems = JSON.parse(JSON.stringify(SUPER_ADMIN_MENU)) as MenuItem[];
    } else {
      // 
    }
  }

  public getCompanyInfo(layoutTypeId: number, companyId: number): void {
    this.httpService.getCompany(companyId).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Company", result);
          return;
        }

        if (result.model) {
          this.companyInfo = result.model;
          this.companySessionService.setCurrentCompany(result.model);
          this.applyPermission(layoutTypeId, this.companyInfo);
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Company"),
      complete: () => null,
    });
  }

  public applyPermission(layoutTypeId: number, companyInfo: CompanyModel): void {

    if ((this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin && !ObjectUtils.isObjNullOrUndefined(companyInfo) && layoutTypeId === SidebarLayoutTypes.CompanyDetail)
      || (ObjectUtils.isNullOrUndefined(layoutTypeId) && this.currentUser?.userTypeId == PredefinedUserTypes.CompanyAdmin && !ObjectUtils.isObjNullOrUndefined(companyInfo))) {

      const filteredMenus: MenuItem[] = JSON.parse(JSON.stringify(this.menuItems)) as MenuItem[];
      this.menuItems = JSON.parse(JSON.stringify(filteredMenus)) as MenuItem[];

    }

    else if (layoutTypeId === SidebarLayoutTypes.CompanyDetail) {

      const filteredMenus = this.menuItems.filter(x => x.id == CompanyMenuTypes.CompanyInformation || x.id == CompanyMenuTypes.CompanyConfig);
      this.menuItems = JSON.parse(JSON.stringify(filteredMenus)) as MenuItem[];

    }
    this.isLoadingSidebarMenu = false;
  }

  public updateUrlWithId(menuData: MenuItem[], IdText: string, replacementId: string): MenuItem[] {
    const result: MenuItem[] = menuData;
    result.forEach((menuItem: MenuItem) => {
      menuItem.link = menuItem.link?.replace(IdText, replacementId);
      if (menuItem.subItems) {
        menuItem.subItems.forEach((subItem: MenuItem) => {
          subItem.link = subItem.link?.replace(IdText, replacementId);

          if (subItem.subItems) {
            subItem.subItems.forEach((child1Item: MenuItem) => {
              child1Item.link = child1Item.link?.replace(IdText, replacementId);

              if (child1Item.subItems) {
                child1Item.subItems.forEach((child2Item: MenuItem) => {
                  child2Item.link = child2Item.link?.replace(IdText, replacementId);

                })
              }
            })
          }
        })
      }
    });
    return result;
  }

  /**
   * Returns true or false if given menu item has child or not
   * @param item menuItem
   */
  hasItems(item: MenuItem) {
    return item.subItems !== undefined ? item.subItems.length > 0 : false;
  }
}
