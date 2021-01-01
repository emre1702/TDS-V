import { NgModule } from '@angular/core';
import { FilterPipe } from './pipes/filter.pipe';
import { LanguagePipe } from './pipes/language.pipe';
import { OrderByPipe } from './pipes/orderby.pipe';
import { ReversePipe } from './pipes/reverse.pipe';
import { SharedModuleComponent } from './shared-module.component';
import { NotificationService } from './services/notification.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ToolbarDirective } from './directives/toolbarDirective';
import { InputTypeDirective } from './directives/input-type-directive';
import { TextareaTypeDirective } from './directives/textarea-type-directive';

const declarations = [SharedModuleComponent, FilterPipe, LanguagePipe, OrderByPipe, ReversePipe, ToolbarDirective, InputTypeDirective, TextareaTypeDirective];

@NgModule({
    declarations: [...declarations],
    exports: [...declarations],
    imports: [MatSnackBarModule],
    providers: [NotificationService],
})
export class SharedModule {}
