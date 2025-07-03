import { Injectable } from '@angular/core';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';

@Injectable({
  providedIn: 'root'
})
export class TimeConversionService {

  public constructor() {
    //
  }

  public getFormattedTime(value: number): string {
    if (ObjectUtils.isNullOrUndefined(value) || !value) {
      return '';
    }
    const hours = Math.floor(value / 3600);
    const minutes = Math.floor((value % 3600) / 60);

    // Format hours, minutes, and seconds into a string
    const formattedTime = `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}`;
    return formattedTime;
  }

  public convertTimeToSeconds(time: string): number | null {
    if (!time || ObjectUtils.isNullOrUndefined(time)) return null;
    // Parse the input as needed, assuming it's in the format "HH:MM:SS"
    const [hours, minutes] = time.split(':').map(Number);

    // Convert to seconds
    return ((hours ?? 0) * 3600) + ((minutes ?? 0) * 60);
  }

  // Function to convert hours to 12-hour format
  public formatHours(hours: number): number {
    if (hours === 0) {
      return 12; // Midnight
    } else if (hours > 12) {
      return hours - 12; // Afternoon
    } else {
      return hours; // Morning
    }
  }

  // Function to add leading zero to single-digit numbers
  public padZero(num: number): string {
    return num < 10 ? '0' + num : '' + num;
  }

}
