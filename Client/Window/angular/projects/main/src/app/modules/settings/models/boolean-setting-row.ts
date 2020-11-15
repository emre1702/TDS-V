import { SettingRow } from './setting-row';
import { SettingType } from '../../../enums/setting-type';
import { SettingRowSettings } from '../interfaces/setting-row-settings';

export class BooleanSettingRow extends SettingRow {

    type = SettingType.boolean;

    constructor(settings: SettingRowSettings) {
        super(settings);
    }
}
