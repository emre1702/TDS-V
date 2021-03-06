import { ChangeDetectorRef, NgModule } from '@angular/core';
import { MatOptionModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatStepperModule } from '@angular/material/stepper';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule, MatPaginatorIntl } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatRippleModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSliderModule } from '@angular/material/slider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CustomMatPaginatorIntl } from './components/custom-mat-paginator-intl.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAppBackgroundDirective } from './directives/mat-app-background.directive';
import { CustomMatSnackBarComponent } from './components/custom-mat-snack-bar.component';
import { ApplyBackgroundService } from './services/apply-background.service';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { MatTabsModule } from '@angular/material/tabs';
import { CommonModule } from '@angular/common';
import { SettingsService } from '../../services/settings.service';
import { MatSelectFocusRemoverDirective } from './directives/mat-select-focus-remover.directive';

@NgModule({
    declarations: [MatAppBackgroundDirective, MatSelectFocusRemoverDirective, CustomMatSnackBarComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
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
        MatStepperModule,
        MatProgressSpinnerModule,
        MatCardModule,
        MatRadioModule,
        DragDropModule,
        MatTabsModule,
        ScrollingModule,
    ],
    exports: [
        ReactiveFormsModule,
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
        MatStepperModule,
        MatProgressSpinnerModule,
        MatCardModule,
        MatRadioModule,
        DragDropModule,
        MatTabsModule,
        ScrollingModule,

        MatAppBackgroundDirective,
        MatSelectFocusRemoverDirective,
    ],
    providers: [ApplyBackgroundService, { provide: MatPaginatorIntl, useClass: CustomMatPaginatorIntl, deps: [SettingsService] }],
})
export class MaterialModule {}
