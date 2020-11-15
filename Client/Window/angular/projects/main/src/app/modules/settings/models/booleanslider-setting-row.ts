import { SettingRow } from './setting-row';
import { SettingType } from '../../../enums/setting-type';
import { SettingRowSettings } from '../interfaces/setting-row-settings';

export class BooleanSliderSettingRow extends SettingRow {

    type = SettingType.booleanSlider;

    constructor(settings: SettingRowSettings) {
        super(settings);
    }
}
