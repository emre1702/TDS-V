import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { UserOnlineService } from "./useronline.service";

@Component({
    selector: "app-useronline",
    templateUrl: "./useronline.component.html",
    styleUrls: ["./useronline.component.css"]
})
export class UserOnlineComponent {
    private userNamesArray: String[] = [];
    get userNames(): String {
        return this.userOnlineService.userNames;
    }
    get amountUserNames(): number {
        return this.userOnlineService.amountUserNames;
    }
    private refreshUsernamesTimeout: number;

    constructor(private userOnlineService: UserOnlineService, private router: Router) { }
}
