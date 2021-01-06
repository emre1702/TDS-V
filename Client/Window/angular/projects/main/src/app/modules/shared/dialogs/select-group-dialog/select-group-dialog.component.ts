import { Component, Inject } from '@angular/core';
import { SettingsService } from '../../../../services/settings.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SelectGroupDialogData } from './models/select-group-dialog-data';

@Component({
    templateUrl: 'select-group-dialog.component.html',
})
export class SelectGroupDialogComponent {
    selectedValue: number;

    constructor(public settings: SettingsService, @Inject(MAT_DIALOG_DATA) public data: SelectGroupDialogData) {}
}
