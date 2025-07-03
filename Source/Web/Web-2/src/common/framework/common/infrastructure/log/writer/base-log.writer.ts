import { ConfigService } from '../../config/config.service';
import { Injectable } from '@angular/core';
import { LogEntryModel } from '../log-entry.model';

@Injectable()
export abstract class BaseLogWriter {
    public constructor(private configService: ConfigService) {
    }

public log(logEntry: LogEntryModel): void {
        // 
    }
}
