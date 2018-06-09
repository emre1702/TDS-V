import { NgModule } from "@angular/core";
import { LogsComponent } from "./logs.component";
import { CommonModule } from "@angular/common";
import { LogsRouter } from "./logs.router";
import { MatOptionModule, MatSelectModule } from "@angular/material";

@NgModule({
    declarations: [LogsComponent],
    imports: [LogsRouter, CommonModule, MatSelectModule, MatOptionModule],
})
export class LogsModule {}
