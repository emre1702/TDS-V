import { Component, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { BooleanSliderSettingRow } from 'projects/main/src/app/modules/settings/models/booleanslider-setting-row';
import { SettingsInfoIndex } from '../../enums/settings-info-index.enum';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsInfo } from '../../interfaces/userpanel-settings-info';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';

@Component({
    selector: 'app-userpanel-settings-info',
    templateUrl: './userpanel-settings-info.component.html'
})
export class UserpanelSettingsInfoComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        // Info //
        new BooleanSliderSettingRow({ 
            defaultValue: true, 
            dataSettingIndex: SettingsInfoIndex.ShowCursorInfo, 
            label: SettingsInfoIndex[SettingsInfoIndex.ShowCursorInfo], 
            tooltipLangKey: "ShowCursorInfoInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: true, 
            dataSettingIndex: SettingsInfoIndex.ShowLobbyLeaveInfo, 
            label: SettingsInfoIndex[SettingsInfoIndex.ShowLobbyLeaveInfo], 
            tooltipLangKey: "ShowLobbyLeaveInfoInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }),
    ];

    constructor(
        private userpanelSettingsService: UserpanelSettingsNormalService) {
    }

    ngOnInit() {
        const container = this.getSettingsContainer();
        for (const setting of this.userpanelSettings.filter(s => s instanceof SettingRow) as SettingRow[]) {
            setting.formControl.setValue(container[setting.dataSettingIndex]);
        }
    }

    save(obj: UserpanelSettingsInfo) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.Info, obj).subscribe();
    }

    private getSettingsContainer(): UserpanelSettingsInfo {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.Info] as UserpanelSettingsInfo;
    }
}   