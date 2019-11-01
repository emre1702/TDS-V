import { Component, Inject, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SettingsService } from '../../../services/settings.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'custom-lobby-password-dialog',
  templateUrl: 'custom-lobby-password-dialog.html',
})
// tslint:disable-next-line: component-class-suffix
export class CustomLobbyPasswordDialog {
  input = "";

  constructor(
    public dialogRef: MatDialogRef<CustomLobbyPasswordDialog>,
    @Inject(MAT_DIALOG_DATA) public password: string, private changeDetector: ChangeDetectorRef, public settings: SettingsService) { }

  onCancelClick(): void {
    this.dialogRef.close();
    this.changeDetector.detectChanges();
  }

  isPasswordCorrectGetPasswort() {
    if (this.password !== this.input) {
        return false;
    }
    return this.password;
  }
}
