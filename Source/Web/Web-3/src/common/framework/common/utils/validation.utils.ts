import { NgForm } from '@angular/forms';
import { ObjectUtils } from './object.utils';
import { ReturnErrorItem } from '../models/result/return-error-item.model';

export class ValidationUtils {

    public static isWholeNumber(obj: unknown): boolean {
        // tslint:disable-next-line: use-isnan
        if (obj == null || obj === undefined || (Number.isNaN(obj))) {
            return false;
        }
        return Number(obj) % 1 === 0;
    }

    public static setNgFormValidation(errors: ReturnErrorItem[], f: NgForm): NgForm {
        if (!ObjectUtils.isNullOrUndefined(errors)) {
            if (errors.length > 0) {
                errors.forEach(prop => {
                    for (const field in f.controls) { // 'field' is a string
                        if (field.toUpperCase() === prop.key.toUpperCase()) {
                            f.controls[field]?.setErrors({ invalid: true });
                            break;
                        }
                    }
                });
            }
        }
        return f;
    }
}
