import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { 
  MatButtonModule, 
  MatTabsModule, 
  MatIconModule, 
  MatFormFieldModule, 
  MatToolbarModule, 
  MatCheckboxModule, 
  MatSidenavModule, 
  MatListModule,
  MatMenuModule
} from "@angular/material";
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { AngularDraggableModule } from "angular2-draggable";

import { AppComponent } from "./app.component";
import { UserpanelComponent } from "./userpanel/userpanel.component";
import { UserpanelContentDirective } from "./userpanel/content/userpanelcontent.directive";
import { UserpanelContentService } from "./userpanel/content/userpanelcontent.service";

import { UserpanelAdminComponent } from "./userpanel/admin/admin.component";
import { UserpanelDonatorComponent } from "./userpanel/donator/donator.component";
import { UserpanelReportsComponent } from "./userpanel/reports/reports.component";
import { UserpanelRulesComponent } from "./userpanel/rules/rules.component";
import { UserpanelSettingsComponent } from "./userpanel/settings/settings.component";
import { UserpanelSuggestionsComponent } from "./userpanel/suggestions/suggestions.component";

import { RAGE } from "./rageconnector/rageconnector.service";
import { RAGEModule } from "./rageconnector/rageconnector.module";

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
    NoopAnimationsModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatMenuModule,
    AngularDraggableModule,
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
  providers: [UserpanelContentService],
  bootstrap: [AppComponent]
})
export class AppModule { }
