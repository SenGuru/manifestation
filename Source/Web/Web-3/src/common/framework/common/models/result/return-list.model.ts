import { IReturnModel } from './i-return.model';
import { ObjectUtils } from '../../utils/object.utils';
import { ReturnError } from './return-error.model';
import { ReturnErrorItem } from './return-error-item.model';
import { ReturnException } from './return-exception.model';
import { ReturnModel } from './return.model';
import { StringUtils } from '../../utils/string.utils';

export class ReturnListModel<T> implements IReturnModel {
    public constructor(public model?: T[], public totalRecords?: number,
        public isSuccess: boolean = false, public successMessage?: string, public errorHolder?: ReturnError) {
    }

    public static success<T>(result: T[]): ReturnListModel<T> {
        const value = new ReturnListModel<T>();
        value.model = result;
        value.totalRecords = result.length;
        value.isSuccess = true;
        return value;
    }

    public static successWithTotalPages<T>(result: T[], totalPages: number): ReturnListModel<T> {
        return new ReturnListModel<T>(result, totalPages, true);
    }

    public static errorWithException<T>(ex: ReturnException): ReturnListModel<T> {
        const value = new ReturnListModel<T>();
        value.isSuccess = false;
        value.errorHolder = new ReturnError('', '', [], ex);
        return value;
    }

    public static errorWithExceptionAndMessage<T>(errorMsg: string, ex?: ReturnException): ReturnListModel<T> {
        const value = new ReturnListModel<T>();
        value.isSuccess = false;
        value.errorHolder = new ReturnError(errorMsg, '', [], ex);
        return value;
    }

    public static errorWithExceptionAndErrorList<T>(errorMsg: string, errorList: ReturnErrorItem[]): ReturnListModel<T> {
        const value = new ReturnListModel<T>();
        value.isSuccess = false;
        value.errorHolder = new ReturnError(errorMsg, '', errorList);
        return value;
    }

    public static isSuccess<T>(result: ReturnModel<T>): boolean {
        if (result == null || result === undefined || result.isSuccess === undefined ||
            result.isSuccess === false || result.model == null || result.model === undefined) {
            return false;
        }
        return true;
    }

    public static getError<T>(result: ReturnModel<T>, defaultErrorMsg?: string): ReturnError | null {

        let value: ReturnError | null = null;
        if (defaultErrorMsg) {
            value = new ReturnError(defaultErrorMsg);
        }

        if (ObjectUtils.isNullOrUndefined(result) || ObjectUtils.isNullOrUndefined(result.errorHolder)) {
            return value;
        }
        return result.errorHolder ?? null;
    }

    public static getErrorMessage<T>(result: ReturnModel<T>, defaultErrorMsg?: string): string {

        if (ObjectUtils.isNullOrUndefined(result) || ObjectUtils.isNullOrUndefined(result.errorHolder) ||
            (ObjectUtils.isNullOrUndefined(result.errorHolder?.friendlyMessage)
            )) {
            return defaultErrorMsg ?? '';
        }

        if (!StringUtils.isArrNullOrEmpty(result.errorHolder?.friendlyMessage ?? '')) {
            return result.errorHolder?.friendlyMessage ?? '';
        }

        if (!StringUtils.isArrNullOrEmpty(result.errorHolder?.internalErrorMessage ?? '')) {
            return result.errorHolder?.internalErrorMessage ?? '';
        }

        if (!ObjectUtils.isNullOrUndefined(result.errorHolder?.errorItemList) &&
            (result.errorHolder?.errorItemList?.length ?? 0) > 0 && !ObjectUtils.isNullOrUndefined(result.errorHolder?.errorItemList?.find(i => i)?.value)) {
            return result.errorHolder?.errorItemList?.find(i => i)?.value ?? '';
        }

        if (!ObjectUtils.isNullOrUndefined(result.errorHolder?.exception) &&
            !StringUtils.isArrNullOrEmpty(result.errorHolder?.exception?.message ?? '') &&
            (result.errorHolder?.exception?.message ?? '') !== '') {
            return result.errorHolder?.exception?.message ?? '';
        }
        return defaultErrorMsg ?? '';
    }

    public static fromJsonString<T>(jsonStr: string): T {
        const obj: T = JSON.parse(jsonStr) as T;
        return obj;
    }

}
