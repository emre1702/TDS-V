import { AuthService } from "./auth/auth.service";
import { LoginComponent } from "./login/login.component";
import { Routes, RouterModule } from "@angular/router";
import { ModuleWithProviders } from "@angular/core";

export const router: Routes = [
    { path: "login", component: LoginComponent },
    { path: "home", loadChildren: "src/app/home/home.module#HomeModule", canLoad: [AuthService] },
    { path: "logs/:logtype", loadChildren: "src/app/logs/logs.module#LogsModule", canLoad: [AuthService] },
    { path: "**", redirectTo: "home", canLoad: [AuthService] }
];

export const appRouter: ModuleWithProviders = RouterModule.forRoot(router);
