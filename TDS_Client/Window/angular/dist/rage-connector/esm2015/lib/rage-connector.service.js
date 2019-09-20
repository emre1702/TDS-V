/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable, NgZone } from '@angular/core';
import * as i0 from "@angular/core";
export class RageConnectorService {
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
/** @nocollapse */ RageConnectorService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function RageConnectorService_Factory() { return new RageConnectorService(i0.ɵɵinject(i0.NgZone)); }, token: RageConnectorService, providedIn: "root" });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmFnZS1jb25uZWN0b3Iuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL3JhZ2UtY29ubmVjdG9yLyIsInNvdXJjZXMiOlsibGliL3JhZ2UtY29ubmVjdG9yLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDOztBQVVuRCxNQUFNLE9BQU8sb0JBQW9COzs7O0lBTS9CLFlBQVksSUFBWTtRQUN0QixvQkFBb0IsQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDO1FBQ2pDLE1BQU0sQ0FBQyxnQkFBZ0IsR0FBRyxJQUFJLENBQUMsZ0JBQWdCLENBQUM7SUFDbEQsQ0FBQzs7Ozs7O0lBRU0sZ0JBQWdCLENBQUMsU0FBaUIsRUFBRSxHQUFHLElBQVM7UUFDckQsb0JBQW9CLENBQUMsSUFBSSxDQUFDLEdBQUc7OztRQUFDLEdBQUcsRUFBRTtZQUNqQyxJQUFJLG9CQUFvQixDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsRUFBRTtnQkFDMUMsS0FBSyxNQUFNLElBQUksSUFBSSxvQkFBb0IsQ0FBQyxNQUFNLENBQUMsU0FBUyxDQUFDLEVBQUU7b0JBQ3pELElBQUksQ0FBQyxHQUFHLElBQUksQ0FBQyxDQUFDO2lCQUNmO2FBQ0Y7WUFDRCxJQUFJLG9CQUFvQixDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUMsRUFBRTs7c0JBQzVDLGlCQUFpQixHQUFHLG9CQUFvQixDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUM7Z0JBQ3hFLG9CQUFvQixDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUMsR0FBRyxTQUFTLENBQUM7Z0JBQzNELEtBQUssTUFBTSxJQUFJLElBQUksaUJBQWlCLEVBQUU7b0JBQ3BDLElBQUksQ0FBQyxHQUFHLElBQUksQ0FBQyxDQUFDO2lCQUNmO2FBQ0Y7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7Ozs7Ozs7OztJQVNNLE1BQU0sQ0FBQyxTQUFpQixFQUFFLFFBQWdDO1FBQy9ELElBQUksQ0FBQyxvQkFBb0IsQ0FBQyxNQUFNLENBQUMsU0FBUyxDQUFDLEVBQUU7WUFDM0Msb0JBQW9CLENBQUMsTUFBTSxDQUFDLFNBQVMsQ0FBQyxHQUFHLEVBQUUsQ0FBQztTQUM3QztRQUNELG9CQUFvQixDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUM7SUFDeEQsQ0FBQzs7Ozs7O0lBRU0sSUFBSSxDQUFDLFNBQWlCLEVBQUUsR0FBRyxJQUFTO1FBQ3pDLElBQUksT0FBTyxFQUFFLElBQUksV0FBVyxFQUFFLHVCQUF1QjtZQUNuRCxPQUFPO1FBQ1QsRUFBRSxDQUFDLE9BQU8sQ0FBQyxTQUFTLEVBQUUsR0FBRyxJQUFJLENBQUMsQ0FBQztJQUNqQyxDQUFDOzs7Ozs7Ozs7O0lBVU0sWUFBWSxDQUFDLFNBQWlCLEVBQUUsSUFBdUIsRUFBRSxRQUFnQztRQUM5RixJQUFJLE9BQU8sRUFBRSxJQUFJLFdBQVcsRUFBRSx1QkFBdUI7WUFDbkQsT0FBTztRQUNULElBQUksQ0FBQyxvQkFBb0IsQ0FBQyxjQUFjLENBQUMsU0FBUyxDQUFDLEVBQUU7WUFDbkQsb0JBQW9CLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQyxHQUFHLEVBQUUsQ0FBQztTQUNyRDtRQUNELG9CQUFvQixDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUM7UUFDOUQsSUFBSSxJQUFJO1lBQ04sRUFBRSxDQUFDLE9BQU8sQ0FBQyxTQUFTLEVBQUUsR0FBRyxJQUFJLENBQUMsQ0FBQzs7WUFFL0IsRUFBRSxDQUFDLE9BQU8sQ0FBQyxTQUFTLENBQUMsQ0FBQztJQUMxQixDQUFDOztBQWpFYyx5QkFBSSxHQUFXLElBQUksQ0FBQztBQUNwQiwyQkFBTSxHQUFnRCxFQUFFLENBQUM7QUFDekQsbUNBQWMsR0FBZ0QsRUFBRSxDQUFDOztZQVBqRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFUb0IsTUFBTTs7Ozs7Ozs7SUFZekIsMEJBQW1DOzs7OztJQUNuQyw0QkFBd0U7Ozs7O0lBQ3hFLG9DQUFnRiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIE5nWm9uZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5kZWNsYXJlIGNvbnN0IG1wOiB7XG4gIHRyaWdnZXIoZXZlbnROYW1lOiBzdHJpbmcsIC4uLmFyZ3M6IGFueSk6IHZvaWQ7XG59O1xuZGVjbGFyZSBjb25zdCB3aW5kb3c6IGFueTtcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCdcbn0pXG5leHBvcnQgY2xhc3MgUmFnZUNvbm5lY3RvclNlcnZpY2Uge1xuXG4gIHByaXZhdGUgc3RhdGljIHpvbmU6IE5nWm9uZSA9IG51bGw7XG4gIHByaXZhdGUgc3RhdGljIGV2ZW50czoge1trZXk6IHN0cmluZ106ICgoLi4uYXJnczogYW55KSA9PiB2b2lkKVtdfSA9IHt9O1xuICBwcml2YXRlIHN0YXRpYyBjYWxsYmFja0V2ZW50czoge1trZXk6IHN0cmluZ106ICgoLi4uYXJnczogYW55KSA9PiB2b2lkKVtdfSA9IHt9O1xuXG4gIGNvbnN0cnVjdG9yKHpvbmU6IE5nWm9uZSkge1xuICAgIFJhZ2VDb25uZWN0b3JTZXJ2aWNlLnpvbmUgPSB6b25lO1xuICAgIHdpbmRvdy5SYWdlQW5ndWxhckV2ZW50ID0gdGhpcy5yYWdlRXZlbnRIYW5kbGVyO1xuICB9XG5cbiAgcHVibGljIHJhZ2VFdmVudEhhbmRsZXIoZXZlbnROYW1lOiBzdHJpbmcsIC4uLmFyZ3M6IGFueSkge1xuICAgIFJhZ2VDb25uZWN0b3JTZXJ2aWNlLnpvbmUucnVuKCgpID0+IHtcbiAgICAgIGlmIChSYWdlQ29ubmVjdG9yU2VydmljZS5ldmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgICBmb3IgKGNvbnN0IGZ1bmMgb2YgUmFnZUNvbm5lY3RvclNlcnZpY2UuZXZlbnRzW2V2ZW50TmFtZV0pIHtcbiAgICAgICAgICBmdW5jKC4uLmFyZ3MpO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgICBpZiAoUmFnZUNvbm5lY3RvclNlcnZpY2UuY2FsbGJhY2tFdmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgICBjb25zdCBjYWxsYmFja0Z1bmN0aW9ucyA9IFJhZ2VDb25uZWN0b3JTZXJ2aWNlLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV07XG4gICAgICAgIFJhZ2VDb25uZWN0b3JTZXJ2aWNlLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV0gPSB1bmRlZmluZWQ7XG4gICAgICAgIGZvciAoY29uc3QgZnVuYyBvZiBjYWxsYmFja0Z1bmN0aW9ucykge1xuICAgICAgICAgIGZ1bmMoLi4uYXJncyk7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICB9KTtcbiAgfVxuXG4gIC8qKlxuICAgKiBBZGRzIGFuIFwiZXZlbnQgbGlzdGVuZXJcIiBmb3IgXCJldmVudHNcIiBmcm9tIFJBR0UgY2xpZW50c2lkZS5cbiAgICogRG9uJ3QgZm9yZ2V0IHRvIHVzZSBsYW1iZGEgZm9yIGNhbGxiYWNrIG9yIGFkZCBiaW5kKHRoaXMpLFxuICAgKiBlbHNlIFwidGhpc1wiIGluIHRoZSBmdW5jdGlvbiB3aWxsIHJlZmVyIHRvIHJhZ2UtY29ubmVjdG9yLnNlcnZpY2UudHMgaW5zdGVhZC5cbiAgICogQHBhcmFtIGV2ZW50TmFtZSBOYW1lIG9mIHRoZSBldmVudCB1c2VkIGluIFJBR0VcbiAgICogQHBhcmFtIGNhbGxiYWNrIEFueSBmdW5jdGlvblxuICAgKi9cbiAgcHVibGljIGxpc3RlbihldmVudE5hbWU6IHN0cmluZywgY2FsbGJhY2s6ICguLi5hcmdzOiBhbnkpID0+IHZvaWQpIHtcbiAgICBpZiAoIVJhZ2VDb25uZWN0b3JTZXJ2aWNlLmV2ZW50c1tldmVudE5hbWVdKSB7XG4gICAgICBSYWdlQ29ubmVjdG9yU2VydmljZS5ldmVudHNbZXZlbnROYW1lXSA9IFtdO1xuICAgIH1cbiAgICBSYWdlQ29ubmVjdG9yU2VydmljZS5ldmVudHNbZXZlbnROYW1lXS5wdXNoKGNhbGxiYWNrKTtcbiAgfVxuXG4gIHB1YmxpYyBjYWxsKGV2ZW50TmFtZTogc3RyaW5nLCAuLi5hcmdzOiBhbnkpIHtcbiAgICBpZiAodHlwZW9mIG1wID09IFwidW5kZWZpbmVkXCIpIC8vIHRlc3Rpbmcgd2l0aG91dCBSQUdFXG4gICAgICByZXR1cm47XG4gICAgbXAudHJpZ2dlcihldmVudE5hbWUsIC4uLmFyZ3MpO1xuICB9XG5cbiAgLyoqXG4gICAqIENhbGxzIGFuIGV2ZW50IGFuZCBhZGRzIGFuIFwiZXZlbnQgbGlzdGVuZXJcIiBmb3IgYSByZXNwb25zZSBmcm9tIFJBR0UgY2xpZW50c2lkZS5cbiAgICogRG9uJ3QgZm9yZ2V0IHRvIHVzZSBsYW1iZGEgZm9yIGNhbGxiYWNrIG9yIGFkZCBiaW5kKHRoaXMpLFxuICAgKiBlbHNlIFwidGhpc1wiIGluIHRoZSBmdW5jdGlvbiB3aWxsIHJlZmVyIHRvIHJhZ2UtY29ubmVjdG9yLnNlcnZpY2UudHMgaW5zdGVhZC5cbiAgICogQHBhcmFtIGV2ZW50TmFtZSBOYW1lIG9mIHRoZSBldmVudCB1c2VkIGluIFJBR0VcbiAgICogQHBhcmFtIGFyZ3MgQW55IGFyZ3VtZW50c1xuICAgKiBAcGFyYW0gY2FsbGJhY2sgQW55IGZ1bmN0aW9uXG4gICAqL1xuICBwdWJsaWMgY2FsbENhbGxiYWNrKGV2ZW50TmFtZTogc3RyaW5nLCBhcmdzOiBhbnlbXSB8IHVuZGVmaW5lZCwgY2FsbGJhY2s6ICguLi5hcmdzOiBhbnkpID0+IHZvaWQpIHtcbiAgICBpZiAodHlwZW9mIG1wID09IFwidW5kZWZpbmVkXCIpIC8vIHRlc3Rpbmcgd2l0aG91dCBSQUdFXG4gICAgICByZXR1cm47XG4gICAgaWYgKCFSYWdlQ29ubmVjdG9yU2VydmljZS5jYWxsYmFja0V2ZW50c1tldmVudE5hbWVdKSB7XG4gICAgICBSYWdlQ29ubmVjdG9yU2VydmljZS5jYWxsYmFja0V2ZW50c1tldmVudE5hbWVdID0gW107XG4gICAgfVxuICAgIFJhZ2VDb25uZWN0b3JTZXJ2aWNlLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV0ucHVzaChjYWxsYmFjayk7XG4gICAgaWYgKGFyZ3MpXG4gICAgICBtcC50cmlnZ2VyKGV2ZW50TmFtZSwgLi4uYXJncyk7XG4gICAgZWxzZVxuICAgICAgbXAudHJpZ2dlcihldmVudE5hbWUpO1xuICB9XG59XG4iXX0=