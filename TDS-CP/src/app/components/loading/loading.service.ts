import { Injectable, Output } from "@angular/core";
import { EventEmitter } from "events";

@Injectable()
export class LoadingService {
    @Output() change: EventEmitter = new EventEmitter();
    showing = false;

    show() {
        this.change.emit("toggle", true);
        this.showing = true;
    }

    hide() {
        this.change.emit("toggle", false);
        this.showing = false;
    }
}
