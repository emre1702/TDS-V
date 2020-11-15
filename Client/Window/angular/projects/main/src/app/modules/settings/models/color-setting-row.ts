import { SettingRow } from './setting-row';
import { SettingType } from '../../../enums/setting-type';
import { SettingRowSettings } from '../interfaces/setting-row-settings';

export class ColorSettingRow extends SettingRow {

    type = SettingType.color;
    toggleColorPicker = false;

    constructor(settings: SettingRowSettings) {
        super(settings);
    }
}
