import { BrowserModule } from "@angular/platform-browser";
import { NgModule, OnInit } from "@angular/core";

import { AppComponent } from "./app.component";

import {
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MAT_SNACK_BAR_DEFAULT_OPTIONS
 } from "@angular/material";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule } from "@angular/common/http";

import { SettingsService } from "./shared/settings.service";
import { LoginComponent } from "./login/login.component";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { AngularDraggableModule } from "angular2-draggable";
import { JwtHelperService, JWT_OPTIONS, JwtModule } from "@auth0/angular-jwt";
import { ROUTES } from "./app.routes";
import { HomeComponent } from "./home/home.component";
import { LoadingComponent } from "./loading/loading.component";
import { LoadingService } from "./loading/loading.service";
import { AuthGuardService } from "./auth/auth-guard.service";
import { AuthService } from "./auth/auth.service";

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        HomeComponent,
        LoadingComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        RouterModule.forRoot(
            ROUTES
        ),
        AngularDraggableModule,
        JwtModule.forRoot({
            config: {
                tokenGetter: () => {
                    return localStorage.getItem("token");
                },
                whitelistedDomains: ["localhost:5000"],
                skipWhenExpired: true
            }
        }),

        MatIconModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        ReactiveFormsModule,
        MatSnackBarModule,
        MatProgressSpinnerModule
    ],
    providers: [
        SettingsService,
        JwtHelperService,
        LoadingService,
        AuthService,
        AuthGuardService,
        {provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {duration: 4000, horizontalPosition: "center", verticalPosition: "bottom", panelClass: "snackBar"}}
    ],
    bootstrap: [AppComponent]
})
export class AppModule {}
