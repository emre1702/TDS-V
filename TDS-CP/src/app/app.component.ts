import { Component, HostListener, ViewChild, ElementRef } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { GlobalDataService } from "./services/globaldata.service";
import { AuthService } from "./services/auth/auth.service";
import { PlayerOnlineService } from "./components/playeronline/playeronline.service";
import { ChatService } from "./components/chat/chat.service";
import { Router } from "../../node_modules/@angular/router";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"]
})
export class AppComponent {
    title = "TDS-CP";

    @ViewChild("chatAppDiv") chatApp: ElementRef;

    constructor(private http: HttpClient, private globaldata: GlobalDataService, private auth: AuthService, private playeronline: PlayerOnlineService, public router: Router, private chatservice: ChatService) {
        this.chatservice.onOpenToggle.subscribe(open => {
            if (open) {
                this.chatApp.nativeElement.style.width = "20%";
                this.chatApp.nativeElement.style.top = "0%";
                this.chatApp.nativeElement.style.height = "100%";
            } else {
                setTimeout(() => {
                    this.chatApp.nativeElement.style.width = "auto";
                    this.chatApp.nativeElement.style.top = "45%";
                    this.chatApp.nativeElement.style.height = "10%";
                }, 200);
            }
        });
    }

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
