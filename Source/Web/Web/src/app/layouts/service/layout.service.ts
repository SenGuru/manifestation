import { BACKGROUND_IMAGE, LAYOUT_POSITION_TYPES, LAYOUT_THEME_COLOR, LAYOUT_TOPBAR_COLOR_TYPES, LAYOUT_TYPES, LAYOUT_WIDTH_TYPES, PERLOADER_TYPES, SIDEBAR_COLOR, SIDEBAR_IMAGE, SIDEBAR_SIZE, SIDEBAR_VIEW, SIDEBAR_VISIBILITY } from '../layout';

import { AppLayoutModel } from 'src/common/models/app-custom-model';
import { Injectable } from '@angular/core';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {

  public isDarkThemeMode: string | null = null;

  public constructor(
    public sessionService: SessionService,
  ) {
    this.isDarkThemeMode = sessionService.isDarkThemeMode() ? 'dark' : 'light';
  }

  public setThemeLayout(theme: string): void {
    const layoutData: AppLayoutModel = {
      LAYOUT: LAYOUT_TYPES.VERTICAL,
      LAYOUT_THEME: theme,
      LAYOUT_THEME_COLOR: LAYOUT_THEME_COLOR.DEFAULT,
      LAYOUT_MODE: this.isDarkThemeMode,
      LAYOUT_WIDTH: LAYOUT_WIDTH_TYPES.FLUID,
      LAYOUT_POSITION: LAYOUT_POSITION_TYPES.FIXED,
      TOPBAR: LAYOUT_TOPBAR_COLOR_TYPES.LIGHT,
      SIDEBAR_COLOR: SIDEBAR_COLOR.DARK,
      SIDEBAR_SIZE: SIDEBAR_SIZE.LARGE,
      SIDEBAR_VIEW: SIDEBAR_VIEW.DEFAULT,
      SIDEBAR_IMAGE: SIDEBAR_IMAGE.NONE,
      SIDEBAR_VISIBILITY: SIDEBAR_VISIBILITY.SHOW,
      DATA_PRELOADER: PERLOADER_TYPES.DISABLE,
      BACKGROUND_IMAGE: BACKGROUND_IMAGE.NONE,
    };

    document.documentElement.setAttribute('data-layout', layoutData?.LAYOUT ?? '');
    document.documentElement.setAttribute('data-theme', layoutData?.LAYOUT_THEME ?? '');
    document.documentElement.setAttribute('data-theme-colors', layoutData?.LAYOUT_THEME_COLOR ?? '');
    document.documentElement.setAttribute('data-bs-theme', layoutData?.LAYOUT_MODE ?? '');
    document.documentElement.setAttribute('data-layout-width', layoutData?.LAYOUT_WIDTH ?? '');
    document.documentElement.setAttribute('data-layout-position', layoutData?.LAYOUT_POSITION ?? '');
    document.documentElement.setAttribute('data-topbar', layoutData?.TOPBAR ?? '');
    document.documentElement.setAttribute('data-preloader', layoutData?.DATA_PRELOADER ?? '');
    document.documentElement.setAttribute('data-sidebar-visibility', layoutData?.SIDEBAR_VISIBILITY ?? '');
    if (layoutData?.LAYOUT == "vertical" || layoutData?.LAYOUT == "twocolumn") document.documentElement.setAttribute('data-sidebar', layoutData?.SIDEBAR_COLOR ?? '');
    if (layoutData?.LAYOUT == "vertical" || layoutData?.LAYOUT == "twocolumn") document.documentElement.setAttribute('data-sidebar-size', layoutData?.SIDEBAR_SIZE ?? '');
    if (layoutData?.LAYOUT == "vertical" || layoutData?.LAYOUT == "twocolumn") document.documentElement.setAttribute('data-sidebar-image', layoutData?.SIDEBAR_IMAGE ?? '');
    if (layoutData?.LAYOUT == "vertical" || layoutData?.LAYOUT == "twocolumn") document.documentElement.setAttribute('data-layout-style', layoutData?.SIDEBAR_VIEW ?? '');
  }
}
