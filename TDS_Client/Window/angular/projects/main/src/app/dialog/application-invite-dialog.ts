import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { SettingsService } from '../services/settings.service';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'application-invite-dialog',
    templateUrl: 'application-invite-dialog.html'
})
// tslint:disable-next-line: component-class-suffix
export class ApplicationInviteDialog {
    message = "";

    constructor(
        public settings: SettingsService,
        public dialogRef: MatDialogRef<ApplicationInviteDialog>) { }
}
