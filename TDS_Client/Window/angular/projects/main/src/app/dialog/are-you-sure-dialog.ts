import { Component, ChangeDetectionStrategy } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { SettingsService } from '../services/settings.service';

@Component({
// tslint:disable-next-line: component-selector
  selector: 'are-you-sure-dialog',
  templateUrl: 'are-you-sure-dialog.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
// tslint:disable-next-line: component-class-suffix
export class AreYouSureDialog {

  constructor(
    public settings: SettingsService,
    public dialogRef: MatDialogRef<AreYouSureDialog>) {}
}
