import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { 
  MatButtonModule, 
  MatTabsModule, 
  MatIconModule, 
  MatFormFieldModule, 
  MatToolbarModule, 
  MatCheckboxModule, 
  MatSidenavModule, 
  MatListModule,
  MatMenuModule,
  MatInputModule,
  MatTooltipModule
} from "@angular/material";
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { AngularDraggableModule } from "angular2-draggable";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

import { AppComponent } from "./app.component";
import { UserpanelComponent } from "./userpanel/userpanel.component";

import { UserpanelAdminComponent } from "./userpanel/admin/admin.component";
import { UserpanelDonatorComponent } from "./userpanel/donator/donator.component";
import { UserpanelReportsComponent } from "./userpanel/reports/reports.component";
import { UserpanelRulesComponent } from "./userpanel/rules/rules.component";
import { UserpanelSettingsComponent } from "./userpanel/settings/settings.component";
import { UserpanelSuggestionsComponent } from "./userpanel/suggestions/suggestions.component";
import { RAGE } from "./rageconnector/rageconnector.service";
import { RAGEModule } from "./rageconnector/rageconnector.module";
import { UserpanelContentDirective } from "./userpanel/content/userpanelcontent.directive";


@NgModule({
  declarations: [
    AppComponent,
    UserpanelComponent,
    UserpanelContentDirective,
    UserpanelAdminComponent,
    UserpanelDonatorComponent,
    UserpanelReportsComponent,
    UserpanelRulesComponent,
    UserpanelSettingsComponent,
    UserpanelSuggestionsComponent
  ],
  imports: [
    BrowserModule,
    MatButtonModule,
    MatTabsModule,
    MatIconModule,
    MatFormFieldModule,
    NoopAnimationsModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatMenuModule,
    MatInputModule,
    AngularDraggableModule,
    MatTooltipModule,
    RAGEModule
  ],
  entryComponents: [
    AppComponent, 
    UserpanelAdminComponent,
    UserpanelDonatorComponent,
    UserpanelReportsComponent,
    UserpanelRulesComponent, 
    UserpanelSettingsComponent,
    UserpanelSuggestionsComponent
  ],
  providers: [RAGE, FormBuilder],
  bootstrap: [AppComponent]
})
export class AppModule { }
