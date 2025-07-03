import { ActivatedRouteSnapshot, Params } from '@angular/router';

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RouterDataService {

  public constructor() {
    // 
  }

  public getRouterData(route: ActivatedRouteSnapshot): Params {
    return this.getRouterDataInternal(route);
  }

  public getRouterParamData(route: ActivatedRouteSnapshot): Params {
    return this.getRouterParamInternal(route);
  }

  private getRouterParamInternal = (route: ActivatedRouteSnapshot): object => ({
    ...route.params,
    ...route.children.reduce((acc, child) =>
      ({ ...this.getRouterParamInternal(child), ...acc }), {})
  });

  private getRouterDataInternal = (route: ActivatedRouteSnapshot): object => ({
    ...route.data,
    ...route.children.reduce((acc, child) =>
      ({ ...this.getRouterDataInternal(child), ...acc }), {})
  });

}
