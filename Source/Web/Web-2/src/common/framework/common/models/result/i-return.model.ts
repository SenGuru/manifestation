import { ReturnError } from './return-error.model';

export interface IReturnModel {
    isSuccess: boolean;
    successMessage?: string | undefined;
    errorHolder?: ReturnError;
    activityId?: string | undefined;
    processId?: number;
    activityDateTime?: Date | null;
    server?: string | undefined;
}

