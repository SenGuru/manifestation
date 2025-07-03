import { TestBed } from '@angular/core/testing';

import { TitleService } from './title.service';

describe('TitleService', (): void => {
  let service: TitleService;

  beforeEach((): void => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TitleService);
  });

  it('should be created', (): void => {
    expect(service).toBeTruthy();
  });
});
