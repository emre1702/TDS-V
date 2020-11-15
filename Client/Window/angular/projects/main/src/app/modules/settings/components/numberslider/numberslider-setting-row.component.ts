import { Input, Component, Output, EventEmitter } from '@angular/core';
import { NumberSliderSettingRow } from '../../models/numberslider-setting-row';
import { SettingsService } from 'projects/main/src/app/services/settings.service';

@Component({
    selector: 'app-numberslider-setting-row',
    templateUrl: './numberslider-setting-row.component.html',
    styleUrls: ['./numberslider-setting-row.component.scss']
})
export class NumberSliderSettingRowComponent {
    @Input() setting: NumberSliderSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService) {}
}
