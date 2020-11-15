import { Input, Component, Output, EventEmitter } from '@angular/core';
import { EnumSettingRow } from '../../models/enum-setting-row';
import { SettingsService } from 'projects/main/src/app/services/settings.service';

@Component({
    selector: 'app-enum-setting-row',
    templateUrl: './enum-setting-row.component.html',
    styleUrls: ['./enum-setting-row.component.scss']
})
export class EnumSettingRowComponent {
    @Input() setting: EnumSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService) {}
}
