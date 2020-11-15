import { SettingRow } from './setting-row';
import { SettingRowSettings } from '../interfaces/setting-row-settings';
import { SettingType } from '../../../enums/setting-type';

export class EnumSettingRow extends SettingRow {

    enumKeys: string[];
    type = SettingType.enum;

    constructor(
            public enumType: {}, 
            settings: SettingRowSettings) {
        super(settings);
        this.enumKeys = this.getEnumKeys(enumType);
    }

    private getEnumKeys(e: {}) {
        return Object.keys(e).filter(x => !(parseInt(x, 10) >= 0));
    }
}