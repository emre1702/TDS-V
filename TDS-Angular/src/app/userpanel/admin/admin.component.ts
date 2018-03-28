import { Component, Input } from "@angular/core";
import { UserpanelContentComponent } from "../content/userpanelcontent.component";
import { UserpanelComponent } from "../userpanel.component";

@Component({
  selector: "app-userpanel-admin",
  templateUrl: "./admin.component.html",
})

export class UserpanelAdminComponent implements UserpanelContentComponent {
    private language;

    onOpen() {}
    onClose() {}
}