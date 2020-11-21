import { Component, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { NumberSettingRow } from 'projects/main/src/app/modules/settings/models/number-setting-row';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';
import { SettingsCooldownsAndDurationsIndex } from '../../enums/settings-cooldowns-and-durations-index.enum';
import { UserpanelSettingsCooldownsAndDurations } from '../../interfaces/userpanel-settings-cooldowns-and-durations';

@Component({
    selector: 'app-userpanel-settings-cooldowns-and-durations',
    templateUrl: './userpanel-settings-cooldowns-and-durations.component.html'
})
export class UserpanelSettingsCooldownsAndDurationsComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        // CooldownsAndDurations //
        new NumberSettingRow({ 
            defaultValue: 150, dataSettingIndex: SettingsCooldownsAndDurationsIndex.BloodscreenCooldownMs, 
            label: SettingsCooldownsAndDurationsIndex[SettingsCooldownsAndDurationsIndex.BloodscreenCooldownMs], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "BloodscreenCooldownMsSettingInfo"},
        0, 1000000, true, true),
        new NumberSettingRow({ 
            defaultValue: 100, dataSettingIndex: SettingsCooldownsAndDurationsIndex.HudAmmoUpdateCooldownMs, 
            label: SettingsCooldownsAndDurationsIndex[SettingsCooldownsAndDurationsIndex.HudAmmoUpdateCooldownMs], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "HudAmmoUpdateCooldownMsSettingInfo"},
        -1, 1000000, false, true),
        new NumberSettingRow({ 
            defaultValue: 100, dataSettingIndex: SettingsCooldownsAndDurationsIndex.HudHealthUpdateCooldownMs, 
            label: SettingsCooldownsAndDurationsIndex[SettingsCooldownsAndDurationsIndex.HudHealthUpdateCooldownMs], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "HudHealthUpdateCooldownMsSettingInfo"},
        -1, 1000000, false, true),
        new NumberSettingRow({ 
            defaultValue: 25, dataSettingIndex: SettingsCooldownsAndDurationsIndex.AFKKickAfterSeconds, 
            label: SettingsCooldownsAndDurationsIndex[SettingsCooldownsAndDurationsIndex.AFKKickAfterSeconds], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "AFKKickAfterSecondsSettingInfo"},
        0, 1000000, true, true),
        new NumberSettingRow({ 
            defaultValue: 10, dataSettingIndex: SettingsCooldownsAndDurationsIndex.AFKKickShowWarningLastSeconds, 
            label: SettingsCooldownsAndDurationsIndex[SettingsCooldownsAndDurationsIndex.AFKKickShowWarningLastSeconds], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "AFKKickShowWarningLastSecondsSettingInfo"},
        0, 1000000, true, true),
        new NumberSettingRow({ 
            defaultValue: 1000, dataSettingIndex: SettingsCooldownsAndDurationsIndex.ShowFloatingDamageInfoDurationMs, 
            label: SettingsCooldownsAndDurationsIndex[SettingsCooldownsAndDurationsIndex.ShowFloatingDamageInfoDurationMs], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "ShowFloatingDamageInfoDurationMsSettingInfo"},
        0, 1000000, true, true),
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

    save(obj: UserpanelSettingsCooldownsAndDurations) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.CooldownsAndDurations, obj).subscribe();
    }

    private getSettingsContainer(): UserpanelSettingsCooldownsAndDurations {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.CooldownsAndDurations] as UserpanelSettingsCooldownsAndDurations;
    }
}   