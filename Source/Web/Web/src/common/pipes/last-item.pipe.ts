import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'lastItem',
    standalone: false
})
export class LastItemPipe implements PipeTransform {
  public transform(value: string): string {
    if (value) {
      const pathSegments = value.split('\\'); 
      return pathSegments.length > 0 ? pathSegments[pathSegments.length - 1] ?? '' : '';
    } else {
      return '';
    }
  }
}
