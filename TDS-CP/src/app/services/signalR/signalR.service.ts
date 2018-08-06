import { Injectable } from "@angular/core";
import { ChatMessage } from "../../models/chatMessage.model";
import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";
import { GlobalDataService } from "../globaldata.service";
import { Subject } from "../../../../node_modules/rxjs";
import { ReportUserEntry } from "../../models/reportUserEntry.model";

@Injectable({
    providedIn: "root",
})
export class SignalRService {
    private hubConnection: HubConnection;
    onHubConnected = new Subject<Boolean>();
    onMessageReceived = new Subject<ChatMessage>();
    onNewUserReport = new Subject<ReportUserEntry>();

    constructor(private globaldata: GlobalDataService) {
        this.createConnection();
        this.registerServerEvents();
        this.startConnection();
    }

    sendChatMessage(message: ChatMessage) {
        this.hubConnection.invoke("SendChatMessage", message);
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

        // REPORTS //
        this.hubConnection.on("AddNewUserReport", (report: ReportUserEntry) => {
            this.onNewUserReport.next(report);
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
