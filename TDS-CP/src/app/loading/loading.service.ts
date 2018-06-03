import { Injectable, Output } from "@angular/core";
import { EventEmitter } from "events";

@Injectable()
export class LoadingService {
    @Output() change: EventEmitter = new EventEmitter();

    show() {
        this.change.emit("toggle", true);
    }

    hide() {
        this.change.emit("toggle", false);
    }
}
