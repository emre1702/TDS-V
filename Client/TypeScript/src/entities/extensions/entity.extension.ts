import { Entity } from "alt-client"

declare module "alt-client" {
    interface Entity {
        exists(): boolean;
        isStreamed(): boolean;
    }
}

Entity.prototype.exists = function () {
    return (this as Entity).valid;
}

Entity.prototype.isStreamed = function () {
    return !!(this as Entity).scriptID;
}
