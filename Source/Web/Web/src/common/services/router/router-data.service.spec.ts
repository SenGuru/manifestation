import { RouterDataService } from './router-data.service';
import { TestBed } from '@angular/core/testing';

describe('RouterDataService', (): void => {
  let service: RouterDataService;

  beforeEach((): void => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RouterDataService);
  });

  it('should be created', (): void => {
    expect(service).toBeTruthy();
  });
});
