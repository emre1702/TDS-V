import { Input, Component, Output, EventEmitter } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { BooleanSettingRow } from '../../models/boolean-setting-row';

@Component({
    selector: 'app-boolean-setting-row',
    templateUrl: './boolean-setting-row.component.html',
    styleUrls: ['./boolean-setting-row.component.scss']
})
export class BooleanSettingRowComponent {
    @Input() setting: BooleanSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService) {}
}
