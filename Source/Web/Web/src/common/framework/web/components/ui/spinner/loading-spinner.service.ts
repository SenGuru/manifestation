import { BehaviorSubject, Observable } from 'rxjs';

import { Injectable } from '@angular/core';

@Injectable()
export class LoadingSpinnerService {

    public spinnerSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);

    public constructor() {
        // 
    }

public show(): void {
        // tslint:disable-next-line: no-debugger
        const counter = this.spinnerSubject.value + 1;
        this.spinnerSubject.next(counter);
    }

public hide(): void {
        const counter = this.spinnerSubject.value - 1;
        this.spinnerSubject.next(counter);
    }

    public getMessage(): Observable<number> {
        return this.spinnerSubject.asObservable();
    }
}
