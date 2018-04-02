import { Component, Input } from "@angular/core";

@Component({
  selector: "app-userpanel-rules",
  templateUrl: "./rules.component.html",
  styleUrls: ["./rules.component.css"]
})

export class UserpanelRulesComponent {
    language: {};
    openedMenu = "normal";

    constructor() {}

    changeMenu ( menu: string ) {
        this.openedMenu = menu;
    }
}