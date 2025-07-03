export class NumberUtils {

  public static isNullOrUndefined(num: number): boolean {
    return ((num === null) || (num === undefined));
  }

  public static isInvalidNumber<T>(id: number): boolean {
    return ((id === null) || (id === undefined) || Number.isNaN(id) || id == 0);
  }

  public cleanFloat(value: string | number): string {
    const num = Number(value);
    if (isNaN(num)) return String(value); // Return as-is if not a number

    // Convert to string with high precision, then trim unnecessary 0s
    let str = num.toPrecision(15); // Use enough precision to capture real value
    str = parseFloat(str).toString(); // Remove trailing zeros/artifacts
    return str;
  }

}