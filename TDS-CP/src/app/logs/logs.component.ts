import { Component, OnInit } from "@angular/core";
import { GlobalDataService } from "../shared/globaldata.service";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "../auth/auth.service";
import { LoadingService } from "../loading/loading.service";

@Component({
    selector: "app-logs",
    templateUrl: "./logs.component.html",
    styleUrls: ["./logs.component.css"],
})
export class LogsComponent {
    public entries: LogEntry[] = [];
    private page = 0;

    constructor(private http: HttpClient, private auth: AuthService, private loading: LoadingService, private globaldata: GlobalDataService) { }

    requestLogEntries(type: number) {
        if (type !== undefined) {
            this.loading.show();
            this.http.get(this.globaldata.apiUrl + "/Logs/logs/" + type + "/" + this.page, {withCredentials: true, headers: this.auth.getHeaders()}).subscribe((logentries: LogEntry[]) => {
                this.entries = logentries;
                this.loading.hide();
            });
        }
    }

}

class LogEntry {
    public id: number;
    public name: string;
    public target: string;
    public type: string;
    public lobby: string;
    public date: string;
}
