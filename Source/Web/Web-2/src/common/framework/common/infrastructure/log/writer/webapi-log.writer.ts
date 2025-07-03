import { BaseLogWriter } from './base-log.writer';
import { ConfigService } from '../../config/config.service';
import { FrameworkServiceApiService } from '../../http/framework-service-api.service';
import { Injectable } from '@angular/core';
import { LogEntryModel } from '../log-entry.model';

@Injectable()
export class WebApiLogWriter extends BaseLogWriter {

   public constructor(configService: ConfigService, private ctServiceApiService: FrameworkServiceApiService) {
        super(configService);
    }

public override log(logEntry: LogEntryModel): void {
        this.ctServiceApiService.log(logEntry)?.subscribe((): void => { 
            // 
        });
    }
}
