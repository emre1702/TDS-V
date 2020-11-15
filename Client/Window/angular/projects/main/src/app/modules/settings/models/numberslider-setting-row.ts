import { SettingRow } from './setting-row';
import { SettingType } from '../../../enums/setting-type';
import { ValidatorFn } from '@angular/forms';
import { minNumberValidator } from '../validators/min-number.validator';
import { SettingRowSettings } from '../interfaces/setting-row-settings';

export class NumberSliderSettingRow extends SettingRow {

    type = SettingType.numberSlider;

    constructor(
            settings: SettingRowSettings, 
            public min?: number,
            public max?: number,
            public step: number = 1) {
        super(settings, NumberSliderSettingRow.getFormControlValidators(min, max));
    }

    static getFormControlValidators(min?: number, max?: number): ValidatorFn[] {
        const validators = [];

        if (min != undefined) {
            validators.push(minNumberValidator(min));
        }
        if (max != undefined) {
            validators.push(minNumberValidator(max));
        }

        return validators;
    }
}
