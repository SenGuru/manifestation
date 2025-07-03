export class RouterUtils {

  public static isNullOrUndefined(num: number): boolean {
    return ((num === null) || (num === undefined));
  }

  public static isInvalidNumber<T>(id: number): boolean {
    return ((id === null) || (id === undefined) || Number.isNaN(id) || id == 0);
  }

  public static getRouterDataNumber<T>(id: number): number | null {
    return this.isInvalidNumber(+id) ? null : +id;
  }

  public static isRouterStringNotValid<T>(id: string): boolean {
    return ((id === null) || (id === undefined) || id == "");
  }

}