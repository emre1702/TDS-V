import { ReportAdminComponent } from "./reportadmin.component";
import { ReportUserComponent } from "./reportuser.component";
import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { HomeModule } from "../home/home.module";

const routes: Routes = [
    { path: "user", component: ReportUserComponent },
    { path: "admin", component: ReportAdminComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ReportRouter {}


