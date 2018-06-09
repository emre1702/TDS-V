import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { GlobalDataService } from "../shared/globaldata.service";
import { HttpClient, HttpParams } from "@angular/common/http";
import { AuthService } from "../auth/auth.service";
import { LoadingService } from "../loading/loading.service";
import { MatPaginator, MatInput } from "@angular/material";

@Component({
    selector: "app-logs",
    templateUrl: "./logs.component.html",
    styleUrls: ["./logs.component.css"],
})
export class LogsComponent {
    @ViewChild("paginator") paginator: MatPaginator;
    @ViewChild("nameOnlyInput") nameOnlyInput: ElementRef;
    @ViewChild("targetOnlyInput") targetOnlyInput: ElementRef;
    @ViewChild("lobbyOnlyInput") lobbyOnlyInput: ElementRef;
    public entries: LogEntry[] = [];
    private page = 0;
    private amountpages = 0;
    private amountrows: number;
    private lasttype;
    private lastnameonly: string;
    private lasttargetonly: string;
    private lastlobbyonly: string;

    constructor(private http: HttpClient, private auth: AuthService, private loading: LoadingService, private globaldata: GlobalDataService) { }

    loadEntries(logentries: LogEntry[]) {
        this.entries = logentries;
        this.loading.hide();
    }

    resetSearch() {
        this.amountrows = undefined;
        this.page = 0;
    }

    getAdditionalWhere(params: HttpParams): HttpParams {
        if (this.nameOnlyInput.nativeElement.value) {
            params = params.set("onlyname", this.nameOnlyInput.nativeElement.value);
            if (this.nameOnlyInput.nativeElement.value !== this.lastnameonly) {
                this.resetSearch();
                this.lastnameonly = this.nameOnlyInput.nativeElement.value;
            }
        }
        if (this.targetOnlyInput.nativeElement.value) {
            params = params.set("onlytarget", this.targetOnlyInput.nativeElement.value);
            if (this.nameOnlyInput.nativeElement.value !== this.lasttargetonly) {
                this.resetSearch();
                this.lasttargetonly = this.nameOnlyInput.nativeElement.value;
            }
        }
        if (this.lobbyOnlyInput.nativeElement.value) {
            params = params.set("onlylobby", this.lobbyOnlyInput.nativeElement.value);
            if (this.nameOnlyInput.nativeElement.value !== this.lastlobbyonly) {
                this.resetSearch();
                this.lastlobbyonly = this.nameOnlyInput.nativeElement.value;
            }
        }
        return params;
    }

    loadAmountRows(params: HttpParams) {
        this.http.get(this.globaldata.apiUrl + "/Logs/logs/amountrows", {params: params, withCredentials: true, headers: this.auth.getHeaders()}).subscribe((amountrows: number) => {
            this.amountrows = amountrows;
            this.amountpages = Math.floor(this.amountrows / this.globaldata.showLogEntriesPerPage) + 1;
            if (this.page >= this.amountpages) {
                this.page = this.amountpages - 1;
            }
            this.loading.hide();
        });
    }

    requestLogEntries(type: number) {
        if (type !== undefined) {
            this.loading.show();
            if (this.lasttype !== type) {
                this.resetSearch();
                this.lasttype = type;
            } else {
                console.log(this.paginator.pageIndex);
                this.page = this.paginator.pageIndex;
            }
            let params = new HttpParams().set("type", type.toString());
            params = this.getAdditionalWhere(params);
            if (!this.amountrows) {
                this.loadAmountRows(params);
            } else {
                params = params.set("page", this.page.toString());
                this.http.get(this.globaldata.apiUrl + "/Logs/logs", {params: params, withCredentials: true, headers: this.auth.getHeaders()}).subscribe(this.loadEntries.bind(this));
            }
        }
    }

}

class LogEntry {
    public id: number;
    public name: string;
    public target: string;
    public type: number;
    public lobby: string;
    public date: string;
}
