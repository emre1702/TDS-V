import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LoadingService } from "../loading/loading.service";
import { GlobalDataService } from "../shared/globaldata.service";
import { UserOnlineService } from "../useronline/useronline.service";

@Component({
    selector: "app-home",
    templateUrl: "./home.component.html",
    styleUrls: ["./home.component.css"]
})

export class HomeComponent implements OnInit {
    constructor(private http: HttpClient, private globaldata: GlobalDataService, private loading: LoadingService, private userOnlineService: UserOnlineService) { }

    ngOnInit() {
        this.userOnlineService.refreshUsernames();
    }
}
