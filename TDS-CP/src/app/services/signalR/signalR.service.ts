import { Injectable } from "@angular/core";
import { ChatMessage } from "../../models/chatMessage.model";
import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";
import { GlobalDataService } from "../globaldata.service";
import { Subject } from "../../../../node_modules/rxjs";
import { ReportUserEntry } from "../../models/reportUserEntry.model";
import { EGroups } from "../../enums/egroups.enum";
import { AuthService } from "../auth/auth.service";

@Injectable({
    providedIn: "root",
})
export class SignalRService {
    private hubConnection: HubConnection;
    private started = false;

    onHubConnected = new Subject<Boolean>();
    onMessageReceived = new Subject<ChatMessage>();
    onNewUserReport = new Subject<ReportUserEntry>();
    onLogoutRequest = new Subject<Boolean>();

    constructor(private globaldata: GlobalDataService, private auth: AuthService) {
        this.start();
    }

    public start() {
        if (this.auth.isAuthenticated() && !this.started) {
            this.started = true;
            this.createConnection();
            this.registerServerEvents();
            this.startConnection();
        }
    }

    sendChatMessage(message: ChatMessage) {
        if (this.auth.isAuthenticated()) {
            this.hubConnection.invoke("SendChatMessage", message);
        }
    }

    addToGroup(group: EGroups, opt?: string) {
        if (this.auth.isAuthenticated()) {
            this.hubConnection.invoke("AddToGroup", group, opt);
        }
    }

    removeFromGroup(group: EGroups, opt?: string) {
        if (this.auth.isAuthenticated()) {
            this.hubConnection.invoke("RemoveFromGroup", group, opt);
        }
    }

    private createConnection() {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(this.globaldata.apiUrl + "/notify")
            .build();
    }

    private registerServerEvents() {

        // CHAT //
        this.hubConnection.on("SendChatMessage", (data: ChatMessage) => {
            this.onMessageReceived.next(data);
        });

        // LOGOUT //
        this.hubConnection.on("Logout", () => {
            this.onLogoutRequest.next(true);
        });
    }

    private startConnection() {
        this.hubConnection
            .start()
            .then(() => {
                this.onHubConnected.next(true);
            })
            .catch(error => {
                console.log("Error - Couldn't connect with hub! Retrying ...");
                setTimeout(this.startConnection(), 5000);
            });

    }
}
