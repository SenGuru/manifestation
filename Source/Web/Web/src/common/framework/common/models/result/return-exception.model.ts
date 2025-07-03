export class ReturnException {
   public constructor(public message?: string, public exceptionType?: string,
                public stackTrace?: string, public source?: string,
                public innerException?: ReturnException) {
    }

}
