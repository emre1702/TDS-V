import { NgModule } from "@angular/core";
import { ReportAdminComponent } from "./reportadmin.component";
import { ReportUserComponent } from "./reportuser.component";
import { CommonModule } from "@angular/common";
import { ReportRouter } from "./report.router";
import { MatOptionModule, MatSelectModule, MatInputModule, MatPaginatorModule } from "@angular/material";

@NgModule({
    declarations: [ReportAdminComponent, ReportUserComponent],
    imports: [ReportRouter, CommonModule, MatSelectModule, MatOptionModule, MatInputModule, MatPaginatorModule],
})
export class ReportModule {}
