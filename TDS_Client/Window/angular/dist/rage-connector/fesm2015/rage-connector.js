import { Injectable, NgZone, ɵɵdefineInjectable, ɵɵinject, Component, NgModule } from '@angular/core';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class RageConnectorService {
    /**
     * @param {?} zone
     */
    constructor(zone) {
        RageConnectorService.zone = zone;
        window.RageAngularEvent = this.rageEventHandler;
    }
    /**
     * @param {?} eventName
     * @param {...?} args
     * @return {?}
     */
    rageEventHandler(eventName, ...args) {
        RageConnectorService.zone.run((/**
         * @return {?}
         */
        () => {
            if (RageConnectorService.events[eventName]) {
                for (const func of RageConnectorService.events[eventName]) {
                    func(...args);
                }
            }
            if (RageConnectorService.callbackEvents[eventName]) {
                /** @type {?} */
                const callbackFunctions = RageConnectorService.callbackEvents[eventName];
                RageConnectorService.callbackEvents[eventName] = undefined;
                for (const func of callbackFunctions) {
                    func(...args);
                }
            }
        }));
    }
    /**
     * Adds an "event listener" for "events" from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param {?} eventName Name of the event used in RAGE
     * @param {?} callback Any function
     * @return {?}
     */
    listen(eventName, callback) {
        if (!RageConnectorService.events[eventName]) {
            RageConnectorService.events[eventName] = [];
        }
        RageConnectorService.events[eventName].push(callback);
    }
    /**
     * @param {?} eventName
     * @param {...?} args
     * @return {?}
     */
    call(eventName, ...args) {
        if (typeof mp == "undefined") // testing without RAGE
            return;
        mp.trigger(eventName, ...args);
    }
    /**
     * Calls an event and adds an "event listener" for a response from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param {?} eventName Name of the event used in RAGE
     * @param {?} args Any arguments
     * @param {?} callback Any function
     * @return {?}
     */
    callCallback(eventName, args, callback) {
        if (typeof mp == "undefined") // testing without RAGE
            return;
        if (!RageConnectorService.callbackEvents[eventName]) {
            RageConnectorService.callbackEvents[eventName] = [];
        }
        RageConnectorService.callbackEvents[eventName].push(callback);
        if (args)
            mp.trigger(eventName, ...args);
        else
            mp.trigger(eventName);
    }
}
RageConnectorService.zone = null;
RageConnectorService.events = {};
RageConnectorService.callbackEvents = {};
RageConnectorService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root'
            },] }
];
/** @nocollapse */
RageConnectorService.ctorParameters = () => [
    { type: NgZone }
];
/** @nocollapse */ RageConnectorService.ngInjectableDef = ɵɵdefineInjectable({ factory: function RageConnectorService_Factory() { return new RageConnectorService(ɵɵinject(NgZone)); }, token: RageConnectorService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    RageConnectorService.zone;
    /**
     * @type {?}
     * @private
     */
    RageConnectorService.events;
    /**
     * @type {?}
     * @private
     */
    RageConnectorService.callbackEvents;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class RageConnectorComponent {
    constructor() { }
    /**
     * @return {?}
     */
    ngOnInit() {
    }
}
RageConnectorComponent.decorators = [
    { type: Component, args: [{
                selector: 'lib-rage-connector',
                template: `
    <p>
      rage-connector works!
    </p>
  `
            }] }
];
/** @nocollapse */
RageConnectorComponent.ctorParameters = () => [];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class RageConnectorModule {
}
RageConnectorModule.decorators = [
    { type: NgModule, args: [{
                declarations: [RageConnectorComponent],
                imports: [],
                exports: [RageConnectorComponent]
            },] }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { RageConnectorComponent, RageConnectorModule, RageConnectorService };
//# sourceMappingURL=rage-connector.js.map
