import { NgZone } from '@angular/core';
export declare class RageConnectorService {
    private zone;
    private events;
    private callbackEvents;
    constructor(zone: NgZone);
    rageEventHandler(eventName: string, ...args: any): void;
    /**
     * Adds an "event listener" for "events" from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param eventName Name of the event used in RAGE
     * @param callback Any function
     */
    listen(eventName: string, callback: (...args: any) => void): void;
    call(eventName: string, ...args: any): void;
    /**
     * Calls an event and adds an "event listener" for a response from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param eventName Name of the event used in RAGE
     * @param args Any arguments
     * @param callback Any function
     */
    callCallback(eventName: string, args: any[] | undefined, callback: (...args: any) => void): void;
}
