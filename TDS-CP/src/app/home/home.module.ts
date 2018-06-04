import { NgModule } from "@angular/core";
import { HomeComponent } from "./home.component";
import { CommonModule } from "@angular/common";
import { HomeRouter } from "./home.router";

@NgModule({
    declarations: [HomeComponent],
    imports: [HomeRouter, CommonModule],
})
export class HomeModule {}
