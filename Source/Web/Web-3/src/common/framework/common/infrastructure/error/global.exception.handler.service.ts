import { ErrorHandler , Injectable } from '@angular/core';

import { LogService } from '../log/log.service';
import { LogVerbosity } from '../log/log-verbosity';

@Injectable()
export class GlobalExceptionHandlerService extends ErrorHandler {
    public constructor(private logService: LogService) {
        super();
    }
     
public override handleError(error: unknown): void {
        this.logService.error(LogVerbosity.normal, error);
        super.handleError(error);
    }
}
