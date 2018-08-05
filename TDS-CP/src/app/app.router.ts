import { Routes, RouterModule } from "@angular/router";
import { ModuleWithProviders } from "@angular/core";
import { LoginComponent } from "./components/login/login.component";
import { AuthService } from "./services/auth/auth.service";

export const router: Routes = [
    { path: "login", component: LoginComponent },
    { path: "home", loadChildren: "src/app/components/home/home.module#HomeModule", canLoad: [AuthService] },
    { path: "logs/:logtype", loadChildren: "src/app/components/logs/logs.module#LogsModule", canLoad: [AuthService] },
    { path: "report", loadChildren: "src/app/components/report/report.module#ReportModule", canLoad: [AuthService] },
    { path: "**", redirectTo: "home", canLoad: [AuthService] }
];

export const appRouter: ModuleWithProviders = RouterModule.forRoot(router);
