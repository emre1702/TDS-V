import { Component, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { ColorSettingRow } from 'projects/main/src/app/modules/settings/models/color-setting-row';
import { SettingsIngameColorsIndex } from '../../enums/settings-ingame-colors-index.enum';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsIngameColors } from '../../interfaces/userpanel-settings-ingame-colors';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';
import { SettingChangedEvent } from 'projects/main/src/app/modules/settings/interfaces/setting-changed-event';
import { DToClientEvent } from 'projects/main/src/app/enums/dtoclientevent.enum';
import { RageConnectorService } from 'rage-connector';
import { SettingAtClientside } from '../../enums/setting-index-at-clientside';

@Component({
    selector: 'app-userpanel-settings-ingame-colors',
    templateUrl: './userpanel-settings-ingame-colors.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserpanelSettingsIngameColorsComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        // IngameColors //
        new ColorSettingRow({
            defaultValue: 'rgba(150,0,0,0.35)', dataSettingIndex: SettingsIngameColorsIndex.MapBorderColor, 
            label: SettingsIngameColorsIndex[SettingsIngameColorsIndex.MapBorderColor], 
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new ColorSettingRow({
            defaultValue: 'rgba(0, 0, 0, 1)', dataSettingIndex: SettingsIngameColorsIndex.NametagDeadColor, 
            label: SettingsIngameColorsIndex[SettingsIngameColorsIndex.NametagDeadColor], 
            tooltipLangKey: "NametagDeadColorSettingInfo",
            containerGetter: this.getSettingsContainer.bind(this),
            nullable: true
        }),
        new ColorSettingRow({
            defaultValue: 'rgba(50, 0, 0, 1)', dataSettingIndex: SettingsIngameColorsIndex.NametagHealthEmptyColor, 
            label: SettingsIngameColorsIndex[SettingsIngameColorsIndex.NametagHealthEmptyColor], 
            tooltipLangKey: "NametagHealthEmptyColorSettingInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new ColorSettingRow({
            defaultValue: 'rgba(0, 255, 0, 1)', dataSettingIndex: SettingsIngameColorsIndex.NametagHealthFullColor, 
            label: SettingsIngameColorsIndex[SettingsIngameColorsIndex.NametagHealthFullColor], 
            tooltipLangKey: "NametagHealthFullColorSettingInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new ColorSettingRow({
            defaultValue: undefined, dataSettingIndex: SettingsIngameColorsIndex.NametagArmorEmptyColor, 
            label: SettingsIngameColorsIndex[SettingsIngameColorsIndex.NametagArmorEmptyColor], 
            tooltipLangKey: "NametagArmorEmptyColorSettingInfo",
            containerGetter: this.getSettingsContainer.bind(this),
            nullable: true
        }),
        new ColorSettingRow({
            defaultValue: 'rgba(255, 255, 255, 1)', dataSettingIndex: SettingsIngameColorsIndex.NametagArmorFullColor, 
            label: SettingsIngameColorsIndex[SettingsIngameColorsIndex.NametagArmorFullColor], 
            tooltipLangKey: "NametagArmorFullColorSettingInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }),       
    ];

    constructor(
        private userpanelSettingsService: UserpanelSettingsNormalService,
        private rageConnector: RageConnectorService) {
    }

    ngOnInit() {
        const container = this.getSettingsContainer();
        for (const setting of this.userpanelSettings.filter(s => s instanceof SettingRow) as SettingRow[]) {
            setting.formControl.setValue(container[setting.dataSettingIndex]);
        }
    }
    
    save(obj: UserpanelSettingsIngameColors) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.IngameColors, obj).subscribe();
    }

    changed(event: SettingChangedEvent) {
        const settingName = SettingsIngameColorsIndex[event.index];
        if (!(settingName in SettingAtClientside)) {
            return;
        }
        let index = SettingAtClientside[settingName];
        this.rageConnector.call(DToClientEvent.OnColorSettingChange, index, event.value);
    }

    private getSettingsContainer(): UserpanelSettingsIngameColors {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.IngameColors] as UserpanelSettingsIngameColors;
    }
}   