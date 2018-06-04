import { Component, OnInit, OnDestroy } from "@angular/core";
import { GlobalDataService } from "./shared/globaldata.service";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "./auth/auth.service";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit, OnDestroy {
    title = "TDS-CP";
    private refreshUsernamesTimeout: NodeJS.Timer;

    constructor(private http: HttpClient, private globaldata: GlobalDataService, private auth: AuthService) {}

    private refreshUsernames() {
        if (this.auth.isAuthenticated()) {
            this.http.get(this.globaldata.apiUrl + "/User").subscribe((usernames: string[]) => {
                this.globaldata.userNames = usernames;
            });
        }
    }

    ngOnInit() {
        this.refreshUsernamesTimeout = setTimeout(this.refreshUsernames, 30 * 1000);
        this.refreshUsernames();
    }

    ngOnDestroy() {
        clearTimeout(this.refreshUsernamesTimeout);
    }
}
