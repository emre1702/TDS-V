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

import { GlobalDataService } from "./services/globaldata.service";
import { ReactiveFormsModule } from "@angular/forms";
import { AngularDraggableModule } from "angular2-draggable";
import { JwtHelperService, JWT_OPTIONS, JwtModule } from "@auth0/angular-jwt";
import { appRouter } from "./app.router";
import { AuthService } from "./services/auth/auth.service";
import { ReversePipe } from "./pipes/reverse/reverse.pipe";
import { SignalRService } from "./services/signalR/signalR.service";
import { LoginComponent } from "./components/login/login.component";
import { LoadingComponent } from "./components/loading/loading.component";
import { PlayerOnlineComponent } from "./components/playeronline/playeronline.component";
import { NavigatorComponent } from "./components/navigator/navigator.component";
import { LoadingService } from "./components/loading/loading.service";
import { PlayerOnlineService } from "./components/playeronline/playeronline.service";

export function getToken() {
    return localStorage.getItem("token");
}

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        LoadingComponent,
        PlayerOnlineComponent,
        NavigatorComponent,
        ReversePipe
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
        SignalRService,
        PlayerOnlineService,
        {provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {duration: 4000, horizontalPosition: "center", verticalPosition: "bottom", panelClass: "snackBar"}}
    ],
    bootstrap: [AppComponent]
})
export class AppModule {}
