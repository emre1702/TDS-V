import { Component, Inject, ChangeDetectionStrategy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SettingsService } from '../../../services/settings.service';
import { LoadMapDialogGroupDto } from '../models/LoadMapDialogGroupDto';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'load-map-dialog',
    templateUrl: 'load-map-dialog.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
// tslint:disable-next-line: component-class-suffix
export class LoadMapDialog {

    chosenMapId: number;

    constructor(
        public settings: SettingsService,
        public dialogRef: MatDialogRef<LoadMapDialog>,
        @Inject(MAT_DIALOG_DATA) public data: LoadMapDialogGroupDto[]) { }

    cancel(): void {
        this.dialogRef.close();
    }

}
