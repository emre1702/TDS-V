import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { GlobalDataService } from "../shared/globaldata.service";
import { PlayerOnlineService } from "../playeronline/playeronline.service";

@Component({
    selector: "app-navigator",
    templateUrl: "./navigator.component.html",
    styleUrls: ["./navigator.component.css"]
})
export class NavigatorComponent {

    constructor(private router: Router, private playeronline: PlayerOnlineService, private globaldata: GlobalDataService) { }

    logout() {
        localStorage.removeItem("token");
        this.playeronline.ngOnDestroy();
        this.router.navigateByUrl("login");
    }

}
