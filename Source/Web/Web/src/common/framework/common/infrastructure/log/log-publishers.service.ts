import { BaseLogWriter } from './writer/base-log.writer';
import { ConfigService } from '../config/config.service';
import { ConsoleLogWriter } from './writer/console-log.writer';
import { ElectronShellLogWriter } from './writer/electron-shell-log.writer';
import { Injectable } from '@angular/core';
import { LogEntryModel } from './log-entry.model';
import { WebApiLogWriter } from './writer/webapi-log.writer';

@Injectable()
export class LogPublishersService {

  private writers: BaseLogWriter[] = [];

  public constructor(private configService: ConfigService, webApiLogWriter: WebApiLogWriter,
    consoleLogWriter: ConsoleLogWriter, electronShellWriter: ElectronShellLogWriter,) {

    if (configService.logTargetConfiguration.logToConsole) {
      this.writers.push(consoleLogWriter);
    }

    if (configService.logTargetConfiguration.logToWebApi) {
      this.writers.push(webApiLogWriter);
    }

    if (configService.logTargetConfiguration.logToElectronShell) {
      this.writers.push(electronShellWriter);
    }
  }

public log(logEntry: LogEntryModel): void {
    for (const writer of this.writers) {
      writer.log(logEntry);
    }
  }

}
