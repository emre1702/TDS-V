import { FormControl } from '@angular/forms';
import { SettingType } from '../../../enums/setting-type';

export interface UserpanelSettingRow {
    type: SettingType;
    formControl?: FormControl;
    settingObject?: () => object;

    enum?: any;
    defaultValue?: any;
    initialValue?: any;

    onlyInt?: boolean;
    min?: number;
    max?: number;
    nullable?: boolean;
    toggleColorPicker?: boolean;
    step?: number;

    dataSettingIndex?: number;
    title?: string;

    tooltipLangKey?: string;

    onValueChanged?: () => void;
    onClick?: () => void;
}
