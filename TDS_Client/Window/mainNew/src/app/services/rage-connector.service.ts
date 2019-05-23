import { Injectable } from '@angular/core';

declare const mp: {
  trigger(eventName: string, ...args: any): void;
};
declare const window: any;

@Injectable({
  providedIn: 'root'
})
export class RageConnectorService {
  private events = new Map<string, (...args: any) => void>();
  private callbackEvents = new Map<string, (...args: any) => void>();

  constructor() {
    window.RageAngularEvent = this.rageEventHandler.bind(this);
  }

  public rageEventHandler(eventName: string, ...args: any) {
    if (this.events.has(eventName)) {
      this.events[eventName](...args);
      return;
    }
    if (this.callbackEvents.has(eventName)) {
      const callbackFunction = this.callbackEvents[eventName];
      this.callbackEvents.delete(eventName);
      callbackFunction(...args);
      return;
    }
  }

  /**
   * Adds an "event listener" for "events" from RAGE clientside.
   * Don't forget to use lambda for callback or add bind(this),
   * else "this" in the function will refer to rage-connector.service.ts instead.
   * @param eventName Name of the event used in RAGE
   * @param callback Any function
   */
  public listen(eventName: string, callback: (...args: any) => void) {
    this.events[eventName] = callback;
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
  public callCallback(eventName: string, args: any[], callback: (...args: any) => void) {
    this.callCallback[eventName] = callback;
    mp.trigger(eventName, ...args);
  }

}
