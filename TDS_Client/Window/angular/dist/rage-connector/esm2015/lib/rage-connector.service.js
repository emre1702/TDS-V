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
    rageEventHandler(eventName, ...args) {
        this.zone.run((/**
         * @return {?}
         */
        () => {
            if (this.events[eventName]) {
                for (const func of this.events[eventName]) {
                    func(...args);
                }
            }
            if (this.callbackEvents[eventName]) {
                /** @type {?} */
                const callbackFunctions = this.callbackEvents[eventName];
                this.callbackEvents[eventName] = undefined;
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
        if (!this.events[eventName]) {
            this.events[eventName] = [];
        }
        this.events[eventName].push(callback);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmFnZS1jb25uZWN0b3Iuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL3JhZ2UtY29ubmVjdG9yLyIsInNvdXJjZXMiOlsibGliL3JhZ2UtY29ubmVjdG9yLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDOztBQVVuRCxNQUFNLE9BQU8sb0JBQW9COzs7O0lBSy9CLFlBQW9CLElBQVk7UUFBWixTQUFJLEdBQUosSUFBSSxDQUFRO1FBSHhCLFdBQU0sR0FBZ0QsRUFBRSxDQUFDO1FBQ3pELG1CQUFjLEdBQWdELEVBQUUsQ0FBQztRQUd2RSxNQUFNLENBQUMsZ0JBQWdCLEdBQUcsSUFBSSxDQUFDLGdCQUFnQixDQUFDO0lBQ2xELENBQUM7Ozs7OztJQUVNLGdCQUFnQixDQUFDLFNBQWlCLEVBQUUsR0FBRyxJQUFTO1FBQ3JELElBQUksQ0FBQyxJQUFJLENBQUMsR0FBRzs7O1FBQUMsR0FBRyxFQUFFO1lBQ2pCLElBQUksSUFBSSxDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsRUFBRTtnQkFDMUIsS0FBSyxNQUFNLElBQUksSUFBSSxJQUFJLENBQUMsTUFBTSxDQUFDLFNBQVMsQ0FBQyxFQUFFO29CQUN6QyxJQUFJLENBQUMsR0FBRyxJQUFJLENBQUMsQ0FBQztpQkFDZjthQUNGO1lBQ0QsSUFBSSxJQUFJLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQyxFQUFFOztzQkFDNUIsaUJBQWlCLEdBQUcsSUFBSSxDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUM7Z0JBQ3hELElBQUksQ0FBQyxjQUFjLENBQUMsU0FBUyxDQUFDLEdBQUcsU0FBUyxDQUFDO2dCQUMzQyxLQUFLLE1BQU0sSUFBSSxJQUFJLGlCQUFpQixFQUFFO29CQUNwQyxJQUFJLENBQUMsR0FBRyxJQUFJLENBQUMsQ0FBQztpQkFDZjthQUNGO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOzs7Ozs7Ozs7SUFTTSxNQUFNLENBQUMsU0FBaUIsRUFBRSxRQUFnQztRQUMvRCxJQUFJLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsRUFBRTtZQUMzQixJQUFJLENBQUMsTUFBTSxDQUFDLFNBQVMsQ0FBQyxHQUFHLEVBQUUsQ0FBQztTQUM3QjtRQUNELElBQUksQ0FBQyxNQUFNLENBQUMsU0FBUyxDQUFDLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO0lBQ3hDLENBQUM7Ozs7OztJQUVNLElBQUksQ0FBQyxTQUFpQixFQUFFLEdBQUcsSUFBUztRQUN6QyxJQUFJLE9BQU8sRUFBRSxJQUFJLFdBQVcsRUFBRSx1QkFBdUI7WUFDbkQsT0FBTztRQUNULEVBQUUsQ0FBQyxPQUFPLENBQUMsU0FBUyxFQUFFLEdBQUcsSUFBSSxDQUFDLENBQUM7SUFDakMsQ0FBQzs7Ozs7Ozs7OztJQVVNLFlBQVksQ0FBQyxTQUFpQixFQUFFLElBQXVCLEVBQUUsUUFBZ0M7UUFDOUYsSUFBSSxPQUFPLEVBQUUsSUFBSSxXQUFXLEVBQUUsdUJBQXVCO1lBQ25ELE9BQU87UUFDVCxJQUFJLENBQUMsSUFBSSxDQUFDLGNBQWMsQ0FBQyxTQUFTLENBQUMsRUFBRTtZQUNuQyxJQUFJLENBQUMsY0FBYyxDQUFDLFNBQVMsQ0FBQyxHQUFHLEVBQUUsQ0FBQztTQUNyQztRQUNELElBQUksQ0FBQyxjQUFjLENBQUMsU0FBUyxDQUFDLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO1FBQzlDLElBQUksSUFBSTtZQUNOLEVBQUUsQ0FBQyxPQUFPLENBQUMsU0FBUyxFQUFFLEdBQUcsSUFBSSxDQUFDLENBQUM7O1lBRS9CLEVBQUUsQ0FBQyxPQUFPLENBQUMsU0FBUyxDQUFDLENBQUM7SUFDMUIsQ0FBQzs7O1lBcEVGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQVRvQixNQUFNOzs7Ozs7OztJQVl6QixzQ0FBaUU7Ozs7O0lBQ2pFLDhDQUF5RTs7Ozs7SUFFN0Qsb0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgTmdab25lIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbmRlY2xhcmUgY29uc3QgbXA6IHtcbiAgdHJpZ2dlcihldmVudE5hbWU6IHN0cmluZywgLi4uYXJnczogYW55KTogdm9pZDtcbn07XG5kZWNsYXJlIGNvbnN0IHdpbmRvdzogYW55O1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290J1xufSlcbmV4cG9ydCBjbGFzcyBSYWdlQ29ubmVjdG9yU2VydmljZSB7XG5cbiAgcHJpdmF0ZSBldmVudHM6IHtba2V5OiBzdHJpbmddOiAoKC4uLmFyZ3M6IGFueSkgPT4gdm9pZClbXX0gPSB7fTtcbiAgcHJpdmF0ZSBjYWxsYmFja0V2ZW50czoge1trZXk6IHN0cmluZ106ICgoLi4uYXJnczogYW55KSA9PiB2b2lkKVtdfSA9IHt9O1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgem9uZTogTmdab25lKSB7XG4gICAgd2luZG93LlJhZ2VBbmd1bGFyRXZlbnQgPSB0aGlzLnJhZ2VFdmVudEhhbmRsZXI7XG4gIH1cblxuICBwdWJsaWMgcmFnZUV2ZW50SGFuZGxlcihldmVudE5hbWU6IHN0cmluZywgLi4uYXJnczogYW55KSB7XG4gICAgdGhpcy56b25lLnJ1bigoKSA9PiB7XG4gICAgICBpZiAodGhpcy5ldmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgICBmb3IgKGNvbnN0IGZ1bmMgb2YgdGhpcy5ldmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgICAgIGZ1bmMoLi4uYXJncyk7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIGlmICh0aGlzLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV0pIHtcbiAgICAgICAgY29uc3QgY2FsbGJhY2tGdW5jdGlvbnMgPSB0aGlzLmNhbGxiYWNrRXZlbnRzW2V2ZW50TmFtZV07XG4gICAgICAgIHRoaXMuY2FsbGJhY2tFdmVudHNbZXZlbnROYW1lXSA9IHVuZGVmaW5lZDtcbiAgICAgICAgZm9yIChjb25zdCBmdW5jIG9mIGNhbGxiYWNrRnVuY3Rpb25zKSB7XG4gICAgICAgICAgZnVuYyguLi5hcmdzKTtcbiAgICAgICAgfVxuICAgICAgfVxuICAgIH0pO1xuICB9XG5cbiAgLyoqXG4gICAqIEFkZHMgYW4gXCJldmVudCBsaXN0ZW5lclwiIGZvciBcImV2ZW50c1wiIGZyb20gUkFHRSBjbGllbnRzaWRlLlxuICAgKiBEb24ndCBmb3JnZXQgdG8gdXNlIGxhbWJkYSBmb3IgY2FsbGJhY2sgb3IgYWRkIGJpbmQodGhpcyksXG4gICAqIGVsc2UgXCJ0aGlzXCIgaW4gdGhlIGZ1bmN0aW9uIHdpbGwgcmVmZXIgdG8gcmFnZS1jb25uZWN0b3Iuc2VydmljZS50cyBpbnN0ZWFkLlxuICAgKiBAcGFyYW0gZXZlbnROYW1lIE5hbWUgb2YgdGhlIGV2ZW50IHVzZWQgaW4gUkFHRVxuICAgKiBAcGFyYW0gY2FsbGJhY2sgQW55IGZ1bmN0aW9uXG4gICAqL1xuICBwdWJsaWMgbGlzdGVuKGV2ZW50TmFtZTogc3RyaW5nLCBjYWxsYmFjazogKC4uLmFyZ3M6IGFueSkgPT4gdm9pZCkge1xuICAgIGlmICghdGhpcy5ldmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgdGhpcy5ldmVudHNbZXZlbnROYW1lXSA9IFtdO1xuICAgIH1cbiAgICB0aGlzLmV2ZW50c1tldmVudE5hbWVdLnB1c2goY2FsbGJhY2spO1xuICB9XG5cbiAgcHVibGljIGNhbGwoZXZlbnROYW1lOiBzdHJpbmcsIC4uLmFyZ3M6IGFueSkge1xuICAgIGlmICh0eXBlb2YgbXAgPT0gXCJ1bmRlZmluZWRcIikgLy8gdGVzdGluZyB3aXRob3V0IFJBR0VcbiAgICAgIHJldHVybjtcbiAgICBtcC50cmlnZ2VyKGV2ZW50TmFtZSwgLi4uYXJncyk7XG4gIH1cblxuICAvKipcbiAgICogQ2FsbHMgYW4gZXZlbnQgYW5kIGFkZHMgYW4gXCJldmVudCBsaXN0ZW5lclwiIGZvciBhIHJlc3BvbnNlIGZyb20gUkFHRSBjbGllbnRzaWRlLlxuICAgKiBEb24ndCBmb3JnZXQgdG8gdXNlIGxhbWJkYSBmb3IgY2FsbGJhY2sgb3IgYWRkIGJpbmQodGhpcyksXG4gICAqIGVsc2UgXCJ0aGlzXCIgaW4gdGhlIGZ1bmN0aW9uIHdpbGwgcmVmZXIgdG8gcmFnZS1jb25uZWN0b3Iuc2VydmljZS50cyBpbnN0ZWFkLlxuICAgKiBAcGFyYW0gZXZlbnROYW1lIE5hbWUgb2YgdGhlIGV2ZW50IHVzZWQgaW4gUkFHRVxuICAgKiBAcGFyYW0gYXJncyBBbnkgYXJndW1lbnRzXG4gICAqIEBwYXJhbSBjYWxsYmFjayBBbnkgZnVuY3Rpb25cbiAgICovXG4gIHB1YmxpYyBjYWxsQ2FsbGJhY2soZXZlbnROYW1lOiBzdHJpbmcsIGFyZ3M6IGFueVtdIHwgdW5kZWZpbmVkLCBjYWxsYmFjazogKC4uLmFyZ3M6IGFueSkgPT4gdm9pZCkge1xuICAgIGlmICh0eXBlb2YgbXAgPT0gXCJ1bmRlZmluZWRcIikgLy8gdGVzdGluZyB3aXRob3V0IFJBR0VcbiAgICAgIHJldHVybjtcbiAgICBpZiAoIXRoaXMuY2FsbGJhY2tFdmVudHNbZXZlbnROYW1lXSkge1xuICAgICAgdGhpcy5jYWxsYmFja0V2ZW50c1tldmVudE5hbWVdID0gW107XG4gICAgfVxuICAgIHRoaXMuY2FsbGJhY2tFdmVudHNbZXZlbnROYW1lXS5wdXNoKGNhbGxiYWNrKTtcbiAgICBpZiAoYXJncylcbiAgICAgIG1wLnRyaWdnZXIoZXZlbnROYW1lLCAuLi5hcmdzKTtcbiAgICBlbHNlXG4gICAgICBtcC50cmlnZ2VyKGV2ZW50TmFtZSk7XG4gIH1cbn1cbiJdfQ==