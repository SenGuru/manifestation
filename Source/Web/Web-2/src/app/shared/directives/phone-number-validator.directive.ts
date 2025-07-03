/* eslint-disable @typescript-eslint/no-unsafe-assignment */

import { AbstractControl, NG_VALIDATORS, UntypedFormControl, ValidationErrors, Validator, ValidatorFn } from '@angular/forms';

import { Directive } from '@angular/core';
import { parsePhoneNumberFromString } from 'libphonenumber-js';

@Directive({
    selector: '[appPhoneNumberValidator]',
    providers: [
        {
            provide: NG_VALIDATORS,
            useClass: PhoneNumberValidatorDirective,
            multi: true
        }
    ],
    standalone: false
})
export class PhoneNumberValidatorDirective implements Validator {

  public validator: ValidatorFn;
  public constructor() {
    this.validator = this.phoneNumberValidator();
  }

  public validate(c: UntypedFormControl): ValidationErrors | null {
    return this.validator(c);
  }

  public phoneNumberValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      if (control.value != null && control.value !== '') {
        // eslint-disable-next-line  
        const phoneNumber = parsePhoneNumberFromString(control.value, 'IN');
        // eslint-disable-next-line  
        const isValid = phoneNumber ? phoneNumber.isValid() : false;
        if (isValid) {
          return null;
        } else {
          return {
            phoneNumberValidator: { valid: false }
          };
        }
      } else {
        return null;
      }
    };
  }

}
