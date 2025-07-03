import { LogLevel } from './log-level';
import { LogVerbosity } from './log-verbosity';

export class LogLevelHelper {
  public static toVerboseStringValue(logLevel: LogVerbosity): string {
    switch (logLevel) {

      case LogVerbosity.normal:
        return '00';

      case LogVerbosity.troubleShooting:
        return '01';

      case LogVerbosity.extreme:
        return '02';

      default:
        return '00';
    }
  }

  public static toStringValue(logLevel: LogLevel): string {
    switch (logLevel) {

      case LogLevel.debug:
        return 'DEBUG';

      case LogLevel.information:
        return 'INFO';

      case LogLevel.warning:
        return 'WARN';

      case LogLevel.error:
        return 'Error';

      case LogLevel.fatal:
        return 'FATAL';

      default:
        return 'DEBUG';
    }
  }

}
