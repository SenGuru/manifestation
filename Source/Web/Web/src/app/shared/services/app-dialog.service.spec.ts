import { AppDialogService } from './app-dialog.service';
import { TestBed } from '@angular/core/testing';

describe('AppDialogService', (): void => {
  let service: AppDialogService;

  beforeEach((): void => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppDialogService);
  });

  it('should be created', (): void => {
    expect(service).toBeTruthy();
  });
});
