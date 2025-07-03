import { LogLevelHelper } from './log-level-helper';

export class LogEntryModel {
    public constructor(public verbosity: number, public logLevel: number, public date: Date, public app: string, public message: string) {
    }

    public formattedMessage(): string {
        const date = `${this.date.getHours()}:${this.date.getMinutes()}:${this.date.getSeconds()}:${this.date.getMilliseconds()}}:`;
        const lLevel = LogLevelHelper.toStringValue(this.logLevel);
        const vLevel = LogLevelHelper.toVerboseStringValue(this.verbosity);
        return `CTWeb\t${vLevel}\t${lLevel}\t${'CTWeb'}\t${date}\t{}\t${this.message}`;
      }
}
