import { Component, ElementRef, ViewChild } from "@angular/core";
import { trigger, transition, state, style, animate } from "../../../../node_modules/@angular/animations";
import { ChatService } from "./chat.service";
import { AnimationEvent } from "@angular/animations";

@Component({
    selector: "app-chat",
    templateUrl: "./chat.component.html",
    styleUrls: ["./chat.component.css"],
    animations: [
        trigger("chatOpened", [
            state("true", style({
                "width": "95%",
            })),
            state("false", style({
                "width": 0,
            })),
            transition("true => false", animate("200ms ease-out")),
            transition("false => true", animate("200ms ease-in"))
        ])
    ]
})
export class ChatComponent {
    opened = true;
    @ViewChild("tabOpenerButton") tabOpener: ElementRef;

    constructor(public chatservice: ChatService) {}

    toggleOpenState() {
        this.opened = !this.opened;
        this.chatservice.toggleOpenState(this.opened);
    }

    animationStart(event: AnimationEvent) {
        if (this.opened) {
            event.element.hidden = false;
            this.tabOpener.nativeElement.style.height = "10vh";
        }
    }

    animationDone(event: AnimationEvent) {
        if (!this.opened) {
            event.element.hidden = true;
            this.tabOpener.nativeElement.style.height = "100%";
        }
    }
}
