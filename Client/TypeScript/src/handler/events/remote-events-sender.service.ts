import ToServerEvent from "../../data/enums/events/to-server-event.enum";
import cooldownEventsDict from "../../data/constants/events/event-cooldowns.default";
import { emitServer } from "alt-client";


class RemoteEventsSender {

    send(eventName: ToServerEvent, ...args: any[]): boolean {
        const cooldownData = cooldownEventsDict[eventName];
        if (!cooldownData) {
            emitServer(eventName, args);
            return true;
        }

        const currentMs = Date.now();
        if (cooldownData.lastExecMs && currentMs - cooldownData.lastExecMs < cooldownData.cooldownMs) {
            return false;
        }

        cooldownData.lastExecMs = currentMs;
        emitServer(eventName, args);
        return true;
    }

    sendFromBrowser(...args: any[]): boolean {
        const cooldownData = cooldownEventsDict[args[0]];
        if (!cooldownData) {
            emitServer(ToServerEvent.FromBrowserEvent, args);
            return true;
        }

        const currentMs = Date.now();
        if (cooldownData.lastExecMs && currentMs - cooldownData.lastExecMs < cooldownData.cooldownMs) {
            return false;
        }

        cooldownData.lastExecMs = currentMs;
        emitServer(ToServerEvent.FromBrowserEvent, args);
        return true;
    }

    sendIgnoreCooldown(eventName: ToServerEvent, ...args: any[]): boolean {
        const cooldownData = cooldownEventsDict[eventName];
        if (!cooldownData) {
            emitServer(eventName, args);
            return true;
        }

        const currentMs = Date.now();
        cooldownData.lastExecMs = currentMs;
        emitServer(eventName, args);
        return true;
    }
}

export default RemoteEventsSender;
