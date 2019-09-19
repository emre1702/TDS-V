/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable, NgZone } from '@angular/core';
import * as i0 from "@angular/core";
var RageConnectorService = /** @class */ (function () {
    function RageConnectorService(zone) {
        this.zone = zone;
        this.events = {};
        this.callbackEvents = {};
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
        var _this = this;
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        this.zone.run((/**
         * @return {?}
         */
        function () {
            var e_1, _a, e_2, _b;
            if (_this.events[eventName]) {
                try {
                    for (var _c = tslib_1.__values(_this.events[eventName]), _d = _c.next(); !_d.done; _d = _c.next()) {
                        var func = _d.value;
                        func.apply(void 0, tslib_1.__spread(args));
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
            if (_this.callbackEvents[eventName]) {
                /** @type {?} */
                var callbackFunctions = _this.callbackEvents[eventName];
                _this.callbackEvents[eventName] = undefined;
                try {
                    for (var callbackFunctions_1 = tslib_1.__values(callbackFunctions), callbackFunctions_1_1 = callbackFunctions_1.next(); !callbackFunctions_1_1.done; callbackFunctions_1_1 = callbackFunctions_1.next()) {
                        var func = callbackFunctions_1_1.value;
                        func.apply(void 0, tslib_1.__spread(args));
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
        if (!this.events[eventName]) {
            this.events[eventName] = [];
        }
        this.events[eventName].push(callback);
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
        mp.trigger.apply(mp, tslib_1.__spread([eventName], args));
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
        if (!this.callbackEvents[eventName]) {
            this.callbackEvents[eventName] = [];
        }
        this.callbackEvents[eventName].push(callback);
        if (args)
            mp.trigger.apply(mp, tslib_1.__spread([eventName], args));
        else
            mp.trigger(eventName);
    };
    RageConnectorService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root'
                },] }
    ];
    /** @nocollapse */
    RageConnectorService.ctorParameters = function () { return [
        { type: NgZone }
    ]; };
    /** @nocollapse */ RageConnectorService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function RageConnectorService_Factory() { return new RageConnectorService(i0.ɵɵinject(i0.NgZone)); }, token: RageConnectorService, providedIn: "root" });
    return RageConnectorService;
}());
export { RageConnectorService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    RageConnectorService.prototype.events;
    /**
     * @type {?}
     * @private
     */
    RageConnectorService.prototype.callbackEvents;
    /**
     * @type {?}
     * @private
     */
    RageConnectorService.prototype.zone;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmFnZS1jb25uZWN0b3Iuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL3JhZ2UtY29ubmVjdG9yLyIsInNvdXJjZXMiOlsibGliL3JhZ2UtY29ubmVjdG9yLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSxNQUFNLGVBQWUsQ0FBQzs7QUFPbkQ7SUFRRSw4QkFBb0IsSUFBWTtRQUFaLFNBQUksR0FBSixJQUFJLENBQVE7UUFIeEIsV0FBTSxHQUFnRCxFQUFFLENBQUM7UUFDekQsbUJBQWMsR0FBZ0QsRUFBRSxDQUFDO1FBR3ZFLE1BQU0sQ0FBQyxnQkFBZ0IsR0FBRyxJQUFJLENBQUMsZ0JBQWdCLENBQUM7SUFDbEQsQ0FBQzs7Ozs7O0lBRU0sK0NBQWdCOzs7OztJQUF2QixVQUF3QixTQUFpQjtRQUF6QyxpQkFlQztRQWYwQyxjQUFZO2FBQVosVUFBWSxFQUFaLHFCQUFZLEVBQVosSUFBWTtZQUFaLDZCQUFZOztRQUNyRCxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUc7OztRQUFDOztZQUNaLElBQUksS0FBSSxDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsRUFBRTs7b0JBQzFCLEtBQW1CLElBQUEsS0FBQSxpQkFBQSxLQUFJLENBQUMsTUFBTSxDQUFDLFNBQVMsQ0FBQyxDQUFBLGdCQUFBLDRCQUFFO3dCQUF0QyxJQUFNLElBQUksV0FBQTt3QkFDYixJQUFJLGdDQUFJLElBQUksR0FBRTtxQkFDZjs7Ozs7Ozs7O2FBQ0Y7WUFDRCxJQUFJLEtBQUksQ0FBQyxjQUFjLENBQUMsU0FBUyxDQUFDLEVBQUU7O29CQUM1QixpQkFBaUIsR0FBRyxLQUFJLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQztnQkFDeEQsS0FBSSxDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUMsR0FBRyxTQUFTLENBQUM7O29CQUMzQyxLQUFtQixJQUFBLHNCQUFBLGlCQUFBLGlCQUFpQixDQUFBLG9EQUFBLG1GQUFFO3dCQUFqQyxJQUFNLElBQUksOEJBQUE7d0JBQ2IsSUFBSSxnQ0FBSSxJQUFJLEdBQUU7cUJBQ2Y7Ozs7Ozs7OzthQUNGO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDO0lBRUQ7Ozs7OztPQU1HOzs7Ozs7Ozs7SUFDSSxxQ0FBTTs7Ozs7Ozs7SUFBYixVQUFjLFNBQWlCLEVBQUUsUUFBZ0M7UUFDL0QsSUFBSSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsU0FBUyxDQUFDLEVBQUU7WUFDM0IsSUFBSSxDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsR0FBRyxFQUFFLENBQUM7U0FDN0I7UUFDRCxJQUFJLENBQUMsTUFBTSxDQUFDLFNBQVMsQ0FBQyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQztJQUN4QyxDQUFDOzs7Ozs7SUFFTSxtQ0FBSTs7Ozs7SUFBWCxVQUFZLFNBQWlCO1FBQUUsY0FBWTthQUFaLFVBQVksRUFBWixxQkFBWSxFQUFaLElBQVk7WUFBWiw2QkFBWTs7UUFDekMsSUFBSSxPQUFPLEVBQUUsSUFBSSxXQUFXLEVBQUUsdUJBQXVCO1lBQ25ELE9BQU87UUFDVCxFQUFFLENBQUMsT0FBTyxPQUFWLEVBQUUsb0JBQVMsU0FBUyxHQUFLLElBQUksR0FBRTtJQUNqQyxDQUFDO0lBRUQ7Ozs7Ozs7T0FPRzs7Ozs7Ozs7OztJQUNJLDJDQUFZOzs7Ozs7Ozs7SUFBbkIsVUFBb0IsU0FBaUIsRUFBRSxJQUF1QixFQUFFLFFBQWdDO1FBQzlGLElBQUksT0FBTyxFQUFFLElBQUksV0FBVyxFQUFFLHVCQUF1QjtZQUNuRCxPQUFPO1FBQ1QsSUFBSSxDQUFDLElBQUksQ0FBQyxjQUFjLENBQUMsU0FBUyxDQUFDLEVBQUU7WUFDbkMsSUFBSSxDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUMsR0FBRyxFQUFFLENBQUM7U0FDckM7UUFDRCxJQUFJLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQztRQUM5QyxJQUFJLElBQUk7WUFDTixFQUFFLENBQUMsT0FBTyxPQUFWLEVBQUUsb0JBQVMsU0FBUyxHQUFLLElBQUksR0FBRTs7WUFFL0IsRUFBRSxDQUFDLE9BQU8sQ0FBQyxTQUFTLENBQUMsQ0FBQztJQUMxQixDQUFDOztnQkFwRUYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFUb0IsTUFBTTs7OytCQUEzQjtDQTRFQyxBQXJFRCxJQXFFQztTQWxFWSxvQkFBb0I7Ozs7OztJQUUvQixzQ0FBaUU7Ozs7O0lBQ2pFLDhDQUF5RTs7Ozs7SUFFN0Qsb0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgTmdab25lIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbmRlY2xhcmUgY29uc3QgbXA6IHtcbiAgdHJpZ2dlcihldmVudE5hbWU6IHN0cmluZywgLi4uYXJnczogYW55KTogdm9pZDtcbn07XG5kZWNsYXJlIGNvbnN0IHdpbmRvdzogYW55O1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290J1xufSlcbmV4cG9ydCBjbGFzcyBSYWdlQ29ubmVjdG9yU2VydmljZSB7XG5cbiAgcHJpdmF0ZSBldmVudHM6IHtba2V5OiBzdHJpbmddOiAoKC4uLmFyZ3M6IGFueSkgPT4gdm9pZClbXX0gPSB7fTtcbiAgcHJpdmF0ZSBjYWxsYmFja0V2ZW50czoge1trZXk6IHN0cmluZ106ICgoLi4uYXJnczogYW55KSA9PiB2b2lkKVtdfSA9IHt9O1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgem9uZTogTmdab25lKSB7XG4gICAgd2luZG93LlJhZ2VBbmd1bGFyRXZlbnQgPSB0aGlzLnJhZ2VFdmVudEhhbmRsZXI7XG4gIH1cblxuICBwdWJsaWMgcmFnZUV2ZW50SGFuZGxlcihldmVudE5hbWU6IHN0cmluZywgLi4uYXJnczogYW55KSB7XG4gICAgdGhpcy56b25lLnJ1bigoKSA9PiB7XG4gICAgICBpZiAodGhpcy5ldmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgICBmb3IgKGNvbnN0IGZ1bmMgb2YgdGhpcy5ldmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgICAgIGZ1bmMoLi4uYXJncyk7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIGlmICh0aGlzLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV0pIHtcbiAgICAgICAgY29uc3QgY2FsbGJhY2tGdW5jdGlvbnMgPSB0aGlzLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV07XG4gICAgICAgIHRoaXMuY2FsbGJhY2tFdmVudHNbZXZlbnROYW1lXSA9IHVuZGVmaW5lZDtcbiAgICAgICAgZm9yIChjb25zdCBmdW5jIG9mIGNhbGxiYWNrRnVuY3Rpb25zKSB7XG4gICAgICAgICAgZnVuYyguLi5hcmdzKTtcbiAgICAgICAgfVxuICAgICAgfVxuICAgIH0pO1xuICB9XG5cbiAgLyoqXG4gICAqIEFkZHMgYW4gXCJldmVudCBsaXN0ZW5lclwiIGZvciBcImV2ZW50c1wiIGZyb20gUkFHRSBjbGllbnRzaWRlLlxuICAgKiBEb24ndCBmb3JnZXQgdG8gdXNlIGxhbWJkYSBmb3IgY2FsbGJhY2sgb3IgYWRkIGJpbmQodGhpcyksXG4gICAqIGVsc2UgXCJ0aGlzXCIgaW4gdGhlIGZ1bmN0aW9uIHdpbGwgcmVmZXIgdG8gcmFnZS1jb25uZWN0b3Iuc2VydmljZS50cyBpbnN0ZWFkLlxuICAgKiBAcGFyYW0gZXZlbnROYW1lIE5hbWUgb2YgdGhlIGV2ZW50IHVzZWQgaW4gUkFHRVxuICAgKiBAcGFyYW0gY2FsbGJhY2sgQW55IGZ1bmN0aW9uXG4gICAqL1xuICBwdWJsaWMgbGlzdGVuKGV2ZW50TmFtZTogc3RyaW5nLCBjYWxsYmFjazogKC4uLmFyZ3M6IGFueSkgPT4gdm9pZCkge1xuICAgIGlmICghdGhpcy5ldmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgdGhpcy5ldmVudHNbZXZlbnROYW1lXSA9IFtdO1xuICAgIH1cbiAgICB0aGlzLmV2ZW50c1tldmVudE5hbWVdLnB1c2goY2FsbGJhY2spO1xuICB9XG5cbiAgcHVibGljIGNhbGwoZXZlbnROYW1lOiBzdHJpbmcsIC4uLmFyZ3M6IGFueSkge1xuICAgIGlmICh0eXBlb2YgbXAgPT0gXCJ1bmRlZmluZWRcIikgLy8gdGVzdGluZyB3aXRob3V0IFJBR0VcbiAgICAgIHJldHVybjtcbiAgICBtcC50cmlnZ2VyKGV2ZW50TmFtZSwgLi4uYXJncyk7XG4gIH1cblxuICAvKipcbiAgICogQ2FsbHMgYW4gZXZlbnQgYW5kIGFkZHMgYW4gXCJldmVudCBsaXN0ZW5lclwiIGZvciBhIHJlc3BvbnNlIGZyb20gUkFHRSBjbGllbnRzaWRlLlxuICAgKiBEb24ndCBmb3JnZXQgdG8gdXNlIGxhbWJkYSBmb3IgY2FsbGJhY2sgb3IgYWRkIGJpbmQodGhpcyksXG4gICAqIGVsc2UgXCJ0aGlzXCIgaW4gdGhlIGZ1bmN0aW9uIHdpbGwgcmVmZXIgdG8gcmFnZS1jb25uZWN0b3Iuc2VydmljZS50cyBpbnN0ZWFkLlxuICAgKiBAcGFyYW0gZXZlbnROYW1lIE5hbWUgb2YgdGhlIGV2ZW50IHVzZWQgaW4gUkFHRVxuICAgKiBAcGFyYW0gYXJncyBBbnkgYXJndW1lbnRzXG4gICAqIEBwYXJhbSBjYWxsYmFjayBBbnkgZnVuY3Rpb25cbiAgICovXG4gIHB1YmxpYyBjYWxsQ2FsbGJhY2soZXZlbnROYW1lOiBzdHJpbmcsIGFyZ3M6IGFueVtdIHwgdW5kZWZpbmVkLCBjYWxsYmFjazogKC4uLmFyZ3M6IGFueSkgPT4gdm9pZCkge1xuICAgIGlmICh0eXBlb2YgbXAgPT0gXCJ1bmRlZmluZWRcIikgLy8gdGVzdGluZyB3aXRob3V0IFJBR0VcbiAgICAgIHJldHVybjtcbiAgICBpZiAoIXRoaXMuY2FsbGJhY2tFdmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgdGhpcy5jYWxsYmFja0V2ZW50c1tldmVudE5hbWVdID0gW107XG4gICAgfVxuICAgIHRoaXMuY2FsbGJhY2tFdmVudHNbZXZlbnROYW1lXS5wdXNoKGNhbGxiYWNrKTtcbiAgICBpZiAoYXJncylcbiAgICAgIG1wLnRyaWdnZXIoZXZlbnROYW1lLCAuLi5hcmdzKTtcbiAgICBlbHNlXG4gICAgICBtcC50cmlnZ2VyKGV2ZW50TmFtZSk7XG4gIH1cbn1cbiJdfQ==