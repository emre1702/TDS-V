import { FormControl } from '@angular/forms';
import { SettingType } from '../../../enums/setting-type';

export interface UserpanelSettingRow {
    type: SettingType;
    formControl: FormControl;

    enum?: any;
    defaultValue: any;
    initialValue?: any;
    placeholder?: string;

    onlyInt?: boolean;
    min?: number;
    max?: number;

    dataSettingIndex: string;

    onValueChanged?: () => void;
}
