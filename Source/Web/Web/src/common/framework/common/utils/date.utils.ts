import { ObjectUtils } from "./object.utils";

export class DateUtils {

  private static _monthList: string[] = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
  public constructor() {
    // 
  }

  public static timeDiffInMilliSec(startTime: Date, endTime: Date): number {
    const timeDiff = endTime.valueOf() - startTime.valueOf();
    return timeDiff;
  }

  public static timeDiffInSec(startTime: Date, endTime: Date): number {
    let timeDiff = this.timeDiffInMilliSec(startTime, endTime);
    timeDiff /= 1000;
    const seconds = Math.round(timeDiff);
    return seconds;
  }

  public static timeDiffInMin(startTime: Date, endTime: Date): number {
    let timeDiff = this.timeDiffInSec(startTime, endTime);
    timeDiff /= 60;
    return timeDiff;
  }

  public static dateToString(value: Date): string {
    if (value === undefined || value === null)
      return "";
    const zeroPad = (n: number): string | number => n > 9 ? n : '0' + n;
    return `${value.getFullYear()}-${zeroPad(value.getMonth() + 1)}-${zeroPad(value.getDate())}`;

  }

  public static parseAsLocal(value: Date): Date {
    if (value === undefined || value === null)
      return value;
    const inputDate = new Date(value);
    return new Date(inputDate.getUTCFullYear(), inputDate.getUTCMonth(), inputDate.getUTCDate(), inputDate.getUTCHours(), inputDate.getUTCMinutes(), inputDate.getUTCSeconds(), inputDate.getUTCMilliseconds());
  }

  public static getMonthNamefromMonthNumber(value: number): string {
    return DateUtils._monthList[value] ?? '';
  }

  public static parseAsUTC(value: Date): Date {
    if (ObjectUtils.isNullOrUndefined(value)) return value;
    const workDayDateValue = new Date(value);
    // Adjust the date to UTC without changing the time
    const utcDate = new Date(workDayDateValue.getTime() - (workDayDateValue.getTimezoneOffset() * 60000));
    // Format the date to the desired UTC string format
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const utc: any = utcDate.toISOString().replace('Z', '') + 'Z';
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    return utc as Date;
  }

  public static getDateValueWithoutTime(date: Date): Date {
    if (ObjectUtils.isNullOrUndefined(date)) return date;
    const sd = String(date)?.split("T");
    if (sd && sd.length > 1) {
      const dateValue = sd[0] ?? ''; // yyyy-mm-dd
      return new Date(dateValue);
    } else {
      return date;
    }
  }

  public static convertSecondsToHour(value: number): string {
    if (ObjectUtils.isNullOrUndefined(value) || !value) {
      return '';
    }
    const hours = Math.floor(value / 3600);
    const minutes = Math.floor((value % 3600) / 60);
    const period = hours >= 12 ? 'PM' : 'AM';
    // Convert hour from 24-hour format to 12-hour format
    const displayHour = hours % 12 || 12;
    // Format hours, minutes into a string
    const formattedTime = `${String(displayHour).padStart(2, '0')}:${String(minutes).padStart(2, '0')} ${period}`;
    return formattedTime;
  }

  public static getFormattedDate(date: Date): string {
    if (!date) return '';
    const dateValue = new Date(date);
    const day = dateValue.getDate().toString().padStart(2, '0');
    const month = (dateValue.getMonth() + 1).toString().padStart(2, '0');
    const year = dateValue.getFullYear();
    return `${day}/${month}/${year}`;
  }

  public static getFormattedDateWithTime(date: Date): string {
    if (!date) return '';
    const dateValue = new Date(date);
    const day = dateValue.getDate().toString().padStart(2, '0');
    const month = (dateValue.getMonth() + 1).toString().padStart(2, '0');
    const year = dateValue.getFullYear(); 
    const hours = dateValue.getHours();
    const minutes = dateValue.getMinutes().toString().padStart(2, '0');
    const ampm = hours >= 12 ? 'PM' : 'AM';
    const formattedHours = (hours % 12 || 12).toString().padStart(2, '0');
    return `${day}/${month}/${year} ${formattedHours}:${minutes} ${ampm}`; 
  }
 
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public getUtcDateValue(d: Date): any {
    const workDayDateValue = new Date(d);

    // Adjust the date to UTC without changing the time
    const utcDate = new Date(workDayDateValue.getTime() - (workDayDateValue.getTimezoneOffset() * 60000));

    // Format the date to the desired UTC string format
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const utcString: any = utcDate.toISOString().replace('Z', '') + 'Z';
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    return utcString;
  }
}
