export class StringUtils {

  public static isArrNullOrEmpty(obj: string): boolean {
    return ((obj === null) || (obj === undefined) || obj.length === 0 || obj.trim() === '');
  }

  public static ifNullOrEmptyUseDefault(obj: string, defaultValue: string): string {
    return this.isArrNullOrEmpty(obj) ? defaultValue : obj;
  }

  /* 
      * Change First Letter as a capital and add a space before every capital letter(start of word)
      * to CAPITALIZED substring of a string, place a circumflex(^) syntex on both sides of the text. like: "Hello ^world^"
      * if needed any extra space use hyphen (-)
      * Input: "maxZipFileSize-in^mb^"  Output: "Max Zip File Size in MB"
      **/
  public static inputHeaderFormat(str: string): string {
    return (str && typeof str === "string") ? str.replace(/^[a-z]/g, x => x.toUpperCase()).replace(/[A-Z]+/g, " $&").trim().replace(/[\^](\w*\s*)+[\^]/g, x => x.toUpperCase()).replace(/[\^]/g, " ").replace(/\\-/g, ' ').replace(/\s\s+/g, ' ').trim() : "";
  }

  public static makeFirstLetterAsUpperCase(value: string): string {
    if (!value) return value;
    return value.charAt(0).toUpperCase() + value.slice(1);
  }

  public static makeFirstLetterAsLowerCase(value: string): string {
    if (!value) return value;
    return value.charAt(0).toLowerCase() + value.slice(1);
  }

  public static getWrappedText(value: string): string {
    let result = '';
    if (value !== null && value !== undefined) {
      result = value.length > 30 ? value.slice(0, 29) + '...' : value;
    }
    return result;
  }

}
