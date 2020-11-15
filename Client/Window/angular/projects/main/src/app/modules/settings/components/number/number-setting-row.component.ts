import { Input, Component, Output, EventEmitter } from '@angular/core';
import { NumberSettingRow } from '../../models/number-setting-row';
import { SettingsService } from 'projects/main/src/app/services/settings.service';

@Component({
    selector: 'app-number-setting-row',
    templateUrl: './number-setting-row.component.html',
    styleUrls: ['./number-setting-row.component.scss']
})
export class NumberSettingRowComponent {
    @Input() setting: NumberSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService) {}
}
