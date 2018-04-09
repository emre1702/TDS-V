import { Component, OnInit } from "@angular/core";
import { UserpanelComponent } from "./userpanel/userpanel.component";
import { RAGE } from "./rageconnector/rageconnector.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit {
    static opened = {
      userpanel: false
    };
    static menus = {
      userpanel: UserpanelComponent
    };

    constructor( private rage: RAGE ) {}

    ngOnInit() {
        AppComponent.openMenu( "userpanel" ); 
        this.rage.Client.listen( () => AppComponent.openMenu( "userpanel" ), "openUserpanel" );
        this.rage.Client.listen( () => AppComponent.openMenu( "userpanel" ), "closeUserpanel" );
        this.rage.Client.listen( AppComponent.syncLanguage, "syncLanguage" ); 

        this.rage.Client.call( {
            fn: "requestAngularBrowserData",
            args: []
        }, ( response: object ) => {
            AppComponent.Settings.adminLvl = response["adminlvl"];
            AppComponent.Settings.myLanguage = response["language"];
        } );
    }

    static openMenu( menu: string ) {
        this.opened[menu] = true;
    }

    static closeMenu( menu: string ) {
        this.opened[menu] = false;
    }

    static syncLanguage( language: "GERMAN" | "ENGLISH" ) {
        this.Settings.myLanguage = language;
    }

    get opened() {
      return AppComponent.opened;
    }

    get menus() {
      return AppComponent.menus;
    }

    public static Settings: AppSettings = {
        myLanguage: "ENGLISH",
        adminLvl: 4   // TODO: DEBUG!
    };
}

export interface AppSettings {
    myLanguage: "GERMAN" | "ENGLISH";
    adminLvl: number;
}
