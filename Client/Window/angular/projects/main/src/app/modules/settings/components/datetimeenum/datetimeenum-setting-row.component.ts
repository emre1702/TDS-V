import { Input, Component, Output, EventEmitter } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { DateTimeEnumSettingRow } from '../../models/datetimeenum-setting-row';

@Component({
    selector: 'app-datetimeenum-setting-row',
    templateUrl: './datetimeenum-setting-row.component.html',
    styleUrls: ['./datetimeenum-setting-row.component.scss']
})
export class DateTimeEnumSettingRowComponent {
    @Input() setting: DateTimeEnumSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService) {}
}
