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
    }

    static openMenu( menu: string ) {
        this.opened[menu] = true;
    }

    static closeMenu( menu: string ) {
        this.opened[menu] = false;
    }

    get opened() {
      return AppComponent.opened;
    }

    get menus() {
      return AppComponent.menus;
    }
}
