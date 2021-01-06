import { NgModule } from '@angular/core';
import { FilterPipe } from './pipes/filter.pipe';
import { LanguagePipe } from './pipes/language.pipe';
import { OrderByPipe } from './pipes/orderby.pipe';
import { ReversePipe } from './pipes/reverse.pipe';
import { SharedModuleComponent } from './shared-module.component';
import { NotificationService } from './services/notification.service';
import { ToolbarDirective } from './directives/toolbarDirective';
import { InputTypeDirective } from './directives/input-type-directive';
import { TextareaTypeDirective } from './directives/textarea-type-directive';
import { SelectGroupDialogComponent } from './dialogs/select-group-dialog/select-group-dialog.component';
import { MaterialModule } from '../material/material.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

const declarations = [
    SharedModuleComponent,
    SelectGroupDialogComponent,
    FilterPipe,
    LanguagePipe,
    OrderByPipe,
    ReversePipe,
    ToolbarDirective,
    InputTypeDirective,
    TextareaTypeDirective,
];

@NgModule({
    declarations: [...declarations],
    exports: [...declarations],
    imports: [MaterialModule, FormsModule, CommonModule],
    providers: [NotificationService],
})
export class SharedModule {}
