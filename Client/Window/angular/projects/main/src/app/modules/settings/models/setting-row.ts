import { SettingType } from '../../../enums/setting-type';
import { FormControl, ValidatorFn, Validators } from '@angular/forms';
import { SettingRowSettings } from '../interfaces/setting-row-settings';

export class SettingRow {
    type: SettingType;
    formControl: FormControl;
    
    defaultValue: any;
    dataSettingIndex: number;
    label: string;
    
    placeHolder?: string;
    tooltipLangKey?: string;
    onValueChanged?: (setting: SettingRow) => void;
    containerGetter?: () => {};
    condition?: () => boolean;

    readonly: boolean = false;
    nullable: boolean = false;

    constructor(settings: SettingRowSettings, validators: ValidatorFn[] = []) {
        this.defaultValue = settings.defaultValue;
        this.dataSettingIndex = settings.dataSettingIndex;
        this.label = settings.label;
        this.placeHolder = settings.placeHolder;
        this.tooltipLangKey = settings.tooltipLangKey;
        this.onValueChanged = settings.onValueChanged;
        this.containerGetter = settings.containerGetter;
        this.condition = settings.condition;

        if (settings.readonly != undefined) {
            this.readonly = settings.readonly;
        }
        if (settings.nullable != undefined) {
            this.nullable = settings.nullable;
        }

        if (!this.nullable) {
            validators.push(Validators.required);
        }

        this.formControl = new FormControl(this.defaultValue, !this.readonly ? validators : []);
    }
}
