import { Component, ChangeDetectionStrategy } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'application-invite-dialog',
    templateUrl: 'application-invite-dialog.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
// tslint:disable-next-line: component-class-suffix
export class ApplicationInviteDialog {
    message = "";

    constructor(
        public settings: SettingsService,
        public dialogRef: MatDialogRef<ApplicationInviteDialog>) { }
}
