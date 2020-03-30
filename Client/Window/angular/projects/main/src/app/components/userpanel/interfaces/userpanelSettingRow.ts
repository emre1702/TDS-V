import { FormControl } from '@angular/forms';
import { SettingType } from '../../../enums/setting-type';

export interface UserpanelSettingRow {
    type: SettingType;
    formControl: FormControl;

    enum?: any;
    defaultValue: any;
    initialValue?: any;

    onlyInt?: boolean;
    min?: number;
    max?: number;
    nullable: boolean;

    dataSettingIndex: number;

    tooltipLangKey?: string;

    onValueChanged?: () => void;
}
