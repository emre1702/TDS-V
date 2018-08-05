import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { SignalRService } from "../../services/signalR/signalR.service";
import { ChatMessage } from "../../models/chatMessage.model";
import { Subject } from "../../../../node_modules/rxjs";

@Injectable({
    providedIn: "root"
})
export class ChatService {
    activatedbool = false;
    entries: ChatMessage[] = [];
    started = false;

    onOpenToggle = new Subject<Boolean>();

    constructor(private signalR: SignalRService, private router: Router) {
        if (router.url !== "/login") {
            this.start();
        }
    }

    start() {
        if (this.started) {
            return;
        }
        this.started = true;
        this.signalR.onHubConnected.subscribe(() => {
            if (this.activatedbool) {
                return;
            }
            this.activatedbool = true;
            this.signalR.requestLastChatMessages();
        });
        this.signalR.onMessageReceived.subscribe((message: ChatMessage) => {
            this.entries.push(message);
        });
        this.signalR.onLastMessagesReceived.subscribe((data: any) => {
            console.log(data);
            this.entries = data;
        });
        if (this.activatedbool) {
            this.signalR.requestLastChatMessages();
        }
    }

    get activated() {
        if (this.router.url !== "/login") {
            return this.activated;
        }
        return false;
    }

    sendChatMessage(text: string) {
        if (this.router.url !== "/login") {
            let message = new ChatMessage("Bonus", text);
            message.sent = new Date();
            this.signalR.sendChatMessage(message);
        }
    }

    toggleOpenState(bool: Boolean) {
        this.onOpenToggle.next(bool);
    }
}
