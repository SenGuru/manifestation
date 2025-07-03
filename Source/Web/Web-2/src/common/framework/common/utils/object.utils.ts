import { COMMON_POCO_FIELDS } from 'src/common/constants/app.constants';
import { IStringIndexedObjModel } from '../models/i-string-indexed-object,model';
import { Injectable } from '@angular/core';

@Injectable()
export class ObjectUtils {
  public static isNullOrUndefined<T>(obj: T): boolean {
    return ((obj === null) || (obj === undefined));
  }

  public static isObjNullOrUndefined<T>(obj: T): boolean {
    return ((obj === null) || (obj === undefined) || (Object.keys(obj).length === 0));
  }

  public static isArrWithItem<T>(obj: T): boolean {
    return (Array.isArray(obj) && (obj !== undefined) && obj.length > 0);
  }

  public static isRouterNumberNotValid(id: number): boolean {
    return ((id === null) || (id === undefined) || Number.isNaN(id) || id == 0);
  }

  public static isRouterStringNotValid(id: string): boolean {
    return ((id === null) || (id === undefined) || id == "");
  }

  public static getRouterDataNumber(id: number): number | null {
    return this.isRouterNumberNotValid(+id) ? null : +id;
  }

  public static isArrNullOrEmpty<T>(arr: T[]): boolean {
    return ((arr === null) || (arr === undefined) || arr.length === 0);
  }

  public static hasKey(obj: object, key: string): boolean {
    for (const prop of Object.keys(obj)) {
      if (prop === key) {
        return true;
      }
    }
    return false;
  }

  public static getWrappedText(value: string, size: number, hideElipse: boolean = false): string {
    let result = '';
    if (size <= 0) size = 1
    if (value !== null && value !== undefined) {
      const elipses = hideElipse ? '' : '...';
      result = value.length > size ? value.slice(0, (size - 1)) + elipses : value;
    }
    return result;
  }

  public static getExcludedCommonPropertiesFromModel<T>(model: T): string[] {
    let result: string[] = [];
    if (model == null || model == undefined || !model) return result;
    const properties = Object.getOwnPropertyNames(model)
    result = properties
      .filter(x => !x.toLowerCase().includes("id") && !x.toLowerCase().includes("ids") && !COMMON_POCO_FIELDS.includes(x))
      .map(x => x);

    return result;
  }

  public static getGroupedDataByProperty<T>(data: T[], property: string): T[][] {
    if ((property === null || property === undefined || property === "") || (data === null || data === undefined || data.length === 0)) return [];
    const obj: T[][] = Object.values(data.reduce((acc: object, item: T) => {
      // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-unsafe-member-access
      // @ts-expect-error -- ignore this error
      // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
      acc[item[property]] = [...(acc[item[property]] || []), item];
      return acc;
    }, {}));

    return obj;
  }

  public static assignExistingProperties(targetObj: object, sourceObj: object): void {
    Object.keys(targetObj).forEach(key => {
      if (Object.hasOwn(sourceObj, key)) {
        //@ts-expect-error -- the line can contains error
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        targetObj[key] = sourceObj[key];
      }
    });
  }

  public static swapItems<T>(array: T[], index1: number, index2: number): T[] {
    const result = [...array];
    if (index1 >= 0 && index1 < array.length && index2 >= 0 && index2 < array.length) {
      [result[index1], result[index2]] = [result[index2] as T, result[index1] as T];
    }
    return result;
  }

}