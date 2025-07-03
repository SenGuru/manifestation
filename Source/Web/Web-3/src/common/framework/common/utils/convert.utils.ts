import { ObjectUtils } from './object.utils';
import { PredefinedCurrencyTypes } from 'src/common/models/app-enums';

export class ConvertUtils {
  public static toNumber(obj: unknown, defaultValue = 0): number {
    if (ObjectUtils.isNullOrUndefined(obj)) {
      return defaultValue;
    }
    const result = Number(obj);
    if (isNaN(result)) {
      return defaultValue;
    }
    return result;
  }

  public static splitByUpperCase(obj: string): string {
    if (ObjectUtils.isNullOrUndefined(obj)) {
      return obj;
    }
    return obj.match(/[A-Z][a-z]+|[0-9]+/g)?.join(' ') ?? '';
  }

  public static lowerCaseFirstLetter(field: string): string {
    return field.charAt(0).toLowerCase() + field.slice(1);
  }

  public static upperCaseFirstLetter(field: string): string {
    return field.charAt(0).toUpperCase() + field.slice(1);
  }

  public static getFileExtension3(filename: string): string {
    return filename.slice((filename.lastIndexOf(".") - 1 >>> 0) + 2);
  }

  public static getFormattedCurrecyText(value: number, predefinedCurrencyTypeId?: number): string {
    if (predefinedCurrencyTypeId === PredefinedCurrencyTypes.USD) {
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 2
      }).format(value);
    } else {
      return new Intl.NumberFormat('en-IN', {
        style: 'currency',
        currency: 'INR',
        minimumFractionDigits: 2
      }).format(value);
    }
  }

}
