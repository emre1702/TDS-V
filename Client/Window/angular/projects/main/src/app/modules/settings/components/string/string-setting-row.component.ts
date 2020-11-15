import { Input, Component, Output, EventEmitter } from '@angular/core';
import { StringSettingRow } from '../../models/string-setting-row';
import { SettingsService } from 'projects/main/src/app/services/settings.service';

@Component({
    selector: 'app-string-setting-row',
    templateUrl: './string-setting-row.component.html',
    styleUrls: ['./string-setting-row.component.scss']
})
export class StringSettingRowComponent {
    @Input() setting: StringSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService) {}
}
