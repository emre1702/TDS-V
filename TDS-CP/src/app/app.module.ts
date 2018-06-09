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
    MAT_SNACK_BAR_DEFAULT_OPTIONS,
    MatToolbarModule,
    MatMenuModule
 } from "@angular/material";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule } from "@angular/common/http";

import { GlobalDataService } from "./shared/globaldata.service";
import { LoginComponent } from "./login/login.component";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { AngularDraggableModule } from "angular2-draggable";
import { JwtHelperService, JWT_OPTIONS, JwtModule } from "@auth0/angular-jwt";
import { appRouter } from "./app.router";
import { LoadingComponent } from "./loading/loading.component";
import { LoadingService } from "./loading/loading.service";
import { AuthService } from "./auth/auth.service";
import { PlayerOnlineComponent } from "./playeronline/playeronline.component";
import { PlayerOnlineService } from "./playeronline/playeronline.service";
import { NavigatorComponent } from "./navigator/navigator.component";

export function getToken() {
    return localStorage.getItem("token");
}

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        LoadingComponent,
        PlayerOnlineComponent,
        NavigatorComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        appRouter,
        AngularDraggableModule,
        JwtModule.forRoot({
            config: {
                tokenGetter: getToken,
                skipWhenExpired: true
            }
        }),

        MatIconModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        ReactiveFormsModule,
        MatSnackBarModule,
        MatProgressSpinnerModule,
        MatToolbarModule,
        MatMenuModule
    ],
    providers: [
        GlobalDataService,
        JwtHelperService,
        LoadingService,
        AuthService,
        PlayerOnlineService,
        {provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {duration: 4000, horizontalPosition: "center", verticalPosition: "bottom", panelClass: "snackBar"}}
    ],
    bootstrap: [AppComponent]
})
export class AppModule {}
