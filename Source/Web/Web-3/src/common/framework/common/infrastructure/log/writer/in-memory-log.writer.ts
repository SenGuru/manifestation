import { BaseLogWriter } from './base-log.writer';
import { ConfigService } from '../../config/config.service';
import { Injectable } from '@angular/core';
import { LogEntryModel } from '../log-entry.model';

@Injectable()
export class InMemoryLogWriter extends BaseLogWriter {
    private logStrs: string[] = [];

   public constructor(configService: ConfigService) {
        super(configService);
    }

public override log(logEntry: LogEntryModel): void {
        this.logStrs.push(logEntry.formattedMessage());
    }
}
