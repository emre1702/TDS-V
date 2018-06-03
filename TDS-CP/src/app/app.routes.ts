import { AuthGuardService as AuthGuard } from "./auth/auth-guard.service";
import { LoginComponent } from "./login/login.component";
import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";

export const ROUTES: Routes = [
    { path: "login", component: LoginComponent },
    { path: "home", component: HomeComponent, canActivate: [AuthGuard] },
    { path: "**", redirectTo: "home", canActivate: [AuthGuard] }
];
