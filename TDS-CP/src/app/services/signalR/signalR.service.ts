import { Injectable } from "@angular/core";
import { ChatMessage } from "../../models/chatMessage.model";
import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";
import { GlobalDataService } from "../globaldata.service";
import { Subject } from "../../../../node_modules/rxjs";

@Injectable({
    providedIn: "root",
})
export class SignalRService {
    private hubConnection: HubConnection;
    onHubConnected = new Subject<Boolean>();
    onMessageReceived = new Subject<ChatMessage>();
    onLastMessagesReceived = new Subject<ChatMessage[]>();

    constructor(private globaldata: GlobalDataService) {
        this.createConnection();
        this.registerServerEvents();
        this.startConnection();
    }

    sendChatMessage(message: ChatMessage) {
        this.hubConnection.invoke("SendChatMessage", message);
    }

    requestLastChatMessages() {
        this.hubConnection.invoke("SendLastChatMessages");
    }

    private createConnection() {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(this.globaldata.apiUrl + "/notify")
            .build();
    }

    private registerServerEvents() {
        this.hubConnection.on("SendChatMessage", (data: ChatMessage) => {
            this.onMessageReceived.next(data);
        });
        this.hubConnection.on("SendLastChatMessages", (data: ChatMessage[]) => {
            this.onLastMessagesReceived.next(data);
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
