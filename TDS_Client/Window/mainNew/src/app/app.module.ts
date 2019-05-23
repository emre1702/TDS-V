import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { MapVotingComponent } from './mapvoting/mapvoting.component';

import { DragDropModule } from '@angular/cdk/drag-drop';

import {
  MatButtonModule,
  MatToolbarModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule
} from "@angular/material";
import { MapVotingNavPipe } from './mapvoting/pipes/mapvotingNav.pipe';
import { LanguagePipe } from './pipes/language.pipe';

@NgModule({
  declarations: [
    AppComponent,
    MapVotingComponent,
    MapVotingNavPipe,
    LanguagePipe
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    DragDropModule,
    MatButtonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
