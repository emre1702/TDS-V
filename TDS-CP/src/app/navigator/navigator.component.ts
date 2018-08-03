import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { GlobalDataService } from "../shared/globaldata.service";
import { PlayerOnlineService } from "../playeronline/playeronline.service";
import { AuthService } from "../auth/auth.service";
import { HttpClient } from "../../../node_modules/@angular/common/http";

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
