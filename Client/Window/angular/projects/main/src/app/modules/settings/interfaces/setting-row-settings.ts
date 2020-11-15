import { SettingRow } from '../models/setting-row';

export interface SettingRowSettings {
    defaultValue: any,
    dataSettingIndex: number,
    label: string,
            
    placeHolder?: string,
    tooltipLangKey?: string,
    onValueChanged?: (setting: SettingRow) => void,
    containerGetter?: () => {},

    readonly?: boolean;
    nullable?: boolean;
}