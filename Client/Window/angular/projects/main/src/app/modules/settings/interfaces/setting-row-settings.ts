import { SettingRow } from '../models/setting-row';

export interface SettingRowSettings {
    defaultValue: any,
    dataSettingIndex: number,
    label: string,
            
    placeHolder?: string,
    tooltipLangKey?: string,
    onValueChanged?: (setting: SettingRow) => void,
    containerGetter?: () => {},
    condition?: () => boolean;

    readonly?: boolean;
    nullable?: boolean;
}