/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable, NgZone } from '@angular/core';
import * as i0 from "@angular/core";
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
                    for (var _c = tslib_1.__values(RageConnectorService.events[eventName]), _d = _c.next(); !_d.done; _d = _c.next()) {
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
            if (RageConnectorService.callbackEvents[eventName]) {
                /** @type {?} */
                var callbackFunctions = RageConnectorService.callbackEvents[eventName];
                RageConnectorService.callbackEvents[eventName] = undefined;
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
        if (!RageConnectorService.callbackEvents[eventName]) {
            RageConnectorService.callbackEvents[eventName] = [];
        }
        RageConnectorService.callbackEvents[eventName].push(callback);
        if (args)
            mp.trigger.apply(mp, tslib_1.__spread([eventName], args));
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
    /** @nocollapse */ RageConnectorService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function RageConnectorService_Factory() { return new RageConnectorService(i0.ɵɵinject(i0.NgZone)); }, token: RageConnectorService, providedIn: "root" });
    return RageConnectorService;
}());
export { RageConnectorService };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmFnZS1jb25uZWN0b3Iuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL3JhZ2UtY29ubmVjdG9yLyIsInNvdXJjZXMiOlsibGliL3JhZ2UtY29ubmVjdG9yLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSxNQUFNLGVBQWUsQ0FBQzs7QUFPbkQ7SUFTRSw4QkFBWSxJQUFZO1FBQ3RCLG9CQUFvQixDQUFDLElBQUksR0FBRyxJQUFJLENBQUM7UUFDakMsTUFBTSxDQUFDLGdCQUFnQixHQUFHLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQztJQUNsRCxDQUFDOzs7Ozs7SUFFTSwrQ0FBZ0I7Ozs7O0lBQXZCLFVBQXdCLFNBQWlCO1FBQUUsY0FBWTthQUFaLFVBQVksRUFBWixxQkFBWSxFQUFaLElBQVk7WUFBWiw2QkFBWTs7UUFDckQsb0JBQW9CLENBQUMsSUFBSSxDQUFDLEdBQUc7OztRQUFDOztZQUM1QixJQUFJLG9CQUFvQixDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsRUFBRTs7b0JBQzFDLEtBQW1CLElBQUEsS0FBQSxpQkFBQSxvQkFBb0IsQ0FBQyxNQUFNLENBQUMsU0FBUyxDQUFDLENBQUEsZ0JBQUEsNEJBQUU7d0JBQXRELElBQU0sSUFBSSxXQUFBO3dCQUNiLElBQUksZ0NBQUksSUFBSSxHQUFFO3FCQUNmOzs7Ozs7Ozs7YUFDRjtZQUNELElBQUksb0JBQW9CLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQyxFQUFFOztvQkFDNUMsaUJBQWlCLEdBQUcsb0JBQW9CLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQztnQkFDeEUsb0JBQW9CLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQyxHQUFHLFNBQVMsQ0FBQzs7b0JBQzNELEtBQW1CLElBQUEsc0JBQUEsaUJBQUEsaUJBQWlCLENBQUEsb0RBQUEsbUZBQUU7d0JBQWpDLElBQU0sSUFBSSw4QkFBQTt3QkFDYixJQUFJLGdDQUFJLElBQUksR0FBRTtxQkFDZjs7Ozs7Ozs7O2FBQ0Y7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7SUFFRDs7Ozs7O09BTUc7Ozs7Ozs7OztJQUNJLHFDQUFNOzs7Ozs7OztJQUFiLFVBQWMsU0FBaUIsRUFBRSxRQUFnQztRQUMvRCxJQUFJLENBQUMsb0JBQW9CLENBQUMsTUFBTSxDQUFDLFNBQVMsQ0FBQyxFQUFFO1lBQzNDLG9CQUFvQixDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsR0FBRyxFQUFFLENBQUM7U0FDN0M7UUFDRCxvQkFBb0IsQ0FBQyxNQUFNLENBQUMsU0FBUyxDQUFDLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO0lBQ3hELENBQUM7Ozs7OztJQUVNLG1DQUFJOzs7OztJQUFYLFVBQVksU0FBaUI7UUFBRSxjQUFZO2FBQVosVUFBWSxFQUFaLHFCQUFZLEVBQVosSUFBWTtZQUFaLDZCQUFZOztRQUN6QyxJQUFJLE9BQU8sRUFBRSxJQUFJLFdBQVcsRUFBRSx1QkFBdUI7WUFDbkQsT0FBTztRQUNULEVBQUUsQ0FBQyxPQUFPLE9BQVYsRUFBRSxvQkFBUyxTQUFTLEdBQUssSUFBSSxHQUFFO0lBQ2pDLENBQUM7SUFFRDs7Ozs7OztPQU9HOzs7Ozs7Ozs7O0lBQ0ksMkNBQVk7Ozs7Ozs7OztJQUFuQixVQUFvQixTQUFpQixFQUFFLElBQXVCLEVBQUUsUUFBZ0M7UUFDOUYsSUFBSSxPQUFPLEVBQUUsSUFBSSxXQUFXLEVBQUUsdUJBQXVCO1lBQ25ELE9BQU87UUFDVCxJQUFJLENBQUMsb0JBQW9CLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQyxFQUFFO1lBQ25ELG9CQUFvQixDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUMsR0FBRyxFQUFFLENBQUM7U0FDckQ7UUFDRCxvQkFBb0IsQ0FBQyxjQUFjLENBQUMsU0FBUyxDQUFDLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO1FBQzlELElBQUksSUFBSTtZQUNOLEVBQUUsQ0FBQyxPQUFPLE9BQVYsRUFBRSxvQkFBUyxTQUFTLEdBQUssSUFBSSxHQUFFOztZQUUvQixFQUFFLENBQUMsT0FBTyxDQUFDLFNBQVMsQ0FBQyxDQUFDO0lBQzFCLENBQUM7SUFqRWMseUJBQUksR0FBVyxJQUFJLENBQUM7SUFDcEIsMkJBQU0sR0FBZ0QsRUFBRSxDQUFDO0lBQ3pELG1DQUFjLEdBQWdELEVBQUUsQ0FBQzs7Z0JBUGpGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBVG9CLE1BQU07OzsrQkFBM0I7Q0E4RUMsQUF2RUQsSUF1RUM7U0FwRVksb0JBQW9COzs7Ozs7SUFFL0IsMEJBQW1DOzs7OztJQUNuQyw0QkFBd0U7Ozs7O0lBQ3hFLG9DQUFnRiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIE5nWm9uZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5kZWNsYXJlIGNvbnN0IG1wOiB7XG4gIHRyaWdnZXIoZXZlbnROYW1lOiBzdHJpbmcsIC4uLmFyZ3M6IGFueSk6IHZvaWQ7XG59O1xuZGVjbGFyZSBjb25zdCB3aW5kb3c6IGFueTtcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCdcbn0pXG5leHBvcnQgY2xhc3MgUmFnZUNvbm5lY3RvclNlcnZpY2Uge1xuXG4gIHByaXZhdGUgc3RhdGljIHpvbmU6IE5nWm9uZSA9IG51bGw7XG4gIHByaXZhdGUgc3RhdGljIGV2ZW50czoge1trZXk6IHN0cmluZ106ICgoLi4uYXJnczogYW55KSA9PiB2b2lkKVtdfSA9IHt9O1xuICBwcml2YXRlIHN0YXRpYyBjYWxsYmFja0V2ZW50czoge1trZXk6IHN0cmluZ106ICgoLi4uYXJnczogYW55KSA9PiB2b2lkKVtdfSA9IHt9O1xuXG4gIGNvbnN0cnVjdG9yKHpvbmU6IE5nWm9uZSkge1xuICAgIFJhZ2VDb25uZWN0b3JTZXJ2aWNlLnpvbmUgPSB6b25lO1xuICAgIHdpbmRvdy5SYWdlQW5ndWxhckV2ZW50ID0gdGhpcy5yYWdlRXZlbnRIYW5kbGVyO1xuICB9XG5cbiAgcHVibGljIHJhZ2VFdmVudEhhbmRsZXIoZXZlbnROYW1lOiBzdHJpbmcsIC4uLmFyZ3M6IGFueSkge1xuICAgIFJhZ2VDb25uZWN0b3JTZXJ2aWNlLnpvbmUucnVuKCgpID0+IHtcbiAgICAgIGlmIChSYWdlQ29ubmVjdG9yU2VydmljZS5ldmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgICBmb3IgKGNvbnN0IGZ1bmMgb2YgUmFnZUNvbm5lY3RvclNlcnZpY2UuZXZlbnRzW2V2ZW50TmFtZV0pIHtcbiAgICAgICAgICBmdW5jKC4uLmFyZ3MpO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgICBpZiAoUmFnZUNvbm5lY3RvclNlcnZpY2UuY2FsbGJhY2tFdmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgICBjb25zdCBjYWxsYmFja0Z1bmN0aW9ucyA9IFJhZ2VDb25uZWN0b3JTZXJ2aWNlLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV07XG4gICAgICAgIFJhZ2VDb25uZWN0b3JTZXJ2aWNlLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV0gPSB1bmRlZmluZWQ7XG4gICAgICAgIGZvciAoY29uc3QgZnVuYyBvZiBjYWxsYmFja0Z1bmN0aW9ucykge1xuICAgICAgICAgIGZ1bmMoLi4uYXJncyk7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICB9KTtcbiAgfVxuXG4gIC8qKlxuICAgKiBBZGRzIGFuIFwiZXZlbnQgbGlzdGVuZXJcIiBmb3IgXCJldmVudHNcIiBmcm9tIFJBR0UgY2xpZW50c2lkZS5cbiAgICogRG9uJ3QgZm9yZ2V0IHRvIHVzZSBsYW1iZGEgZm9yIGNhbGxiYWNrIG9yIGFkZCBiaW5kKHRoaXMpLFxuICAgKiBlbHNlIFwidGhpc1wiIGluIHRoZSBmdW5jdGlvbiB3aWxsIHJlZmVyIHRvIHJhZ2UtY29ubmVjdG9yLnNlcnZpY2UudHMgaW5zdGVhZC5cbiAgICogQHBhcmFtIGV2ZW50TmFtZSBOYW1lIG9mIHRoZSBldmVudCB1c2VkIGluIFJBR0VcbiAgICogQHBhcmFtIGNhbGxiYWNrIEFueSBmdW5jdGlvblxuICAgKi9cbiAgcHVibGljIGxpc3RlbihldmVudE5hbWU6IHN0cmluZywgY2FsbGJhY2s6ICguLi5hcmdzOiBhbnkpID0+IHZvaWQpIHtcbiAgICBpZiAoIVJhZ2VDb25uZWN0b3JTZXJ2aWNlLmV2ZW50c1tldmVudE5hbWVdKSB7XG4gICAgICBSYWdlQ29ubmVjdG9yU2VydmljZS5ldmVudHNbZXZlbnROYW1lXSA9IFtdO1xuICAgIH1cbiAgICBSYWdlQ29ubmVjdG9yU2VydmljZS5ldmVudHNbZXZlbnROYW1lXS5wdXNoKGNhbGxiYWNrKTtcbiAgfVxuXG4gIHB1YmxpYyBjYWxsKGV2ZW50TmFtZTogc3RyaW5nLCAuLi5hcmdzOiBhbnkpIHtcbiAgICBpZiAodHlwZW9mIG1wID09IFwidW5kZWZpbmVkXCIpIC8vIHRlc3Rpbmcgd2l0aG91dCBSQUdFXG4gICAgICByZXR1cm47XG4gICAgbXAudHJpZ2dlcihldmVudE5hbWUsIC4uLmFyZ3MpO1xuICB9XG5cbiAgLyoqXG4gICAqIENhbGxzIGFuIGV2ZW50IGFuZCBhZGRzIGFuIFwiZXZlbnQgbGlzdGVuZXJcIiBmb3IgYSByZXNwb25zZSBmcm9tIFJBR0UgY2xpZW50c2lkZS5cbiAgICogRG9uJ3QgZm9yZ2V0IHRvIHVzZSBsYW1iZGEgZm9yIGNhbGxiYWNrIG9yIGFkZCBiaW5kKHRoaXMpLFxuICAgKiBlbHNlIFwidGhpc1wiIGluIHRoZSBmdW5jdGlvbiB3aWxsIHJlZmVyIHRvIHJhZ2UtY29ubmVjdG9yLnNlcnZpY2UudHMgaW5zdGVhZC5cbiAgICogQHBhcmFtIGV2ZW50TmFtZSBOYW1lIG9mIHRoZSBldmVudCB1c2VkIGluIFJBR0VcbiAgICogQHBhcmFtIGFyZ3MgQW55IGFyZ3VtZW50c1xuICAgKiBAcGFyYW0gY2FsbGJhY2sgQW55IGZ1bmN0aW9uXG4gICAqL1xuICBwdWJsaWMgY2FsbENhbGxiYWNrKGV2ZW50TmFtZTogc3RyaW5nLCBhcmdzOiBhbnlbXSB8IHVuZGVmaW5lZCwgY2FsbGJhY2s6ICguLi5hcmdzOiBhbnkpID0+IHZvaWQpIHtcbiAgICBpZiAodHlwZW9mIG1wID09IFwidW5kZWZpbmVkXCIpIC8vIHRlc3Rpbmcgd2l0aG91dCBSQUdFXG4gICAgICByZXR1cm47XG4gICAgaWYgKCFSYWdlQ29ubmVjdG9yU2VydmljZS5jYWxsYmFja0V2ZW50c1tldmVudE5hbWVdKSB7XG4gICAgICBSYWdlQ29ubmVjdG9yU2VydmljZS5jYWxsYmFja0V2ZW50c1tldmVudE5hbWVdID0gW107XG4gICAgfVxuICAgIFJhZ2VDb25uZWN0b3JTZXJ2aWNlLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV0ucHVzaChjYWxsYmFjayk7XG4gICAgaWYgKGFyZ3MpXG4gICAgICBtcC50cmlnZ2VyKGV2ZW50TmFtZSwgLi4uYXJncyk7XG4gICAgZWxzZVxuICAgICAgbXAudHJpZ2dlcihldmVudE5hbWUpO1xuICB9XG59XG4iXX0=