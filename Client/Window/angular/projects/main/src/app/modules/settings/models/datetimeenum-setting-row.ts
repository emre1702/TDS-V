import { SettingRow } from './setting-row';
import { SettingRowSettings } from '../interfaces/setting-row-settings';
import { DatePipe } from '@angular/common';
import { SettingType } from '../../../enums/setting-type';

export class DateTimeEnumSettingRow extends SettingRow {

    enumOriginalKeys = {};
    enumKeys: string[];
    type = SettingType.dateTimeEnum;

    constructor(
            public enumType: {}, 
            settings: SettingRowSettings) {
        super(settings);
        this.enumKeys = this.getEnumKeys(enumType);
    }

    private getEnumKeys(e: {}) {
        const datePipe = new DatePipe('en-US');
        const currentDate = new Date();

        const keys = Object.keys(e).filter(x => !(parseInt(x, 10) >= 0));
        for (const index in keys) {
            const oldValue = keys[index];
            keys[index] = datePipe.transform(currentDate, oldValue);
            this.enumOriginalKeys[keys[index]] = oldValue;
        }
        return keys;
    }
}