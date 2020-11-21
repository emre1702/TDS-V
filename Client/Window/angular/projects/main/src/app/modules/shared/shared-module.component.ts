import { Component } from '@angular/core';
import { NotificationService } from './services/notification.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomMatSnackBarComponent } from '../material/components/custom-mat-snack-bar.component';

@Component({
    selector: "app-shared-module",
    template: ""
})
export class SharedModuleComponent {

    constructor(
            notificationService: NotificationService,
            snackBar: MatSnackBar) {
        notificationService.notificationSubscription.subscribe(data => {
            snackBar.openFromComponent(CustomMatSnackBarComponent, { data: data.message, duration: data.duration });
        });
    }
}