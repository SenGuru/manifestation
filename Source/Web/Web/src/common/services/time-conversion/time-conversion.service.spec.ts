import { TestBed } from '@angular/core/testing';

import { TimeConversionService } from './time-conversion.service';

describe('TimeConversionService', (): void => {
  let service: TimeConversionService;

  beforeEach((): void => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TimeConversionService);
  });

  it('should be created', (): void => {
    expect(service).toBeTruthy();
  });
});
