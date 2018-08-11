import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { PlayerOnlineService } from "../playeronline/playeronline.service";
import { HttpClient } from "@angular/common/http";
import { GlobalDataService } from "../../services/globaldata.service";
import { AuthService } from "../../services/auth/auth.service";
import { SignalRService } from "../../services/signalR/signalR.service";
import { EGroups } from "../../enums/egroups.enum";
import { MatSnackBar } from "../../../../node_modules/@angular/material";

@Component({
    selector: "app-navigator",
    templateUrl: "./navigator.component.html",
    styleUrls: ["./navigator.component.css"]
})
export class NavigatorComponent {

    constructor(public router: Router, public globaldata: GlobalDataService, private signalR: SignalRService, private auth: AuthService,
            private playeronline: PlayerOnlineService, private http: HttpClient, private snackBar: MatSnackBar) {
        signalR.onLogoutRequest.subscribe((error) => this.logout(error));
    }

    logoutStart() {
        this.signalR.onLogoutRequest.next();
    }

    logout(error: string) {
        console.log("on log out navigator");
        this.auth.removeAuthentication();
        this.signalR.removeFromGroup(EGroups.Loggedin);
        this.playeronline.ngOnDestroy();
        if (error) {
            this.snackBar.open(error);
        }
    }
}
