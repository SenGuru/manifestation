import { BaseHttpClientService } from '../http/base-http-client.service';
import { FrameworkConfiguration } from './models/framework-configuration.model';
import { Injectable } from '@angular/core';
import { LogTargetConfiguration } from './models/log-target-config.model';
import { ServiceConfiguration } from './models/server-config.model';
import { UrlConfigService } from './url-config.service';
import { environment } from '../../../../../environments/environment';

@Injectable()
export class ConfigService {

  public logTargetConfiguration: LogTargetConfiguration = new LogTargetConfiguration();
  public serviceConfiguration: ServiceConfiguration = new ServiceConfiguration();
  public urlConfigService: UrlConfigService;
  public frameworkConfiguration: FrameworkConfiguration = new FrameworkConfiguration();
  public isConfigLoad = false;
  public configMessage = 'Not Initialized';

  public constructor(private baseHttpClientService: BaseHttpClientService) {
    const params = new URLSearchParams(window.location.search);
    this.serviceConfiguration.baseApiServerUrl = environment.baseUrl;
    this.serviceConfiguration.baseApiServerQueryParam = environment.queryParam;

    this.urlConfigService = new UrlConfigService(this);
    this.loadInitialConfiguration();
  }

public loadInitialConfiguration(): void {
    // 
  }

public updateLogLevel(newLogLevel: number): void {
    this.frameworkConfiguration.verbosity = newLogLevel;
  }

}
