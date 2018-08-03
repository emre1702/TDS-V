import { Component, OnInit } from "@angular/core";
import { GlobalDataService } from "../shared/globaldata.service";
import { LoadingService } from "../loading/loading.service";
import { HttpClient, HttpParams } from "@angular/common/http";
import { AuthService } from "../auth/auth.service";

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

class ReportUserEntry {
    public id: number;
    public author: string;
    public title: string;
    public amountanswers: number;
    public foradminlvl: number;
    public open: boolean;
}
