import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router, CanLoad } from "@angular/router";
import { HttpHeaders } from "@angular/common/http";

@Injectable()
export class AuthService implements CanLoad {
    private headers: HttpHeaders;

    constructor(private jwtHelper: JwtHelperService, public router: Router) { }

    getHeaders(): HttpHeaders {
        if (this.isAuthenticated()) {
            if (!this.headers) {
                this.headers = new HttpHeaders({"Authorization": "Bearer " + localStorage.getItem("token")});
            }
            return this.headers;
        }
        return null;
    }

    public isAuthenticated(): boolean {
        const token = localStorage.getItem("token");
        return token != null && !this.jwtHelper.isTokenExpired(token);
    }

    canLoad(): boolean {
        if (!this.isAuthenticated()) {
            this.router.navigateByUrl("login");
            return false;
        }
        return true;
    }

    public removeAuthentication() {
        localStorage.removeItem("token");
        this.headers = null;
        this.router.navigateByUrl("login");
    }
}
