import { Component } from "@angular/core";
import { FormControl, FormGroup, Validators, NgForm } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material";
import { LoadingService } from "../loading/loading.service";
import { PlayerOnlineService } from "../playeronline/playeronline.service";
import { GlobalDataService } from "../../services/globaldata.service";
import { ChatService } from "../chat/chat.service";
import { SignalRService } from "../../services/signalR/signalR.service";

@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"]
})
export class LoginComponent {
    hidePassword = true;
    loginForm = new FormGroup({
        username: new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
        password: new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(100)])
    });

    constructor(private http: HttpClient, private snackBar: MatSnackBar, private settings: GlobalDataService, private loading: LoadingService, private router: Router,
        private playerOnlineService: PlayerOnlineService, private chat: ChatService, private signalR: SignalRService ) { }

    onSubmit(form: NgForm) {
        if (!this.loading.showing) {
            this.loading.show();
            this.http.post(this.settings.apiUrl + "/Login", form.value).subscribe((data: {uid: number, token: string, username: string, adminlvl: number, error: string}) => {
                if (data.token) {
                    localStorage.setItem("token", data.token);
                    localStorage.setItem("username", data.username);
                    localStorage.setItem("adminlvl", data.adminlvl.toString());
                    this.router.navigateByUrl("home");
                    this.signalR.start();
                    this.playerOnlineService.startRefreshingPlayernames();
                    this.chat.start();
                } else {
                    this.snackBar.open(data.error);
                }
                this.loading.hide();
            });
        }
    }
}
