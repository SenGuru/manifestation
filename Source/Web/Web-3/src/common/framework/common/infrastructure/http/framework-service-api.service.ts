import { ConfigService } from '../config/config.service';
import { FrameworkConfiguration } from '../config/models/framework-configuration.model';
import { FrameworkHttpClientService } from './framework-http-client.service';
import { Injectable } from '@angular/core';
import { LogEntryModel } from '../log/log-entry.model';
import { Observable } from 'rxjs';
import { ReturnModel } from '../../models/result/return.model';

@Injectable({
  providedIn: 'root'
})
export class FrameworkServiceApiService {

  public constructor(private httpClient: FrameworkHttpClientService, private configService: ConfigService) {
  }

  public log(logEntry: LogEntryModel): Observable<ReturnModel<boolean>> | undefined {
    return;
    // return this.httpClient.postWithModel<boolean, LogEntryModel>(
    //    this.configService.urlConfigService.getUrl(Urls.Logging.log), logEntry);
  }

  public frameworkConfiguration(): Observable<ReturnModel<FrameworkConfiguration>> | undefined {
    return;
    // return this.httpClient.get<FrameworkConfiguration>(this.configService.urlConfigService.getUrl(Urls.Configuration.ctConfiguration));
  }

}
