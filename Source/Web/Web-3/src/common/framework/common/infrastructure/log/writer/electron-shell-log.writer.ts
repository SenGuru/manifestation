import { BaseLogWriter } from './base-log.writer';
import { ConfigService } from '../../config/config.service';
import { Injectable } from '@angular/core';
import { LogEntryModel } from '../log-entry.model';
import { ObjectUtils } from '../../../utils/object.utils';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
declare let electronShellLogObject: any;

@Injectable()
export class ElectronShellLogWriter extends BaseLogWriter {

    private static isElectronLoggerAvailable = true;

   public constructor(configService: ConfigService) {
        super(configService);
        try {
            if (ObjectUtils.isNullOrUndefined(electronShellLogObject)) {
                ElectronShellLogWriter.isElectronLoggerAvailable = false;
            }
        } catch {
            ElectronShellLogWriter.isElectronLoggerAvailable = false;
        }

    }

public override log(logEntry: LogEntryModel): void {
        if (ElectronShellLogWriter.isElectronLoggerAvailable === false) {
            return;
        }

        if (ObjectUtils.isNullOrUndefined(electronShellLogObject)) {
            return;
        }
        // eslint-disable-next-line @typescript-eslint/no-unsafe-call
        electronShellLogObject.log(logEntry);
    }
}
