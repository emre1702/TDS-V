import { Component, OnInit } from "@angular/core";
import { LoadingService } from "../loading/loading.service";
import { HttpClient, HttpParams } from "@angular/common/http";
import { ReportUserEntry } from "../../models/reportUserEntry.model";
import { GlobalDataService } from "../../services/globaldata.service";
import { AuthService } from "../../services/auth/auth.service";

@Component({
    selector: "app-reportuser",
    templateUrl: "./reportuser.component.html",
    styleUrls: ["./report.component.css"]
})
export class ReportUserComponent implements OnInit {
    entries: ReportUserEntry[] = [];
    selected: number;

    constructor(private http: HttpClient, public globaldata: GlobalDataService, private loading: LoadingService, private auth: AuthService) { }

    ngOnInit() {
        this.loading.show();
        this.http.get(this.globaldata.apiUrl + "/reports/user", {withCredentials: true, headers: this.auth.getHeaders()}).subscribe((data: ReportUserEntry[]) => {
            this.entries = data;
            console.log(data.length);
            this.loading.hide();
        });
    }

    toggleOpenState() {
        this.entries[this.selected].open = !this.entries[this.selected].open;
        this.http.post(this.globaldata.apiUrl + "/reports/user/toggle_open", null, {params: new HttpParams().set("reportid", this.entries[this.selected].id.toString()), withCredentials: true, headers: this.auth.getHeaders()}).subscribe(() => {});
    }

    viewReport() {

    }

    createReport() {

    }

}
