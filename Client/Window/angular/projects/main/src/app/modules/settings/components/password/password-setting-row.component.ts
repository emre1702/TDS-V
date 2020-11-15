import { Input, Component, Output, EventEmitter } from '@angular/core';
import { PasswordSettingRow } from '../../models/password-setting-row';
import { SettingsService } from 'projects/main/src/app/services/settings.service';

@Component({
    selector: 'app-password-setting-row',
    templateUrl: './password-setting-row.component.html',
    styleUrls: ['./password-setting-row.component.scss']
})
export class PasswordSettingRowComponent {
    @Input() setting: PasswordSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService) {}
}
