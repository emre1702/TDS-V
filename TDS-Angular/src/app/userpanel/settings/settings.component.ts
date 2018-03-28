import { Component, Input } from "@angular/core";
import { UserpanelContentComponent } from "../content/userpanelcontent.component";
import { UserpanelComponent } from "../userpanel.component";

@Component({
  selector: "app-userpanel-settings",
  templateUrl: "./settings.component.html",
})

export class UserpanelSettingsComponent implements UserpanelContentComponent {
    onOpen() {}
    onClose() {}
}