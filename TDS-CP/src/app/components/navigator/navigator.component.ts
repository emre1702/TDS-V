import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { PlayerOnlineService } from "../playeronline/playeronline.service";
import { HttpClient } from "@angular/common/http";
import { GlobalDataService } from "../../services/globaldata.service";
import { AuthService } from "../../services/auth/auth.service";

@Component({
    selector: "app-navigator",
    templateUrl: "./navigator.component.html",
    styleUrls: ["./navigator.component.css"]
})
export class NavigatorComponent {

    constructor(public router: Router, private playeronline: PlayerOnlineService, public globaldata: GlobalDataService, private auth: AuthService, private http: HttpClient) { }

    logout() {
        this.auth.removeAuthentication();
        this.http.post(this.globaldata.apiUrl + "/Logout", {withCredentials: true, header: this.auth.getHeaders()}).subscribe(() => {});
        this.playeronline.ngOnDestroy();
    }

}
