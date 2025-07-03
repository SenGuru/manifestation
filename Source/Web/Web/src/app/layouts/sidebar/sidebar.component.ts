import { ADMIN_COMPANY_MENU, SUPER_ADMIN_MENU } from '../menus/super-admin-menu';
import { AfterViewInit, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { CompanyMenuTypes, CustomerMenuTypes, SidebarLayoutTypes } from 'src/common/models/app-custom-enums';
import { CompanyModel, LoggedInUserModel } from 'src/common/models/app-model';
import { NavigationEnd, Params, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { ADMIN_MENU } from '../menus/admin-menu';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CompanySessionService } from 'src/common/services/session/company-session-service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { MenuItem } from 'src/common/models/app-custom-model';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { PredefinedUserTypes } from 'src/common/models/app-enums';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TranslateService } from '@ngx-translate/core';

@UntilDestroy()
@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss'],
    standalone: false
})
export class SidebarComponent implements OnInit, AfterViewInit {

  @ViewChild('sideMenu') public sideMenu!: ElementRef;
  @Output() public mobileMenuButtonClicked = new EventEmitter();
  public menu: unknown;
  public toggle: unknown = true;
  public menuItems: MenuItem[] = [];
  public currentUser: LoggedInUserModel | null = null;
  public initialUrl = '';
  public companyInfo: CompanyModel | undefined;
  public destinationUrl = '';
  public isLoadingSidebarMenu: boolean = false;

  public constructor(
    private router: Router,
    public translate: TranslateService,
    private sessionService: SessionService,
    private enumViewService: EnumViewService,
    private routerDataService: RouterDataService,
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private companySessionService: CompanySessionService,
  ) {
    translate.setDefaultLang('en');
  }

  public ngOnInit(): void {
    // Menu Items
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initialUrl = this.enumViewService.getInititalUrl(this.currentUser?.userTypeId ?? 0);
    this.initSidebarTabs();
    // eslint-disable-next-line 
    this.router.events.pipe(untilDestroyed(this)).subscribe((event: any) => {
      if (document.documentElement.getAttribute('data-layout') != "twocolumn") {
        if (event instanceof NavigationEnd || event?.routerEvent instanceof NavigationEnd) {
          this.initSidebarTabs();
          this.initActiveMenu();
        }
      }
    });
  }

