import { Component, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { BooleanSliderSettingRow } from 'projects/main/src/app/modules/settings/models/booleanslider-setting-row';
import { NumberSettingRow } from 'projects/main/src/app/modules/settings/models/number-setting-row';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsKillInfo } from '../../interfaces/userpanel-settings-kill-info';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';
import { SettingChangedEvent } from 'projects/main/src/app/modules/settings/interfaces/setting-changed-event';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { SettingsKillInfoIndex } from '../../enums/settings-kill-info-index.enum';
import { KillMessagesService } from '../../../../hud/kill-messages/services/kill-messages.service';

@Component({
    selector: 'app-userpanel-settings-kill-info',
    templateUrl: './userpanel-settings-kill-info.component.html'
})
export class UserpanelSettingsKillInfoComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        new BooleanSliderSettingRow({ 
            defaultValue: this.settings.Settings[7], 
            dataSettingIndex: SettingsKillInfoIndex.KillInfoShowIcon, 
            label: SettingsKillInfoIndex[SettingsKillInfoIndex.KillInfoShowIcon], 
            tooltipLangKey: "KillInfoShowIconInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new NumberSettingRow({ 
            defaultValue: this.settings.Settings[8], 
            dataSettingIndex: SettingsKillInfoIndex.KillInfoFontSize, 
            label: SettingsKillInfoIndex[SettingsKillInfoIndex.KillInfoFontSize], 
            tooltipLangKey: "KillInfoFontWidthInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }, 0, 10, true, false),
        new NumberSettingRow({ 
            defaultValue: this.settings.Settings[9], 
            dataSettingIndex: SettingsKillInfoIndex.KillInfoIconWidth, 
            label: SettingsKillInfoIndex[SettingsKillInfoIndex.KillInfoIconWidth], 
            tooltipLangKey: "KillInfoIconWidthInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }, 0, 1920, true, true),
        new NumberSettingRow({ 
            defaultValue: this.settings.Settings[12], 
            dataSettingIndex: SettingsKillInfoIndex.KillInfoIconHeight, 
            label: SettingsKillInfoIndex[SettingsKillInfoIndex.KillInfoIconHeight], 
            tooltipLangKey: "KillInfoIconHeightInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }, 0, 1920, true, true),
        new NumberSettingRow({ 
            defaultValue: this.settings.Settings[10], 
            dataSettingIndex: SettingsKillInfoIndex.KillInfoSpacing, 
            label: SettingsKillInfoIndex[SettingsKillInfoIndex.KillInfoSpacing], 
            tooltipLangKey: "KillInfoSpacingInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }, -1920, 1920, false, true),
        new NumberSettingRow({ 
            defaultValue: this.settings.Settings[11], 
            dataSettingIndex: SettingsKillInfoIndex.KillInfoDuration, 
            label: SettingsKillInfoIndex[SettingsKillInfoIndex.KillInfoDuration], 
            tooltipLangKey: "KillInfoDurationInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }, 0, 1000, true, false),
        new ButtonRow("TestKillInfo", this.sendTestKillInfo.bind(this)),
    ];

    constructor(
        private userpanelSettingsService: UserpanelSettingsNormalService,
        private settings: SettingsService,
        private killMessagesService: KillMessagesService) {
    }

    ngOnInit() {
        const container = this.getSettingsContainer();
        for (const setting of this.userpanelSettings.filter(s => s instanceof SettingRow) as SettingRow[]) {
            setting.formControl.setValue(container[setting.dataSettingIndex]);
        }
    }

    save(obj: UserpanelSettingsKillInfo) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.KillInfo, obj).subscribe();
    }

    changed(data: SettingChangedEvent) {
        switch (data.index) {
            case SettingsKillInfoIndex.KillInfoShowIcon:
                this.settings.Settings[7] = data.value;
                break;
            case SettingsKillInfoIndex.KillInfoFontSize:
                this.settings.Settings[8] = data.value;
                break;
            case SettingsKillInfoIndex.KillInfoIconWidth:
                this.settings.Settings[9] = data.value;
                break;
            case SettingsKillInfoIndex.KillInfoSpacing:
                this.settings.Settings[10] = data.value;
                break;
            case SettingsKillInfoIndex.KillInfoDuration:
                this.settings.Settings[11] = data.value;
                break;
            case SettingsKillInfoIndex.KillInfoIconHeight:
                this.settings.Settings[12] = data.value;
                break;
        }
        this.settings.KillInfoSettingsChanged.emit(null);
    }

    private sendTestKillInfo() {
        this.killMessagesService.addTestDeathInfo();
    }

    private getSettingsContainer(): UserpanelSettingsKillInfo {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.KillInfo] as UserpanelSettingsKillInfo;
    }
}   