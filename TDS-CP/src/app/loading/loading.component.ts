import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { LoadingService } from "./loading.service";

@Component({
    selector: "app-loading",
    templateUrl: "./loading.component.html",
    styleUrls: ["./loading.component.css"]
})
export class LoadingComponent implements OnInit {
    @ViewChild("spinner") spinner: ElementRef;
    activated = false;

    constructor(private loadingService: LoadingService) { }

    ngOnInit() {
        this.loadingService.change.addListener("toggle", isActivated => {
            this.activated = isActivated;
        });
    }

}