  public initSidebarTabs(): void {
    this.isLoadingSidebarMenu = true;
    const routerParams: Params = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    const routerData = this.routerDataService.getRouterData(this.router.routerState.snapshot.root);
    //@ts-expect-error -- this line expected to have an error
    // eslint-disable-next-line 
    let companyId = RouterUtils.getRouterDataNumber(routerParams?.companyId) ?? 0;

    if (this.currentUser?.userTypeId !== PredefinedUserTypes.SuperAdmin) {
      companyId = this.currentUser?.companyId ?? 0;
    }

    //@ts-expect-error -- this line expected to have an error
    // eslint-disable-next-line 
    const layoutTypeId = RouterUtils.getRouterDataNumber(routerParams?.layoutTypeId) ?? 0;
    //@ts-expect-error -- this line expected to have an error
    // eslint-disable-next-line 
    const customerId = RouterUtils.getRouterDataNumber(routerParams?.customerId) ?? 0;
    //@ts-expect-error -- this line expected to have an error
    // eslint-disable-next-line 
    const analysisRuleId = RouterUtils.getRouterDataNumber(routerParams?.analysisRuleId) ?? 0;

    this.menuItems = [];
    // eslint-disable-next-line 
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
    // eslint-disable-next-line 
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

  public defaultSidebarCompanyInformation(): void {
    this.menuItems = JSON.parse(JSON.stringify(SUPER_ADMIN_MENU)) as MenuItem[];
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

  public defaultSidebarMovieInformation(): void {
    if (this.currentUser && this.currentUser?.userTypeId == PredefinedUserTypes.SuperAdmin) {
      this.menuItems = JSON.parse(JSON.stringify(SUPER_ADMIN_MENU)) as MenuItem[];
    } else {
      // 
    }
  }

  /***
   * Activate droup down set
   */
  public ngAfterViewInit(): void {
    setTimeout((): void => {
      this.initActiveMenu();
    }, 0);
  }

  public removeActivation(items: Element[]): void {
    items.forEach((item: Element) => {
      item.classList.remove("active");
    });
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

  public toggleItem(item: MenuItem): void {
    this.menuItems.forEach((menuItem: MenuItem) => {

      if (menuItem == item) {
        menuItem.isCollapsed = !menuItem.isCollapsed
      } else {
        menuItem.isCollapsed = true
      }
      if (menuItem.subItems) {
        menuItem.subItems.forEach((subItem: MenuItem) => {

          if (subItem == item) {
            menuItem.isCollapsed = !menuItem.isCollapsed
            subItem.isCollapsed = !subItem.isCollapsed
          } else {
            subItem.isCollapsed = true
          }
          if (subItem.subItems) {
            subItem.subItems.forEach((childitem: MenuItem) => {

              if (childitem == item) {
                childitem.isCollapsed = !childitem.isCollapsed
                subItem.isCollapsed = !subItem.isCollapsed
                menuItem.isCollapsed = !menuItem.isCollapsed
              } else {
                childitem.isCollapsed = true
              }
              if (childitem.subItems) {
                childitem.subItems.forEach((childrenitem: MenuItem) => {

                  if (childrenitem == item) {
                    childrenitem.isCollapsed = false
                    childitem.isCollapsed = false
                    subItem.isCollapsed = false
                    menuItem.isCollapsed = false
                  } else {
                    childrenitem.isCollapsed = true
                  }
                })
              }
            })
          }
        })
      }
    });
  }

  public activateParentDropdown(item: Element): boolean {
    item.classList.add("active");
    const parentCollapseDiv = item.closest(".collapse.menu-dropdown");

    if (parentCollapseDiv) {
      parentCollapseDiv.parentElement?.children[0]?.classList.add("active");
      if (parentCollapseDiv.parentElement?.closest(".collapse.menu-dropdown")) {
        if (parentCollapseDiv.parentElement?.closest(".collapse")?.previousElementSibling)
          parentCollapseDiv.parentElement?.closest(".collapse")?.previousElementSibling?.classList.add("active");
        if (parentCollapseDiv.parentElement?.closest(".collapse")?.previousElementSibling?.closest(".collapse")) {
          parentCollapseDiv.parentElement?.closest(".collapse")?.previousElementSibling?.closest(".collapse")?.previousElementSibling?.classList?.add("active");
        }
      }
      return false;
    }
    return false;
  }

  public updateActive(event: Event): void {
    const ul = document.getElementById("navbar-nav");
    if (ul) {
      const items = Array.from(ul.querySelectorAll("a.nav-link"));
      this.removeActivation(items);
    }
    this.activateParentDropdown((event.currentTarget as HTMLInputElement));
  }

  public initActiveMenu(): void {
    const pathUrlWithoutBaseURL = this.router.url;
    const pathUrlWithBaseURL = window.location.href;
    const active = this.findMenuItem(pathUrlWithoutBaseURL, this.menuItems);
    if (active) this.toggleItem(active)
    const ul = document.getElementById("navbar-nav");
    if (ul) {
      const items = Array.from(ul.querySelectorAll("a.nav-link"));
      const activeItems = items.filter((x: Element) => x.classList.contains("active"));
      this.removeActivation(activeItems);

      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const matchingMenuItem = items.find((x: any) => x.href === pathUrlWithBaseURL);
      if (matchingMenuItem) {
        this.activateParentDropdown(matchingMenuItem);
      }
    }
  }
  /**
   * Returns true or false if given menu item has child or not
   * @param item menuItem
   */
  public hasItems(item: MenuItem): boolean {
    return item.subItems !== undefined ? item.subItems.length > 0 : false;
  }

  /**
   * Toggle the menu bar when having mobile screen
   */
  public toggleMobileMenu(event: Event): void {
    const sidebarsize = document.documentElement.getAttribute("data-sidebar-size");
    if (sidebarsize == 'sm-hover-active') {
      document.documentElement.setAttribute("data-sidebar-size", 'sm-hover');

    } else {
      document.documentElement.setAttribute("data-sidebar-size", 'sm-hover-active')
    }
  }

  /**
   * SidebarHide modal
   * @param content modal content
   */
  public SidebarHide(): void {
    document.body.classList.remove('vertical-sidebar-enable');
  }

  private findMenuItem(pathUrl: string, menuItems: MenuItem[]): MenuItem | null {
    for (const menuItem of menuItems) {
      if (menuItem.link && menuItem.link === pathUrl) {
        return menuItem;
      }

      if (menuItem.subItems) {
        const foundItem = this.findMenuItem(pathUrl, menuItem.subItems);
        if (foundItem) {
          return foundItem;
        }
      }
    }

    return null;
  }

}
