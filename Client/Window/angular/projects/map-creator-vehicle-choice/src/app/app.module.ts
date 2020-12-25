import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { LanguagePipe } from './pipes/language.pipe';
import { InputTypeDirective } from './extensions/inputTypeDirective';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MaterialCssVarsModule } from 'angular-material-css-vars';

@NgModule({
    declarations: [AppComponent, LanguagePipe, InputTypeDirective],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MatToolbarModule,
        MatFormFieldModule,
        MatTableModule,
        MatPaginatorModule,
        MatInputModule,
        DragDropModule,

        MaterialCssVarsModule.forRoot({
            isAutoContrast: true,
            isDarkTheme: true,
            darkThemeClass: 'isDarkTheme',
            lightThemeClass: 'isLightTheme',
            primary: 'rgba(0,0,77,1)',
            accent: 'rgb(255,152,0)',
            warn: 'rgba(244,67,54,1)',
        }),
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}
