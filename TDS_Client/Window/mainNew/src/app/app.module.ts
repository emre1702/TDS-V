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
  MatOptionModule
} from "@angular/material";
import { MapVotingNavPipe } from './components/mapvoting/pipes/mapvotingNav.pipe';
import { LanguagePipe } from './pipes/language.pipe';
import { TeamOrdersComponent } from './components/teamorders/teamorders.component';
import { OrderByPipe } from './pipes/orderby.pipe';
import { MapCreatorComponent } from './components/mapcreator/map-creator.component';

@NgModule({
  declarations: [
    AppComponent,
    MapVotingComponent,
    TeamOrdersComponent,
    MapCreatorComponent,
    OrderByPipe,
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
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule
  ],
  providers: [OrderByPipe],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
