import { Input, Component, Output, EventEmitter } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { BooleanSliderSettingRow } from '../../models/booleanslider-setting-row';

@Component({
    selector: 'app-booleanslider-setting-row',
    templateUrl: './booleanslider-setting-row.component.html',
    styleUrls: ['./booleanslider-setting-row.component.scss']
})
export class BooleanSliderSettingRowComponent {
    @Input() setting: BooleanSliderSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService) {}
}
