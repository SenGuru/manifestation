import { DatePipe, LowerCasePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { UtcToBrowserTimeDateFormatType } from 'src/common/models/app-custom-model';

@Pipe({
    name: 'utcToBrowserTime',
    standalone: false
})
export class UtcToBrowserTimePipe implements PipeTransform {

  public transform(date: Date | string, type?: UtcToBrowserTimeDateFormatType): string {
    if (ObjectUtils.isNullOrUndefined(date) || date == '') return "";
    date = new Date(date);
    let timeOffsetInMS: number = date.getTimezoneOffset();
    timeOffsetInMS = timeOffsetInMS < 0 ? timeOffsetInMS * 60000 * -1 : timeOffsetInMS * 60000;
    const dd = date.getUTCDate().toString();
    const ll = date.getMonth().toString();
    const yy = date.getFullYear().toString();
    const hh = date.getUTCHours().toString();
    const mm = date.getUTCMinutes().toString();
    const newDate = new Date(Number(yy), Number(ll), Number(dd), Number(hh), Number(mm));
    const agent = window.navigator.userAgent.toLowerCase();

    let result: string | null | undefined = '';
    let localDate = '';
    if (type == 'split') {
      if (agent.indexOf('firefox') > -1) {
        newDate.setTime(date.getTime() + (timeOffsetInMS));
        const localTime = new DatePipe('en-US').transform(newDate, 'hh:mm a');
        const localDay = new DatePipe('en-US').transform(newDate, 'LL/dd/yy');
        result = `${localDay}\n${localTime}`;
      } else {
        localDate = date.toString() + 'Z';
        const localTime = new DatePipe('en-US').transform(localDate, 'hh:mm a');
        const localDay = new DatePipe('en-US').transform(localDate, 'LL/dd/yy');
        result = `${localDay}\n${localTime}`;
      }
    } else if (type == 'formatted') {
      localDate = date.toString() + 'Z';
      result = new DatePipe('en-US').transform(localDate, 'd MMM y, EEEE')?.toString();
    }
    else {
      if (agent.indexOf('firefox') > -1) {
        newDate.setTime(date.getTime() + (timeOffsetInMS));
        result = new DatePipe('en-US').transform(newDate, 'LL/dd/yy hh:mm a');
      } else {
        localDate = date.toString() + 'Z';
        result = new DatePipe('en-US').transform(localDate, 'LL/dd/yy hh:mm a');
      }
    }
    if (type == 'formatted') {
      return result ?? '';
    } else {
      return new LowerCasePipe().transform(result)?.toUpperCase() ?? '';
    }
  }

}
