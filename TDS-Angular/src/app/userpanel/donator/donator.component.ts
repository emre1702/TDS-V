import { Component, Input } from "@angular/core";
import { UserpanelContentComponent } from "../content/userpanelcontent.component";
import { UserpanelComponent } from "../userpanel.component";

@Component({
  selector: "app-userpanel-donator",
  templateUrl: "./donator.component.html",
})

export class UserpanelDonatorComponent implements UserpanelContentComponent {
  onOpen() {}
  onClose() {}
}