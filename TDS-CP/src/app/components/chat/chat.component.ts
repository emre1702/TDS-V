import { Component, ElementRef, ViewChild, OnDestroy, ChangeDetectorRef, AfterViewInit } from "@angular/core";
import { trigger, transition, state, style, animate } from "../../../../node_modules/@angular/animations";
import { ChatService } from "./chat.service";
import { MediaMatcher } from "../../../../node_modules/@angular/cdk/layout";
import { Router } from "../../../../node_modules/@angular/router";
import { MatInput } from "../../../../node_modules/@angular/material";
import { Subscription } from "../../../../node_modules/rxjs";

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
export class ChatComponent implements OnDestroy, AfterViewInit {
    mobileQuery: MediaQueryList;
    messageTooShort = false;
    messageTooLong = false;
    scrollBeforeAfterView = false;
    @ViewChild("tabOpenerButton") tabOpener: ElementRef;
    @ViewChild("chatinput") chatInput: MatInput;
    @ViewChild("contentplace") contentPlace: ElementRef;

    private _mobileQueryListener: () => void;

    constructor(public chatservice: ChatService, private changeDetectorRef: ChangeDetectorRef, private media: MediaMatcher, public router: Router) {
        this.mobileQuery = this.media.matchMedia("(max-width: 599px)");
        this._mobileQueryListener = () => this.changeDetectorRef.detectChanges();
        this.mobileQuery.addListener(this._mobileQueryListener);
    }

    ngOnDestroy() {
        this.mobileQuery.removeListener(this._mobileQueryListener);
    }

    ngAfterViewInit() {
        this.scrollChatToBottom();
        this.chatservice.onEntryChange.subscribe(this.scrollChatToBottom.bind(this));
    }

    private scrollChatToBottom() {
        setTimeout(() => {
            this.contentPlace.nativeElement.scrollTo({left: 0 , top: this.contentPlace.nativeElement.scrollHeight, behavior: "smooth"});
        }, 500);
    }

    sendChatMessage() {
        const text = this.chatInput.value;
        if (text.length < 3) {
            this.messageTooShort = true;
        } else {
            this.messageTooShort = false;
        }

        if (text.length > 100) {
            this.messageTooLong = true;
        } else {
            this.messageTooLong = false;
        }
        if (!this.messageTooShort && !this.messageTooLong) {
            this.chatservice.sendChatMessage(text);
            this.chatInput.value = "";
            this.chatInput.errorState = false;
        } else {
            this.chatInput.errorState = true;
        }

    }
}
