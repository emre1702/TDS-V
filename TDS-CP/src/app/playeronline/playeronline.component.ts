import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { PlayerOnlineService } from "./playeronline.service";
import { GlobalDataService } from "../shared/globaldata.service";

@Component({
    selector: "app-playeronline",
    templateUrl: "./playeronline.component.html",
    styleUrls: ["./playeronline.component.css"]
})
export class PlayerOnlineComponent {
    private refreshPlayernamesTimeout: number;
    private get playerOnlineArray() {
        return this.playerOnlineService.playerOnlineArray;
    }

    constructor(private playerOnlineService: PlayerOnlineService, private router: Router, private globaldata: GlobalDataService) { }
}
