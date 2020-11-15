import { SettingRow } from './setting-row';
import { SettingType } from '../../../enums/setting-type';
import { ValidatorFn, Validators } from '@angular/forms';
import { minNumberValidator } from '../validators/min-number.validator';
import { SettingRowSettings } from '../interfaces/setting-row-settings';
import { maxNumberValidator } from '../validators/max-number.validator';

export class NumberSettingRow extends SettingRow {

    type = SettingType.number;

    private static readonly intRegex = /^-?(0|[1-9]\d*)$/;
    private static readonly uintRegex = /^(0|[1-9]\d*)$/;
    private static readonly decimalRegex =  /^-?(0|[1-9]\d*)(\.\d+)?$/;
    private static readonly udecimalRegex =  /^(0|[1-9]\d*)(\.\d+)?$/;

    constructor(
            settings: SettingRowSettings, 
            min?: number,
            max?: number,
            unsigned: boolean = false,
            onlyInt: boolean = false) {
        super(settings, NumberSettingRow.getFormControlValidators(min, max, unsigned, onlyInt));
    }

    static getFormControlValidators(min?: number,
            max?: number,
            unsigned?: boolean,
            onlyInt?: boolean): ValidatorFn[] {
        const validators = [];

        if (min != undefined) {
            validators.push(minNumberValidator(min));
        }
        if (max != undefined) {
            validators.push(maxNumberValidator(max));
        }

        if (unsigned) {
            if (onlyInt) {
                validators.push(Validators.pattern(NumberSettingRow.uintRegex));
            } else {
                validators.push(Validators.pattern(NumberSettingRow.udecimalRegex));
            }
        } else {
            if (onlyInt) {
                validators.push(Validators.pattern(NumberSettingRow.intRegex));
            } else {
                validators.push(Validators.pattern(NumberSettingRow.decimalRegex));
            }
        }
        return validators;
    }
}
