import { Component, Inject } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { LoadMapDialogGroupDto } from '../models/LoadMapDialogGroupDto';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'load-map-dialog',
    templateUrl: 'load-map-dialog.html'
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
