import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router, CanLoad } from "@angular/router";

@Injectable()
export class AuthService implements CanLoad {

    constructor(private jwtHelper: JwtHelperService, public router: Router) { }

    public isAuthenticated(): boolean {
        const token = localStorage.getItem("token");
        return token != null && !this.jwtHelper.isTokenExpired(token);
    }

    canLoad(): boolean {
        if (!this.isAuthenticated()) {
            console.log("no");
            this.router.navigateByUrl("login");
            return false;
        }
        console.log("yes");
        return true;
    }
}
