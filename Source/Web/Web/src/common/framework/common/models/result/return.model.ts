import { IReturnModel } from './i-return.model';
import { ObjectUtils } from '../../utils/object.utils';
import { ReturnError } from './return-error.model';
import { ReturnErrorHolder } from './return-error-holder';
import { ReturnErrorItem } from './return-error-item.model';
import { ReturnException } from './return-exception.model';
import { StringUtils } from '../../utils/string.utils';

export class ReturnModel<T> implements IReturnModel {

  // tslint:disable-next-line: max-line-length
  public constructor(public model: T | null = null,
    public successMessage?: string | undefined,
    public isSuccess: boolean = false,
    public errorHolder?: ReturnError,
    public totalRecords?: number,
    public activityId?: string | undefined,
    public processId?: number,
    public activityDateTime?: Date,
    public server?: string | undefined,) { // public statusText: string = '', public status?: number
  }

  public static success<T>(result: T): ReturnModel<T> {
    const value = new ReturnModel<T>();
    value.model = result;
    value.isSuccess = true;
    return value;
  }

  public static error<T>(msg: string): ReturnModel<T> {
    const value = new ReturnModel<T>();
    value.isSuccess = false;
    value.errorHolder = new ReturnError(msg);
    return value;
  }

  public static errorWithException<T>(ex: ReturnException): ReturnModel<T> {
    const value = new ReturnModel<T>();
    value.isSuccess = false;
    value.errorHolder = new ReturnError('', '', [], ex);
    return value;
  }

  public static errorWithExceptionAndMessage<T>(errorMsg: string, ex?: ReturnException): ReturnModel<T> {
    const value = new ReturnModel<T>();
    value.isSuccess = false;
    value.errorHolder = new ReturnError(errorMsg, '', [], ex);
    return value;
  }

  public static errorWithExceptionAndErrorList<T>(errorMsg: string, errorList: ReturnErrorItem[]): ReturnModel<T> {
    const value = new ReturnModel<T>();
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

  public static getErrorHolder<T>(result: ReturnModel<T>, defaultErrorMsg?: string): ReturnErrorHolder | null {

    let value: ReturnErrorHolder | null = null;
    if (defaultErrorMsg) {
      value = new ReturnErrorHolder(defaultErrorMsg);
    }

    if (ObjectUtils.isNullOrUndefined(result) || ObjectUtils.isNullOrUndefined(result.errorHolder)) {
      return value;
    }
    return result?.errorHolder ?? null;
  }

  public static getErrorMessage<T>(result: ReturnModel<T>, defaultErrorMsg?: string): string {
    if (ObjectUtils.isNullOrUndefined(result) || ObjectUtils.isNullOrUndefined(result.errorHolder) ||
      (ObjectUtils.isNullOrUndefined(result.errorHolder?.friendlyMessage))) {
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

  public static getErrorMessageFromHttpError(error: unknown, defaultErrorMsg?: string): string {
    const obj = error as IReturnModel;
    if (obj == null) {
      return String(error);
    } else {
      return '';
      // const rm: ReturnModel<boolean> = obj as ReturnModel<boolean>;
      // return ReturnModel.getErrorMessage(rm , ''); // rm.statusText);
    }
  }

  public static fromJsonString<T>(jsonStr: string): T {
    const obj: T = JSON.parse(jsonStr) as T;
    return obj;
  }
}
