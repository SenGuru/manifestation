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
  
  public static getWrappedTextWithIdx(value: string, index?: number): string {
    const idx = index ? index : 30;
    let result = '';
    if (value !== null && value !== undefined) {
      result = value.length > idx ? value.slice(0, (idx - 1)) + '...' : value;
    }
    return result;
  }

  public static generateGUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
      const r = Math.random() * 16 | 0;
      const v = c === 'x' ? r : (r & 0x3 | 0x8); // UUID v4 format
      return v.toString(16);
    });
  }

}
