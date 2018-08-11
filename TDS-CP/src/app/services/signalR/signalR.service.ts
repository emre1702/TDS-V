import { Injectable, EventEmitter } from "@angular/core";
import { ChatMessage } from "../../models/chatMessage.model";
import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";
import { GlobalDataService } from "../globaldata.service";
import { ReportUserEntry } from "../../models/reportUserEntry.model";
import { EGroups } from "../../enums/egroups.enum";
import { AuthService } from "../auth/auth.service";
import { Router } from "../../../../node_modules/@angular/router";

@Injectable({
    providedIn: "root",
})
export class SignalRService {
    private hubConnection: HubConnection;
    private started = false;

    onHubConnected = new EventEmitter(true);
    onMessageReceived = new EventEmitter<ChatMessage>(true);
    onNewUserReport = new EventEmitter<ReportUserEntry>(true);
    onLogoutRequest = new EventEmitter<string>(true);

    constructor(private globaldata: GlobalDataService, private auth: AuthService, private router: Router) {
        this.start();
    }

    public start() {
        if (this.auth.isAuthenticated() && !this.started) {
            this.started = true;
            this.createConnection();
            this.registerServerEvents();
            this.startConnection();
            this.onLogoutRequest.subscribe(() => {
                this.stop();
            });
        }
    }

    private stop() {
        if (this.started) {
            this.started = false;
            this.hubConnection.stop();
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
            this.onMessageReceived.emit(data);
        });

        // LOGOUT //
        this.hubConnection.on("Logout", () => {
            this.onLogoutRequest.emit("Server requested a logout!");
        });
    }

    private startConnection() {
        this.hubConnection
            .start()
            .then(() => {
                this.onHubConnected.emit();
            })
            .catch(error => {
                console.log("Error - Couldn't connect with hub! Retrying ...");
                setTimeout(this.startConnection(), 5000);
                if (this.router.url !== "/login") {
                    this.onLogoutRequest.emit("Can't connect to the server!");
                }
            });
        this.hubConnection.onclose(error => {
            if (error) {
                console.log("Error - Couldn't connect with hub! Retrying ...");
                setTimeout(this.startConnection(), 5000);
                this.onLogoutRequest.emit("Disconnected to the server!");
            }

        });

    }
}
