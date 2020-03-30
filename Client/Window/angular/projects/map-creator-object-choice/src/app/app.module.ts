import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { LanguagePipe } from './pipes/language.pipe';
import { MatToolbarModule, MatFormFieldModule, MatTableModule, MatPaginatorModule, MatInputModule } from '@angular/material';
import 'hammerjs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { InputTypeDirective } from './extensions/inputTypeDirective';

@NgModule({
  declarations: [
    AppComponent,
    LanguagePipe,
    InputTypeDirective
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    DragDropModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
