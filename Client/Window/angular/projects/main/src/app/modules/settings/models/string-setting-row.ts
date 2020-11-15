import { SettingRow } from './setting-row';
import { SettingType } from '../../../enums/setting-type';
import { ValidatorFn, Validators } from '@angular/forms';
import { SettingRowSettings } from '../interfaces/setting-row-settings';

export class StringSettingRow extends SettingRow {

    type = SettingType.string;

    constructor(
            settings: SettingRowSettings, 
            minLength?: number,
            maxLength?: number,
            public showing: boolean = false,
            public canToggleShowing: boolean = true) {
        super(settings, StringSettingRow.getFormControlValidators(minLength, maxLength));
    }

    static getFormControlValidators(minLength?: number, maxLength?: number): ValidatorFn[] {
        const validators = [];

        if (minLength != undefined) {
            validators.push(Validators.minLength(minLength));
        }
        if (maxLength != undefined) {
            validators.push(Validators.maxLength(maxLength));
        }
        return validators;
    }
}
