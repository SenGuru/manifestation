import { Pipe, PipeTransform } from '@angular/core';

import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';

@Pipe({
    name: 'secondsToHour',
    standalone: false
})
export class SecondsToHourPipe implements PipeTransform {

  public transform(value: number): string {
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

}
