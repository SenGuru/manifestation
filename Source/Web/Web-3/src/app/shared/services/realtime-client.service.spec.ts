import { RealtimeClientService } from './realtime-client.service';
import { TestBed } from '@angular/core/testing';

describe('RealtimeClientService', (): void => {
  let service: RealtimeClientService;

  beforeEach((): void => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RealtimeClientService);
  });

  it('should be created', (): void => {
    expect(service).toBeTruthy();
  });
});
