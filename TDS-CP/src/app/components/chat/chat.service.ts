import { Injectable, EventEmitter } from "@angular/core";
import { Router } from "@angular/router";
import { SignalRService } from "../../services/signalR/signalR.service";
import { ChatMessage } from "../../models/chatMessage.model";
import { HttpClient } from "@angular/common/http";
import { GlobalDataService } from "../../services/globaldata.service";
import { AuthService } from "../../services/auth/auth.service";

@Injectable({
    providedIn: "root"
})
export class ChatService {
    activatedbool = false;
    entries: ChatMessage[] = [];
    started = false;

    onEntryChange = new EventEmitter(true);

    constructor(private signalR: SignalRService, private router: Router, private http: HttpClient, private globaldata: GlobalDataService, private auth: AuthService) {
        if (router.url !== "/login") {
            this.start();
        }
    }

    start() {
        if (this.started) {
            return;
        }
        this.started = true;
        if (!this.activatedbool) {
            this.signalR.onHubConnected.subscribe(() => {
                if (this.activatedbool) {
                    return;
                }
                this.activatedbool = true;
                this.requestLastChatMessages();
            });
        } else {
            this.requestLastChatMessages();
        }
        this.signalR.onMessageReceived.subscribe((message: ChatMessage) => {
            this.entries.push(message);
            this.onEntryChange.next();
        });
    }

    requestLastChatMessages() {
        this.http.get(this.globaldata.apiUrl + "/chat", {withCredentials: true, headers: this.auth.getHeaders()}).subscribe((datas: ChatMessage[]) => {
            this.entries = datas;
            this.onEntryChange.next();
        });
    }

    get activated() {
        if (this.router.url !== "/login") {
            return this.activated;
        }
        return false;
    }

    sendChatMessage(text: string) {
        if (this.router.url !== "/login") {
            let message = new ChatMessage(this.globaldata.username, text);
            message.sent = new Date();
            this.entries.push(message);
            this.signalR.sendChatMessage(message);
            this.onEntryChange.next(true);
        }
    }
}
