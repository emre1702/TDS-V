import { Component, Input } from "@angular/core";
import { UserpanelContentComponent } from "../content/userpanelcontent.component";
import { UserpanelComponent } from "../userpanel.component";

@Component({
  selector: "app-userpanel-suggestions",
  templateUrl: "./suggestions.component.html",
})

export class UserpanelSuggestionsComponent implements UserpanelContentComponent {
    onOpen() {}
    onClose() {}
}