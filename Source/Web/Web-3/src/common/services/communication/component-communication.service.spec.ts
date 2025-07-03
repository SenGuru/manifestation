import { ComponentCommunicationService } from './component-communication.service';
import { TestBed } from '@angular/core/testing';

describe('ComponentCommunicationService', (): void => {
  let service: ComponentCommunicationService;

  beforeEach((): void => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ComponentCommunicationService);
  });

  it('should be created', (): void => {
    expect(service).toBeTruthy();
  });
});
