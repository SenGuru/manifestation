import { Subject, Subscription } from 'rxjs';
import { filter, map } from 'rxjs/operators';

import { Injectable } from '@angular/core';

interface Event {
    type: string | undefined;
    payload?: unknown;
}

type EventCallback = (payload: unknown) => void;

@Injectable({
    providedIn: 'root'
})
export class EventService {

    private handler = new Subject<Event>();
    public constructor() {
        // 
    }

    /**
     * Broadcast the event
     * @param type type of event
     * @param payload payload
     */
    public broadcast(type: string, payload = {}): void {
        this.handler.next({ type, payload });
    }

    /**
     * Subscribe to event
     * @param type type of event
     * @param callback call back function
     */
    public subscribe(type: string, callback: EventCallback): Subscription {
        return this.handler.pipe(
            filter(event => event.type === type)).pipe(
                map(event => event.payload))
            .subscribe(callback);
    }
}