import { SettingType } from '../../../enums/setting-type';

export class ButtonRow {
    type = SettingType.button;

    constructor(
        public label: string,

        public onClick: () => void
    ) {}
}