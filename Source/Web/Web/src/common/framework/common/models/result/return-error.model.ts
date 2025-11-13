import { ReturnErrorItem } from './return-error-item.model';
import { ReturnException } from './return-exception.model';

export class ReturnError {
   public constructor(public friendlyMessage?: string, public internalErrorMessage?: string,
                public errorItemList?: ReturnErrorItem[], public exception?: ReturnException,
               ) {
    }
}

