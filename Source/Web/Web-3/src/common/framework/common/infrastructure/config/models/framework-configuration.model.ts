import { LogVerbosity } from '../../log/log-verbosity';

export class FrameworkConfiguration {
    public appTimeoutInSecs: number;
    public dialogTimeoutInSecs: number;
    public verbosity: LogVerbosity;

   public constructor() {
        this.appTimeoutInSecs = 15;
        this.dialogTimeoutInSecs = 5;
        this.verbosity = LogVerbosity.normal;
    }

}
