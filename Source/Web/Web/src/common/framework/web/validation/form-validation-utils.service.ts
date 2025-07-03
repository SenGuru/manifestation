import { FormControl } from '@angular/forms';
import { Injectable } from '@angular/core';
import { ObjectUtils } from '../../common/utils/object.utils';
import { StringUtils } from '../../common/utils/string.utils';
import { ValidationUtils } from '../../common/utils/validation.utils';

@Injectable()
export class FormValidationUtils {

     public nameValidator(control: FormControl): object {
        const falseResult = { 'nameValidator': true };
        if (ObjectUtils.isNullOrUndefined(control) || ObjectUtils.isNullOrUndefined(control.value)) {
            return falseResult;
        }

        // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
        if (StringUtils.isArrNullOrEmpty(control.value) === false) {
            return {};
        } else {
            return falseResult;
        }
    }

    public wholeNumberValidator(control: FormControl): object {
        const falseResult = { 'wholeNumberValidator': true };
        if (ObjectUtils.isNullOrUndefined(control) || ObjectUtils.isNullOrUndefined(control.value)) {
            return falseResult;
        }

        if (ValidationUtils.isWholeNumber(control.value)) {
            return {};
        } else {
            return falseResult;
        }
    }

}
