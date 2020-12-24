import { Injectable, NgZone } from '@angular/core';

enum DToServerEvent {
    FromBrowserEvent = 'c64',
}

declare const mp: {
    events: {
        add(eventName: string, method: () => void): void;
        remove(eventName: string, method?: () => void): void;
    };

    trigger(eventName: string, ...args: any): void;
};
declare const window: any;

@Injectable({
    providedIn: 'root',
})
export class RageConnectorService {
    private static zone: NgZone = null;
    private static events: { [key: string]: ((...args: any) => void)[] } = {};
    private static callbackEvents: { [key: string]: ((...args: any) => void)[] } = {};
    private static callbacksWrapper = new Map<(...args: any) => void, (...args: any) => void>();

    constructor(zone: NgZone) {
        RageConnectorService.zone = zone;
        window.RageAngularEvent = this.rageEventHandler.bind(this);
    }

    public rageEventHandler(eventName: string, ...args: any) {
        RageConnectorService.zone.run(() => {
            if (eventName == DToServerEvent.FromBrowserEvent) {
                eventName = args[0];
                args.shift();
            }
            if (RageConnectorService.events[eventName]) {
                for (const func of RageConnectorService.events[eventName]) {
                    func(...args);
                }
            }
            if (RageConnectorService.callbackEvents[eventName]) {
                const callbackFunctions = RageConnectorService.callbackEvents[eventName];
                RageConnectorService.callbackEvents[eventName] = undefined;
                for (const func of callbackFunctions) {
                    RageConnectorService.callbacksWrapper.delete(func);
                    mp.events.remove(eventName, func);
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
        if (typeof mp == 'undefined')
            // testing without RAGE
            return;

        if (!RageConnectorService.events[eventName]) {
            RageConnectorService.events[eventName] = [];
        }
        RageConnectorService.events[eventName].push(callback);

        const wrapperCallback = (...args: any) => RageConnectorService.zone.run(() => callback(...args));
        RageConnectorService.callbacksWrapper.set(callback, wrapperCallback);
        mp.events.add(eventName, wrapperCallback);
    }

    public remove(eventName: string, callback?: (...args: any) => void) {
        if (typeof mp == 'undefined')
            // testing without RAGE
            return;

        if (!RageConnectorService.events[eventName]) {
            return;
        }

        if (callback) {
            for (let i = RageConnectorService.events[eventName].length - 1; i >= 0; --i) {
                if (RageConnectorService.events[eventName][i] == callback) {
                    RageConnectorService.events[eventName].splice(i, 1);
                }
            }
            const wrapper = RageConnectorService.callbacksWrapper.get(callback);
            if (wrapper) {
                mp.events.remove(eventName, wrapper);
                RageConnectorService.callbacksWrapper.delete(callback);
            }
            mp.events.remove(eventName, callback);
        } else {
            if (RageConnectorService.events[eventName]) {
                for (const func of RageConnectorService.events[eventName]) {
                    RageConnectorService.callbacksWrapper.delete(func);
                }
                RageConnectorService.events[eventName] = undefined;
            }

            mp.events.remove(eventName);
        }
    }

    public call(eventName: string, ...args: any) {
        if (typeof mp == 'undefined')
            // testing without RAGE
            return;

        mp.trigger(eventName, ...args);
    }

    public callServer(eventName: string, ...args: any) {
        if (typeof mp == 'undefined')
            // testing without RAGE
            return;

        mp.trigger(DToServerEvent.FromBrowserEvent, eventName, ...args);
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
        if (typeof mp == 'undefined')
            // testing without RAGE
            return;
        if (!RageConnectorService.callbackEvents[eventName]) {
            RageConnectorService.callbackEvents[eventName] = [];
        }
        RageConnectorService.callbackEvents[eventName].push(callback);

        this.addCallbackFunction(eventName, callback);

        if (args) mp.trigger(eventName, ...args);
        else mp.trigger(eventName);
    }

    /**
     * Calls an event and adds an "event listener" for a response from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param eventName Name of the event used in RAGE
     * @param args Any arguments
     * @param callback Any function
     */
    public callCallbackServer(eventName: string, args: any[] | undefined, callback: (...args: any) => void) {
        if (typeof mp == 'undefined')
            // testing without RAGE
            return;
        if (!RageConnectorService.callbackEvents[eventName]) {
            RageConnectorService.callbackEvents[eventName] = [];
        }
        RageConnectorService.callbackEvents[eventName].push(callback);

        this.addCallbackFunction(eventName, callback);

        if (args) mp.trigger(DToServerEvent.FromBrowserEvent, eventName, ...args);
        else mp.trigger(DToServerEvent.FromBrowserEvent, eventName);
    }

    private addCallbackFunction(eventName: string, callback: (...args: any) => void) {
        const callbackFunc = function func(...args: any) {
            mp.events.remove(eventName, func);
            RageConnectorService.zone.run(() => {
                callback(...args);
            });
        };
        mp.events.add(eventName, callbackFunc);
    }
}
