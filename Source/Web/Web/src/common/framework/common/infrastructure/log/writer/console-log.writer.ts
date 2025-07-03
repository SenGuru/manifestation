import { BaseLogWriter } from './base-log.writer';
import { ConfigService } from '../../config/config.service';
import { Injectable } from '@angular/core';
import { LogEntryModel } from '../log-entry.model';
import { LogLevel } from '../log-level';

@Injectable()
export class ConsoleLogWriter extends BaseLogWriter {

   public constructor(configService: ConfigService) {
        super(configService);
    }

public override log(logEntry: LogEntryModel): void {

        switch (logEntry.logLevel) {
            case LogLevel.debug:
                // eslint-disable-next-line no-console
                console.trace(logEntry.formattedMessage());
                break;

            case LogLevel.information:
                // eslint-disable-next-line no-console
                console.info(logEntry.formattedMessage());
                break;

            case LogLevel.warning:
                // eslint-disable-next-line no-console
                console.warn(logEntry.formattedMessage());
                break;

            case LogLevel.error:
                // eslint-disable-next-line no-console
                console.error(logEntry.formattedMessage());
                break;

            case LogLevel.fatal:
                // eslint-disable-next-line no-console
                console.error(logEntry.formattedMessage());
                break;

            default:
                // eslint-disable-next-line no-console
                console.log(logEntry.formattedMessage());
                break;
        }
    }
}
