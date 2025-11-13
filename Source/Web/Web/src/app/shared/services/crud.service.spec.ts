import { CrudService } from './crud.service';
import { TestBed } from '@angular/core/testing';

describe('CrudService', (): void => {
  let service: CrudService;

  beforeEach((): void => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CrudService);
  });

  it('should be created', (): void => {
    expect(service).toBeTruthy();
  });
});
