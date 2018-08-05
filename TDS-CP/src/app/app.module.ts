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
    MatMenuModule,
    MatSidenavModule
 } from "@angular/material";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { GlobalDataService } from "./services/globaldata.service";
import { ReactiveFormsModule } from "@angular/forms";
import { AngularDraggableModule } from "angular2-draggable";
import { JwtHelperService, JwtModule } from "@auth0/angular-jwt";
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
import { ChatComponent } from "./components/chat/chat.component";
import { HttpClientModule } from "../../node_modules/@angular/common/http";
import { ChatService } from "./components/chat/chat.service";

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
        ReversePipe,
        ChatComponent
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
        MatMenuModule,
        MatSidenavModule
    ],
    providers: [
        GlobalDataService,
        JwtHelperService,
        LoadingService,
        AuthService,
        SignalRService,
        PlayerOnlineService,
        ChatService,
        {provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {duration: 4000, horizontalPosition: "center", verticalPosition: "bottom", panelClass: "snackBar"}}
    ],
    bootstrap: [AppComponent]
})
export class AppModule {}
