import { Injectable, ChangeDetectorRef, NgZone } from '@angular/core';

declare const mp: {
  trigger(eventName: string, ...args: any): void;
};
declare const window: any;

@Injectable({
  providedIn: 'root'
})
export class RageConnectorService {
  private static events: {[key: string]: ((...args: any) => void)[]} = {};
  private static callbackEvents: {[key: string]: ((...args: any) => void)[]} = {};
  private static zone: NgZone;

  constructor(zone: NgZone) {
    RageConnectorService.zone = zone;
    window.RageAngularEvent = RageConnectorService.rageEventHandler;
  }

  public static rageEventHandler(eventName: string, ...args: any) {
    RageConnectorService.zone.run(() => {
      if (RageConnectorService.events[eventName]) {
        for (const func of RageConnectorService.events[eventName]) {
          func(...args);
        }
      }
      if (RageConnectorService.callbackEvents[eventName]) {
        const callbackFunctions = RageConnectorService.callbackEvents[eventName];
        RageConnectorService.callbackEvents[eventName] = undefined;
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
    if (!RageConnectorService.events[eventName]) {
      RageConnectorService.events[eventName] = [];
    }
    RageConnectorService.events[eventName].push(callback);
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
    if (typeof mp == "undefined") // testing without RAGE
      return;
    if (!RageConnectorService.callbackEvents[eventName]) {
      RageConnectorService.callbackEvents[eventName] = [];
    }
    RageConnectorService.callbackEvents[eventName].push(callback);
    mp.trigger(eventName, ...args);
  }

}
