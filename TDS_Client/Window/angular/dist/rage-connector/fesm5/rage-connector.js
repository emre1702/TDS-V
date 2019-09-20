import { __values, __spread } from 'tslib';
import { Injectable, NgZone, ɵɵdefineInjectable, ɵɵinject, Component, NgModule } from '@angular/core';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var RageConnectorService = /** @class */ (function () {
    function RageConnectorService(zone) {
        RageConnectorService.zone = zone;
        window.RageAngularEvent = this.rageEventHandler;
    }
    /**
     * @param {?} eventName
     * @param {...?} args
     * @return {?}
     */
    RageConnectorService.prototype.rageEventHandler = /**
     * @param {?} eventName
     * @param {...?} args
     * @return {?}
     */
    function (eventName) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        RageConnectorService.zone.run((/**
         * @return {?}
         */
        function () {
            var e_1, _a, e_2, _b;
            if (RageConnectorService.events[eventName]) {
                try {
                    for (var _c = __values(RageConnectorService.events[eventName]), _d = _c.next(); !_d.done; _d = _c.next()) {
                        var func = _d.value;
                        func.apply(void 0, __spread(args));
                    }
                }
                catch (e_1_1) { e_1 = { error: e_1_1 }; }
                finally {
                    try {
                        if (_d && !_d.done && (_a = _c.return)) _a.call(_c);
                    }
                    finally { if (e_1) throw e_1.error; }
                }
            }
            if (RageConnectorService.callbackEvents[eventName]) {
                /** @type {?} */
                var callbackFunctions = RageConnectorService.callbackEvents[eventName];
                RageConnectorService.callbackEvents[eventName] = undefined;
                try {
                    for (var callbackFunctions_1 = __values(callbackFunctions), callbackFunctions_1_1 = callbackFunctions_1.next(); !callbackFunctions_1_1.done; callbackFunctions_1_1 = callbackFunctions_1.next()) {
                        var func = callbackFunctions_1_1.value;
                        func.apply(void 0, __spread(args));
                    }
                }
                catch (e_2_1) { e_2 = { error: e_2_1 }; }
                finally {
                    try {
                        if (callbackFunctions_1_1 && !callbackFunctions_1_1.done && (_b = callbackFunctions_1.return)) _b.call(callbackFunctions_1);
                    }
                    finally { if (e_2) throw e_2.error; }
                }
            }
        }));
    };
    /**
     * Adds an "event listener" for "events" from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param eventName Name of the event used in RAGE
     * @param callback Any function
     */
    /**
     * Adds an "event listener" for "events" from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param {?} eventName Name of the event used in RAGE
     * @param {?} callback Any function
     * @return {?}
     */
    RageConnectorService.prototype.listen = /**
     * Adds an "event listener" for "events" from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param {?} eventName Name of the event used in RAGE
     * @param {?} callback Any function
     * @return {?}
     */
    function (eventName, callback) {
        if (!RageConnectorService.events[eventName]) {
            RageConnectorService.events[eventName] = [];
        }
        RageConnectorService.events[eventName].push(callback);
    };
    /**
     * @param {?} eventName
     * @param {...?} args
     * @return {?}
     */
    RageConnectorService.prototype.call = /**
     * @param {?} eventName
     * @param {...?} args
     * @return {?}
     */
    function (eventName) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        if (typeof mp == "undefined") // testing without RAGE
            return;
        mp.trigger.apply(mp, __spread([eventName], args));
    };
    /**
     * Calls an event and adds an "event listener" for a response from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param eventName Name of the event used in RAGE
     * @param args Any arguments
     * @param callback Any function
     */
    /**
     * Calls an event and adds an "event listener" for a response from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param {?} eventName Name of the event used in RAGE
     * @param {?} args Any arguments
     * @param {?} callback Any function
     * @return {?}
     */
    RageConnectorService.prototype.callCallback = /**
     * Calls an event and adds an "event listener" for a response from RAGE clientside.
     * Don't forget to use lambda for callback or add bind(this),
     * else "this" in the function will refer to rage-connector.service.ts instead.
     * @param {?} eventName Name of the event used in RAGE
     * @param {?} args Any arguments
     * @param {?} callback Any function
     * @return {?}
     */
    function (eventName, args, callback) {
        if (typeof mp == "undefined") // testing without RAGE
            return;
        if (!RageConnectorService.callbackEvents[eventName]) {
            RageConnectorService.callbackEvents[eventName] = [];
        }
        RageConnectorService.callbackEvents[eventName].push(callback);
        if (args)
            mp.trigger.apply(mp, __spread([eventName], args));
        else
            mp.trigger(eventName);
    };
    RageConnectorService.zone = null;
    RageConnectorService.events = {};
    RageConnectorService.callbackEvents = {};
    RageConnectorService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root'
                },] }
    ];
    /** @nocollapse */
    RageConnectorService.ctorParameters = function () { return [
        { type: NgZone }
    ]; };
    /** @nocollapse */ RageConnectorService.ngInjectableDef = ɵɵdefineInjectable({ factory: function RageConnectorService_Factory() { return new RageConnectorService(ɵɵinject(NgZone)); }, token: RageConnectorService, providedIn: "root" });
    return RageConnectorService;
}());
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
var RageConnectorComponent = /** @class */ (function () {
    function RageConnectorComponent() {
    }
    /**
     * @return {?}
     */
    RageConnectorComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
    };
    RageConnectorComponent.decorators = [
        { type: Component, args: [{
                    selector: 'lib-rage-connector',
                    template: "\n    <p>\n      rage-connector works!\n    </p>\n  "
                }] }
    ];
    /** @nocollapse */
    RageConnectorComponent.ctorParameters = function () { return []; };
    return RageConnectorComponent;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var RageConnectorModule = /** @class */ (function () {
    function RageConnectorModule() {
    }
    RageConnectorModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [RageConnectorComponent],
                    imports: [],
                    exports: [RageConnectorComponent]
                },] }
    ];
    return RageConnectorModule;
}());

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
