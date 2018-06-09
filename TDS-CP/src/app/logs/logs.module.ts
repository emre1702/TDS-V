import { NgModule } from "@angular/core";
import { LogsComponent } from "./logs.component";
import { CommonModule } from "@angular/common";
import { LogsRouter } from "./logs.router";
import { MatOptionModule, MatSelectModule, MatInputModule, MatPaginatorModule } from "@angular/material";

@NgModule({
    declarations: [LogsComponent],
    imports: [LogsRouter, CommonModule, MatSelectModule, MatOptionModule, MatInputModule, MatPaginatorModule],
})
export class LogsModule {}
