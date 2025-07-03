import { AbstractControl, NG_VALIDATORS, UntypedFormControl, ValidationErrors, Validator, ValidatorFn } from '@angular/forms';

import { Directive } from '@angular/core';

@Directive({
    selector: '[appEmailvalidator]',
    providers: [
        {
            provide: NG_VALIDATORS,
            useClass: EmailvalidatorDirective,
            multi: true
        }
    ],
    standalone: false
})
export class EmailvalidatorDirective implements Validator {

  public validator: ValidatorFn;
  public constructor() {
    this.validator = this.emailValidator();
  }

  public validate(c: UntypedFormControl): ValidationErrors | null {
    return this.validator(c);
  }

  public emailValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      if (control.value != null && control.value !== '') {
        // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
        const isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(control.value);
        if (isValid) {
          return null;
        } else {
          return {
            emailvalidator: { valid: false }
          };
        }
      } else {
        return null;
      }
    };
  }
}
