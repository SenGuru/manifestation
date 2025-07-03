import { HttpErrorResponse } from '@angular/common/http';
import { IReturnModel } from './i-return.model';
import { ObjectUtils } from '../../utils/object.utils';
import { ReturnError } from './return-error.model';
import { ReturnErrorHolder } from './return-error-holder';
import { ReturnErrorItem } from './return-error-item.model';
import { StringUtils } from '../../utils/string.utils';

export class ReturnModelHelper {

  public static getError(result: IReturnModel, defaultErrorMsg?: string): ReturnError | null {

    let value: ReturnError | null = null;
    if (defaultErrorMsg) {
      value = new ReturnError(defaultErrorMsg);
    }

    if (ObjectUtils.isNullOrUndefined(result) || ObjectUtils.isNullOrUndefined(result.errorHolder)) {
      return value;
    }
    return result.errorHolder ?? null;
  }

  public static getErrorHolderErrorItems(result: IReturnModel, defaultErrorMsg?: string): ReturnErrorItem[] {
    const value = this.getErrorHolder(result, defaultErrorMsg);
    if (ObjectUtils.isNullOrUndefined(value) || ObjectUtils.isNullOrUndefined(value?.errorItemList)) {
      return [];
    }
    return value?.errorItemList ?? [];
  }

  public static getErrorHolder(result: IReturnModel, defaultErrorMsg?: string): ReturnErrorHolder | null {

    let value: ReturnErrorHolder | null = null;
    if (defaultErrorMsg) {
      value = new ReturnErrorHolder(defaultErrorMsg);
    }

    if (ObjectUtils.isNullOrUndefined(result) || ObjectUtils.isNullOrUndefined(result.errorHolder)) {
      return value;
    }
    return result.errorHolder ?? null;
  }

  public static getErrorMessage(result: IReturnModel, defaultErrorMsg?: string): string {
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

  public static getErrorHolderErrorItemsFromHttpError(httpError: HttpErrorResponse, defaultErrorMsg?: string): ReturnErrorItem[] {
    if (!ObjectUtils.isNullOrUndefined(httpError)) {
      if (!ObjectUtils.isNullOrUndefined(httpError.error)) {
        const returnErrorItems: ReturnErrorItem[] = [];
        const errorHolder = new ReturnErrorItem("Error", httpError?.message ?? defaultErrorMsg);
        returnErrorItems.push(errorHolder);
        return returnErrorItems;
      }
    }
    return [];
  }

  public static determineIfObjectIsReturnModel(obj: object): boolean {
    if (ObjectUtils.isNullOrUndefined(obj)) {
      return false;
    }
    if (!(typeof obj === 'object')) {
      return false;
    }

    if (!('isSuccess' in obj)) {
      return false;
    }
    if (!('errorHolder' in obj)) {
      return false;
    }
    return true;
  }

}
