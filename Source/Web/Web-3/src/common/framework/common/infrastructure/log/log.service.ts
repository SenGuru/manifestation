import { ConfigService } from '../config/config.service';
import { Injectable } from '@angular/core';
import { LogEntryModel } from './log-entry.model';
import { LogLevel } from './log-level';
import { LogPublishersService } from './log-publishers.service';
import { LogVerbosity } from './log-verbosity';

@Injectable()
export class LogService {

  public constructor(private configService: ConfigService, private logPublishersService: LogPublishersService) {
  }

  public debugNormal(msg: unknown): void {
    this.debug(LogVerbosity.normal, msg);
  }

  public debugTroubleShooting(msg: unknown): void {
    this.debug(LogVerbosity.troubleShooting, msg);
  }

  public debugExtremeVerbose(msg: unknown): void {
    this.debug(LogVerbosity.extreme, msg);
  }

  public debug(verbosity: LogVerbosity, msg: unknown): void {
    if (this.canLog(verbosity)) {
      this.doLog(verbosity, LogLevel.debug, msg);
    }
  }

  public infoNormal(msg: unknown): void {
    this.info(LogVerbosity.normal, msg);
  }

  public infoTroubleShooting(msg: unknown): void {
    this.info(LogVerbosity.troubleShooting, msg);
  }

  public infoExtremeVerbose(msg: unknown): void {
    this.info(LogVerbosity.extreme, msg);
  }

  public info(verbosity: LogVerbosity, msg: unknown): void {
    if (this.canLog(verbosity)) {
      this.doLog(verbosity, LogLevel.information, msg);
    }
  }

  public warningNormal(msg: unknown): void {
    this.warning(LogVerbosity.normal, msg);
  }

  public warningTroubleShooting(msg: unknown): void {
    this.warning(LogVerbosity.troubleShooting, msg);
  }

  public warningExtremeVerbose(msg: unknown): void {
    this.warning(LogVerbosity.extreme, msg);
  }

  public warning(verbosity: LogVerbosity, msg: unknown): void {
    if (this.canLog(verbosity)) {
      this.doLog(verbosity, LogLevel.warning, msg);
    }
  }

  public errorNormal(msg: unknown): void {
    this.error(LogVerbosity.normal, msg);
  }

  public errorTroubleShooting(msg: unknown): void {
    this.error(LogVerbosity.troubleShooting, msg);
  }

  public errorExtremeVerbose(msg: unknown): void {
    this.error(LogVerbosity.extreme, msg);
  }

  public error(verbosity: LogVerbosity, msg: unknown): void {
    if (this.canLog(verbosity)) {
      this.doLog(verbosity, LogLevel.error, msg);
    }
  }

  public fatalNormal(msg: unknown): void {
    this.fatal(LogVerbosity.normal, msg);
  }

  public fatalTroubleShooting(msg: unknown): void {
    this.fatal(LogVerbosity.troubleShooting, msg);
  }

  public fatalExtremeVerbose(msg: unknown): void {
    this.fatal(LogVerbosity.extreme, msg);
  }

  public fatal(verbosity: LogVerbosity, msg: unknown): void {
    if (this.canLog(verbosity)) {
      this.doLog(verbosity, LogLevel.fatal, msg);
    }
  }

  private canLog(verbosity: LogVerbosity): boolean {
    const local = this.configService.frameworkConfiguration.verbosity;
    return local >= verbosity;
  }

  private doLog(verbosity: LogVerbosity, logLevel: LogLevel, obj: unknown): void {
    const msg = JSON.stringify(obj);
    const logEntry = new LogEntryModel(verbosity, logLevel, new Date(), 'CTWeb', msg);
    this.logPublishersService.log(logEntry);
  }

}
