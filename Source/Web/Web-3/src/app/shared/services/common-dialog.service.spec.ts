import { CommonDialogService } from './common-dialog.service';
import { TestBed } from '@angular/core/testing';

fdescribe('commonDialogService', (): void => {
  let service: CommonDialogService;

  beforeEach((): void => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommonDialogService);
  });

  fit('should return default message', (): void => {
    const defaultMessage = 'Passed';
    const title = 'Testing all Values'
    expect(service.errorWithTitle(title,defaultMessage)).toBeTruthy()
  });

  it('should return default message error is undefined', (): void => {
    const obj = {
       success: {
         errorHolder: {},
         isSuccess : true,
         successMessage: ''
       }
    };
    const defaultMessage = 'Passed';
    const title = 'Testing error undefined'
    expect(service.errorWithTitle(title,defaultMessage)).toBeTruthy()
  });

  it('should return default message http response is null', (): void => {
    const obj = null;
    const defaultMessage = 'Passed';
    const title = 'Testing error undefined'
    expect(service.errorWithTitle(title,defaultMessage)).toBeTruthy()
  });

  it('should return default message http response is undefined', (): void => {
    const obj = undefined;
    const defaultMessage = 'Passed';
    const title = 'Testing error undefined'
    expect(service.errorWithTitle(title,defaultMessage)).toBeTruthy()
  });

  it('should return default message isSuccess property is mising', (): void => {
    const obj = {
      error: {
        errorHolder: {},
        isError : true,
        successMessage: ''
      }
   };
    const defaultMessage = 'Passed';
    const title = 'Testing error undefined'
    expect(service.errorWithTitle(title,defaultMessage)).toBeTruthy()
  });

  it('should return error within error holder', (): void => {
    const obj = {
      error: {
        errorHolder: {friendlyMessage :'Custom Error Message'},
        isError : true,
        successMessage: ''
      }
   };
    const defaultMessage = 'Passed';
    const title = 'Testing error undefined'
    expect(service.errorWithTitle(title,defaultMessage)).toBeTruthy();
  });
});
