import { Injectable, OnDestroy } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { AuthService } from "../../services/auth/auth.service";
import { GlobalDataService } from "../../services/globaldata.service";

@Injectable({
    providedIn: "root"
})
export class PlayerOnlineService implements OnDestroy {
    playerOnlineArray: Player[] = [];
    private refreshPlayernamesTimeout: number;

    constructor(private http: HttpClient, private globaldata: GlobalDataService, private auth: AuthService, private router: Router) {
        if (router.url !== "/login") {
            this.startRefreshingPlayernames();
        }
    }

    refreshPlayernames() {
        if (this.auth.isAuthenticated()) {
            this.http.get(this.globaldata.apiUrl + "/Player/names", {withCredentials: true, headers: this.auth.getHeaders()}).subscribe((playersonline: Player[]) => {
                this.playerOnlineArray = playersonline;
                this.playerOnlineArray.sort(this.playerSorter);
            });
        }
    }

    startRefreshingPlayernames() {
        if (!this.globaldata.showingPlayername) {
            this.globaldata.showingPlayername = true;
            this.refreshPlayernamesTimeout = setTimeout(this.refreshPlayernames.bind(this), 30 * 1000);
            this.refreshPlayernames();
        } else {
            console.log("SHOWING USERNAME");
        }
    }

    ngOnDestroy() {
        if (this.refreshPlayernamesTimeout) {
            clearTimeout(this.refreshPlayernamesTimeout);
            this.refreshPlayernamesTimeout = undefined;
            this.globaldata.showingPlayername = false;
        }
    }

    private playerSorter(a: Player, b: Player): number {
        if (a.name < b.name) {
            return -1;
        } else if (a.name > b.name) {
            return 1;
        } else {
            return 0;
        }
    }
}

export class Player {
    public name: string;
    public adminLvl: number;
}