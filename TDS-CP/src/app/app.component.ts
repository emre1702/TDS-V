import { Component, HostListener } from "@angular/core";
import { HttpClient } from "../../node_modules/@angular/common/http";
import { GlobalDataService } from "./shared/globaldata.service";
import { AuthService } from "./auth/auth.service";
import { PlayerOnlineService } from "./playeronline/playeronline.service";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"]
})
export class AppComponent {
    title = "TDS-CP";

    constructor(private http: HttpClient, private globaldata: GlobalDataService, private auth: AuthService, private playeronline: PlayerOnlineService) {}

    @HostListener("window:unload", ["$event"])
    unloadHandler(event) {
        if (this.auth.isAuthenticated()) {
            this.http.post(this.globaldata.apiUrl + "/Logout", {withCredentials: true, header: this.auth.getHeaders()}).subscribe(() => {
                this.auth.removeAuthentication();
                this.playeronline.ngOnDestroy();
            });
        }
    }
}
