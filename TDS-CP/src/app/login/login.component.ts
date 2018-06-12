import { Component, OnInit, Output } from "@angular/core";
import { FormControl, FormGroup, Validators, NgForm } from "@angular/forms";
import { HttpClient, HttpHeaders, HttpRequest } from "@angular/common/http";
import { GlobalDataService } from "../shared/globaldata.service";
import { Router } from "@angular/router";
import { EventEmitter } from "events";
import { MatSnackBar } from "@angular/material";
import { LoadingService } from "../loading/loading.service";
import { PlayerOnlineService } from "../playeronline/playeronline.service";

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
        private playerOnlineService: PlayerOnlineService ) { }

    onSubmit(form: NgForm) {
        if (!this.loading.showing) {
            this.loading.show();
            this.http.post(this.settings.apiUrl + "/Login", form.value).subscribe((data: {token: string, adminlvl: number, error: string}) => {
                if (data.token) {
                    localStorage.setItem("token", data.token);
                    localStorage.setItem("adminlvl", data.adminlvl.toString());
                    this.router.navigateByUrl("home");
                    this.playerOnlineService.startRefreshingPlayernames();
                } else {
                    this.snackBar.open(data.error);
                }
                this.loading.hide();
            });
        }
    }
}
