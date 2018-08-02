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
    private entries: ReportUserEntry[] = [];
    private selected: number;

    constructor(private http: HttpClient, private globaldata: GlobalDataService, private loading: LoadingService, private auth: AuthService) { }

    ngOnInit() {
        this.loading.show();
        this.http.get(this.globaldata.apiUrl + "/reports/user", {withCredentials: true, headers: this.auth.getHeaders()}).subscribe((data: ReportUserEntry[]) => {
            this.entries = data;
            console.log(data.length);
            this.loading.hide();
        });
    }

    viewReport() {

    }

    toggleOpenState() {
        this.entries[this.selected].open = !this.entries[this.selected].open;
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
