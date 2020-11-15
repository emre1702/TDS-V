import { NgModule } from "@angular/core";
import { FilterPipe } from './pipes/filter.pipe';
import { LanguagePipe } from './pipes/language.pipe';
import { OrderByPipe } from './pipes/orderby.pipe';
import { ReversePipe } from './pipes/reverse.pipe';
import { SharedModuleComponent } from './shared-module.component';
import { NotificationService } from './services/notification.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
    declarations: [
        SharedModuleComponent,
        
        FilterPipe,
        LanguagePipe,
        OrderByPipe,
        ReversePipe
    ],
    exports: [
        SharedModuleComponent,

        FilterPipe,
        LanguagePipe,
        OrderByPipe,
        ReversePipe
    ],
    imports: [
        MatSnackBarModule
    ],
    providers: [NotificationService]
})
export class SharedModule {

}