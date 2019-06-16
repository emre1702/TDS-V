import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SettingsService } from 'src/app/services/settings.service';

@Component({
// tslint:disable-next-line: component-selector
  selector: 'load-map-dialog',
  templateUrl: 'load-map-dialog.html'
})
// tslint:disable-next-line: component-class-suffix
export class LoadMapDialog {

  chosenMap: string;

  constructor(
    public settings: SettingsService,
    public dialogRef: MatDialogRef<LoadMapDialog>,
    @Inject(MAT_DIALOG_DATA) public data: string[]) {}

  cancel(): void {
    this.dialogRef.close();
  }

}
