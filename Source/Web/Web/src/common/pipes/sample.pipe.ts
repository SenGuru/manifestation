import { Injectable, Pipe, PipeTransform } from '@angular/core';

/**
 * Transforms any input value
 */
@Pipe({
    name: 'samplePipe',
    standalone: false
})
@Injectable()
export class SamplePipe implements PipeTransform {
  public transform(value: unknown, args?: unknown[]): string {
    return value?.toString() ?? '';
  }
}
