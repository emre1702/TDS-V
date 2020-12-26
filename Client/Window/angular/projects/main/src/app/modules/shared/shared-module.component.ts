import { Component } from '@angular/core';
import { NotificationService } from './services/notification.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomMatSnackBarComponent } from '../material/components/custom-mat-snack-bar.component';
import { SettingsService } from '../../services/settings.service';
import { LanguagePipe } from './pipes/language.pipe';

@Component({
    selector: 'app-shared-module',
    template: '',
})
export class SharedModuleComponent {
    constructor(notificationService: NotificationService, snackBar: MatSnackBar, settings: SettingsService) {
        const langPipe = new LanguagePipe();

        notificationService.notificationSubscription.subscribe((data) => {
            const msg = langPipe.transform(data.message, settings.Lang);
            snackBar.openFromComponent(CustomMatSnackBarComponent, { data: msg, duration: data.duration });
        });
    }
}
