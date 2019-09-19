import { Injectable, NgZone } from '@angular/core';

declare const mp: {
  trigger(eventName: string, ...args: any): void;
};
declare const window: any;

@Injectable({
  providedIn: 'root'
})
export class RageConnectorService {

  private events: {[key: string]: ((...args: any) => void)[]} = {};
  private callbackEvents: {[key: string]: ((...args: any) => void)[]} = {};

  constructor(private zone: NgZone) {
    window.RageAngularEvent = this.rageEventHandler;
  }

  public rageEventHandler(eventName: string, ...args: any) {
    this.zone.run(() => {
      if (this.events[eventName]) {
        for (const func of this.events[eventName]) {
          func(...args);
        }
      }
      if (this.callbackEvents[eventName]) {
        const callbackFunctions = this.callbackEvents[eventName];
        this.callbackEvents[eventName] = undefined;
        for (const func of callbackFunctions) {
          func(...args);
        }
      }
    });
  }

  /**
   * Adds an "event listener" for "events" from RAGE clientside.
   * Don't forget to use lambda for callback or add bind(this),
   * else "this" in the function will refer to rage-connector.service.ts instead.
   * @param eventName Name of the event used in RAGE
   * @param callback Any function
   */
  public listen(eventName: string, callback: (...args: any) => void) {
    if (!this.events[eventName]) {
      this.events[eventName] = [];
    }
    this.events[eventName].push(callback);
  }

  public call(eventName: string, ...args: any) {
    if (typeof mp == "undefined") // testing without RAGE
      return;
    mp.trigger(eventName, ...args);
  }

  /**
   * Calls an event and adds an "event listener" for a response from RAGE clientside.
   * Don't forget to use lambda for callback or add bind(this),
   * else "this" in the function will refer to rage-connector.service.ts instead.
   * @param eventName Name of the event used in RAGE
   * @param args Any arguments
   * @param callback Any function
   */
  public callCallback(eventName: string, args: any[] | undefined, callback: (...args: any) => void) {
    if (typeof mp == "undefined") // testing without RAGE
      return;
    if (!this.callbackEvents[eventName]) {
      this.callbackEvents[eventName] = [];
    }
    this.callbackEvents[eventName].push(callback);
    if (args)
      mp.trigger(eventName, ...args);
    else
      mp.trigger(eventName);
  }
}
