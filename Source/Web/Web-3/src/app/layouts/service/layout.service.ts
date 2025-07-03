import { DATA_LAYOUT_MODE, LAYOUT_MODE_TYPES, LAYOUT_WIDTH_TYPES, SIDEBAR_TYPE, TOPBAR_MODE_TYPES } from '../layout';

import { AppLayoutModel } from 'src/common/models/app-custom-model';
import { Injectable } from '@angular/core';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {

  public isDarkThemeMode: string = '';

  public constructor(
    public sessionService: SessionService,
  ) {
    this.isDarkThemeMode = sessionService.isDarkThemeMode() ? 'dark' : 'light';
  }

  public setThemeLayout(
    LAYOUT_MODE: string,
    SIDEBAR_MODE: string,
    TOPBAR_TYPE: string,): void {
    const layoutData: AppLayoutModel = {
      LAYOUT_MODE: LAYOUT_MODE,
      DATA_LAYOUT: DATA_LAYOUT_MODE.VERTICAL,
      LAYOUT_WIDTH: LAYOUT_WIDTH_TYPES.FLUID,
      SIDEBAR_MODE: SIDEBAR_MODE,
      TOPBAR_TYPE: TOPBAR_TYPE,
    };

    document.body.setAttribute('data-bs-theme', layoutData.LAYOUT_MODE);
    document.body.setAttribute('data-layout-size', layoutData.LAYOUT_WIDTH);
    document.body.setAttribute('data-sidebar', layoutData.SIDEBAR_MODE);
    document.body.setAttribute('data-topbar', layoutData.TOPBAR_TYPE);

    switch (layoutData.SIDEBAR_MODE) {
      case "light":
        document.body.setAttribute('data-sidebar', 'light');
        document.body.setAttribute('data-topbar', 'dark');
        document.body.removeAttribute('data-sidebar-size');
        document.body.removeAttribute('data-layout-size');
        document.body.removeAttribute('data-keep-enlarged');
        document.body.classList.remove('vertical-collpsed');
        document.body.removeAttribute('data-layout-scrollable');
        break;
      case "compact":
        document.body.setAttribute('data-sidebar-size', 'small');
        document.body.setAttribute('data-sidebar', 'dark');
        document.body.removeAttribute('data-topbar');
        document.body.removeAttribute('data-layout-size');
        document.body.removeAttribute('data-keep-enlarged');
        document.body.classList.remove('sidebar-enable');
        document.body.classList.remove('vertical-collpsed');
        document.body.removeAttribute('data-layout-scrollable');
        break;
      case "dark":
        document.body.setAttribute('data-sidebar', 'dark');
        document.body.removeAttribute('data-topbar');
        document.body.removeAttribute('data-layout-size');
        document.body.removeAttribute('data-keep-enlarged');
        document.body.removeAttribute('data-sidebar-size');
        document.body.classList.remove('sidebar-enable');
        document.body.classList.remove('vertical-collpsed');
        document.body.removeAttribute('data-layout-scrollable');
        break;
      case "icon":
        document.body.classList.add('vertical-collpsed');
        document.body.setAttribute('data-sidebar', 'dark');
        document.body.removeAttribute('data-layout-size');
        document.body.setAttribute('data-keep-enlarged', "true");
        document.body.removeAttribute('data-topbar');
        document.body.removeAttribute('data-layout-scrollable');
        break;
      case "colored":
        document.body.classList.remove('sidebar-enable');
        document.body.classList.remove('vertical-collpsed');
        document.body.setAttribute('data-sidebar', 'colored');
        document.body.removeAttribute('data-layout-size');
        document.body.removeAttribute('data-keep-enlarged');
        document.body.removeAttribute('data-topbar');
        document.body.removeAttribute('data-layout-scrollable');
        document.body.removeAttribute('data-sidebar-size');
        break;
      default:
        document.body.setAttribute('data-sidebar', 'dark');
        break;
    }

    switch (layoutData.LAYOUT_WIDTH) {
      case LAYOUT_WIDTH_TYPES.FLUID:
        document.body.setAttribute("data-layout-size", "fluid");
        document.body.classList.remove("vertical-collpsed");
        document.body.removeAttribute("data-layout-scrollable");
        break;
      case LAYOUT_WIDTH_TYPES.BOXED:
        document.body.setAttribute("data-layout-size", "boxed");
        document.body.classList.add("vertical-collpsed");
        document.body.removeAttribute("data-layout-scrollable");
        break;
      case LAYOUT_WIDTH_TYPES.SCROLLABLE:
        document.body.removeAttribute("data-layout-size");
        document.body.setAttribute("data-layout-scrollable", "true");
        document.body.setAttribute("data-layout-size", "fluid");
        document.body.classList.remove("right-bar-enabled", "vertical-collpsed");
        break;
      default:
        document.body.setAttribute("data-layout-size", "fluid");
        break;
    }

    if (layoutData.TOPBAR_TYPE == TOPBAR_MODE_TYPES.DARK) {
      document.documentElement.setAttribute('data-topbar', TOPBAR_MODE_TYPES.DARK);
    } else {
      document.documentElement.setAttribute('data-topbar', TOPBAR_MODE_TYPES.LIGHT);
    }

  }
}
