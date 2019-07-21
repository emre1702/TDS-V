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
  MatCheckboxModule
} from "@angular/material";
import { MapVotingNavPipe } from './components/mapvoting/pipes/mapvotingNav.pipe';
import { LanguagePipe } from './pipes/language.pipe';
import { TeamOrdersComponent } from './components/teamorders/teamorders.component';
import { OrderByPipe } from './pipes/orderby.pipe';
import { MapCreatorComponent } from './components/mapcreator/map-creator.component';
import { FreeroamComponent } from './components/freeroam/freeroam.component';
import { LoadMapDialog } from './components/mapcreator/loadmapdialog/load-map-dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomLobbyMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby.component';
import { CustomLobbyPasswordDialog } from './components/lobbychoice/dialog/custom-lobby-password-dialog';
import { LobbyChoiceComponent } from './components/lobbychoice/lobby-choice/lobby-choice.component';
import { TeamChoiceComponent } from './team-choice/team-choice.component';

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
    LanguagePipe,
    LoadMapDialog,
    CustomLobbyPasswordDialog,
    LobbyChoiceComponent,
    TeamChoiceComponent
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
    MatCheckboxModule
  ],
  entryComponents: [LoadMapDialog, CustomLobbyPasswordDialog],
  providers: [OrderByPipe],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
