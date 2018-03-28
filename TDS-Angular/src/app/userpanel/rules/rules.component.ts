import { Component, Input, ChangeDetectorRef } from "@angular/core";
import { UserpanelComponent } from "../userpanel.component";
import { RAGE } from "../../rageconnector/rageconnector.service";
import { UserpanelContentComponent } from "../content/userpanelcontent.component";
import { UserpanelContentService } from "../content/userpanelcontent.service";

@Component({
  selector: "app-userpanel-rules",
  templateUrl: "./rules.component.html",
  styleUrls: ["./rules.component.css"]
})

export class UserpanelRulesComponent implements UserpanelContentComponent {
    opened = false;
    language = {};
    openedMenu = "normal";

    constructor( private rage: RAGE, private userpanelContentService: UserpanelContentService, private cdRef: ChangeDetectorRef ) {}

    onOpen() {
        if ( this.language === {} )
            this.language = this.userpanelContentService.getLang( "rules" );
        this.opened = true;
        this.cdRef.detectChanges();
    }

    onClose() {
        this.opened = false;
        this.cdRef.detectChanges();
    }

    changeMenu ( menu: string ) {
        this.openedMenu = menu;
    }
}