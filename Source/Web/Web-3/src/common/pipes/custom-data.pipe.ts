import { DatePipe, LowerCasePipe } from '@angular/common';
import { Injectable, Pipe, PipeTransform, } from '@angular/core';

import { ObjectUtils } from '../framework/common/utils/object.utils';

/**
 * Transforms any input value
 */
@Pipe({
  name: 'customDate'
})
@Injectable()
export class CustomDate implements PipeTransform {
  public transform(date: Date | string, format: string): string {
      let result: string | null = '';
      if (!ObjectUtils.isNullOrUndefined(date)) {
      date = new Date(date); // if orginal type was a string
      const dd = date.getUTCDate().toString();
      const ll = date.getMonth().toString();
      const yy = date.getFullYear().toString();
      const hh = date.getUTCHours().toString();
      const mm = date.getUTCMinutes().toString();
      const newDate = new Date(Number(yy), Number(ll), Number(dd), Number(hh), Number(mm));
      const agent = window.navigator.userAgent.toLowerCase();
      let localDate = '';
      if (format === 'date') {
        result = new DatePipe('en-US').transform(newDate, 'LL/dd/yy');
      }
      if (format === 'datetime') {
        result = new DatePipe('en-US').transform(newDate, 'LL/dd/yy hh:mm a');
      }
      if (format === 'datetimezone') {
        let timeOffsetInMS: number = date.getTimezoneOffset();
        timeOffsetInMS = timeOffsetInMS < 0 ? timeOffsetInMS * 60000 * -1 : timeOffsetInMS * 60000;
        // if (timeOffsetInMS < 0) {
          // localDate = new Date(date.setHours(date.getHours() + 8)).toString() + 'Z';
        // } else if (timeOffsetInMS === 28800) {
          // localDate = date.toString();
        // }

        if (agent.indexOf('firefox') > -1) {
          newDate.setTime(date.getTime() + (timeOffsetInMS));
          result = new DatePipe('en-US').transform(newDate, 'LL/dd/yy hh:mm a');
        } else {
          localDate = date.toString() + 'Z';
          result = new DatePipe('en-US').transform(localDate, 'LL/dd/yy hh:mm a');
        }
      }
      if (format === 'datezone') {
        const timeOffsetInMS: number = date.getTimezoneOffset();
        // timeOffsetInMS = timeOffsetInMS < 0 ? timeOffsetInMS * 60000 * -1 : timeOffsetInMS * 60000;
        // newDate.setTime(date.getTime() + (timeOffsetInMS));
        // if (timeOffsetInMS < 0) {
        //   localDate = new Date(date.setHours(date.getHours() + 8)).toString() + 'Z';
        // } else if (timeOffsetInMS === 28800) {
        //  localDate = date.toString();
        // }
        if (agent.indexOf('firefox') > -1) {
          if (timeOffsetInMS < 0) {
            newDate.setTime(date.getTime() + (timeOffsetInMS));
          }
          result = new DatePipe('en-US').transform(newDate, 'LL/dd/yy');
        } else {
          if (timeOffsetInMS > 0) {
            result = new DatePipe('en-US').transform(date.toString(), 'LL/dd/yy');
          } else {
            localDate = date.toString() + 'Z';
            result = new DatePipe('en-US').transform(localDate, 'LL/dd/yy');
          }
        }
      }
    }
    return new LowerCasePipe().transform(result) ?? '';
  }
}
