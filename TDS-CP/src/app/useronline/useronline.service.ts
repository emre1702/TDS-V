import { Injectable, OnDestroy } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { GlobalDataService } from "../shared/globaldata.service";
import { AuthService } from "../auth/auth.service";

@Injectable()
export class UserOnlineService implements OnDestroy {
    private userNamesArray: String[] = [];
    get userNames(): String {
        return this.userNamesArray.join(", ");
    }
    get amountUserNames(): number {
        return this.userNamesArray.length;
    }
    private refreshUsernamesTimeout: number;

    constructor(private http: HttpClient, private globaldata: GlobalDataService, private auth: AuthService) { }

    refreshUsernames() {
        if (this.auth.isAuthenticated()) {
            this.http.get(this.globaldata.apiUrl + "/User/names").subscribe((usernames: string[]) => {
                this.userNamesArray = usernames;
                this.userNamesArray.sort();
            });
        }
    }

    startRefreshingUsernames() {
        if (!this.globaldata.showingUsername) {
            this.globaldata.showingUsername = true;
            this.refreshUsernamesTimeout = setTimeout(this.refreshUsernames.bind(this), 30 * 1000);
            this.refreshUsernames();
        } else {
            console.log("SHOWING USERNAME");
        }
    }

    ngOnDestroy() {
        if (this.refreshUsernamesTimeout) {
            clearTimeout(this.refreshUsernamesTimeout);
            this.globaldata.showingUsername = false;
        }
    }
}
