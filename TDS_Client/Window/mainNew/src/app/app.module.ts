import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { MapVotingComponent } from './components/mapvoting/mapvoting.component';

import { DragDropModule } from '@angular/cdk/drag-drop';

import {
  MatButtonModule,
  MatToolbarModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule,
  MatFormFieldModule,
  MatInputModule,
  MatSelectModule,
  MatOptionModule,
  MatTableModule,
  MatAutocompleteModule,
  MatSortModule,
  MatDialogModule,
  MatSnackBarModule,
  MatRippleModule,
  MatSliderModule,
  MatExpansionModule,
  MatGridListModule,
  MatCheckboxModule,
  MatMenuModule,
  MatBadgeModule,
  MatTooltipModule,
  MatSlideToggleModule,
  MatButtonToggleModule,
  MatPaginatorModule,
  MatPaginatorIntl
} from "@angular/material";
import { MapVotingNavPipe } from './components/mapvoting/pipes/mapvotingNav.pipe';
import { LanguagePipe } from './pipes/language.pipe';
import { TeamOrdersComponent } from './components/teamorders/teamorders.component';
import { OrderByPipe } from './pipes/orderby.pipe';
import { MapCreatorComponent } from './components/mapcreator/map-creator.component';
import { FreeroamComponent } from './components/freeroam/freeroam.component';
import { LoadMapDialog } from './components/mapcreator/dialog/load-map-dialog';
import { AreYouSureDialog } from './dialog/are-you-sure-dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomLobbyMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby.component';
import { CustomLobbyPasswordDialog } from './components/lobbychoice/dialog/custom-lobby-password-dialog';
import { LobbyChoiceComponent } from './components/lobbychoice/lobby-choice/lobby-choice.component';
import { TeamChoiceComponent } from './components/team-choice/team-choice.component';
import { UserpanelComponent } from './components/userpanel/userpanel.component';
import { UserpanelCommandNavPipe } from './components/userpanel/pipes/userpanelCommandNav.pipe';
import { UserpanelCommandsComponent } from './components/userpanel/userpanel-commands/userpanel-commands.component';
import { UserpanelRulesComponent } from './components/userpanel/userpanel-rules/userpanel-rules.component';
import { UserpanelRulesNavPipe } from './components/userpanel/pipes/userpanelRulesNav.pipe';
import { UserpanelFAQsComponent } from './components/userpanel/userpanel-faqs/userpanel-faqs.component';
import { UserpanelSettingsComponent } from './components/userpanel/userpanel-settings/userpanel-settings.component';
import { UserpanelStatsComponent } from './components/userpanel/userpanel-stats/userpanel-stats.component';
import { MapCreatorObjectChoiceComponent } from './components/mapcreator/map-creator-object-choice/map-creator-object-choice.component';
import { CustomMatPaginatorIntl } from './extensions/customMatPaginatorIntl';

@NgModule({
  declarations: [
    AppComponent,
    MapVotingComponent,
    TeamOrdersComponent,
    MapCreatorComponent,
    FreeroamComponent,
    CustomLobbyMenuComponent,
    OrderByPipe,
    MapVotingNavPipe,
    UserpanelCommandNavPipe,
    UserpanelRulesNavPipe,
    LanguagePipe,
    LoadMapDialog,
    AreYouSureDialog,
    CustomLobbyPasswordDialog,
    LobbyChoiceComponent,
    TeamChoiceComponent,
    UserpanelComponent,
    UserpanelCommandsComponent,
    UserpanelRulesComponent,
    UserpanelFAQsComponent,
    UserpanelSettingsComponent,
    UserpanelStatsComponent,
    MapCreatorObjectChoiceComponent
  ],
  imports: [
    ReactiveFormsModule,
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    DragDropModule,
    MatButtonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    MatOptionModule,
    MatTableModule,
    MatAutocompleteModule,
    MatSortModule,
    MatDialogModule,
    MatRippleModule,
    MatSliderModule,
    MatExpansionModule,
    MatGridListModule,
    MatCheckboxModule,
    MatMenuModule,
    MatBadgeModule,
    MatTooltipModule,
    MatSlideToggleModule,
    MatButtonToggleModule,
    MatPaginatorModule
  ],
  entryComponents: [LoadMapDialog, AreYouSureDialog, CustomLobbyPasswordDialog],
  providers: [
    OrderByPipe,
    { provide: MatPaginatorIntl, useClass: CustomMatPaginatorIntl }
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
